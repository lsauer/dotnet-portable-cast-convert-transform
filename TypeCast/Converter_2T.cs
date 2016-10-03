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
    using System.Reflection.Emit;
    using System.Runtime.Serialization;

    using Core.TypeCast.Base;

    /// <summary>
    /// The generic, strictly-typed converter class, for type casting and simple conversions.
    /// </summary>
    /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
    /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
    /// <remarks>the class is declared sealed as the only converter implementation must be herein and no further implementation must be allowed, 
    /// to enforce decoupling between arbitrary converter logic with a guaranteed implementation of the underlying converter-container in question</remarks>
    public sealed class Converter<TIn, TOut> : Converter<TIn, TOut, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Converter{TIn, TOut}"/> class.
        /// </summary>
        /// <param name="converter"> The converter function. <seealso cref="Converter.Function"/></param>
        /// <exception cref="ConverterException">Throws a <see cref="ConverterException"/> caused by <see cref="ConverterCause.ConverterArgumentNull"/>
        /// </exception>
        public Converter(Func<TIn, TOut> converter)
            : base(converter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Converter{TIn, TOut}"/> class.
        /// </summary>
        /// <param name="converterDefault"> The converter default function which takes a default-value parameter as the second argument  <seealso cref="Converter.FunctionDefault"/>
        ///  wherein <typeparamref name="TIn"/> is the source / from <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/>, and 
        /// <typeparamref name="TOut"/> is the target / to <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/>
        /// </param>
        /// <param name="argument">The <seealso cref="Type"/> of the argument passed to <see cref="Converter.FunctionDefault"/></param>
        /// <exception cref="ConverterException">Throws a <see cref="ConverterException"/> caused by <see cref="ConverterCause.ConverterArgumentNull"/>
        /// </exception>
        public Converter(Func<TIn, TOut, TOut> converterDefault)
            : base(converterDefault)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Converter{TIn, TOut}"/> class.
        /// </summary>
        /// <param name="converterDefaultAnyType"> The converter default function which takes a default-value parameter as the second argument  <seealso cref="Converter.FunctionDefault"/>
        ///  wherein <typeparamref name="TIn"/> is the source / from <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/>, and 
        /// <typeparamref name="TOut"/> is the target / to <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/>
        /// </param>
        /// <exception cref="ConverterException">Throws a <see cref="ConverterException"/> caused by <see cref="ConverterCause.ConverterArgumentNull"/>
        /// </exception>
        public Converter(Func<TIn, object, TOut> converterDefaultAnyType)
            : base(converterDefaultAnyType)
        {
            //this.Argument = argument ?? typeof(object);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Converter{TIn, TOut}"/> class.
        /// </summary>
        /// <param name="converterDefault"> The converter default function which takes a default-value parameter as the second argument  <seealso cref="Converter.FunctionDefault"/>
        ///  wherein <typeparamref name="TIn"/> is the source / from <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/>, and 
        /// <typeparamref name="TOut"/> is the target / to <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/>
        /// </param>
        /// <param name="converterInfo">A <see cref="MethodInfo"/> instance for the conversion function</param>
        /// <param name="argument">The <seealso cref="Type"/> of the argument passed to <see cref="Converter.FunctionDefault"/></param>
        /// <exception cref="ConverterException">Throws a <see cref="ConverterException"/> caused by <see cref="ConverterCause.ConverterArgumentNull"/>
        /// </exception>
        public Converter(MethodInfo converterInfo, Type argument)
            : base(converterInfo, argument)
        {
        }
    }
}