using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Globalization;

namespace ProjectTo
{
    /// <summary>
    /// Creates LINQ expression, converting one IQueryable to another
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class ProjectionBuilder<TSource> : IProjectionBuilder<TSource>
    {
        /// <summary>
        /// The root parameter of expression tree
        /// </summary>
        protected readonly ParameterExpression rootParameter = Expression.Parameter(typeof(TSource), "__p__");

        /// <summary>
        /// Creates LINQ expression, using default mapping rules
        /// </summary>
        public Expression<Func<TSource, TDest>> ToExpression<TDest>()
        {
            return ToExpression<TDest>(mapper => { });
        }

        /// <summary>
        /// Creates an expression, using custom and default mapping rules
        /// </summary>
        public Expression<Func<TSource, TDest>> ToExpression<TDest>(Action<Mapper<TSource, TDest>> customMap)
        {
            var customMappings = new List<Mapping>();
            var ignoredProperties = new List<PropertyInfo>();
            var mapper = new Mapper<TSource, TDest>(customMappings, ignoredProperties);
            customMap(mapper);

            var cacheKey = CreateCacheKey(typeof(TSource), typeof(TDest), customMappings, ignoredProperties, mapper.IsDefaultMappingIgnored);
            var expr = ProjectionCache.Current.FindValue(cacheKey) as Expression<Func<TSource, TDest>>;
            if (expr == null)
            {
                expr = BuildExpression<TDest>(customMappings, ignoredProperties, mapper.IsDefaultMappingIgnored);
                ProjectionCache.Current.SetValue(cacheKey, expr);
            }

            return expr;
        }

        /// <summary>
        /// Create unique key for expression
        /// </summary>
        private static string CreateCacheKey(Type TSource, Type TDest, List<Mapping> customMappings, List<PropertyInfo> ignoredProperties, bool isDefaultMappingIgnored)
        {
            var cacheKey = String.Format("{0}${1}", TSource.FullName, TDest.FullName);
            if (customMappings.Count > 0)
            {
                var mappingsString = String.Join("$", customMappings.Select(m => String.Format("{0};{1}", m.DestPropertyInfo.Name, m.Transform)).ToArray());
                cacheKey += mappingsString;
            }
            if (ignoredProperties.Count > 0)
            {
                cacheKey += ("$IGNORED:" + String.Join("$", ignoredProperties.Select(p => p.Name).ToArray()));
            }
            if (isDefaultMappingIgnored)
            {
                cacheKey += "$CustomMappingIgnored$";
            }
            return cacheKey;
        }

        private Expression<Func<TSource, TDest>> BuildExpression<TDest>(IEnumerable<Mapping> customMaps, IEnumerable<PropertyInfo> ignoredProperties, bool isDefaultMappingIgnored)
        {
            var sourceMembers = typeof(TSource).GetProperties();
            var destinationMembers = typeof(TDest).GetProperties();

            var resultExp = Expression.MemberInit(
                        Expression.New(typeof(TDest)),
                        destinationMembers
                            .Where(dest => !ignoredProperties.Contains(dest))
                            .Select(dest => CreateAssignment(dest, customMaps, sourceMembers, isDefaultMappingIgnored))
                            .Where(d => d != null)
                            .ToArray());

            var result =
                Expression.Lambda<Func<TSource, TDest>>(
                    resultExp,
                    rootParameter);
            return result;
        }

        /// <summary>
        /// Returns expression, that creates (initialize) destination property
        /// </summary>
        private MemberBinding CreateAssignment(PropertyInfo dest, IEnumerable<Mapping> customMaps, PropertyInfo[] sourceMembers, bool isDefaultMappingIgnored)
        {
            var customMap = customMaps.FirstOrDefault(m => m.DestPropertyInfo == dest);
            if (customMap != null)
            {
                LambdaExpression transformExpression = customMap.Transform;
                //Now we need to replace root parameter in this expression to the root parameter of
                //the final expression tree
                var visitor = new ParameterReplacerVisitor(transformExpression.Parameters[0].Name, rootParameter);
                var newExpression = (LambdaExpression)visitor.Visit(transformExpression);

                var res = Expression.Bind(dest, newExpression.Body);
                return res;
            }

            //If default mapping is ignored we don't automatically map anything
            if (isDefaultMappingIgnored)
                return null;

            //Now perform the default mapping
            //Check for exact match
            var sourceProp = sourceMembers.FirstOrDefault(p => String.Compare(p.Name, dest.Name, true, CultureInfo.InvariantCulture) == 0);
            if (sourceProp != null)
            {
                var exp = Expression.Property(rootParameter, sourceProp);
                //Map only assignable types
                if (dest.PropertyType.IsAssignableFrom(exp.Type) == false)
                    return null;

                try
                {
                    var res = Expression.Bind(dest, exp);
                    return res;
                }
                catch (ArgumentException)
                {
                    //it's ok. It means - the types of properties are not assignable 
                    return null;
                }
            }

            return FindCamelCaseAssignment(dest, sourceMembers);
        }

        /// <summary>
        /// Finds CamelCase match (e.g. PersonName -> Person.Name)
        /// </summary>
        private MemberAssignment FindCamelCaseAssignment(MemberInfo destinationProperty, PropertyInfo[] sourceProperties)
        {
            var allCombinations = StringHelper.SplitToWordGroups(destinationProperty.Name);
            foreach (var comb in allCombinations)
            {
                var properties = sourceProperties;
                PropertyInfo prop = null;
                Expression pe = rootParameter;

                foreach (var word in comb)
                {
                    if (properties == null)
                        break;
                    prop = properties.FirstOrDefault(p => p.Name == word);
                    if (prop == null)
                        break;
                    properties = prop.PropertyType.GetProperties();
                    pe = Expression.Property(pe, prop);
                }
                if (prop != null)
                {
                    var result = Expression.Bind(destinationProperty, pe);
                    return result;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Replaces all parameters with some name to new parameter
    /// </summary>
    public class ParameterReplacerVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression newParameter;
        private readonly string parameterName;
        /// <summary>
        /// Replaces all parameters with some name to new parameter
        /// </summary>
        public ParameterReplacerVisitor(string parameterName, ParameterExpression newParameter)
        {
            this.newParameter = newParameter;
            this.parameterName = parameterName;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Name == parameterName)
                return newParameter;
            return node;
        }
    }
}
