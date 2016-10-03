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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Core.Extensions;
    using Core.Singleton;
    using Core.TypeCast.Base;


    /// <summary>
    /// The thread-safe, static collection of <see cref="Converter"/> items, using <see cref="Core.Singleton"/> and supporting <see cref="Core.Singleton.ISingleton"/>
    /// </summary>
    /// <remarks> See https://github.com/lsauer/csharp-singleton for more information.</remarks>
    /// <remarks> Building the project requires the Singleton-library: https://www.nuget.org/packages/CSharp.Portable-Singleton/ </remarks>
    /// <remarks>If <see cref="SingletonAttribute.CreateInternal"/> is set to `false`, <see cref="ConverterCollection"/> must explicitly be instantiated using 
    /// `new ConverterCollection(...)` and  not use <see cref="Singleton{ConverterCollection}.CurrentInstance"/> before instantiation. 
    /// If <see cref="SingletonAttribute.CreateInternal"/> is set to `true`, lazy-instantiation of the singleton is possible at any time, 
    /// after the program's assembly entry point is reached.</remarks>
    public partial class ConverterCollection : Singleton<ConverterCollection>, IConverterCollection, IDisposable, IEnumerable<Converter>, IQueryable<Converter>
    {
        /// <summary>
        /// Allows to perform a deferred adding operation of multiple adds using a common Base-Class <see cref="Type"/> argument, 
        /// common <see cref="ConverterCollectionSettings"/> as well as a mutual <see cref="CancellationToken"/> for the added group of converters.
        /// The operation is applied upon invoking <see cref="End"/>, and can be explicitly canceled by invoking <see cref="Cancel"/>
        /// </summary>
        /// <typeparam name="TBase">The declaring, attributed <see cref="Type"/> of the converter-functions to add as a group.</typeparam>
        public class AddBuilder<TBase> where TBase : class
        {
            /// <summary>
            /// A reference to the <see cref="ConverterCollection"/> instance
            /// </summary>
            ConverterCollection baseClass;

            /// <summary>
            /// An optional reference to the mutual <see cref="CancellationToken"/>
            /// </summary>
            CancellationToken cancellationToken;

            /// <summary>
            /// The list of deferred actions to Invoke upon calling <see cref="End"/>
            /// </summary>
            List<Action> actions;

            /// <summary>
            /// A reference to an optional <see cref="ConverterCollectionSettings"/> instance to be applied only for the group of converter-functions
            /// </summary>
            ConverterCollectionSettings settings;

            /// <summary>
            /// Creates a new instance of <see cref="AddBuilder{TBase}"/> for grouped-adding of converter-functions with default arguments defined 
            /// in the constructor
            /// </summary>
            /// <param name="converterCollection">A reference to the <see cref="ConverterCollection"/> instance</param>
            /// <param name="settings">A reference to an optional <see cref="ConverterCollectionSettings"/> instance to be applied only for the 
            /// group of converter-functions</param>
            /// <param name="cancellationToken">An optional value-instance to the mutual <see cref="CancellationToken"/></param>
            public AddBuilder(ConverterCollection converterCollection,
                            ConverterCollectionSettings settings = null,
                            CancellationToken cancellationToken = default(CancellationToken))
                : this(converterCollection: converterCollection, settings: settings, cancellationToken: ref cancellationToken)
            {

            }

            /// <summary>
            /// Creates a new instance of <see cref="AddBuilder{TBase}"/> for grouped-adding of converter-functions with mutual arguments defined 
            /// in the constructor
            /// </summary>
            /// <param name="converterCollection">A reference to the <see cref="ConverterCollection"/> instance</param>
            /// <param name="settings">A reference to an optional <see cref="ConverterCollectionSettings"/> instance to be applied only for the 
            /// group of converter-functions</param>
            /// <param name="cancellationToken">An optional reference to the mutual <see cref="CancellationToken"/></param>
            public AddBuilder(ConverterCollection converterCollection,
                ConverterCollectionSettings settings,
                ref CancellationToken cancellationToken)
            {
                this.baseClass = converterCollection;
                this.settings = settings;
                this.cancellationToken = cancellationToken;
                this.actions = new List<Action>();
            }

            /// <summary>
            /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
            /// </summary>
            /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
            /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
            /// <param name="converterAction">A function delegate <see cref="Func{TIn, TOut}"/> to use as the <see cref="Converter.Function"/></param>
            /// <returns>Returns a (nested-class) instance of <see cref="AddBuilder{TBase}"/> with strongly-typed generic methods for deferred grouped-adding.</returns>
            /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
            /// if an internal error occurred</exception>
            public AddBuilder<TBase> Add<TIn, TOut>(Func<TIn, TOut> converterAction)
            {
                actions.Add(() => this.baseClass.Add<TIn, TOut, TBase>(converterAction, this.cancellationToken));
                return this;
            }

            /// <summary>
            /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
            /// </summary>
            /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
            /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
            /// <param name="converterActionDefault">A function delegate <see cref="Func{TIn, TOut, TOut}"/> to use as the <see cref="Converter.Function"/></param>
            /// <returns>Returns a (nested-class) instance of <see cref="AddBuilder{TBase}"/> with strongly-typed generic methods for deferred grouped-adding.</returns>
            /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
            /// if an internal error occurred</exception>
            public AddBuilder<TBase> Add<TIn, TOut>(Func<TIn, TOut, TOut> converterActionDefault)
            {
                actions.Add(() => this.baseClass.Add<TIn, TOut, TBase>(converterActionDefault, this.cancellationToken));
                return this;
            }

            /// <summary>
            /// Dummy function to explicitly end the current chained builder operation without applying any deferred calls and clear any resources
            /// </summary>
            /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
            public IConverterCollection Cancel()
            {
                return baseClass;
            }

            /// <summary>
            /// End the deferred adding operation by invocation the deferred list of <see cref="actions"/>
            /// </summary>
            /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
            public IConverterCollection End()
            {
                // temporarily switch out settings for any passed builder-settings argument
                if(this.settings != null)
                {
                    var tmp = this.baseClass.Settings;
                    this.baseClass.Settings = this.settings;
                    actions.Add(() => this.baseClass.Settings = tmp);
                }
                actions.Count(a => { a.Invoke(); return true; });
                return baseClass;
            }
        }
    }
}