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
    /// Use <see cref="ConverterMethodAttribute"/> to declare a method in an arbitrary class as a logical Converter function. 
    /// The only restriction towards the attributed method is a maximum of two defined function parameters. This limitation is by design, 
    /// to advocate the use of the single responsibility principle, and building complex converters out of smaller ones, as 
    /// implemented by a different library.
    /// Take a look at the examples and documentation for more information.
    /// </summary>
    /// <remarks>
    /// The attribute provides collective initialization through the <see cref="ConverterCollection"/> class.
    /// </remarks>
    /// <example> **Example:**  Declaring an arbitrary class method to be loaded and encapsulated into a converter instance
    /// <code>
    /// ```cs
    ///     [Converter(loadOnDemand: false, name: nameof(Program.BioCore.ToDNA))]
    ///     public class StringToDNA {
    ///         ... 
    ///         // Constructor with dependency injection
    ///         public StringToDNA(IConverterCollection collection) 
    ///         { 
    ///             ... collection.Add((Func&lt;string, DNA>)this.Convert); ...
    ///         }
    ///         // method added explicitly by the constructor supporting converter-dependency-injection
    ///         // Note: It is recommended to derive the class `StringToDNA`  from `ConverterCollectionDependency`
    ///         public object Convert(object valueIn)
    ///         { 
    ///             ... 
    ///         }
    ///         // method loaded automatically, through attribution
    ///         [ConverterMethod]        
    ///         public DNA Convert(string valueIn) 
    ///         { 
    ///             ... 
    ///         }
    ///         ...
    ///     }
    /// ```
    /// </code>
    /// </example>
    /// Reference: <seealso cref="Converter.Function"/>, <seealso cref="Converter.FunctionDefault"/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ConverterMethodAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterMethodAttribute"/> class, to declare a custom <see cref="Converter"/> function
        /// </summary>
        /// <param name="loadOnDemand">Set to `true` to allow joint initialization by the lazy instancing of the <see cref="ConverterCollection"/> <see cref="Core.Singleton"/>  </param>
        /// <param name="isStatic"> Set to `true` to invoke the method with `this` reference set to null, otherwise with an instance reference passed. Default is `true`. </param>
        /// <param name="name"> An alias as a string, ideally set via the <see cref="nameof"/> operator to identify among ambiguous converters.</param>
        /// <param name="dependencyInjection">
        /// Whether the declaring converter class is instantiated via dependency Injection.
        /// </param>
        public ConverterMethodAttribute(bool isStatic = true, bool loadOnDemand = true, string name = "", bool passInstance = false)
        {
            this.LoadOnDemand = loadOnDemand;
            this.Name = name;
            this.IsStatic = isStatic;
            this.PassInstance = passInstance;
        }

        /// <summary>
        /// Gets or sets the base type.
        /// </summary>
        /// <seealso cref="Converter.BaseType"/>
        public Type BaseType { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Converter{TIn, TOut}"/> Converter is supposed to be loaded in the <see cref="ConverterCollection"/> upon first use.
        /// </summary>
        /// <example> **Example:**  A Converter `StringToDNA ` is allowed to be initialized by the <see cref="ConverterMethodAttribute"/> 
        /// <code>```cs
        ///     [Converter(loadOnDemand: false, name: nameof(Program.BioCode.ToDNA))]
        ///     internal class StringToDNA {
        ///         ... 
        ///         // Constructor with dependency injection
        ///         public StringToDNA(IConverterCollection collection) 
        ///         { 
        ///             ... collection.Add((Func&lt;string, DNA>)this.Convert); ...
        ///         }
        ///         // method added explicitly by the constructor supporting converter-dependency-injection
        ///         // Note: It is recommended to derive the class `StringToDNA`  from `ConverterCollectionDependency`
        ///         public object Convert(object valueIn)
        ///         { 
        ///             ... 
        ///         }
        ///         // method loaded automatically, through attribution
        ///         [Converter(loadOnDemand: true, name: nameof(Program.ConvertDNA))]
        ///         public StringToDNA(IConverterCollection collection, bool storeRNA = false)
        ///         { 
        ///             ... 
        ///         }
        ///         ...
        ///     }
        ///     ...
        /// ```</code>
        /// </example>
        public bool LoadOnDemand { get; }

        /// <summary>
        /// Gets a value indicating whether the method is invoked with `this` set to null (`true`) or with an instance reference (`false`). Default is `true`.
        /// </summary>
        public bool IsStatic { get; }

        /// <summary>
        /// Gets the <see cref="Type.Name"/> or an empty string if no <see cref="Name"/> is set.
        /// </summary>
        /// <example> **Example:**  A Converter `StringToDNA` is assigned an alias for function lookup through <see cref="ObjectExtension.Transform{TBase, TOut}(object, object, string, bool, bool)"/>
        /// <code>
        /// ```cs
        ///     [Converter(name: nameof(StringToDNA))]
        ///     internal class StringToDNA
        ///     {   
        ///         ...
        ///         public StringToDNA(IConverterCollection collection,  storeRNA = false)
        ///         { 
        ///             ... 
        ///         }
        ///         ...
        ///     }
        ///     ...
        /// ```</code>
        /// </example>
        /// <remarks>The name is used for unique function lookup amongst disambiguates based on the input/output types</remarks>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the method should be wrapped and passed an instance of its containing class as the first argument (`true`). 
        /// Default is <see cref="false"/>, which does not wrap the attributed method.
        /// </summary>
        public bool PassInstance { get; }

        /// <summary>
        /// A string representation of the current attribute.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="string"/> representation of the fields in <see cref="ConverterMethodAttribute"/>.
        /// </returns>
        public override string ToString()
        {
            return $"[Sta:{this.IsStatic},LoD:{this.LoadOnDemand},BaTy:{this.BaseType},Name:{this.Name}]";
        }

    }
}