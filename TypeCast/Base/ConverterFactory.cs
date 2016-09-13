// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.0.1.4                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast.Base
{
    using System;
    using Core.TypeCast;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Creates new instances of <see cref="Converter{TIn, TOut}"/> dependent on the source <typeparamref name="TIn"/> and target <see cref="Type"/> <typeparamref name="TOut"/>
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public class ConverterFactory : ConverterFactory<Converter>
    {
        /// <summary>
        /// Creates new <see cref="Converter{TIn, TOut}"/> instances dependent  on the source <typeparamref name="TIn"/> and target <see cref="Type"/> <typeparamref name="TOut"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <param name="method">The converter method taking only one input parameter of <see cref="Type"/> <typeparamref name="TIn"/></param>
        /// <remarks>Only one method may be passed during instance creation, as compatible standard converters (<see cref="Converter.Standard"/>) are merged automatically.</remarks>
        /// <returns>Returns a new instance of <see cref="Converter{TIn, TOut}"/> upon success</returns>
        public override Converter Create<TIn, TOut>(Func<TIn, TOut> method)
        {
            var converter = new Converter<TIn, TOut>(method);

            return converter;
        }

        /// <summary>
        /// Creates new <see cref="Converter{TIn, TOut, TArg}"/> instances dependent on the source <typeparamref name="TIn"/> and <typeparamref name="TOut"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <param name="method">The converter method taking two input parameters of <see cref="Type"/> <typeparamref name="TIn"/> and <typeparamref name="TOut"/></param>
        /// <remarks>Only one method may be passed during instance creation, as compatible standard converters (<see cref="Converter.Standard"/>) are merged automatically.</remarks>
        /// <returns>Returns a new instance of <see cref="Converter{TIn, TOut, TArg}"/> upon success</returns>
        public override Converter Create<TIn, TOut>(Func<TIn, TOut, TOut> method)
        {
            var converter = new Converter<TIn, TOut>(method);

            return converter;
        }
    }
}