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
    /// Creates new instances of <see cref="Converter{TIn, TOut}"/> or <see cref="Converter{TIn, TOut, TArg}"/> dependent on the 
    /// number of parameters defined in <paramref name="methodInfo"/> and whether a delegate was passed.
    /// </summary>
    /// <typeparam name="TConverter">The generic type of the Converter class to instance</typeparam>
    public class ConverterFactoryRT<TConverter> : Factory<Converter, MethodInfo, object> where TConverter : class
    {
        public override Converter Create(MethodInfo method)
        {
            return this.Create(method, null);
        }

        /// <summary>
        ///     Creates a strictly typed <see cref="Converter{TIn, TOut, TArg}"/> container in case of any attribute-assigned method
        ///     or a passed <see cref="MethodInfo"/> with a parameter argument count of two. Otherwise a strictly typed <see cref="Converter{TIn, TOut}"/> container 
        ///     instance is created and returned.
        /// </summary>
        /// <param name="methodInfo">A methodInfo for a converter method, crucially confining the number of function parameters between one and two. 
        /// Otherwise an <see cref="ConverterException"/> is raised.</param>
        /// <param name="converterDelegate">An optional delegate of the converter method.</param>
        /// <returns>A new instance of a <see cref="Converter"/> with a parent of either <see cref="Converter{TIn, TOut, TArg}"/> or <see cref="Converter{TIn, TOut}"/>.</returns>
        /// <exception cref="ConverterException">If the number or types of the parameters mismatch an exception is raised.</exception>
        public override Converter Create(MethodInfo methodInfo, object converterDelegate = null)
        {
            if(methodInfo == null && converterDelegate == null)
            {
                throw new ConverterException(ConverterCause.ConverterArgumentNull);
            }

            var parameterInfos = methodInfo.GetParameters();

            if(parameterInfos.Length == 0)
            {
                throw new ConverterException(ConverterCause.ConverterArgumentDelegateNoParameters);
            }

            bool isInstanceMethod = converterDelegate == null;

            var parameterTypes = new ConverterParameters(methodInfo.ReturnParameter, parameterInfos);

            Converter converter;

            var methodAttribute = methodInfo.GetCustomAttribute(typeof(ConverterMethodAttribute)) as ConverterMethodAttribute;

            // create either a Converter_T2 or Converter_T3 container
            if(parameterInfos.Length == 2 || isInstanceMethod == true)
            {
                converter = Instantiate<Converter>(typeof(Converter<,,>), parameters: parameterTypes.ToArray(), args: new[] { converterDelegate ?? methodInfo, parameterTypes.Arg });

                converter.FunctionDefaultAttribute = methodAttribute;
            }
            else if(parameterInfos.Length == 1)
            {
                converter = Instantiate<Converter>(typeof(Converter<,>), parameters: new[] { parameterTypes.In, parameterTypes.Out }, args: converterDelegate);
                converter.FunctionAttribute = methodAttribute;
            }
            else
            {
                throw new ConverterException(ConverterCause.ConverterArgumentDelegateTooManyParameters);
            }

            return converter;
        }
    }
}