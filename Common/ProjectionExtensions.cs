using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodePlex.LinqProjector
{
    public static class ProjectionExtensions
    {
        public static IProjectionExpression<TSource> Project<TSource>(this IQueryable<TSource> source)
        {
            return new ProjectionExpression<TSource>(source);
        }
    }
}
