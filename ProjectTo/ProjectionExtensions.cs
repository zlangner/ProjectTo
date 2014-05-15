using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ProjectTo
{
	public static class ProjectionExtensions
	{
		public static IProjectionExpression<TSource> Project<TSource>(this IQueryable<TSource> source)
		{
			return new ProjectionExpression<TSource>(source);
		}

		public static Expression<Func<TSource, TDest>> Map<TSource, TDest, TProperty>(this Expression<Func<TSource, TDest>> exp, Expression<Func<TDest, TProperty>> property, Expression<Func<TSource, TProperty>> transform)
		{
			return (new ProjectionBuilder<TSource>()).ToExpression<TDest>(m => m.Map(property, transform));
		}

		public static Expression<Func<TSource, TDest>> Map<TSource, TDest>(this Expression<Func<TSource, TDest>> exp, Expression<Func<TSource, TDest>> transform)
		{
			return (new ProjectionBuilder<TSource>()).ToExpression<TDest>(m => m.Map(transform));
		}
	}
}
