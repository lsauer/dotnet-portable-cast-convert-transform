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
    using System.Collections.Concurrent;
    using System.Reflection;
    using System.Threading;

    using Core.TypeCast.Base;

    public interface IConverterCollection
    {
        /// <summary>
        /// Sets the <see cref="Assembly"/> as initialized in <see cref="AssemblyInitialized"/>
        /// </summary>
        /// <param name="type">the <see cref="TypeInfo"/> instance from which to lookup the <see cref="TypeInfo.Assembly"/></param>
        /// <returns>Returns true if the concurrent dictionary update succeeded</returns>
        ConcurrentDictionary<Assembly, bool> AssemblyInitialized { get; }

        /// <summary>
        /// The <see cref="BlockingCollection{Converter}"/> class provides a thread-safe collection with full support of the Producer-Consumer pattern, 
        /// to store a collection of <see cref="Converter"/> instances.
        /// </summary>
        BlockingCollection<Converter> Items { get; }

        /// <summary>Gets the count of singletons in the list.</summary>
        int Count { get; }

        /// <summary>
        /// The settings for the <see cref="ConverterCollection"/>.
        /// </summary>
        ConverterCollectionSettings Settings { get; set; }

        /// <summary>
        /// Adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/>
        /// </summary>
        /// <param name="converter">The <see cref="Converter"/> instance to add to the <see cref="ConverterCollection.Items"/></param>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="allowDisambiguates">Optional <see cref="bool"/> value to indicate whether to allow multiple converters with the 
        /// same return and parameter <see cref="Type"/>s in the collection of <see cref="Items"/>. Only set to `true` for transform functions.</param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterCollectionException">Throws an exception if adding failed</exception>
        IConverterCollection Add(Converter converter, Type baseType = null, bool allowDisambiguates = false, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <param name="converterAction">A function delegate <see cref="Func{TIn, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        IConverterCollection Add<TIn, TOut>(Func<TIn, TOut> converterAction, Type baseType = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TBase">The <see cref="Type"/> of the declaring and attributed custom converter class</typeparam>
        /// <param name="converterAction">A function delegate <see cref="Func{TIn, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        IConverterCollection Add<TIn, TOut, TBase>(Func<TIn, TOut> converterAction, CancellationToken cancellationToken = default(CancellationToken)) where TBase : class;

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TArg">The Argument <see cref="Type"/> for generic converters using see <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object)"/>. 
        /// <param name="converterActionAny">A function delegate <see cref="Func{TIn, TArg, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        IConverterCollection Add<TIn, TArg, TOut>(Func<TIn, TArg, TOut> converterActionAny, Type baseType = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <param name="converterActionDefault">A function delegate <see cref="Func{TIn, TOut, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        IConverterCollection Add<TIn, TOut, TBase>(Func<TIn, TOut, TOut> converterAction, CancellationToken cancellationToken = default(CancellationToken)) where TBase : class;

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <param name="converterDelegate">An optional function delegate to use as the <see cref="Converter.Function"/></param>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        IConverterCollection Add(object converterDelegate, Type baseType = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <param name="converterDelegate">An optional function delegate to use as the <see cref="Converter.Function"/></param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        IConverterCollection Add<TBase>(object converterDelegate, CancellationToken cancellationToken = default(CancellationToken)) where TBase : class;

    }
}