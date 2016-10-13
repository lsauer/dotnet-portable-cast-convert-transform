// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;

    using Core.TypeCast.Base;

    /// <summary>
    /// The settings for the <see cref="ConverterCollection"/>.
    /// </summary>
    public class ConverterCollectionSettings
    {
        /// <summary>
        /// If to use the converter-default wrapper or throw an exception.
        /// </summary>
        private bool converterDefaultWrapperOrException;

        /// <summary>
        /// If to use the function-default wrapper.
        /// </summary>
        private bool useFunctionDefaultWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterCollectionSettings"/> class.
        /// </summary>
        /// <param name="defaultValueAnyType">Set to `true` if any argument type is allowed. If set to `false` <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object, bool)"/> cannot be used.</param>
        /// <param name="useFunctionDefaultWrapper">
        /// Whether to use a function default wrapper if the <see cref="Converter.FunctionDefault"/> is `null`, yet required for <see cref="Converter.Convert"/>.
        /// </param>
        /// <param name="numberFormat">
        /// The number format used for the conversion functions within the <see cref="ConverterCollection"/> instance.
        /// </param>
        /// <param name="converterMissingException">Whether to throw a converter missing exception.  </param>
        /// <param name="converterClassExistsException">Whether to throw a converter class exists exception.  </param>
        /// <param name="autoInitialize">whether to allow auto-initialization through attribute scanning of the entry-assembly.</param>
        /// <param name="allowGenericTypes">Whether to allow generic types as source or target types of converters</param>
        /// <param name="allowExplicitObject">Whether to allow generic types as source or target types of converters. See <see cref="AllowExplicitObject"/>.</param>
        /// <param name="allowDynamicType">whether to allow invoking dynamic implicit casting as a cast Fallback. See <see cref="AllowDynamicType"/>.</param>
        /// <param name="converterDefaultWrapperOrException"> Whether to use a default-value wrapper if one is required or throw a default-function missing exception. </param>
        /// <param name="boundedCapacity">The bounded capacity of <see cref="BlockingCollection{Converter}"/> instance . See <see cref="BoundedCapacity"/>.</param>
        public ConverterCollectionSettings(
            NumberFormatInfo numberFormat = null,
            bool defaultValueAnyType = false,
            bool useFunctionDefaultWrapper = true,
            bool converterMissingException = false,
            bool converterClassExistsException = false,
            bool autoInitialize = true,
            bool allowGenericTypes = true,
            bool allowExplicitObject = true,
            bool allowDynamicType = true,
            bool converterDefaultWrapperOrException = true,
            int boundedCapacity = 10000)
        {
            this.DefaultValueAnyType = defaultValueAnyType;
            this.UseFunctionDefaultWrapper = useFunctionDefaultWrapper;
            this.NumberFormat = numberFormat ?? DefaultNumberFormat;
            this.ConverterMissingException = converterMissingException;
            this.ConverterClassExistsException = converterClassExistsException;
            this.AutoInitialize = autoInitialize;
            this.AllowGenericTypes = allowGenericTypes;
            this.AllowExplicitObject = allowExplicitObject;
            this.AllowDynamicType = allowDynamicType;
            this.ConverterDefaultWrapperOrException = converterDefaultWrapperOrException;
            this.BoundedCapacity = boundedCapacity;
        }

        /// <summary>
        /// Gets or sets the default number format.
        /// </summary>
        public static NumberFormatInfo DefaultNumberFormat { get; set; } = new NumberFormatInfo { NumberGroupSeparator = ".", NumberDecimalDigits = 2 };

        /// <summary>
        /// Gets or sets whether to throw an <see cref="ConverterException"/> if generic types are passed as the source our target <see cref="Type"/>.
        /// </summary>
        public bool AllowGenericTypes { get; set; }

        /// <summary>
        /// Gets or sets whether to throw an <see cref="ConverterException"/> if the conversion Source or target type is set explicitly to <see cref="Type"/> `object`.
        /// </summary>
        public bool AllowExplicitObject { get; set; }

        /// <summary>
        /// Gets or sets whether to allow invoking dynamic implicit casting as a cast Fallback in <see cref="ObjectExtension.InvokeConvert{TIn, TOut}(TIn, out TOut, object, bool, Converter, IConvertContext, string)"/>.
        /// </summary>
        /// The implementation is based on C# 4+'s core feature using a <see langword="dynamic"/> Type using internal CLR reflection logic, but may alternatively be invoked via explicit reflection as well.
        /// <remarks>See http://stackoverflow.com/a/2090228/901946 </remarks>
        public bool AllowDynamicType { get; set; }

        /// <summary>
        /// Gets or sets whether to allow auto-initialization through attribute scanning of the entry-assembly. Auto-discovery may not be available during testing and in special runtime environments.
        /// </summary>
        public bool AutoInitialize { get; set; }

        /// <summary>
        /// Gets or sets the bounded capacity of <see cref="BlockingCollection{Converter}"/> instance in <see cref="ConverterCollection"/>, 
        /// which limit the collection size of <see cref="ConverterCollection.Items"/> to a specific number of items at any given time.
        /// </summary>
        public int BoundedCapacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use a default-value wrapper if no default-function is set or throw a default-function is missing exception.
        /// </summary>
        public bool ConverterDefaultWrapperOrException
        {
            get
            {
                return this.converterDefaultWrapperOrException;
            }

            set
            {
                this.useFunctionDefaultWrapper = value;
                this.converterDefaultWrapperOrException = !value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to throw a default-function is missing exception.
        /// </summary>
        public bool ConverterMissingException { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to throw a converter class exists exception.
        /// </summary>
        public bool ConverterClassExistsException { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the second argument of <see cref="Converter.FunctionDefault"/> is <see cref="Type"/>-checked to 
        /// enforce type-equality of the argument/default-value with the return Type of the Function
        /// </summary>
        /// <remarks>Sets the <see cref="Converter.DefaultValueAnyType"/> auto-property upon adding a <see cref="Converter"/> instance 
        /// to the ´<see cref="ConverterCollection"/>. However if the value changed whilst adding converters, only instances added after the value change will be affected.</remarks>
        /// <seealso cref="Converter.DefaultValueAnyType"/>
        public bool DefaultValueAnyType { get; set; }

        /// <summary>
        /// Gets or sets the number format used by default for <see cref="Converter"/> instances.
        /// </summary>
        public NumberFormatInfo NumberFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use default-value function-wrapper.
        /// </summary>
        public bool UseFunctionDefaultWrapper
        {
            get
            {
                return this.useFunctionDefaultWrapper;
            }

            set
            {
                this.useFunctionDefaultWrapper = value;
                this.converterDefaultWrapperOrException = !value;
            }
        }
    }
}