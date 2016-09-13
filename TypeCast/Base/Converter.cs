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
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>The Converter base class, providing a simple container for conversion types, <see cref="ConverterAttribute" /> and corresponding conversion functions</summary>
    /// <remarks>The Converter is usually invoked through <see cref="ObjectExtension.CastTo{TOut}" /> and <see cref="ObjectExtension.TryCast{TIn,TOut}" /></remarks>
    /// <seealso cref="Converter{TIn,TOut}" />
    /// <seealso cref="ConverterCollection" />
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DataContract(IsReference = false, Name = "BaseConverter", Namespace = nameof(Base))]
    public abstract class Converter : IConverter
    {
        /// <summary>
        ///     The attribute for any arbitrary class of <see cref="BaseType" /> that provides conversion functionality. If no <see cref="Attribute" /> is present the
        ///     value is set `null`.
        /// </summary>
        /// <remarks>Requires a <seealso cref="BaseType" /> to be set</remarks>
        private ConverterAttribute attribute;

        /// <summary>Initializes a new instance of the <see cref="Core.TypeCast.Base.Converter" /> class.</summary>
        /// <param name="from"> The source/from conversion <see cref="Type" /></param>
        /// <param name="to"> The target/to conversion <see cref="Type" /> </param>
        /// <param name="argument">The <seealso cref="Type" /> of the argument passed to <see cref="Converter.FunctionDefault" /></param>
        /// <param name="baseType">The <see cref="Type" /> of any underlying converter class if one exists</param>
        /// <param name="attribute">The <see cref="ConverterAttribute" /> of the converter-class if present.</param>
        /// <remarks>Only invoked by <see cref="Converter{TIn,TOut}" /></remarks>
        /// <seealso cref="Converter{TIn,TOut}" />
        protected Converter(Type from, Type to, Type argument = null, Type baseType = null, ConverterAttribute attribute = null)
        {
            this.From = from?.GetTypeInfo();
            this.To = to?.GetTypeInfo();
            this.Argument = argument?.GetTypeInfo();
            this.BaseType = baseType?.GetTypeInfo();
            this.Attribute = attribute;
            this.NameSpace = attribute?.NameSpace;
        }

        /// <summary>The <seealso cref="Type" /> of the argument passed to <see cref="Converter.FunctionDefault" />
        /// </summary>
        [DataMember]
        public TypeInfo Argument { get; set; }

        /// <summary>
        ///     Returns `true` if the Argument-<see cref="Type"/>is the default value <see cref="object"/>. As all converters are contained in strongly typed <see cref="Converter{TIn, TOut, TArg}"/> 's 
        ///     the argument is set to <see cref="object"/> if unused. However explicit conversion to the <see cref="object"/> is still possible.
        /// </summary>
        public virtual TypeInfo ArgumentStandard { get; } = typeof(object).GetTypeInfo();

        /// <summary>Gets or sets the <see cref="ConverterAttribute" /> if the custom converter class has one defined, else it is set `null`.</summary>
        /// <remarks>The value is set once upon first access and is not updated thereafter, unless reset to `null`</remarks>
        /// <seealso cref="ConverterCollection.GetConverterAttributeFromIConverter(System.Reflection.TypeInfo,Core.TypeCast.ConverterAttribute,bool)" />
        public ConverterAttribute Attribute
        {
            get
            {
                if(this.attribute == null && this.BaseType != null)
                {
                    this.attribute = this.BaseType.GetCustomAttribute<ConverterAttribute>();
                    if(this.attribute != null)
                    {
                        this.attribute.BaseType = this.BaseType;
                    }
                }
                return this.attribute;
            }
            set
            {
                this.attribute = value;
            }
        }

        /// <summary>Gets the reference to the Base instance of <see cref="Type"/> <see cref="BaseType"/>  if one exists, otherwise the value is `null`.</summary>
        public object Base { get; set; }


        /// <summary>Gets the underlying <see cref="Type" /> of the custom converter class if one exists, otherwise the value is `null`.</summary>
        [DataMember]
        public TypeInfo BaseType { get; set; }

        /// <summary>Gets or sets the underlying <see cref="ConverterCollection" /> if the converter has been added to one, otherwise the value is `null`.</summary>
        public IConverterCollection Collection { get; set; }

        /// <summary>
        /// Optional <see cref="bool"/> value to indicate whether a <see cref="ConverterCollection"/> is allowed to contain multiple converters with the 
        /// same source and target <see cref="Type"/>s in the collection of <see cref="Items"/>. Only set to `true` for transform functions.</param>
        /// </summary>
        [DataMember]
        public bool AllowDisambiguates { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the second argument of <see cref="Converter.FunctionDefault" /> is <see cref="Type" />-checked to enforce type
        ///     quality with the return Type of the Function
        /// </summary>
        /// <seealso cref="ConverterCollectionSettings.DefaultValueAnyType" />
        [DataMember]
        public bool DefaultValueAnyType { get; set; }

        /// <summary>Gets the <see cref="Type" /> of the object instance which is to be converted.</summary>
        [DataMember]
        public TypeInfo From { get; private set; }

        /// <summary>The boxed converter function of <see cref="Type.DeclaringType" /> <see cref="IConverter{TIn,TOut}.Convert" />
        /// </summary>
        /// <returns>`null` if no function is set</returns>
        [DataMember]
        public object Function { get; set; }

        /// <summary>Gets an instance of <see cref="MethodInfo"/> describing the <see cref="Function"/>. 
        /// May be null if the converter constructor was not passed a <see cref="MethodInfo"/> instance.
        /// </summary>
        /// <returns>`null` if no function is set</returns>
        [DataMember]
        protected MethodInfo FunctionInfo { get; set; }

        /// <summary>Gets an instance of <see cref="ConverterMethodAttribute"/> of the <see cref="Function"/> if it exists. 
        /// May also be `null` if the attribute property has not been set.
        /// </summary>
        /// <returns>`null` if no function is set</returns>
        [DataMember]
        public ConverterMethodAttribute FunctionAttribute { get; set; }

        /// <summary>
        ///     The boxed converter function with an additional second argument value that allows passing a any arbitrary or default-value which may be returned in case of conversion
        ///     failure or the conversion result yielding `null`. It may also be used as an essential argument for <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object)"/>
        /// </summary>
        /// <returns>`null` if no function is set</returns>
        /// <remarks>The actual implementation and use of the second parameter lies solely within the scope of the programmer implementing the converter logic</remarks>
        [DataMember]
        public object FunctionDefault { get; set; }

        /// <summary>Gets an instance of <see cref="MethodInfo"/> describing the <see cref="FunctionDefault"/>. 
        /// May be null if the converter constructor was not passed a <see cref="MethodInfo"/> instance.
        /// </summary>
        /// <returns>`null` if no function is set</returns>
        [DataMember]
        protected MethodInfo FunctionDefaultInfo { get; set; }

        /// <summary>Gets an instance of <see cref="ConverterMethodAttribute"/> of the <see cref="FunctionDefault"/> if it exists. 
        /// May also be `null` if the attribute property has not been set.
        /// </summary>
        /// <returns>`null` if no function is set</returns>
        [DataMember]
        public ConverterMethodAttribute FunctionDefaultAttribute { get; set; }

        /// <summary>Gets a value indicating whether the converter has a <see cref="FunctionDefault" /> set.</summary>
        [DataMember]
        public bool HasDefaultFunction
        {
            get
            {
                return this.FunctionDefault != null && this.FunctionDefault is Delegate;
            }
        }

        /// <summary>Gets a value indicating whether the converter has only a <see cref="FunctionDefault" /> set.</summary>
        [DataMember]
        public bool HasDefaultFunctionOnly
        {
            get
            {
                return this.Function == null && this.HasDefaultFunction;
            }
        }

        /// <summary>Gets a value indicating whether the converter has either <see cref="Function" /> or <see cref="FunctionDefault" /> set.</summary>
        [DataMember]
        public bool HasFunction
        {
            get
            {
                return (this.Function ?? this.FunctionDefault) != null && (this.Function ?? this.FunctionDefault) is Delegate;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="Converter" /> instance is strictly typed such that the <seealso cref="Argument" /> <see cref="Type" />
        ///     is set to the <see cref="Type" /> of <see cref="To" />.
        /// </summary>
        /// <remarks>
        ///     <seealso cref="Converter{TIn,TOut}" /> inherits from <seealso cref="Converter{TIn,TOut,TArg}" />. Due to the strictly-typed nature of the converter
        ///     instances the <seealso cref="FunctionDefault" /> is only passes a default-value if the passed value is of the same type as <see cref="To" />. This property
        ///     allows quick lookups.
        /// </remarks>
        [DataMember]
        public bool Standard
        {
            get
            {
                if(typeof(MulticastDelegate).GetTypeInfo().IsAssignableFrom(this.BaseType) == true)
                {
                    return false;
                }
                return ((this.HasDefaultFunction == true) && (this.Argument == this.To))
                    || ((this.HasDefaultFunctionOnly == false) && (this.HasFunction == true) && (this.Argument == this.To || this.Argument == this.ArgumentStandard));
            }
        }

        /// <summary>
        ///     Gets the <see cref="Type.Namespace" /> that the <see cref="Converter" /> instance is assigned to, with the possibility of many converters declaring
        ///     the same namespace , that is using a one to many relationship. The handling of the <see cref="NameSpace" /> lies within the responsibility of the
        ///     <see cref="ConverterCollection" />
        /// </summary>
        /// <remarks>The namespace is used for grouping, filtering and on-demand loading purposes.</remarks>
        /// <seealso cref="ConverterAttribute.LoadOnDemand" />
        [DataMember]
        public string NameSpace { get; private set; }

        /// <summary>Gets the <see cref="Type" /> that the instance will be converted to.</summary>
        [DataMember]
        public TypeInfo To { get; private set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to use a default-value wrapper function if a default-value was passed but no conversion function taking a
        ///     default-value exists
        /// </summary>
        /// <seealso cref="ConverterCollectionSettings.UseFunctionDefaultWrapper" />
        /// <seealso cref="FunctionDefault" />
        [DataMember]
        public bool UseFunctionDefaultWrapper { get; set; }

        /// <summary>Provides a formatted string showing the state of the Converter Types and Type information for debugging and logging</summary>
        /// <returns>Returns a A formatted string showing the state of the Converter Types and Type information for debugging and logging</returns>
        /// <example>
        ///     **Example:** Using DebuggerDisplayAttribute in Live Debuggers. nq = no quotes, to omit the typical string quotes
        ///     <code>
        /// ```cs
        ///     [DebuggerDisplay("{DebuggerDisplay,nq}")]
        ///     [Converter]
        ///     public class CustomeConverter { .... }
        /// 
        /// ```
        /// </code>
        /// </example>
        private string DebuggerDisplay
        {
            get
            {
                return this.ToString();
            }
        }

        /// <summary> The converter function as part of the <see cref="IConverter" /> interface support. </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="defaultValue">The optional default value to be passed if the conversion fails or is `null`.</param>
        /// <returns>The converted value as a boxed <see cref="object" />.</returns>
        /// <remarks>
        /// <remarks>The actual implementation and use of the second parameter lies solely within the scope of the programmer implementing the converter logic</remarks>
        /// </remarks>
        public abstract object Convert(object value, object defaultValue = null);

        /// <summary>Overrides <see cref="object.ToString()" /> to provide a string representation of the underlying conversion types</summary>
        /// <returns>Returns a string containing information about the conversion types <see cref="From" /> and <see cref="To" /> as well as the <see cref="BaseType" />
        /// </returns>
        public override string ToString()
        {
            return
                $"{this.GetType().Name}[({this.From?.Name}, {this.Argument?.Name}) => {this.To?.Name}] BaseType: {this.BaseType?.Name}, Attribute: {this.Attribute}";
        }

        /// <summary>Overrides <see cref="object.ToString()" /> to provide information about the conversion types</summary>
        /// <param name="fullName">Whether to show the full reflected name information</param>
        /// <returns>Returns a string containing information about the conversion types <see cref="From" /> and <see cref="To" /> as well as the <see cref="BaseType" />
        /// </returns>
        /// <seealso cref="ToString()" />
        public string ToString(bool fullName)
        {
            if(fullName == false)
            {
                return this.ToString();
            }

            return
                $"{this.GetType().FullName}[({this.From?.FullName}, {this.Argument?.FullName}) => {this.To?.FullName}] BaseType: {this.BaseType?.FullName}, Attribute: {this.Attribute}";
        }

        /// <summary> Checks the types being in the correct source and target format, if not an <see cref="Exception"/> is thrown. </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="defaultValue">The optional default value to be passed if the conversion fails or is `null`.</param>
        /// <exception cref="ConverterException">Throws exceptions based on mismatching types or null references</exception>
        protected abstract void CheckConvertTypes(object value, object defaultValue);
    }
}