// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast.Base
{
    using System;
    using System.Linq;
    using Core.TypeCast;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.Serialization;

    using Core.Extensions;

    /// <summary>
    /// Creates new instances of <see cref="Converter{TIn, TOut}"/> dependent on the source <see cref="Type"/> `TIn`  and target <see cref="Type"/> `TOut`
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public class ConverterFactory : ConverterFactory<Converter>
    {
        /// <summary>
        /// Creates new <see cref="Converter{TIn, TOut}"/> instances dependent  on the source <typeparamref name="TIn"/> and target <see cref="Type"/> <typeparamref name="TOut"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <param name="method">The converter method taking only one input parameter of <see cref="Type"/> <typeparamref name="TIn"/></param>
        /// <remarks>Only one method may be passed during instance creation, as compatible standard converters (<see cref="Converter.Standard"/>) are merged automatically.</remarks>
        /// <returns>Returns a new instance of <see cref="Converter{TIn, TOut}"/> upon success</returns>
        public override Converter Create<TIn, TOut>(Func<TIn, TOut> method)
        {
            var converter = new Converter<TIn, TOut>(method);
            converter.MergeConverterMethodAttributes(method.GetMethodInfo());

            return converter;
        }

        /// <summary>
        /// Creates new <see cref="Converter{TIn, TOut, TArg}"/> instances dependent on the source <typeparamref name="TIn"/> and <typeparamref name="TOut"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <param name="method">The converter method taking two input parameters of <see cref="Type"/> <typeparamref name="TIn"/> and <typeparamref name="TOut"/></param>
        /// <remarks>Only one method may be passed during instance creation, as compatible standard converters (<see cref="Converter.Standard"/>) are merged automatically.</remarks>
        /// <returns>Returns a new instance of <see cref="Converter{TIn, TOut, TArg}"/> upon success</returns>
        public override Converter Create<TIn, TOut>(Func<TIn, TOut, TOut> method)
        {
            var converter = new Converter<TIn, TOut>(method);
            converter.MergeConverterMethodAttributes(method.GetMethodInfo());

            return converter;
        }

        /// <summary>
        /// Create a converter-wrapper which passes the own class-instance of a given method <paramref name="declaredMethod"/> as first argument for methods which are attributed 
        /// by <see cref="ConverterMethodAttribute"/> yet do not have any arguments in their function declaration or have <see cref="ConverterMethodAttribute.PassInstance"/> set to <see langword="true"/>
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the class which contains the method <paramref name="declaredMethod"/></param>
        /// <param name="declaredMethod">The method declared to be bound to the new <see cref="Converter"/> instance.</param>
        /// <returns>A new <see cref="Converter"/> instance with the bound method of <paramref name="declaredMethod"/>.</returns>
        public Converter CreateWrapper(TypeInfo type, MethodInfo declaredMethod)
        {
            MethodInfo makeConverter;
            var declaredMethodParameters = declaredMethod.GetParameters();

            if (declaredMethodParameters.Length == 0)
            {
                makeConverter = typeof(ConverterFactory).GetTypeInfo()
                                .GetDeclaredMethod(nameof(ConverterFactory.ConverterWrapper))
                                .MakeGenericMethod(type.AsType(), declaredMethod.ReturnParameter.ParameterType);
            }
            else if (declaredMethodParameters.Length == 1)
            {
                // The first argument is already of the containing class, e.g. static implicit or explicit operators:
                if (declaredMethodParameters.First().ParameterType.GetTypeInfo() == type)
                {
                    makeConverter = typeof(ConverterFactory).GetTypeInfo()
                                    .GetDeclaredMethod(nameof(ConverterFactory.ConverterWrapperSelf))
                                    .MakeGenericMethod(type.AsType(), declaredMethod.ReturnParameter.ParameterType);
                }

                // The method requires an existing instance of its declaring class. Let's wrap it to invoke it with an instance:
                else
                {
                    makeConverter = typeof(ConverterFactory).GetTypeInfo()
                                    .GetDeclaredMethod(nameof(ConverterFactory.ConverterWrapperAny))
                                    .MakeGenericMethod(type.AsType(), declaredMethodParameters.FirstOrDefault().ParameterType, declaredMethod.ReturnParameter.ParameterType);
                }

            }

            else
            {
                throw new ConverterException(ConverterCause.ConverterArgumentDelegateTooManyParameters);
            }
            var args = new object[] { declaredMethod };

            var converter = (Converter)makeConverter.Invoke(this, args);

            converter.MergeConverterMethodAttributes(declaredMethod);

            // merge any hereto declared ConverterMethodAttribute
            if (declaredMethodParameters.Length == 0)
            {
                converter.FunctionAttribute = declaredMethod.GetCustomAttribute<ConverterMethodAttribute>();
            }
            else if(declaredMethodParameters.Length == 1)
            {
                converter.FunctionDefaultAttribute = declaredMethod.GetCustomAttribute<ConverterMethodAttribute>();
            }

            return converter;
        }

        /// <summary>
        /// Wraps an argument-less function to be invoked as an instance method and creates a new <see cref="Converter"/> instance
        /// </summary>
        /// <typeparam name="TClass">The <see cref="Type"/> of the class instance which invokes the method</typeparam>
        /// <typeparam name="TOut">The <see cref="Type"/> of the function return argument</typeparam>
        /// <param name="declaredMethod">The method declared to be bound to the new <see cref="Converter"/> instance.</param>
        /// <returns>Returns a new <see cref="Converter"/> instance bound to an function-wrapper taking an instance method of <see cref="Type"/> <typeparamref name="TClass"/> as first argument.</returns>
        public Converter ConverterWrapper<TClass, TOut>(MethodInfo declaredMethod)
        {
            return this.Create<TClass, TOut>(InvocationWrapper<TClass, TOut>(declaredMethod));
        }

        /// <summary>
        /// Wraps a function with one argument for self-invocation, wherein the argument is already of the containing class, e.g. static implicit or explicit operator methods
        /// </summary>
        /// <typeparam name="TClass">The <see cref="Type"/> of the class instance which invokes the method</typeparam>
        /// <typeparam name="TOut">The <see cref="Type"/> of the function return argument</typeparam>
        /// <param name="declaredMethod">The method declared to be bound to the new <see cref="Converter"/> instance.</param>
        /// <returns>Returns a new <see cref="Converter"/> instance bound to an function-wrapper taking an instance method of <see cref="Type"/> <typeparamref name="TClass"/> as first argument.</returns>
        public Converter ConverterWrapperSelf<TClass, TOut>(MethodInfo declaredMethod)
        {
            return this.Create<TClass, TOut>(InvocationWrapperSelf<TClass, TOut>(declaredMethod));
        }

        /// <summary>
        /// Wraps an argument-less function to be invoked as an instance method and creates a new <see cref="Converter"/> instance
        /// </summary>
        /// <typeparam name="TClass">The <see cref="Type"/> of the class instance which invokes the method</typeparam>
        /// <typeparam name="TArg">The <see cref="Type"/> of the function argument</typeparam>
        /// <typeparam name="TOut">The <see cref="Type"/> of the function return argument</typeparam>
        /// <param name="declaredMethod">The method declared to be bound to the new <see cref="Converter"/> instance.</param>
        /// <returns>Returns a new <see cref="Converter"/> instance bound to an function-wrapper taking an instance method of <see cref="Type"/> <typeparamref name="TClass"/> as first argument,
        ///  and an secondly any arbitrary parameter of <see cref="Type"/> <typeparamref name="TArg"/>.</returns>
        public static Converter ConverterWrapperAny<TClass, TArg, TOut>(MethodInfo declaredMethod)
        {
            return new Converter<TClass, TOut, TArg>(InvocationWrapperAny<TClass, TArg, TOut>(declaredMethod));
        }

        /// <summary>
        /// Wraps an argument-less function to be invoked as an instance method whose first argument is the instance of <see cref="Type"/> <typeparamref name="TClass"/>
        /// </summary>
        /// <typeparam name="TClass">The <see cref="Type"/> of the class instance which invokes the method</typeparam>
        /// <typeparam name="TOut">The <see cref="Type"/> of the function return argument</typeparam>
        /// <param name="declaredMethod">The method declared to be bound to the new <see cref="Converter"/> instance.</param>
        /// <returns>Returns a new <see cref="Func{TClass, TOut}"/> taking an instance method of <see cref="Type"/> <typeparamref name="TClass"/> as first argument.</returns>
        private static Func<TClass, TOut> InvocationWrapper<TClass, TOut>(MethodInfo declaredMethod)
        {
            return (instance) => (TOut)declaredMethod.Invoke(instance, null);
        }

        /// <summary>
        /// Wraps a function, taking its own containing class-instance as argument, and invokes the method in this instance whilst passing it as the first argument
        /// </summary>
        /// <typeparam name="TClass">The <see cref="Type"/> of the class instance which invokes the method</typeparam>
        /// <typeparam name="TOut">The <see cref="Type"/> of the function return argument</typeparam>
        /// <param name="declaredMethod">The method declared to be bound to the new <see cref="Converter"/> instance.</param>
        /// <returns>Returns a new <see cref="Func{TClass, TOut}"/> taking an instance method of <see cref="Type"/> <typeparamref name="TClass"/> as first argument.</returns>
        private static Func<TClass, TOut> InvocationWrapperSelf<TClass, TOut>(MethodInfo declaredMethod)
        {
            return (instance) => (TOut)declaredMethod.Invoke(instance, new object[] { instance });
        }

        /// <summary>
        /// Wraps an argument-less function to be invoked as an instance method whose first argument is the instance of <see cref="Type"/> <typeparamref name="TClass"/>
        /// </summary>
        /// <typeparam name="TClass">The <see cref="Type"/> of the class instance which invokes the method</typeparam>
        /// <typeparam name="TArg">The <see cref="Type"/> of the function argument</typeparam>
        /// <typeparam name="TOut">The <see cref="Type"/> of the function return argument</typeparam>
        /// <param name="declaredMethod">The method declared to be bound to the new <see cref="Converter"/> instance.</param>
        /// <returns>Returns a new <see cref="Func{TClass, TArg, TOut}"/> taking an instance method of <see cref="Type"/> <typeparamref name="TClass"/> as first argument, 
        /// and an secondly any arbitrary parameter of <see cref="Type"/> <typeparamref name="TArg"/>.</returns>
        private static Func<TClass, TArg, TOut> InvocationWrapperAny<TClass, TArg, TOut>(MethodInfo declaredMethod)
        {
            return (instance, parameter) => (TOut)declaredMethod.Invoke(instance, new object[] { parameter });
        }
    }
}