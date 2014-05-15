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
	public class ProjectionExpression<TSource> : ProjectionBuilder<TSource>, IProjectionExpression<TSource>
	{
		/// <summary>
		/// Source IQueryable, that we are going to project
		/// </summary>
		private readonly IQueryable<TSource> source;

		/// <summary>
		/// Creates LINQ expression, converting one IQueryable to another
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		public ProjectionExpression(IQueryable<TSource> source)
		{
			this.source = source;
		}

		/// <summary>
		/// Creates LINQ expression, using default mapping rules
		/// </summary>
		public IQueryable<TDest> To<TDest>()
		{
			return source.Select(base.ToExpression<TDest>());
		}

		/// <summary>
		/// Creates LINQ expression, using custom and default mapping rules
		/// </summary>
		public IQueryable<TDest> To<TDest>(Action<Mapper<TSource, TDest>> customMap)
		{
			return source.Select(base.ToExpression<TDest>(customMap));
		}
	}
}
