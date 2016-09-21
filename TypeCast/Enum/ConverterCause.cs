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

    using Core.Extensions;
    using Core.TypeCast.Base;

    /// <summary>
    /// Contains the reasons for a <see cref="ConverterException"/> to be raised.
    /// </summary>
    public enum ConverterCause : ulong
    {
        /// <summary>
        /// Indicates the default or unspecified value
        /// </summary>
        [Description("Indicates the default or unspecified value")]
        None = 0, 

        /// <summary>
        /// Indicates an unknown or undefined flag
        /// </summary>
        [Description("Indicates the default or unspecified value")]
        Unknown = 1 << 0, 

        /// <summary>
        /// Indicates a default or normal flag
        /// </summary>
        [Description("Indicates the default or unspecified value")]
        Default = 1 << 1, 

        /// <summary>
        /// Indicates that the <see cref="Converter.Function"/> property is not set and `null`
        /// </summary>
        [Description("Indicates that the Converter function is not set and null")]
        ConverterFunctionNull = 1 << 2, 

        /// <summary>
        /// Indicates that the <see cref="Converter{TIn, TOut}"/> or an passed argument to a Converter method is `null`
        /// </summary>
        [Description("Indicates that the Converter or an passed argument to the converter is `null`")]
        ConverterArgumentNull = 1 << 3, 

        /// <summary>
        /// Indicates a generic input type <see cref="Converter.From"/> was passed, whilst being explicitly disallowed in the <see cref="ConverterCollectionSettings"/> instance
        /// </summary>
        [Description("Indicates a generic input type was passed, whilst being explicitly disallowed in the ConverterCollectionSettings instance")]
        ConverterArgumentGenericType = 1 << 4, 

        /// <summary>
        /// Indicates that a <see cref="Converter{TIn, TOut}"/> method got passed an argument of a <see cref="System.Type"/>`T` that does not match TIn or TOut respectively
        /// </summary>
        [Description("Indicates that a Converter method got passed an argument of a Type `T` that does not match TIn or TOut respectively")]
        ConverterArgumentWrongType = 1 << 5, 

        /// <summary>
        /// Indicates that the <see cref="Converter{TIn, TOut}"/>, defined by the Input and Output conversion Types `TIn --> TOut` already exists
        /// </summary>
        [Description("Indicates that the Converter, defined by the Input and Output conversion Types TIn --> TOut already exists")]
        ConverterExists = 1 << 6, 

        /// <summary>
        /// Indicates that the <see cref="Converter{TIn,TOut}.Convert(object,object)"/> method failed near or at the custom convert Function invocation 
        /// </summary>
        [Description("Indicates that the convert method failed near or at the custom convert Function invocation ")]
        ConvertFailed = 1 << 7, 

        /// <summary>
        /// Indicates that the custom converter function wrapper yielded an error
        /// </summary>
        /// <seealso cref="ConverterExtension.FunctionDefaultWrapper{TIn,TOut}(Core.TypeCast.Base.Converter)"/>
        [Description("Indicates that the custom converter function wrapper yielded an error")]
        ConverterWrapperError = 1 << 8, 

        /// <summary>
        /// Indicates an internal error state that may be specified further in the <see cref="Exception.InnerException"/> property
        /// </summary>
        /// <remarks>Can be raised when the Converter's constructor must be explicitly called with parameters</remarks>
        [Description("Indicates an internal error state that may be specified further in the InnerException property")]
        InternalError = 1 << 9, 

        /// <summary>
        /// Dependency Injection failed due to a `null` reference
        /// </summary>
        /// <remarks><para>Indicates that a conversion related class constructor was injected with an argument that is null</para></remarks>
        [Description("Indicates that a conversion related class constructor was injected with an argument that is null")]
        ConstructorInjectionNull = 1 << 10, 

        /// <summary>
        /// The <see cref="Converter"/> could not be added to the <see cref="ConverterCollection"/>
        /// </summary>
        /// <remarks>The <see cref="Converter"/> instance was not added to the <see cref="BlockingCollection{T}"/> <see cref="ConverterCollection.Items"/></remarks>
        /// <seealso cref="ConverterCollection.Add{TIn,TOut}(System.Func{TIn,TOut},System.Type,System.Threading.CancellationToken)"/>
        [Description("The converter could not be added to the ConverterCollection")]
        ConverterCollectionAddFailed = 1 << 11, 

        /// <summary>
        /// Indicates that the constructor of a parent <see cref="Converter{TIn, TOut}"/> class `T` requires parameters for proper instancing, yet was invoked parameter-less.
        /// </summary>
        /// <remarks>User-thrown exception in the custom parameter-less constructor of the logical Converter class</remarks>
        [Description("Indicates that the constructor of a conversion-related class requires parameters for proper instancing, yet was invoked parameterless.")]
        InstanceRequiresParameters = 1 << 12, 

        /// <summary>
        /// Indicates that a method in a converter or conversion-related class instance does not possess a logical implementation.
        /// </summary>
        /// <remarks>Make sure methods marked as virtual are overwritten.</remarks>
        [Description("Indicates that a method in a converter or conversion-related class instance does not possess a logical implementation.")]
        ConverterNotImplemented = 1 << 13,

        /// <summary>
        /// Indicates that the conversion source type is set to object, which is not sensible. Omit setting both target type parameters instead
        /// </summary>
        [Description("Indicates that the conversion source type is set to object, which is not sensible. Omit setting both target type parameters instead")]
        ConverterTypeInIsExplicitObject = 1 << 14,

        /// <summary>
        /// Indicates that passed converter function takes too many parameters. A maximum of two is permitted.
        /// </summary>
        [Description("Indicates that passed converter function takes too many parameters. A maximum of two is permitted.")]
        ConverterArgumentDelegateTooManyParameters = 1 << 15,

        /// <summary>
        /// Indicates that the passed converter function takes no parameters. At least one up two a maximum of two is required.
        /// </summary>
        [Description("Indicates that the passed converter function takes no parameters. At least one up two a maximum of two is required.")]
        ConverterArgumentDelegateNoParameters = 1 << 16,

        /// <summary>
        /// Indicates that the reflection of the loading assembly failed, likely due to changes or security fixes for newer portable library class releases
        /// </summary>
        [Description("Indicates that the reflection of the loading assembly failed, likely due to changes or security fixes for newer portable library class releases")]
        ConverterAutoInitializationFailed = 1 << 17,

        /// <summary>
        /// Indicates that the source and target types do not match. If they do not match use other functions instead, like CastTo and ConvertTo.
        /// </summary>
        [Description("Indicates that the source and target types do not match. If they do not match use other functions instead, like CastTo and ConvertTo.")]
        TransformRequiresEqualInOutTypes = 1 << 18,

        /// <summary>
        /// Indicates that the cast failed, likely due to a missing converter. These exceptions can be suppressed with a function argument.
        /// </summary>
        [Description("Indicates that the cast failed, likely due to a missing converter. These exceptions can be suppressed with a function argument.")]
        InvalidCast = 1 << 19,

        /// <summary>
        /// Indicates that the argument types of a given delegate do not match the return type and / or types of the provided parameters.
        /// </summary>
        [Description("Indicates that the argument types of a given delegate do not match the return type and / or types of the provided parameters.")]
        DelegateArgumentWrongType = 1 << 20,

        /// <summary>
        /// Indicates that the input argument was badly formatted.
        /// </summary>
        [Description("Indicates that the input argument was badly formatted.")]
        BadInputFormat = 1 << 21,

        /// <para>Flag Combinations</para>
        /// <para></para>
        /// <summary>
        /// Indicates a null-reference for an existing Converter default function
        /// </summary>
        [Description("Indicates a null-reference for an existing Converter default function")]
        ConverterFunctionDefaultNull = Default | ConverterFunctionNull, 
    }
}