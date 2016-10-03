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

    /// <summary>The generic repository interface with one lookup identifier</summary>
    /// <remarks>Implemented by any instantiable object which can provide data by passing one Identifier to facilitate lookup</remarks>
    /// <example><code>
    /// ```cs
    ///     ConverterCollection.CurrentInstance.Add( (int[] a, IModelWeather model) =>  { 
    ///         ...repo... 
    ///     });
    ///     IRepositoryEnumerable&lt;IModelWeather> repositoryQuery = new []{40420,52000,80801,20030}
    ///                                                                 .ConvertTo&lt;IRepositoryEnumerable&lt;IModelWeather>>( modelInstance );
    ///     foreach( var item in repositoryQuery)
    ///     {
    ///         ...
    ///     }
    /// ```
    /// </code></example>
    public interface IRepositoryEnumerable<out TOut> : IRepository<TOut> where TOut : IEnumerable { }

    /// <summary>The generic repository interface with one lookup identifier, and a strict entity return type of <typeparamref name="TOut"/></summary>
    /// <typeparam name="TId1">The <see cref="Type"/> of the lookup identifier.</typeparam>
    /// <typeparam name="TOut">The <see cref="Type"/> of the entity returned from the repository method <see cref="IRepository{TId1, TOut}.Get(TId1)"/></typeparam>
    /// <remarks>Implemented by any instantiable object which can provide data by passing one Identifier to facilitate lookup</remarks>
    public interface IRepositoryEnumerable<in TId1, out TOut> : IRepository<TId1, TOut> where TOut : IEnumerable { }

    /// <summary>The generic repository interface with twp lookup identifiers, and a strict entity return type of <typeparamref name="TOut"/></summary>
    /// <typeparam name="TId1">The <see cref="Type"/> of the lookup identifier </typeparam>
    /// <typeparam name="TId2">The <see cref="Type"/> of the lookup identifier </typeparam>
    /// <typeparam name="TOut">The <see cref="Type"/> of the entity returned from the repository method <see cref="IRepository{TId1, TOut}.Get(TId1)"/></typeparam>
    /// <remarks>Implemented by any instantiable object which can provide data by passing two Identifiers to facilitate lookup</remarks>
    public interface IRepositoryEnumerable<in TId1, in TId2, out TOut> : IRepository<TId1, TId2, TOut> where TOut : IEnumerable { }

}