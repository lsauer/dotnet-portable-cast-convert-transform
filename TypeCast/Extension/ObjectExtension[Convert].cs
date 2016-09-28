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
        /// <summary>The convert method following can involve up to three different types to convert an arbitrary input type to another arbitrary output type.
        /// using a third model parameter which encapsulates all data and fields required for the conversion function. 
        /// A maximum of three involving types by design to enforce adherence to the single responsibility principle.</summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// A context provides meta-data about the conversion arguments and is rarely needed.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>Returns the converted value of <see cref="Type"/> <typeparamref name="TOut" /> </returns>
        /// <example>
        ///     <code>
        /// ```cs    
        ///       using System.Drawing;
        ///        Func&lt;Point, Rectangle, Size> delAnyFunc = (ap, rect) =>
        ///        {
        ///            if(ap.X * ap.Y > rect.X * rect.Y)
        ///            {
        ///                return new Size(ap.X, ap.Y);
        ///            }
        ///            return new Size(rect.X, rect.Y);
        ///        };
        ///        ConverterCollection.CurrentInstance.Add(delAnyFunc);
        ///        Point somePoint = new Point(1, 2);
        ///        Size size = somePoint.ConvertTo&lt;Point, Size>(new Rectangle(2, 2, 4, 4));
        /// ```
        /// </code>
        /// </example>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// <seealso cref="TryConvert{TOut}(object, out TOut, object, bool)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut ConvertTo<TIn, TOut>(this TIn self, object model, bool withContext = false)
        {
            TOut result = default(TOut);
            self.TryConvert<TOut>(out result, model, throwException: model != null, withContext: withContext);
            return result;
        }

        /// <summary>The convert method following can involve up to three different types to convert an arbitrary input type to another arbitrary output type.
        /// using a third model parameter which encapsulates all data and fields required for the conversion function. 
        /// A maximum of three involving types by design to enforce adherence to the single responsibility principle.</summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process 
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>Returns the converted value of <see cref="Type"/> <typeparamref name="TOut" /> </returns>
        /// <example>
        ///     <code>
        /// ```cs    
        ///       using System.Drawing;
        ///        Func&lt;Point, Rectangle, Size> delAnyFunc = (ap, rect) =>
        ///        {
        ///            if(ap.X * ap.Y > rect.X * rect.Y)
        ///            {
        ///                return new Size(ap.X, ap.Y);
        ///            }
        ///            return new Size(rect.X, rect.Y);
        ///        };
        ///        ConverterCollection.CurrentInstance.Add(delAnyFunc);
        ///        Point somePoint = new Point(1, 2);
        ///        Size size = somePoint.ConvertTo&lt;Size>(new Rectangle(2, 2, 4, 4));
        /// ```
        /// </code>
        /// </example>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// <seealso cref="TryConvert{TOut}(object, out TOut, object, bool)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut ConvertTo<TOut>(this object self, object model, bool withContext = false)
        {
            TOut result = default(TOut);
            self.TryConvert<TOut>(out result, model, throwException: model != null, withContext: withContext);
            return result;
        }


        /// <summary>The convert method following the `Try` convention, of the .Net Framework, returning a <see cref="bool"/> success 
        /// status rather than throwing an <see cref="Exception"/> upon a failed conversion. Conversion can involve up to three different types to convert 
        /// an arbitrary input type to another arbitrary output type, by using a third model parameter which encapsulates all data and fields required for the conversion function. 
        /// A maximum of three involving types is by design, to enforce adherence to the single responsibility principle.</summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="result">The variable reference to which the conversion result is assigned.</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <see cref="Type" /> <typeparamref name="TOut"/></param>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>Returns the converted value of <see cref="Type"/> <typeparamref name="TOut" /> </returns>
        /// <returns>The success state as <see cref="bool" /> indicating if the conversion succeeded (`true`) or failed (`false`).</returns>
        /// <remarks>
        ///     The methods follows the "Try" convention. It accepts an object "value", an out parameter <paramref name="result"/> of type <typeparamref name="TOut"/> and a data-model. 
        ///     If the attempt to convert the value of <paramref name="self"/> into <see cref="Type"/> <typeparamref name="TOut"/> succeeds, `true` is returned, else `result = default(T)`
        ///     and false is returned.
        /// </remarks>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// <seealso cref="GetConverterOrDefault{TIn, TOut}(TIn, out Converter, out TOut, Type, Type, bool, bool, bool)"/>
        /// <seealso cref="InvokeConvert{TIn, TOut}(TIn, out TOut, object, bool, Converter)"/>
        public static bool TryConvert<TOut>(this object self, out TOut result, object model, bool throwException = false, bool withContext = false)
        {
            Converter converter;
            result = default(TOut);
            if(GetConverterOrDefault<object, TOut>(self, out converter, out result, typeArgument: model?.GetType(), throwException: throwException, unboxObjectType: true, withContext: withContext))
            {
                return true;
            }

            return InvokeConvert(self, out result, model, throwException, converter, contextInstance: (withContext ? new ConvertContext(model) : null));
        }

        /// <summary>The convert method following the `Try` convention, of the .Net Framework, returning a <see cref="bool"/> success 
        /// status rather than throwing an <see cref="Exception"/> upon a failed conversion. Conversion can involve up to three different types to convert 
        /// an arbitrary input type to another arbitrary output type, by using a third model parameter which encapsulates all data and fields required for the conversion function. 
        /// A maximum of three involving types is by design, to enforce adherence to the single responsibility principle.</summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="result">The variable reference to which the conversion result is assigned.</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within 
        /// <see cref="Type" /> <typeparamref name="TOut"/></param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TArg">The Argument <see cref="Type"/> for generic converters using see <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object)"/>. 
        /// <returns>Returns the converted value of <see cref="Type"/> <typeparamref name="TOut" /> </returns>
        /// <returns>The success state as <see cref="bool" /> indicating if the conversion succeeded (`true`) or failed (`false`).</returns>
        /// <remarks>
        ///     The methods follows the "Try" convention. It accepts an object "value", an out parameter <paramref name="result"/> of type <typeparamref name="TOut"/> and a data-model. 
        ///     If the attempt to convert the value of <paramref name="self"/> into <see cref="Type"/> <typeparamref name="TOut"/> succeeds, `true` is returned, else `result = default(T)`
        ///     and false is returned.
        /// </remarks>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException" />if the conversion fails</exception>
        /// <remarks>note: The <see cref="ConverterCollection"/> is lazy instantiated upon the first invocation of the method</remarks>
        /// <seealso cref="GetConverterOrDefault{TIn, TOut}(TIn, out Converter, out TOut, Type, Type, bool, bool, bool)"/>
        /// <seealso cref="InvokeConvert{TIn, TOut}(TIn, out TOut, object, bool, Converter)"/>
        public static bool TryConvert<TIn, TOut, TArg>(this TIn self, out TOut result, TArg model, bool throwException = false, bool withContext = false)
        {
            Converter converter;
            result = default(TOut);
            if(GetConverterOrDefault<TIn, TOut>(self, out converter, out result, typeArgument: typeof(TArg), throwException: throwException, unboxObjectType: false, withContext: withContext))
            {
                return true;
            }

            return InvokeConvert(self, out result, model, throwException, converter, contextInstance: (withContext ? new ConvertContext(model) : null));
        }
    }
}