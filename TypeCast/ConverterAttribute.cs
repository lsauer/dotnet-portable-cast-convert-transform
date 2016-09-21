// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Converter pattern library    </summary
// <language>  C# > 6.0                                                         </language>
// <version>   3.1.0.2                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-Converter                       </project>
namespace Core.TypeCast
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Base;

    /// <summary>
    /// Use <see cref="ConverterAttribute"/> to declare a class as a logical Type Converter. As such the only contingent declaration contract requirement is adherence to 
    /// implement a public constructor which takes a `IConverterCollection collection` parameter
    /// </summary>
    /// <remarks>
    /// The attribute provides collective initialization through the <see cref="ConverterCollection"/> class.
    /// </remarks>
    /// <example> **Example:**  
    /// <code>
    /// ```cs
    ///     [Converter(loadOnDemand: false, nameSpace: nameof(Program.NameSpace))]
    ///     public class StringToDNA : IConverter, IConverter&lt;string, DNA>{
    ///         ... 
    ///         public StringToDNA(IConverterCollection collection){ ... collection.Add((Func&lt;string, DNA>)this.Convert); ...}
    ///         public object Convert(object valueIn){ ... }
    ///         public DNA Convert(string valueIn) { ... }
    ///         [ConverterMethod]
    ///         public object ToProtein(string valueIn){
    ///         }
    ///         ...
    ///     }
    /// ```
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    public sealed class ConverterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterAttribute"/> class, to declare a custom <see cref="Converter"/>
        /// </summary>
        /// <param name="loadOnDemand"> Set to `true` to allow joint initialization by the lazy instancing of the <see cref="ConverterCollection"/> <see cref="Core.Singleton"/>  </param>
        /// <param name="nameSpace"> The namespace as a string, ideally set via the <see cref="nameof"/> operator to group converters and enable lazy-loading upon first use.  </param>
        /// <param name="name"> A custom label for the converter or `null` if none is set, particularly used for Transform function disambiguates (i.e. identical In/Out parameters) </param>
        /// <param name="dependencyInjection">Whether the declaring converter class is instantiated via dependency Injection.</param>
        public ConverterAttribute(string nameSpace = "", string name = "", bool loadOnDemand = false, bool dependencyInjection = false)
        {
            this.Id = Guid.NewGuid();
            this.LoadOnDemand = loadOnDemand;
            this.NameSpace = nameSpace;
            this.Name = name;
            this.DependencInjection = dependencyInjection;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConverterAttribute(string nameSpace, Enum name, bool loadOnDemand = false, bool dependencyInjection = false)
            : this(loadOnDemand: loadOnDemand, nameSpace: nameSpace, name: name.ToString(), dependencyInjection: dependencyInjection)
        {
        }


        /// <summary>
        /// Gets a unique ID of the converter based on an underlying <see cref="Guid"/>.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets or sets the base type i.e. the declaring type (see: <see cref="Type.DeclaringType"/> containing or declaring the converter-functions.
        /// </summary>
        public TypeInfo BaseType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dependency injection should be used during instancing of a new custom converter />.
        /// </summary>
        public bool DependencInjection { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Converter{TIn, TOut}"/> Converter is supposed to be initialized merely by declaring the <see cref="ConverterAttribute"/>
        /// </summary>
        /// <example> **Example:**  A Converter `StringToDNA ` is allowed to be initialized by the <see cref="ConverterAttribute"/> 
        /// <code>```cs
        ///     [Converter(loadOnDemand: true, nameSpace: nameof(Program.NameSpace))]
        ///     internal class StringToDNA {
        ///     {   
        ///         ...
        ///         public StringToDNA(IConverterCollection collection, bool storeRNA = false){ ... }
        ///         ...
        ///     }
        ///     ...
        /// ```</code>
        /// </example>
        public bool LoadOnDemand { get; }

        /// <summary>
        /// Gets the <see cref="Type.Namespace"/> or an empty string if no <see cref="NameSpace"/> is set.
        /// </summary>
        /// <example> **Example:**  A Converter `StringToDNA` is added to the collection namespace of the main `Program` class
        /// <code>
        /// ```cs
        ///     [Converter(nameSpace: nameof(Program.NameSpace))]
        ///     internal class StringToDNA {
        ///     {   
        ///         ...
        ///         public StringToDNA(IConverterCollection collection,  storeRNA = false){ ... }
        ///         ...
        ///     }
        ///     ...
        /// ```</code>
        /// </example>
        /// <remarks>The namespace is used for filtering and grouping <see cref="Core.TypeCast.Converters"/></remarks>
        public string NameSpace { get; }


        /// <summary>
        /// Gets a custom label for the converter or `null` if none is set 
        /// </summary>
        /// <remarks>Use for filtering.</remarks>
        public string Name { get; set; }

        public override string ToString()
        {
            return $"[LoD:{this.LoadOnDemand},Base:{this.BaseType?.Name},DepInj:{this.DependencInjection},NamSp:{this.NameSpace},Name:{this.Name}]";
        }

    }
}