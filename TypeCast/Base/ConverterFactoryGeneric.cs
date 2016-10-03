// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
using System;
using System.Reflection;

namespace Core.TypeCast.Base
{
    /// <summary>
    /// The abstract definition class for <see cref="ConverterFactory"/>
    /// </summary>
    /// <typeparam name="TInstance">The <see cref="Type"/> of the converter instances to create.</typeparam>
    public abstract class ConverterFactory<TInstance> : ConverterFactoryRT<TInstance>  where TInstance : class
    {
        /// <summary>
        /// Creates new <typeparamref name="TInstance"/> instances dependent on the source <typeparamref name="TIn"/> and target <see cref="Type"/> <typeparamref name="TOut"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to convert</typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to convert</typeparam>
        /// <param name="method">The converter method taking only one input parameter of <see cref="Type"/> <typeparamref name="TIn"/></param>
        /// <remarks>Only one method may be passed during instance creation, as compatible standard converters (<see cref="Converter.Standard"/>) are merged automatically.</remarks>
        /// <returns>Returns a new instance of <typeparamref name="TInstance"/> upon success</returns>
        public abstract TInstance Create<TIn, TOut>(Func<TIn, TOut> method);

        /// <summary>
        /// Creates new <typeparamref name="TInstance"/> instances dependent on the source <typeparamref name="TIn"/> and target <see cref="Type"/> <typeparamref name="TOut"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to convert</typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to convert</typeparam>
        /// <param name="method">The converter method taking two input parameters of <see cref="Type"/> <typeparamref name="TIn"/> and <typeparamref name="TOut"/></param>
        /// <remarks>Only one method may be passed during instance creation, as compatible standard converters (<see cref="Converter.Standard"/>) are merged automatically.</remarks>
        /// <returns>Returns a new instance of <typeparamref name="TInstance"/> upon success</returns>
        public abstract TInstance Create<TIn, TOut>(Func<TIn, TOut, TOut> method);
    }
}