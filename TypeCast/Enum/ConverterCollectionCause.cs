// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast
{
    using System;

    using Core.Extensions;
    using Core.TypeCast.Base;

    /// <summary>
    /// Contains the reasons for a <see cref="ConverterCollectionException"/> to be raised.
    /// </summary>
    public enum ConverterCollectionCause : ulong
    {
        /// <summary>
        /// Indicates the default or unspecified value.
        /// </summary>
        [Description("Indicates the default or unspecified value.")]
        None = 0, 

        /// <summary>
        /// Indicates an unknown or undefined flag.
        /// </summary>
        [Description("Indicates the default or unspecified value.")]
        Unknown = 1 << 1, 

        /// <summary>
        /// Indicates a default or normal flag.
        /// </summary>
        [Description("Indicates the default or unspecified value.")]
        Default = 1 << 2, 

        /// <summary>
        /// Indicates an internal error state that may be specified further in the <see cref="Exception.InnerException"/> property.
        /// </summary>
        /// <remarks>Can be raised when the Converter's constructor must be explicitly called with parameters.</remarks>
        [Description("Indicates an internal error state that may be specified further in the InnerException property.")]
        InternalError = 1 << 3, 

        /// <summary>
        /// The converter could not be added to the <see cref="ConverterCollection"/>.
        /// </summary>
        /// <seealso cref="ConverterCollection.Add{TIn,TOut}(System.Func{TIn,TOut},System.Type,System.Threading.CancellationToken)"/>
        [Description("The converter could not be added to the ConverterCollection.")]
        AddFailed = 1 << 4,

        /// <summary>
        /// Indicates a null reference for the collection argument.
        /// </summary>
        [Description("Indicates a null reference for the collection argument.")]
        CollectionIsNull = 1 << 5, 

        /// <summary>
        /// Indicates that the <see cref="Converter{TIn, TOut}"/>, defined by the Input and Output conversion Types `TIn --> TOut` already exists.
        /// </summary>
        [Description("Indicates that the Converter, defined by the Input and Output conversion Types TIn --> TOut already exists.")]
        ConverterExists = 1 << 6,

        /// <summary>
        /// Indicates that the <see cref="Converter{TIn, TOut}"/> or an passed argument to a Converter method is `null`.
        /// </summary>
        [Description("Indicates that the Converter or an passed argument to the converter is `null`.")]
        ConverterArgumentNull = 1 << 7,

        /// <summary>
        /// Indicates that a <see cref="ConverterCollection"/> method got passed an argument of a <see cref="System.Type"/>`T` that does not match TIn or TOut respectively.
        /// </summary>
        [Description("Indicates that a ConverterCollection method got passed an argument of a Type `T` that does not match TIn or TOut respectively.")]
        ConverterArgumentWrongType = 1 << 8, 

        /// <summary>
        /// The custom converter / conversion-related class has already been instantiated and added to the to the <see cref="ConverterCollection"/> 
        /// </summary>
        [Description("The custom converter / conversion-related class has already been instantiated and added to the to the ConverterCollection.")]
        ConverterClassExists = 1 << 9,

        /// <summary>
        /// Indicates that the constructor the <see cref="ConverterCollection"/> class requires parameters for proper instancing, yet was invoked without parameters.
        /// </summary>
        /// <remarks>User-thrown exception in the custom parameter-less constructor of the logical Converter class</remarks>
        [Description(
            "Indicates that the constructor the ConverterCollection class requires parameters for proper instancing, yet was invoked without parameters.")]
        InstanceRequiresParameters = 1 << 10,

        /// <summary>
        /// Indicates that the <see cref="Converter.Function"/> property and <see cref="Converter.FunctionDefault"/> is not set to a <see cref="Delegate"/> and `null`.
        /// </summary>
        [Description("Indicates that the Converter function and default-value function is not set to a Delegate and null.")]
        ConverterFunctionsNull = 1 << 11,

        /// <summary>
        /// Indicates that the designated assembly file could not be found, or that the referenced assembly derived from the type is inaccessible to the library.
        /// </summary>
        [Description("Indicates that the designated assembly file could not be found, or that the referenced assembly derived from the type is inaccessible to the library.")]
        AssemblyFileNotFound = 1 << 12,

        /// <para>Flag Combinations</para>
        /// <para></para>
        /// <summary>
        /// The converter could not be added to the <see cref="ConverterCollection"/> because another converter for the specified input/output types already exists.
        /// </summary>
        [Description("Indicates a Converter already exists for the specified input and output Types and was thus not added to the ConverterCollection.")]
        AddFailedConverterExists = AddFailed | ConverterExists,
    }
}