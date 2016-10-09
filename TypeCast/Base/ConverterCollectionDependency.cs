// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast.Base
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    using Core.Extensions;

    /// <summary>The converter dependency provides loose implementation of Converters, merely constricting the constructor to be passed an
    ///     <see cref="IConverterCollection" /> for Dependency Injection. It is not required due to Constructor auto-discovery provided by
    ///     <see cref="TypeInfoExtension.IsDependencyInjectable" />
    ///     yet provides strict typing at compile time and a slight speedup</summary>
    /// <remarks><see cref="ConverterCollectionDependency" /> only serves as a constructor implementation contract</remarks>
    /// <example>
    ///     **Example:** Add DateTime converters loaded upon first access, through strict DependencyInjection
    ///     <code>
    /// ```cs
    ///     [Converter(loadOnDemand: true, nameSpace: nameof(System), dependencyInjection: true)]
    ///     public class ConverterDateTimeDefaults : ConverterCollectionDependency
    ///     {
    ///         public ConverterDateTimeDefaults(IConverterCollection collection) : base(collection)
    ///         {
    ///             this.AddDateTimeConverter(collection: collection);
    ///         }
    ///         ...
    ///     }
    /// ```
    /// </code>
    /// </example>
    [System.Runtime.InteropServices.ComVisible(false)]
    public abstract class ConverterCollectionDependency : DependencyInjection<IConverterCollection>
    {
        /// <summary>Initializes a new instance of the <see cref="ConverterCollectionDependency" /> class.</summary>
        /// <param name="collection">
        ///     The <see cref="IConverterCollection" /> collection which is required as an argument of the parent class constructor for custom
        ///     dependency injection.
        /// </param>
        /// <exception cref="DependencyInjectionException">
        ///     If the <paramref name="collection" /> is `null` a <see cref="DependencyInjectionException"/> is thrown
        /// </exception>
        protected ConverterCollectionDependency(IConverterCollection collection)
            : base(collection)
        {
        }
    }

    /// <summary>The converter dependency for specific implementation of Converters, implementing the logic for adding the converter to the collection</summary>
    /// <typeparam name="TIn">The source / from <see cref="Type" />from which to convert</typeparam>
    /// <typeparam name="TOut">The target / to <see cref="Type" /> to which to converter</typeparam>
    /// <remarks>
    ///     The <see cref="ConverterCollectionDependency{TIn,TOut}" /> necessitates overriding <see cref="Convert" />:
    ///     <code>
    ///  ```cs
    ///     public override TOut Convert(...) { 
    ///         ... 
    ///     }
    /// ``` 
    /// </code>
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class ConverterCollectionDependency<TIn, TOut> : ConverterCollectionDependency, IConverter<TIn, TOut>
    {
        /// <summary>Initializes a new instance of the <see cref="ConverterCollectionDependency{TIn,TOut}" /> class.</summary>
        /// <param name="collection">
        ///     The <see cref="IConverterCollection" /> collection which is required as an argument of the parent class constructor for custom
        ///     dependency injection.
        /// </param>
        /// <exception cref="ConverterException">
        ///     If the <paramref name="collection" /> is null an exception of cause <seealso cref="ConverterCollectionCause.CollectionIsNull" /> is
        ///     thrown
        /// </exception>
        /// <remarks>
        ///     Unlike <see cref="ConverterCollectionDependency" />, the generic class <see cref="ConverterCollectionDependency{TIn,TOut}" /> provides a
        ///     constructor-injection contract and logic to add <see cref="Convert"/> to the <paramref name="collection" />
        /// </remarks>
        protected ConverterCollectionDependency(IConverterCollection collection)
            : base(collection)
        {
            collection.Add((Func<TIn, TOut, TOut>)this.Convert, this.GetType());
        }

        /// <summary> The converter function that needs to be overwritten as part of the <see cref="IConverter" /> interface support. </summary>
        /// <param name="value">The value of <see cref="Type"/> <typeparamref name="TIn"/> to be converted.</param>
        /// <param name="defaultValue">The optional default value of <see cref="Type" /> <typeparamref name="TOut"/> to be passed if the conversion fails or is `null`.</param>
        /// <returns>The value converted to <see cref="Type" /> of <typeparamref name="TOut"/> </returns>
        /// <exception cref="ConverterException">Throws an exception of <see cref="ConverterCause.ConverterNotImplemented" /> if the parent class does not implement
        ///     <code>`public override TOut `</code> <see cref="Convert" />
        /// </exception>
        public abstract TOut Convert(TIn value, TOut defaultValue = default(TOut));
    }
}