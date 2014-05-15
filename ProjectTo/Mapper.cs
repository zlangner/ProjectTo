using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ProjectTo
{
    /// <summary>
    /// Maps custom property (beyond conventions)
    /// </summary>
    public class Mapper<TSource, TDest>
    {	
		private readonly List<Mapping> customMappings;
        private readonly List<PropertyInfo> ignoreProperties;

	    internal bool IsDefaultMappingIgnored { get; set; }

	    internal Mapper(List<Mapping> customMappings, List<PropertyInfo> ignoreProperties)
	    {
			this.IsDefaultMappingIgnored = false;
			this.customMappings = customMappings;
            this.ignoreProperties = ignoreProperties;
        }

        public Mapper<TSource, TDest> Map<TProperty>(Expression<Func<TDest, TProperty>> property, Expression<Func<TSource, TProperty>> transform)
        {
            customMappings.Add(new Mapping(ReflectionHelper.GetPropertyInfo(property), transform));

            return this;
        }

        public Mapper<TSource, TDest> Ignore<TProperty>(Expression<Func<TDest, TProperty>> property)
        {
            ignoreProperties.Add(ReflectionHelper.GetPropertyInfo(property));

            return this;
        }
		/// <summary>
		/// If this option is set all default mapping conventions will be ignored.
		/// So only explicit mapping will be used
		/// </summary>
		/// <returns></returns>
		public Mapper<TSource, TDest> IgnoreDefaultMapping()
		{
			this.IsDefaultMappingIgnored = true;
			return this;
		}
    }
}
