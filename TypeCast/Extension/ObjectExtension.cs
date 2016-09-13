// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.0.1.4                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast
{
    using System;
    using System.Reflection;
    using System.Linq;

    using System.Runtime.CompilerServices;
    using System.Collections;
    using System.Collections.Generic;

    using Core.TypeCast.Base;
    using Core.Extensions;

    /// <summary>The object extension methods to convert an object of an unrestricted unknown type `TIn` to an unrestricted known type `TOut`.</summary>
    public static partial class ObjectExtension
    {
        /// <summary>A method wrapper to safely lookup the required converter instance for the conversion and intercept possible exceptions.
        /// The method returns <see cref="bool"/> `true` if the <paramref name="result"/> value is already determined, in case of <paramref name="self"/> 
        /// already having <see cref="Type"/> <typeparamref name="TOut"/>
        /// </summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="converter">The <see cref="Converter"/> instance for the corresponding types <typeparamref name="TIn"/> and <typeparamref name="TOut"/></param>
        /// <param name="result">  The converted result of Type <typeparamref name="TOut" />. </param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="unboxObjectType"> in case the type is boxed and TIn set to `object`, use <paramref name="typeIn" /> to override the unboxed source <see cref="Type" /></param>
        /// <param name="typeArgument">The argument <see cref="Type"/> of the `model` as used in <see cref="ConvertTo{TIn, TOut}(TIn, object)"/></param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="self"/> to</param>
        /// <param name="typeBase">The base-type <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="self"/> to</param>
        /// <param name="attributeName">A search-string to be contained in the <see cref="ConverterAttribute.Name"/> to filter through</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>The result state as <see cref="bool" /> indicating if the conversion is already finished (`true`) or still pending (`false`).</returns>
        /// <remarks>
        ///     Note that the <see cref="Type" /> `object` as `TOut` or set via `typeOut` is allowed although such a case is not sensible, except under special
        ///     circumstances, including for testing and debugging
        /// </remarks>
        private static bool GetConverterOrDefault<TIn, TOut>(
            TIn self,
            out Converter converter,
            out TOut result,
            Type typeArgument = null,
            Type typeTo = null,
            Type typeBase = null,
            bool throwException = false,
            bool unboxObjectType = true,
            string attributeName = null)
        {
            ConverterCollection.AutoInitialize();
            if(typeof(TIn) == typeof(object))
            {
                if(throwException == true && unboxObjectType == false)
                {
                    throw new ConverterException(ConverterCause.ConverterTypeInIsExplicitObject);
                }

                var typeFrom = unboxObjectType ? self.GetType() : typeof(object);
                if(typeof(TOut) != typeof(object))
                {
                    typeTo = typeTo ?? typeof(TOut);
                }
                converter = ConverterCollection.CurrentInstance.Get(typeFrom: typeFrom.GetTypeInfo(), typeTo: typeTo?.GetTypeInfo(),
                                    typeArgument: typeArgument?.GetTypeInfo(), typeBase: typeBase?.GetTypeInfo(), attributeName: attributeName, loadOnDemand: true);
            }
            else
            {
                converter = ConverterCollection.CurrentInstance.Get<TIn, TOut>(self, typeArgument: typeArgument, loadOnDemand: true);
            }

            if(self is TOut)
            {
                result = (TOut)(object)self;
                return true;
            }

            result = default(TOut);
            return false;
        }

        /// <summary>A method wrapper to safely invoke the converter functions and intercept possible exceptions, returning the conversion status as a <see cref="bool"/>.</summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="result">  The converted result of Type <typeparamref name="TOut" />. </param>
        /// <param name="defaultValue">An optional default value for the given type, which must not be null, otherwise an <see cref="ConverterException" /> may be thrown.</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="converter">The <see cref="Converter"/> instance for the corresponding types <typeparamref name="TIn"/> and <typeparamref name="TOut"/></param>
        /// <param name="contextInstance">>An optional context instance, providing current parameters of the conversion process and context.</param>
        /// <param name="caller">The caller method name which is automatically filled-in via the <see cref="CallerMemberNameAttribute"/>, and used for context information.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>The success state as <see cref="bool" /> indicating if the conversion succeeded (`true`) or failed (`false`).</returns>
        /// <exception cref="Exception">A generic exception that occurred during the invocation of the conversion functions</exception>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        private static bool InvokeConvert<TIn, TOut>(TIn self, out TOut result, object defaultValue, bool throwException, Converter converter, IConvertContext contextInstance = null, [CallerMemberName] string caller = null)
        {
            contextInstance = SetContext<TIn, TOut>(contextInstance, defaultValue, throwException, converter, caller);
            if(converter != null)
            {
                try
                {
                    result = (TOut)converter.Convert(self, contextInstance ?? defaultValue);
                    return true;
                }
                catch(Exception exc)
                {
                    if(throwException == true && exc is IException)
                    {
                        throw;
                    }
                    else
                    {
                        throw new ConverterException(ConverterCause.ConvertFailed, exc);
                    }
                }
            }

            result = default(TOut);
            return false;
        }

        /// <summary>
        /// Updates a ConvertContext instance with the parameters provided
        /// </summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="result">  The converted result of Type <typeparamref name="TOut" />. </param>
        /// <param name="defaultValue">an optional default value for the given type, which must not be null, otherwise an <see cref="ConverterException" /> may be thrown.</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="converter">The <see cref="Converter"/> instance for the corresponding types <typeparamref name="TIn"/> and <typeparamref name="TOut"/></param>
        /// <param name="contextInstance">>An optional context instance, providing current parameters of the conversion process and context.</param>
        /// <param name="caller">The caller method name which is automatically filled-in via the <see cref="CallerMemberNameAttribute"/>, and used for context information.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>An updated ConvertContext instance if one was passed in, else `null` </returns>
        private static IConvertContext SetContext<TIn, TOut>(IConvertContext contextInstance, object defaultValue, bool throwException, Converter converter, string caller)
        {
            if(contextInstance != null)
            {
                contextInstance = new ConvertContext<TIn, TOut>(defaultValue)
                {
                    Argument = defaultValue?.GetType(),
                    Converter = contextInstance.Converter ?? converter,
                    Caller = contextInstance.Caller ?? caller,
                    ThrowExceptions = contextInstance.ThrowExceptions ?? throwException,
                    Nullable = contextInstance.Nullable ?? false
                };
            }

            return contextInstance;
        }

        /// <summary>
        /// Checks if the value of an arbitrary <see cref="Type"/> can be converted to a given <see cref="Type"/> <paramref name="self"/>,
        /// in analogy to the virtual method <see cref="System.ComponentModel.TypeConverter.CanConvertFrom"/> 
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="target">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>Returns `true` if the own value is equal to the <see cref="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanConvertTo<TIn, TOut>(this TIn self, TOut target = default(TOut))
        {
            TOut result = default(TOut);
            Converter converter = null;
            GetConverterOrDefault<TIn, TOut>(self, out converter, out result);
            return converter != null;
        }

        /// <summary>
        /// Checks if the value of an arbitrary <see cref="Type"/> can be converted to a given <see cref="Type"/> <paramref name="self"/>,
        /// in analogy to the virtual method <see cref="System.ComponentModel.TypeConverter.CanConvertFrom"/> 
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="target">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TArg">The Argument <see cref="Type"/> for generic converters using see <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object)"/>. 
        /// <returns>Returns `true` if the own value is equal to the <see cref="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanConvertTo<TOut, TArg>(this object self, TOut target = default(TOut), TArg model = default(TArg))
        {
            TOut result = default(TOut);
            Converter converter = null;
            GetConverterOrDefault<object, TOut>(self, out converter, out result, typeArgument: typeof(TArg), unboxObjectType: true);
            return converter != null;
        }

        /// <summary>
        /// Checks if the value of an arbitrary <see cref="Type"/> can be converted to a given <see cref="Type"/> <paramref name="self"/>,
        /// in analogy to the virtual method <see cref="System.ComponentModel.TypeConverter.CanConvertFrom"/> 
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="self"/> to</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TArg">The Argument <see cref="Type"/> for generic converters using see <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object)"/>. 
        /// <returns>Returns `true` if the own value is equal to the <see cref="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanConvertTo<TIn, TArg>(this TIn self, Type typeTo, TArg model = default(TArg))
        {
            object result = null;
            Converter converter = null;
            GetConverterOrDefault<TIn, object>(self, out converter, out result, typeTo: typeTo, typeArgument: typeof(TArg));
            return converter != null;
        }

        /// <summary>
        /// Checks if the value of an arbitrary <see cref="Type"/> can be converted to a given <see cref="Type"/> <paramref name="self"/>,
        /// in analogy to the virtual method <see cref="System.ComponentModel.TypeConverter.CanConvertFrom"/> 
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="self"/> to</param>
        /// <param name="typeModel">A <see cref="Type"/> of the model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>Returns `true` if the own value is equal to the <see cref="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanConvertTo<TIn>(this TIn self, Type typeTo, Type typeModel)
        {
            object result = null;
            Converter converter = null;
            GetConverterOrDefault<TIn, object>(self, out converter, out result, typeTo: typeTo, typeArgument: typeModel?.GetType());
            return converter != null;
        }

        /// <summary>
        /// Checks if the value of an arbitrary <see cref="Type"/> can be converted to a given <see cref="Type"/> <paramref name="self"/>,
        /// in analogy to the virtual method <see cref="System.ComponentModel.TypeConverter.CanConvertFrom"/> 
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="self"/> to</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>Returns `true` if the own value is equal to the <see cref="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanConvertTo<TIn>(this TIn self, Type typeTo)
        {
            object result = null;
            Converter converter = null;
            GetConverterOrDefault<TIn, object>(self, out converter, out result, typeTo: typeTo, unboxObjectType: true);
            return converter != null;
        }

        /// <summary>
        /// Checks if the value of an arbitrary <see cref="Type"/> can be converted to a given <see cref="Type"/> <paramref name="self"/>,
        /// in analogy to the virtual method <see cref="System.ComponentModel.TypeConverter.CanConvertFrom"/> 
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>Returns `true` if the own value is equal to the <see cref="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanConvertTo<TOut>(this object self)
        {
            TOut result = default(TOut);
            Converter converter = null;
            GetConverterOrDefault<object, TOut>(self, out converter, out result, unboxObjectType: true);
            return converter != null;
        }

        /// <summary>
        /// Checks if the value of an arbitrary <see cref="Type"/> is equal to its <see cref="default"/>
        /// </summary>
        /// <typeparam name="TIn">An arbitrary <see cref="Type"/></typeparam>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <returns>Returns `true` if the own value is equal to the <see cref="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDefaultValue<TIn>(this TIn self)
        {
            return Object.Equals(self, default(TIn)) == true;
        }

        /// <summary>
        /// Checks if the value of an arbitrary <see cref="Type"/> <typeparamref name="TIn"/> is equal to its <see cref="default"/>
        /// </summary>
        /// <typeparam name="TIn">An arbitrary <see cref="Type"/></typeparam>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <returns>Returns `true` if the own value is equal to the <see cref="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDefaultValue<TIn>(this object self)
        {
            return Object.Equals(self, default(TIn)) == true;
        }
    }
}