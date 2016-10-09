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

    using Core.TypeCast.Base;
    using Core.Extensions;

    /// <summary>The object extension methods to convert an object of an unrestricted unknown type `TIn` to an unrestricted known type `TOut`.</summary>
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     Converts from an arbitrary boxed <see cref="Type" /> <see cref="object" /> to a strict unrestricted type <typeparamref name="TOut" />
        /// </summary>
        /// <typeparam name="TOut">The target-<see cref="Type"/> of the <paramref name="self" /> to be converted</typeparam>
        /// <param name="self">The own instance which invokes the static extension method</param>
        /// <param name="defaultValue">A optional default value of the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <returns>Returns the converted value of <see cref="Type"/> <typeparamref name="TOut" /> </returns>
        /// <exception cref="ConverterException">Throws an exception of <see cref="Type"/> <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>Use <see cref="TryConvert{TOut}(object, out TOut, object, bool, bool)" /> instead if you do not want to raise any <see cref="ConverterException" /> 
        /// that might occur during the conversion process.</remarks>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// <seealso cref="TryCast{TIn, TOut}(TIn, out TOut, TOut, bool, bool, IConvertContext)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut CastTo<TOut>(this object self, TOut defaultValue = default(TOut))
        {
            TOut result;
            //self.TryCast(typeTo: typeTo, out result, defaultValue, throwException: false);

            self.TryCast<object, TOut>(out result, defaultValue, throwException: defaultValue?.IsDefaultValue() == true, unboxObjectType: true);
            return result;
        }

        /// <summary>
        ///     Converts from an arbitrary boxed <see cref="Type" /> <see cref="object" /> to a strict unrestricted type <typeparamref name="TOut" />
        /// </summary>
        /// <typeparam name="TOut">The target-<see cref="Type"/> of the <paramref name="self" /> to be converted</typeparam>
        /// <param name="self">The own instance which invokes the static extension method</param>
        /// <returns>Returns the converted value of <see cref="Type"/> <typeparamref name="TOut" /> </returns>
        /// <exception cref="ConverterException">Throws an exception of <see cref="Type"/> <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>Use <see cref="TryConvert{TOut}(object, out TOut, object, bool, bool)" /> instead if you do not want to raise any <see cref="ConverterException" /> 
        /// that might occur during the conversion process.</remarks>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// <seealso cref="TryCast{TIn, TOut}(TIn, out TOut, TOut, bool, bool, IConvertContext)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut CastTo<TOut>(this object self)
        {
            TOut result;
            self.TryCast<object, TOut>(out result, default(TOut), throwException: false, unboxObjectType: true);
            return result;
        }

        /// <summary>
        ///     Converts from an arbitrary <see cref="Nullable{T}"/> <see cref="Type" /> to a strict unrestricted type <typeparamref name="TOut" />
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <param name="self">The own instance which invokes the static extension method</param>
        /// <param name="defaultValue">A optional default value of the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. </param>
        /// <returns>Returns the converted value of <see cref="Type"/> <typeparamref name="TOut" /> </returns>
        /// <example>
        ///     <code>
        /// ```cs    
        ///     int? nullint = new Nullable&lt;int>(5);
        ///     var single = nullint.CastTo&lt;int, float>();
        /// ```
        /// </code>
        /// </example>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>Singled out and explicitly implemented for future reference.</remarks> 
        /// <remarks>Note, that the function is only invoked when <paramref name="self"/> is nullable whilst <typeparamref name="TIn"/> is declared not-nullable, 
        /// as per-the function declaration. This circumstance does not aid code readability.</remarks>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// <seealso cref="ObjectExtension.TryCast{TIn, TOut}(TIn, out TOut, TOut, bool, bool, IConvertContext)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut CastTo<TIn, TOut>(this TIn? self, TOut defaultValue = default(TOut), bool withContext = false) where TIn : struct
        {
            TOut result;
            self.TryCast<object, TOut>(out result, defaultValue: default(TOut), throwException: false, unboxObjectType: false, contextInstance: (withContext ? new ConvertContext(default(TOut)) { Nullable = true} : null));
            return result;
        }

        /// <summary>
        ///     Converts from an arbitrary <see cref="Nullable{T}"/> <see cref="Type" /> to a strict unrestricted type <typeparamref name="TOut" />
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <param name="self">The own instance which invokes the static extension method</param>
        /// <param name="defaultValue">A optional default value of the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <returns>Returns the converted value of <see cref="Type"/> <typeparamref name="TOut" /> </returns>
        /// <example>
        ///     <code>
        /// ```cs    
        ///       // Converter: Nullable&lt;int> --> float
        ///       var cc = ConverterCollection.CurrentInstance;
        ///       cc.Add((int? n) =>
        ///       {
        ///               return (float)(n ?? 0);
        ///       });
        ///       var nullSingle = new Nullable&lt;int>(5).CastTo&lt;int?, float>();
        /// ```
        /// </code>
        /// </example>
        /// <remarks>Singled out and explicitly implemented for future reference</remarks>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// see <see cref="TryCast{TIn, TOut}(TIn, out TOut, TOut, bool, bool, IConvertContext)"/>
        public static TOut CastTo<TIn, TOut>(this TIn self, TOut defaultValue = default(TOut))
        {
            TOut result;

            // check for generic type
            if(ConverterCollection.Initialized == true
                && ConverterCollection.CurrentInstance?.Settings.AllowGenericTypes == false
                && typeof(TIn).IsConstructedGenericType == true)
            {
                throw new ConverterException(ConverterCause.ConverterArgumentGenericType);
            }

            self.TryCast<TIn, TOut>(out result, defaultValue, throwException: true);
            return result;
        }

        /// <summary>
        ///     Converts from an arbitrary unrestricted <see cref="Type" /> to a strict unrestricted type <paramref name="typeTo"/> 
        /// </summary>
        /// <param name="self">The own instance which invokes the static extension method</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of  <paramref name="self"/> to</param>
        /// <param name="defaultValue">A optional default value of the target-<see cref="Type"/><paramref name="typeTo"/></param>
        /// <param name="unboxObjectType">Whether to determine the type from the parameters <paramref name="self"/>.</param>
        /// <returns>Returns the converted value of <see cref="Type"/> <paramref name="typeTo" /> </returns>
        /// <example>
        ///     <code>
        /// ```cs    
        ///       // Converter: Nullable&lt;int> --> float
        ///       var cc = ConverterCollection.CurrentInstance;
        ///       cc.Add((int? n) =>
        ///       {
        ///               return (float)(n ?? 0);
        ///       });
        ///       var nullSingle = new Nullable&lt;int>(5).CastTo(typeof(float));
        /// ```
        /// </code>
        /// </example>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>Use <see cref="TryConvert{TIn, TOut, TArg}(TIn, out TOut, TArg, bool, bool)" /> instead if you do not want to raise <see cref="ConverterException" />
        /// </remarks>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// <seealso cref="TryCast(object, Type, out object, object, bool, bool, IConvertContext)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object CastTo(this object self, Type typeTo, object defaultValue = null, bool unboxObjectType = true)
        {
            object result;
            self.TryCast(typeTo, out result, defaultValue, throwException: false, unboxObjectType: unboxObjectType);
            return result;
        }

        /// <summary>The cast method following the `Try` function convention of the .Net Framework, returning a <see langword="bool"/> success 
        /// status rather than throwing an <see cref="Exception"/> upon a failed conversion.</summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="result">The variable reference to which the conversion result is assigned.</param>
        /// <param name="defaultValue">An optional default value of the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="unboxObjectType">Whether to determine the type from the parameters <paramref name="self"/> and <paramref name="result"/> respectively.</param>
        /// <param name="contextInstance">An optional context instance, providing current parameters of the conversion process and context.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter.Convert(object,object)" /></typeparam>
        /// <returns>The success state as <see cref="bool" /> indicating if the conversion succeeded (`true`) or failed (`false`).</returns>
        /// <example>
        ///     <code>
        /// ```cs    
        ///       using System.Drawing;
        ///       var somestr = "test";
        ///       Image result;
        ///       if( test.TryCast(out result) == true )
        ///       {
        ///         Console.WriteLine("Image successfully created");
        ///       }
        /// ```
        /// </code>
        /// </example>
        /// <remarks>
        ///     The methods follows the "Try" convention. It accepts an object "value" and an out parameter <paramref name="result"/> of type <typeparamref name="TOut"/>. 
        ///     If the attempt to cast the value of <paramref name="self"/> into <see cref="Type"/> <typeparamref name="TOut"/> succeeds, `true` is returned, else `result = default(T)`
        ///     and false is returned.
        /// </remarks>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// <seealso cref="GetConverterOrDefault{TIn, TOut}(TIn, out Converter, out TOut, Type, Type, Type, bool, bool, string, bool)"/>
        /// <seealso cref="InvokeConvert{TIn, TOut}(TIn, out TOut, object, bool, Converter, IConvertContext, string)"/>
        public static bool TryCast<TIn, TOut>(this TIn self, out TOut result, TOut defaultValue = default(TOut), bool throwException = false, bool unboxObjectType = false, IConvertContext contextInstance = null)
        {
            Converter converter;
            var typeArgument = (defaultValue == null || defaultValue.IsDefaultValue() == true) ? null : typeof(TOut);
            if(GetConverterOrDefault(self, out converter, out result, typeArgument: typeArgument, throwException: throwException, unboxObjectType: unboxObjectType))
            {
                return true;
            }

            return InvokeConvert(self, out result, defaultValue, throwException, converter, contextInstance: contextInstance);
        }

        /// <summary>The cast method following the `Try` function convention of the .Net Framework, returning a <see langword="bool"/> success 
        /// status rather than throwing an <see cref="Exception"/> upon a failed conversion.</summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <paramref name="self"/> to</param>
        /// <param name="result">The variable reference to which the conversion result is assigned.</param>
        /// <param name="defaultValue">An optional default value of the target-<see cref="Type"/><paramref name="typeTo"/></param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="unboxObjectType">Whether to determine the type from the parameters <paramref name="self"/> and <paramref name="result"/> respectively.</param>
        /// <param name="contextInstance">An optional context instance, providing current parameters of the conversion process and context.</param>
        /// <returns>The success state as <see cref="bool" /> indicating if the conversion succeeded (`true`) or failed (`false`).</returns>
        /// <remarks>
        ///     The methods follows the "Try" convention. It accepts an object "value" and an out parameter <paramref name="result"/> of type <paramref name="typeTo"/>. 
        ///     If the attempt to cast the value of <paramref name="self"/> into <see cref="Type"/> <paramref name="typeTo"/> succeeds, `true` is returned, else `result = default(T)`
        ///     and false is returned.
        /// </remarks>
        /// <remarks>For strict typing use <see cref="TryCast{TIn, TOut}(TIn, out TOut, TOut, bool, bool, IConvertContext)"/> wherever possible</remarks>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// <seealso cref="GetConverterOrDefault{TIn, TOut}(TIn, out Converter, out TOut, Type, Type, Type, bool, bool, string, bool)"/>
        /// <seealso cref="InvokeConvert{TIn, TOut}(TIn, out TOut, object, bool, Converter, IConvertContext, string)"/>
        public static bool TryCast(this object self, Type typeTo, out object result, object defaultValue, bool throwException = false, bool unboxObjectType = true, IConvertContext contextInstance = null)
        {
            Type typeFrom = null;
            Type typeArgument = null;
            if(unboxObjectType == true)
            {
                typeFrom = self.GetType();
                if(typeFrom == typeTo)
                {
                    result = (object)self;
                    return true;
                }

                typeArgument = defaultValue == null ? null : defaultValue.GetType();
            }

            Converter converter;
            if(GetConverterOrDefault(self, out converter, out result, typeArgument: typeArgument, typeTo: typeTo, throwException: throwException, unboxObjectType: true))
            {
                return true;
            }

            if(defaultValue == null)
            {
                defaultValue = typeTo.GetTypeInfo().GetDefault();
            }

            return InvokeConvert(self, out result, defaultValue, throwException, converter, contextInstance: contextInstance);
        }
    }
}