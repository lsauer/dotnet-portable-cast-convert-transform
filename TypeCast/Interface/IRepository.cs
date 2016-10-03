// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast
{
    using System;
    using System.Collections;

    using Core.TypeCast.Base;

    /// <summary>The base repository interface</summary>
    /// <remarks>Implemented by any instantiable object which can provide data by passing an Identifier to facilitate lookup</remarks>
    public interface IRepository
    {
        /// <summary>
        /// Gets a single entity or a collection of entities, implementing an <see cref="IEnumerable"/> which correspond to the identifier <paramref name="id"/>.
        /// </summary>
        /// <param name="id"> The identifier for facilitating lookup of one or several entities</param>
        /// <returns>An <see cref="object"/> comprising one or several entities which correspond to the identifier <paramref name="id"/>.  </returns>
        object Get(object id);
    }

    /// <summary>The generic repository interface with one lookup identifier</summary>
    /// <remarks>Implemented by any instantiable object which can provide data by passing one Identifier to facilitate lookup</remarks>
    public interface IRepository<out TOut>
    {
        /// <summary>
        /// Gets a single entity or a collection of entities, implementing an <see cref="IEnumerable"/> which correspond to the identifier <paramref name="id"/>.
        /// </summary>
        /// <param name="id"> The identifier for facilitating lookup of one or several entities</param>
        /// <returns>An <see cref="object"/> comprising one or several entities which correspond to the identifier <paramref name="id"/>.  </returns>
        TOut Get(object id);
    }

    /// <summary>The generic repository interface with one lookup identifier, and a strict entity return type of <typeparamref name="TOut"/></summary>
    /// <typeparam name="TId1">The <see cref="Type"/> of the lookup identifier</typeparam>
    /// <typeparam name="TOut">The <see cref="Type"/> of the entity returned from the repository method <see cref="Get(TId1)"/></typeparam>
    /// <remarks>Implemented by any instantiable object which can provide data by passing one Identifier to facilitate lookup</remarks>
    public interface IRepository<in TId1, out TOut>
    {
        /// <summary>The initialize method.</summary>
        /// <param name="id1">The argument value of the lookup identifier</param>
        /// <returns>A strict entity return type of <typeparamref name="TOut"/> which may be a single entity or collection thereof, implementing <see cref="IEnumerable"/>
        /// </returns>
        TOut Get(TId1 id1);
    }

    /// <summary>The generic repository interface with twp lookup identifiers, and a strict entity return type of <typeparamref name="TOut"/></summary>
    /// <typeparam name="TId1">The <see cref="Type"/> of the lookup identifier </typeparam>
    /// <typeparam name="TId2">The <see cref="Type"/> of the lookup identifier </typeparam>
    /// <typeparam name="TOut">The <see cref="Type"/> of the entity returned from the repository method <see cref="IRepository{TId1, TOut}.Get(TId1)"/></typeparam>
    /// <remarks>Implemented by any instantiable object which can provide data by passing two Identifiers to facilitate lookup</remarks>
    public interface IRepository<in TId1, in TId2, out TOut> : IRepository<TId1, TOut>
    {
        /// <summary>The initialize method.</summary>
        /// <param name="id1">The argument value of the lookup identifier</param>
        /// <param name="id2">The argument value of the lookup identifier</param>
        /// <returns>A strict entity return type of <typeparamref name="TOut"/> which may be a single entity or collection thereof, implementing <see cref="IEnumerable"/>
        /// </returns>
        TOut Get(TId1 id1, TId2 id2);
    }
}