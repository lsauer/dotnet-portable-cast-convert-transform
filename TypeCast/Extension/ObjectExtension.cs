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
    using System.Linq;

    using System.Runtime.CompilerServices;
    using System.Collections;
    using System.Collections.Generic;

    using Core.TypeCast.Base;
    using Core.Extensions;

    /// <summary>The object extension methods to convert an object of an unrestricted unknown type `TIn` to an unrestricted known type `TOut`.</summary>
    public static partial class ObjectExtension
    {
        private static Type objectType = typeof(object);
        private static TypeInfo objectTypeInfo = typeof(object).GetTypeInfo();

        /// <summary>A method wrapper to safely lookup the required converter instance for the conversion and intercept possible exceptions.
        /// The method returns <see langword="bool"/> `true` if the <paramref name="result"/> value is already determined, in case of <paramref name="self"/> 
        /// already having <see cref="Type"/> <typeparamref name="TOut"/>
        /// </summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="converter">The <see cref="Converter"/> instance for the corresponding types <typeparamref name="TIn"/> and <typeparamref name="TOut"/></param>
        /// <param name="result">  The converted result of Type <typeparamref name="TOut" />. </param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="unboxObjectType"> in case the type is boxed and TIn set to `object`, use <typeparamref name="TIn" /> to override the unboxed source <see cref="Type" /></param>
        /// <param name="typeArgument">The argument <see cref="Type"/> of the `model` as used in <see cref="ConvertTo{TIn, TOut}(TIn, object, bool)"/></param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <paramref name="self"/> to</param>
        /// <param name="typeBase">The base-type <see cref="Type"/> to which to convert the <see cref="Type"/> of <paramref name="self"/> to</param>
        /// <param name="attributeName">A search-string to be contained in the <see cref="ConverterAttribute.Name"/> to filter through</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. </param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter.Convert(object,object)" /></typeparam>
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
            string attributeName = null,
            bool withContext = false)
        {
            ConverterCollection.AutoInitialize();
            if(typeof(TIn) == objectType || typeof(TOut) == objectType)
            {
                if(throwException == true && unboxObjectType == false && ConverterCollection.CurrentInstance.Settings.AllowGenericTypes == false)
                {
                    throw new ConverterException(ConverterCause.ConverterTypeInIsExplicitObject);
                }

                var typeFrom = unboxObjectType ? self.GetType() : objectType;
                if(typeTo == null || typeof(TOut) != objectType)
                {
                    typeTo = typeTo ?? typeof(TOut);
                }
                converter = ConverterCollection.CurrentInstance.Get(typeFrom: typeFrom.GetTypeInfo(), typeTo: typeTo?.GetTypeInfo(), typeArgument: typeArgument?.GetTypeInfo(), 
                                        typeBase: typeBase?.GetTypeInfo(), attributeName: attributeName, loadOnDemand: true, assignable: false, withContext: withContext);
                // Retry by widening the search to type-inheritance
                if(converter == null)
                {
                    converter = ConverterCollection.CurrentInstance.Get(typeFrom: typeFrom.GetTypeInfo(), typeTo: typeTo?.GetTypeInfo(), typeArgument: typeArgument?.GetTypeInfo(),
                                        typeBase: typeBase?.GetTypeInfo(), attributeName: attributeName, loadOnDemand: false, assignable: true, withContext: withContext);
                }
                // no specific converter found, lets find a generic fallback converter
                if(converter == null && unboxObjectType == true && typeof(TIn) == objectType)
                {
                    converter = ConverterCollection.CurrentInstance.Get(typeFrom: objectTypeInfo, typeTo: typeTo?.GetTypeInfo(), typeArgument: typeArgument?.GetTypeInfo(),
                                        typeBase: typeBase?.GetTypeInfo(), attributeName: attributeName, loadOnDemand: false, assignable: false, withContext: withContext);
                }
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

        /// <summary>A method wrapper to safely invoke the converter functions and intercept possible exceptions, returning the conversion status as a <see langword="bool"/>.</summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="result">  The converted result of Type <typeparamref name="TOut" />. </param>
        /// <param name="defaultValue">An optional default value for the given type, which must not be null, otherwise an <see cref="ConverterException" /> may be thrown.</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="converter">The <see cref="Converter"/> instance for the corresponding types <typeparamref name="TIn"/> and <typeparamref name="TOut"/></param>
        /// <param name="contextInstance">>An optional context instance, providing current parameters of the conversion process and context.</param>
        /// <param name="caller">The caller method name which is automatically filled-in via the <see cref="CallerMemberNameAttribute"/>, and used for context information.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <returns>The success state as <see cref="bool" /> indicating if the conversion succeeded (`true`) or failed (`false`).</returns>
        /// <exception cref="Exception">A generic exception that occurred during the invocation of the conversion functions</exception>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        private static bool InvokeConvert<TIn, TOut>(TIn self, out TOut result, object defaultValue, bool throwException, Converter converter, IConvertContext contextInstance = null, [CallerMemberName] string caller = null)
        {
            if(defaultValue is IConvertContext)
            {
                contextInstance = SetContext<TIn, TOut>(defaultValue as IConvertContext, ((IConvertContext)defaultValue).Value, throwException, converter, caller);
            }
            else
            {
                contextInstance = SetContext<TIn, TOut>(contextInstance, defaultValue, throwException, converter, caller);
            }

            if(converter != null)
            {
                try
                {
                    result = (TOut)converter.Convert(self, contextInstance ?? defaultValue);
                    return IsDefaultValue<TOut>(result) == false;
                }
                catch(Exception exc)
                {
                    if(exc is IException)
                    {
                        if(throwException == true)
                        {
                            throw;
                        }
                    }
                    else if(exc is System.FormatException)
                    {
                        if(throwException == true)
                        {
                            throw new ConverterException(ConverterCause.BadInputFormat, exc);
                        }
                    }
                    else if(exc is System.OverflowException || exc is System.InvalidOperationException || exc is System.ArithmeticException)
                    {
                        if(throwException == true)
                        {
                            throw new ConverterException(ConverterCause.LogicError, exc);
                        }
                    }
                    else
                    {
                        throw new ConverterException(ConverterCause.ConvertFailed, exc);
                    }
                }
            }

            result = default(TOut);

            if(ConverterCollection.CurrentInstance.Settings.AllowDynamicType == true)
            {
                // lets use the internal CLR reflection logic via `dynamic` to invoke a Type's dynamic implicit cast method if available
                try
                {
                    dynamic tmp = self;
                    result = (TOut)tmp;
                    return true;
                }
                // don't handle exceptions any further at this point
                catch(Exception) { }
            }
            return false;
        }

        /// <summary>
        /// Updates a ConvertContext instance with the parameters provided
        /// </summary>
        /// <param name="contextInstance">The current context instance, providing current parameters of the conversion process and context.</param>
        /// <param name="defaultValue">an optional default value for the given type, which must not be null, otherwise an <see cref="ConverterException" /> may be thrown.</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="converter">The <see cref="Converter"/> instance for the corresponding types <typeparamref name="TIn"/> and <typeparamref name="TOut"/></param>
        /// <param name="caller">The caller method name which is automatically filled-in via the <see cref="CallerMemberNameAttribute"/>, and used for context information.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter.Convert(object,object)" /></typeparam>
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
        /// in analogy to the virtual method ' System.ComponentModel.TypeConverter.CanConvertFrom`
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="target">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <returns>Returns `true` if the own value is equal to the <see langword="default"/>, else `false` is returned.</returns>
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
        /// in analogy to the virtual method `System.ComponentModel.TypeConverter.CanConvertFrom`
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="target">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process</param>
        /// <typeparamref name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter.Convert(object,object)" /></typeparamref>
        /// <typeparamref name="TArg">The Argument <see cref="Type"/> for generic converters using see <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object, bool)"/>. </typeparamref>
        /// <returns>Returns `true` if the own value is equal to the <see langword="default"/>, else `false` is returned.</returns>
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
        /// in analogy to the virtual method `System.ComponentModel.TypeConverter.CanConvertFrom`
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <paramref name="self"/> to</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <typeparam name="TArg">The Argument <see cref="Type"/> for generic converters using see <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object, bool)"/>. </typeparam>
        /// <returns>Returns `true` if the own value is equal to the <see langword="default"/>, else `false` is returned.</returns>
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
        /// in analogy to the virtual method `System.ComponentModel.TypeConverter.CanConvertFrom`
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <paramref name="self"/> to</param>
        /// <param name="typeModel">A <see cref="Type"/> of the model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <returns>Returns `true` if the own value is equal to the <see langword="default"/>, else `false` is returned.</returns>
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
        /// in analogy to the virtual method `System.ComponentModel.TypeConverter.CanConvertFrom`
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <paramref name="self"/> to</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <returns>Returns `true` if the own value is equal to the <see langword="default"/>, else `false` is returned.</returns>
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
        /// in analogy to the virtual method `System.ComponentModel.TypeConverter.CanConvertFrom` 
        /// </summary>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <returns>Returns `true` if the own value is equal to the <see langword="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanConvertTo<TOut>(this object self)
        {
            TOut result = default(TOut);
            Converter converter = null;
            GetConverterOrDefault<object, TOut>(self, out converter, out result, unboxObjectType: true);
            return converter != null;
        }

        /// <summary>
        /// Checks if the value of an arbitrary <see cref="Type"/> is equal to its <see langword="default"/>
        /// </summary>
        /// <typeparam name="TIn">An arbitrary <see cref="Type"/></typeparam>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <returns>Returns `true` if the own value is equal to the <see langword="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDefaultValue<TIn>(this TIn self)
        {
            return Object.Equals(self, default(TIn)) == true;
        }

        /// <summary>
        /// Checks if the value of an arbitrary <see cref="Type"/> <typeparamref name="TIn"/> is equal to its <see langword="default"/>
        /// </summary>
        /// <typeparam name="TIn">An arbitrary <see cref="Type"/></typeparam>
        /// <param name="self">The own instance of any unrestricted <see cref="Type"/>, which invokes the static extension method.</param>
        /// <returns>Returns `true` if the own value is equal to the <see langword="default"/>, else `false` is returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDefaultValue<TIn>(this object self)
        {
            return Object.Equals(self, default(TIn)) == true;
        }
    }
}