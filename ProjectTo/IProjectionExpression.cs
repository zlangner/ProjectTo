using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTo
{
    public interface IProjectionExpression<TSource>
    {
        /// <summary>
        /// Project a elements of a query to a to a compatible model.
        /// </summary>
        /// <remarks>
        /// Each property of <typeparamref name="TDest"/> is expected in <typeparamref name="TSource"/> with 
        /// the same name. The source property is assigned to the destination property, and hence must be of the same
        /// type or implicitly castable.
        /// </remarks>
        /// <typeparam name="TDest"></typeparam>
        /// <returns></returns>
        IQueryable<TDest> To<TDest>();

        /// <summary>
        /// Project a elements of a query to a different shape, with custom mappings.
        /// </summary>
        /// <typeparam name="TDest"></typeparam>
        /// <remarks>
        /// Each property of <typeparamref name="TDest"/> is expected in <typeparamref name="TSource"/> with 
        /// the same name. The source property is assigned to the destination property, and hence must be of the same
        /// type or implicitly castable.
        /// <para/>
        /// For destination properties that are not represented by name in the source, or that are not of the same type of that 
        /// cannot be case implicitly, use the <param name="customMapper"/> to assign a value to the property.
        /// </remarks>
        /// <returns></returns>
        IQueryable<TDest> To<TDest>(Action<Mapper<TSource, TDest>> customMapper);
    }
}
