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
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <typeparam name="TOut">The output <see cref="Type"/> which must equal the input type of the own value <paramref name="self"/></typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut Transform<TOut>(this TOut self, Enum functionAlias)
        {
            return Transform<TOut>(self: self, model: null, functionAlias: functionAlias.ToString());
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <typeparam name="TOut">The output <see cref="Type"/> which must equal the input type of the own value <paramref name="self"/></typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="strictTypeCheck">Whether the input and output arguments are of the same type, otherwise an exception is thrown, when set to true.</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut Transform<TOut>(this TOut self, object model, Enum functionAlias, bool strictTypeCheck = true, bool withContext = false)
        {
            return Transform<TOut>(self: self, model: model, functionAlias: functionAlias.ToString(), withContext: withContext);
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <typeparam name="TOut">The output <see cref="Type"/> which must equal the input type of the own value <paramref name="self"/></typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut Transform<TOut>(this object self, Enum functionAlias)
        {
            return (TOut)Transform(self: self, model: null, typeBase: null, typeTo: typeof(TOut), functionAlias: functionAlias.ToString(), throwException: false);
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <typeparam name="TOut">The output <see cref="Type"/> which must equal the input type of the own value <paramref name="self"/></typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut Transform<TOut>(this object self, object model, Enum functionAlias, bool withContext = false)
        {
            return (TOut)Transform(self: self, model: null, typeBase: null, typeTo: typeof(TOut), functionAlias: functionAlias.ToString(), throwException: false, withContext: withContext);
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <typeparam name="TOut">The output <see cref="Type"/> which must equal the input type of the own value <paramref name="self"/></typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut Transform<TOut>(this TOut self, object model, string functionAlias = null, bool withContext = false)
        {
            return (TOut)Transform(self: self, model: model, typeBase: null, typeTo: typeof(TOut), functionAlias: functionAlias, throwException: false, withContext: withContext);
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <typeparam name="TOut">The output <see cref="Type"/> which must equal the input type of the own value <paramref name="self"/></typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="strictTypeCheck">Whether the input and output arguments are of the same type, otherwise an exception is thrown, when set to true.</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut Transform<TBase, TOut>(this object self, object model = null, string functionAlias = null, bool strictTypeCheck = false, bool withContext = false)
        {
            CheckTransformTypes<TOut>(self, strictTypeCheck);
            return (TOut)Transform<TBase, object, TOut>(self: self, model: model, functionAlias: functionAlias, strictTypeCheck: strictTypeCheck, withContext: withContext);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut Transform<TBase, TIn, TOut>(this object self, object model = null, string functionAlias = null, bool strictTypeCheck = false, bool withContext = false)
        {
            CheckTransformTypes<TOut>(self, strictTypeCheck);
            Converter converter = null;
            return (TOut)Transform<TIn>(self: (TIn)self, converter: out converter, model: model, typeBase: typeof(TBase), typeTo: typeof(TOut), functionAlias: functionAlias, throwException: false, withContext: withContext);
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <typeparam name="TOut">The output <see cref="Type"/> which must equal the input type of the own value <paramref name="self"/></typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="typeBase">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut Transform<TOut>(this TOut self, Type typeBase, object model = null, string functionAlias = null, bool withContext = false)
        {
            return (TOut)Transform(self: self, model: model, typeBase: typeBase, typeTo: typeof(TOut), functionAlias: functionAlias, throwException: false, withContext: withContext);
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <typeparam name="TBase">The declaring <see cref="Type"/> of the converter-functions to add as a group.</typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="typeBase">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="self"/> to</param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Transform<TBase>(this object self, object model = null, Type typeBase = null, Type typeTo = null, string functionAlias = null, bool throwException = true, bool withContext = false)
        {
            return Transform(self: self, model: model, typeBase: typeof(TBase), typeTo: typeTo, functionAlias: functionAlias, throwException: throwException, withContext: withContext);
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="typeBase">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="self"/> to</param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Transform(this object self, object model = null, Type typeBase = null, Type typeTo = null, string functionAlias = null, bool throwException = true, bool withContext = false)
        {
            Converter converter = null;
            return Transform(self: self, converter: out converter, model: model, typeBase: typeBase, typeTo: typeTo, functionAlias: functionAlias, throwException: throwException, withContext: withContext);
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="converter">The <see cref="Converter"/> instance for the corresponding types <typeparamref name="TIn"/> and <typeparamref name="TOut"/></param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="typeBase">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="self"/> to</param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Transform<TIn>(this TIn self, out Converter converter, object model = null, Type typeBase = null, Type typeTo = null, string functionAlias = null, bool throwException = true, bool withContext = false)
        {
            object result = null;
            // transform should be used for similar or same types as the input type, as such if no typeTo is supplied let's use the delegate's return-type
            typeTo = typeTo ?? typeBase?.GetTypeInfo().GetReturnParameterType();
            if(typeof(TIn) != objectType && typeBase?.GetTypeInfo().IsInvokableWithParameters(typeTo, self.GetType(), model?.GetType()) == false)
            {
                throw new ConverterException(ConverterCause.DelegateArgumentWrongType);
            }
            GetConverterOrDefault<TIn, object>(self, out converter, out result, typeArgument: model?.GetType(), typeTo: functionAlias != null ? null : typeTo, typeBase: typeBase,
                throwException: throwException, unboxObjectType: typeof(TIn) != objectType, attributeName: functionAlias);

            InvokeConvert(self, out result, model, throwException: throwException, converter: converter, contextInstance: (withContext ? new ConvertContext(model) : null));
            return result;
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="converterAction">A function delegate <see cref="Func{TIn, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="addWithAttributeName">An Enum containing an enumeration of aliases for the converters for unique identification and lookup</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut Transform<TIn, TOut>(this TIn self, Func<TIn, TOut> converterAction, Enum addWithAttributeName, bool throwException = true)
        {
            return Transform<TIn, TOut>(self: self, converterAction: converterAction, addWithAttributeName: addWithAttributeName.ToString(), throwException: throwException);
        }

        /// <summary>
        /// Alias for <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> with parameter `transform` set to true, to allow equality of the input and output types.
        /// Use Transform when the input and output <see cref="Type"/> match, otherwise an exception is raised.
        /// </summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="converterAction">A function delegate <see cref="Func{TIn, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="addWithAttributeName">An Enum containing an enumeration of aliases for the converters for unique identification and lookup</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>Returns the transformed value of the <see cref="Type"/> as <typeparamref name="TOut" /> </returns>
        public static TOut Transform<TIn, TOut>(this TIn self, Func<TIn, TOut> converterAction, string addWithAttributeName = null, bool throwException = true)
        {
            TOut result = default(TOut);
            Converter converter = null;
            converter = ConverterCollection.CurrentInstance.Factory.Create<TIn, TOut>(converterAction);
            var baseType = converterAction?.Target?.GetType().DeclaringType;

            if(String.IsNullOrWhiteSpace(addWithAttributeName) == false)
            {
                // set an alias for later filtering of transform functions
                converter.Attribute = new ConverterAttribute(loadOnDemand: false, name: addWithAttributeName, nameSpace: baseType.Namespace);
                ConverterCollection.CurrentInstance.Add(converter, allowDisambiguates: true);
            }

            InvokeConvert(self: self, result: out result, defaultValue: default(TOut), throwException: throwException, converter: converter);

            return result;
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="result">The variable reference to which the conversion result is assigned.</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="typeBase">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="self"/> to</param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <returns>The success state as <see cref="bool" /> indicating if the transformation succeeded (`true`) or failed (`false`).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryTransform(this object self, out object result, object model = null, Type typeBase = null, Type typeTo = null, string functionAlias = null, bool throwException = false, bool withContext = false)
        {
            Converter converter = null;
            result = Transform(self: self, converter: out converter, model: model, typeBase: typeBase, typeTo: typeTo, functionAlias: functionAlias, throwException: throwException, withContext: withContext);
            return converter != null;
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="result">The variable reference to which the conversion result is assigned.</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="typeBase">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="typeTo">The target <see cref="Type"/> to which to convert the <see cref="Type"/> of <see cref="self"/> to</param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="strictTypeCheck">Whether the input and output arguments are of the same type, otherwise an exception is thrown, when set to true.</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>The success state as <see cref="bool" /> indicating if the transformation succeeded (`true`) or failed (`false`).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryTransform<TIn, TOut>(this TIn self, out TOut result, object model = null, Type typeBase = null, string functionAlias = null, bool throwException = false, bool strictTypeCheck = false, bool withContext = false)
        {
            CheckTransformTypes<TOut>(self, strictTypeCheck: strictTypeCheck);

            Converter converter = null;
            try
            {
                result = (TOut)Transform(self: self, converter: out converter, model: model, typeBase: typeBase, typeTo: typeof(TOut), functionAlias: functionAlias, throwException: throwException, withContext: withContext);
            }
            catch(InvalidCastException exc)
            {
                if(throwException == true)
                {
                    throw new ConverterException(ConverterCause.DelegateArgumentWrongType, exc);
                }
                result = default(TOut);
            }
            return converter != null;
        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="result">The variable reference to which the conversion result is assigned.</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="strictTypeCheck">Whether the input and output arguments are of the same type, otherwise an exception is thrown, when set to true.</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <typeparam name="TBase">The declaring <see cref="Type"/> of the converter-functions to add as a group.</typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>The success state as <see cref="bool" /> indicating if the transformation succeeded (`true`) or failed (`false`).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryTransform<TBase, TOut>(this object self, out TOut result, object model = null, string functionAlias = null, bool throwException = false, bool strictTypeCheck = false, bool withContext = false)
        {
            CheckTransformTypes<TOut>(self, strictTypeCheck: strictTypeCheck);
            Converter converter = null;
            try
            {
                result = (TOut)Transform(self: self, converter: out converter, model: model, typeBase: typeof(TBase), typeTo: typeof(TOut), functionAlias: functionAlias, throwException: throwException, withContext: withContext);
            }
            catch(InvalidCastException exc)
            {
                if(throwException == true)
                {
                    throw new ConverterException(ConverterCause.BadInputFormat, exc);
                }
                result = default(TOut);
            }
            return converter != null;

        }

        /// <summary>
        /// `Transform`is similar to <see cref="ConvertTo{TIn, TOut}(TIn, object)"/> but should be preferred in situations wherein the input and output type are similar or the same. 
        /// All Types involved in the conversion must be from the same namespace. If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
        /// This does not hold true for the "Try" version, which has no optional `strictTypeCheck` argument.
        /// </summary>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="result">The variable reference to which the conversion result is assigned.</param>
        /// <param name="model">An model-instance, for instance a Data-Transfer-Object/DTO that encapsulates further data parameters for the conversion process
        /// to the target-<see cref="Type"/><typeparamref name="TOut"/></param>
        /// <param name="functionAlias">Applies an optional search string to the filter lookup, as Transform functions are allowed to be ambivalent 
        /// i.e. have same argument and return types</param>
        /// <param name="throwException">Whether to throw exceptions. `false` by default such that no <see cref="ConverterException"/> is thrown</param>
        /// <param name="strictTypeCheck">Whether the input and output arguments are of the same type, otherwise an exception is thrown, when set to true.</param>
        /// <param name="withContext">Whether to provide a conversion context with the model argument set within. 
        /// <typeparam name="TBase">The declaring <see cref="Type"/> of the converter-functions to add as a group.</typeparam>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type" />from which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <returns>The success state as <see cref="bool" /> indicating if the transformation succeeded (`true`) or failed (`false`).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryTransform<TBase, TIn, TOut>(this TIn self, out TOut result, object model = null, string functionAlias = null, bool throwException = false, bool strictTypeCheck = false, bool withContext = false)
        {
            CheckTransformTypes<TOut>(self, strictTypeCheck: strictTypeCheck);

            Converter converter = null;
            try
            {
                result = (TOut)Transform<TIn>(self: self, converter: out converter, model: model, typeBase: typeof(TBase), typeTo: typeof(TOut), functionAlias: functionAlias, throwException: throwException, withContext: withContext);
            }
            catch(InvalidCastException exc)
            {
                if(throwException == true)
                {
                    throw new ConverterException(ConverterCause.DelegateArgumentWrongType, exc);
                }
                result = default(TOut);
            }
            return converter != null;
        }

        /// <summary>
        /// If the parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match. 
        /// If `strictTypeCheck` is set to `false` an exception is thrown if the <see cref="Type.Namespace"/> of the input <typeparamref name="TIn"/> and output type <typeparamref name="TOut"/> do not match. 
        /// This does not hold true for the "Try" version, which has no optional `StrictTypeCheck` invocation.
        /// </summary>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="strictTypeCheck"> If the parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckTransformTypes<TOut>(object self, bool strictTypeCheck)
        {
            var sameOrSimilar = self.GetType().GetTypeInfo().IsSameOrSimilar<TOut>();
            if((strictTypeCheck == true && sameOrSimilar == TypeMatch.None))
            {
                throw new ConverterException(ConverterCause.TransformRequiresEqualInOutTypes);
            }
        }
    }
}