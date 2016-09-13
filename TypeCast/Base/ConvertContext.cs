// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.0.1.4                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
using System.Reflection;
using System.Linq;
using System;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Core.TypeCast.Base
{
    using System.Collections.Generic;
    using Extensions;


    /// <summary>
    /// Wraps the model value for the converter in a conversion-context
    /// </summary>
    public class ConvertContext<TIn, TOut> : ConvertContext
    {
        /// <summary>
        /// Creates a new instance of <see cref="ConvertContext{TIn, TOut}"/>
        /// </summary>
        /// <param name="modelValue">The `model` value that was passed as second parameter to the conversion function </param>
        public ConvertContext(object modelValue)
            : base(modelValue)
        {
            this.From = typeof(TIn);
            this.To = typeof(TOut);
        }
    }

    /// <summary>
    /// Wraps the model value for the converter in a conversion-context.
    /// </summary>
    /// <remarks>On rare occasions, a contextual data structure may be required which provides meta-data about the converting-process.</remarks>
    public class ConvertContext : IConvertContext
    {
        /// <summary>
        /// Creates a new instance of <see cref="ConvertContext"/>
        /// </summary>
        /// <param name="modelValue">The `model` value that was passed as second parameter to the conversion function </param>
        public ConvertContext(object modelValue)
        {
            this.Value = modelValue;
        }

        /// <summary>
        /// The source <see cref="Type"/> from which to convert to the <see cref="Type"/> of <see cref="To"/>
        /// </summary>
        public Type From { get; set; }

        /// <summary>
        /// The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="From"/> to
        /// </summary>
        public Type To { get; set; }

        /// <summary>
        /// The argument <see cref="Type"/> of the `model` as used in <see cref="ConvertTo{TIn, TOut}(TIn, object)"/>
        /// </summary>
        public Type Argument { get; set; }

        /// <summary>
        /// An optional default value for the given type, which must not be null, otherwise an <see cref="ConverterException" /> may be thrown.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// The <see cref="Converter"/> instance for the corresponding types <typeparamref name="TIn"/> and <typeparamref name="TOut"/>
        /// </summary>
        public Converter Converter { get; set; }

        /// <summary>
        /// The caller method name which is automatically filled-in via the <see cref="CallerMemberNameAttribute"/>, and used for context information.
        /// </summary>
        public string Caller { get; set; }

        /// <summary>
        /// Whether the original <paramref name="From"/> <see cref="Type"/> was nullable
        /// </summary>
        public bool? Nullable { get; set; }

        /// <summary>
        /// Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown
        /// </summary>
        public bool? ThrowExceptions { get; set; }

        /// <summary>
        /// The conversion method that was invoked
        /// </summary>
        public object Method { get; set; }

        /// <summary>
        /// The <see cref="MethodInfo"/> instance of the <see cref="Method"/>
        /// </summary>
        public MethodInfo MethodInfo { get; set; }
    }


}