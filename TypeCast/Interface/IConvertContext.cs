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
    using System.Reflection;
    using Base;

    /// <summary>
    /// The common <see cref="IConvertContext"/> interface, for providing metadata of the conversion process .
    /// </summary>
    public interface IConvertContext
    {
        /// <summary>
        /// The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" />
        /// </summary>
        Type From { get; }

        /// <summary>
        /// The Target / To- <see cref="Type" /> to which to <see cref="Converter.Convert(object,object)" />
        /// </summary>
        Type To { get; }

        /// <summary>
        /// The <see cref="Type"/> of the second parameter, which is either a model-value or default-value
        /// </summary>
        Type Argument { get; }

        /// <summary>
        /// The conversion value
        /// </summary>
        object Value { get; }

        /// <summary>
        /// The <see cref="Converter"/> instance for the corresponding types <typeparamref name="TIn"/> and <typeparamref name="TOut"/>
        /// </summary>
        Converter Converter { get; }

        /// <summary>
        /// The caller method name which is automatically filled-in via the <see cref="CallerMemberNameAttribute"/>, and used for context information.
        /// </summary>
        string Caller { get; }

        /// <summary>
        /// Whether the original input value was a <see cref="Nullable{T}"/>
        /// </summary>
        bool? Nullable { get; }

        /// <summary>
        /// Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown.
        /// </summary>
        bool? ThrowExceptions { get; }

        /// <summary>
        /// The invoked conversion method
        /// </summary>
        object Method { get; }

        /// <summary>
        /// The <see cref="MethodInfo"/> of the conversion <see cref="Method"/>
        /// </summary>
        MethodInfo MethodInfo { get; }
    }
}