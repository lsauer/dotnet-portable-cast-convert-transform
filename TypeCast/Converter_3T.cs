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
    using System.Reflection.Emit;
    using System.Runtime.Serialization;

    using Core.TypeCast.Base;

    /// <summary>
    /// The specific, strictly-typed converter class for type transformations and complex conversions.
    /// </summary>
    /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
    /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
    /// <typeparam name="TArg">The Argument <see cref="Type"/> for generic converters using see <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object)"/>. 
    /// In <see cref="Converter{TIn, TOut}"/> <typeparamref name="TArg"/> is set to <see cref="object"/></typeparam>
    [DataContract]
    public class Converter<TIn, TOut, TArg> : Converter, IConverter<TIn, TOut>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Converter{TIn, TOut, TArg}"/> class.
        /// </summary>
        /// <param name="converter"> The converter function. <seealso cref="Converter.Function"/></param>
        /// <exception cref="ConverterException">Throws a <see cref="ConverterException"/> caused by <see cref="ConverterCause.ConverterArgumentNull"/>
        /// </exception>
        public Converter(Func<TIn, TOut> converter)
            : base(typeof(TIn), typeof(TOut), typeof(TArg))
        {
            // Prevent null types in the base
            if(converter == null)
            {
                throw new ConverterException(ConverterCause.ConverterArgumentNull);
            }

            this.ConverterFunc = converter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Converter{TIn, TOut, TArg}"/> class.
        /// </summary>
        /// <param name="converterDefault"> The converter default function which takes a default-value parameter as the second argument  <seealso cref="Converter.FunctionDefault"/>
        ///  wherein <typeparamref name="TIn"/> is the source / from <see cref="Type"/>from which to <see cref="Convert(object,object)"/>, and 
        /// <typeparamref name="TOut"/> is the target / to <see cref="Type"/> to which to <see cref="Convert(object,object)"/>
        /// </param>
        /// <exception cref="ConverterException">Throws a <see cref="ConverterException"/> caused by <see cref="ConverterCause.ConverterArgumentNull"/>
        /// </exception>
        public Converter(Func<TIn, TOut, TOut> converterDefault)
            : base(typeof(TIn), typeof(TOut), typeof(TArg))
        {
            if(converterDefault == null)
            {
                throw new ConverterException(ConverterCause.ConverterArgumentNull);
            }

            this.ConverterDefaultFunc = converterDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Converter{TIn, TOut, TArg}"/> class.
        /// </summary>
        /// <param name="converterAny"> The convert function which takes a model parameter as the second argument  <seealso cref="Converter.ConverterDefaultAnyTypeFunc"/>
        ///  wherein <typeparamref name="TIn"/> is the source / from <see cref="Type"/>from which to <see cref="Convert(object,object)"/>, <typeparamref name="TArg"/> is the argument type, 
        ///  ideally a converter-model (see the code-examples) and <typeparamref name="TOut"/> is the target / to <see cref="Type"/> to which to <see cref="Convert(object,object)"/>
        /// </param>
        /// <param name="argument">The optional <seealso cref="Type"/> of the argument passed to <see cref="Converter.FunctionDefault"/>.</param>
        /// <exception cref="ConverterException">Throws a <see cref="ConverterException"/> caused by <see cref="ConverterCause.ConverterArgumentNull"/>
        /// </exception>
        public Converter(Func<TIn, TArg, TOut> converterAny, Type argument = null)
            : base(typeof(TIn), typeof(TOut), argument ?? typeof(TArg))
        {
            if(converterAny == null)
            {
                throw new ConverterException(ConverterCause.ConverterArgumentNull);
            }

            this.DefaultValueAnyType = true;
            this.ConverterAnyFunc = converterAny;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Converter{TIn, TOut, TArg}"/> class.
        /// </summary>
        /// <param name="converterDefaultAnyType"> The convert function which takes a model parameter as the second argument  <seealso cref="Converter.ConverterDefaultAnyTypeFunc"/>
        ///  wherein <typeparamref name="TIn"/> is the source / from <see cref="Type"/>from which to <see cref="Convert(object,object)"/>, <typeparamref name="TArg"/> is the argument type, 
        ///  ideally a converter-model (see the code-examples) and <typeparamref name="TOut"/> is the target / to <see cref="Type"/> to which to <see cref="Convert(object,object)"/>
        /// </param>
        /// <param name="argument">The optional <seealso cref="Type"/> of the argument passed to <see cref="Converter.FunctionDefault"/>.</param>
        /// <exception cref="ConverterException">Throws a <see cref="ConverterException"/> caused by <see cref="ConverterCause.ConverterArgumentNull"/>
        /// </exception>
        public Converter(MethodInfo converterInfo, Type argument = null)
            : base(typeof(TIn), typeof(TOut), argument ?? typeof(TArg))
        {
            var parameterCount = converterInfo.GetParameters().Length;

            if(converterInfo == null || parameterCount == 0)
            {
                throw new ConverterException(ConverterCause.ConverterArgumentNull);
            }

            if(parameterCount == 1)
            {
                this.FunctionInfo = converterInfo;
                this.ConverterFunc = (Func<TIn, TOut>)converterInfo.CreateDelegate(typeof(Func<TIn, TOut>), null);
                ;

            }
            else if(parameterCount == 2)
            {
                this.FunctionDefaultInfo = converterInfo;
                this.DefaultValueAnyType = true;
                this.ConverterAnyFunc = (Func<TIn, TArg, TOut>)converterInfo.CreateDelegate(typeof(Func<TIn, TArg, TOut>), null);
            }
            else
            {
                throw new ConverterException(ConverterCause.ConverterArgumentDelegateTooManyParameters);
            }
        }

        /// <summary>
        /// Gets or sets the converter default function with any arbitrary second argument-type
        /// </summary>
        /// <seealso cref="ConverterCollectionSettings.DefaultValueAnyType"/>
        /// <seealso cref="Converter.DefaultValueAnyType"/>
        [DataMember]
        public Func<TIn, TArg, TOut> ConverterAnyFunc
        {
            get
            {
                return (Func<TIn, TArg, TOut>)this.FunctionDefault;
            }

            set
            {
                this.FunctionDefault = (object)value;
            }
        }

        /// <summary>
        /// Gets or sets the converter default function
        /// </summary>
        [DataMember]
        public Func<TIn, TOut, TOut> ConverterDefaultFunc
        {
            get
            {
                return (Func<TIn, TOut, TOut>)this.FunctionDefault;
            }

            set
            {
                this.FunctionDefault = (object)value;
            }
        }

        /// <summary>
        /// Gets or sets the converter function
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        [DataMember]
        public Func<TIn, TOut> ConverterFunc
        {
            get
            {
                return (Func<TIn, TOut>)this.Function;
            }

            set
            {
                this.Function = (object)value;
            }
        }

        /// <summary> The <see cref="Converter"/> convert function as part of the <see cref="IConverter"/> interface support. </summary>
        /// <param name="value">The value of <see cref="Type"/> <see cref="TIn"/> to be converted.</param>
        /// <param name="defaultValue">The optional default value of <see cref="Type"/> <see cref="TOut"/>to be passed if the conversion fails or is `null`.</param>
        /// <returns>The converted value as a boxed <see cref="object"/>.</returns>
        /// <remarks>The actual implementation and use of the second parameter lies solely within the scope of the programmer implementing the converter logic</remarks>
        /// <exception cref="ConverterException"> Throws an exception if the argument or converter function is null, or if the argument types mismatch
        /// </exception>
        public override object Convert(object value, object defaultValue = null)
        {
            this.CheckConvertTypes(value, defaultValue);

            try
            {
                if(this.HasDefaultFunctionOnly == true || (defaultValue != null && ObjectExtension.IsDefaultValue<TOut>(defaultValue) == false))
                {
                    return this.ConvertDefault(ref value, ref defaultValue);
                }

                if(this.ConverterFunc != null)
                {
                    if(this.Base != null && this.FunctionAttribute?.IsStatic == false && this.FunctionInfo?.IsStatic == false)
                    {
                        return this.FunctionInfo.Invoke(this.Base, new[] { value });
                    }

                    return this.ConverterFunc.Invoke((TIn)value);
                }
            }
            catch(InvalidCastException)
            {
                throw new ConverterException(ConverterCause.InvalidCast);
            }
            catch(Exception)
            {
                throw;
            }

            throw new ConverterException(ConverterCause.ConverterFunctionNull);
        }

        /// <summary> An helper function for the <see cref="Converter.Convert(object, object)"/> function. </summary>
        /// <param name="value">The value of <see cref="Type"/> <see cref="TIn"/> to be converted.</param>
        /// <param name="defaultValue">The default value of <see cref="Type"/> <see cref="TOut"/>to be passed if the conversion fails or is `null`.</param>
        /// <returns>The converted value as a boxed <see cref="object"/>.</returns>
        /// <remarks>The actual implementation and use of the second parameter lies solely within the scope of the programmer implementing the converter logic</remarks>
        /// <exception cref="ConverterException"> Throws an exception if the argument or converter function is null, or if the argument types mismatch
        /// </exception>
        private object ConvertDefault(ref object value, ref object defaultValue)
        {
            if(this.HasDefaultFunction == true)
            {
                // note: no need to check for FunctionDefaultInfo.IsStatic, but strongly typed invocation is preferred
                if(this.Base != null && this.FunctionDefaultAttribute?.IsStatic == false && this.FunctionDefaultInfo?.IsStatic == false)
                {
                    return this.FunctionDefaultInfo.Invoke(this.Base, new[] { value, defaultValue });
                }
                else if(this.DefaultValueAnyType == true)
                {
                    return this.ConverterAnyFunc.Invoke((TIn)value, (TArg)(defaultValue ?? default(TArg)));
                }
                return this.ConverterDefaultFunc.Invoke((TIn)value, (TOut)defaultValue);
            }
            else if(this.UseFunctionDefaultWrapper == true && this.ConverterFunc != null)
            {
                // Create a default-function wrapper using the existing TIn/TOut converter 
                try
                {
                    Func<TIn, TOut, TOut> defaultWrapper = this.FunctionDefaultWrapper<TIn, TOut>();
                    return (TOut)defaultWrapper((TIn)value, (TOut)defaultValue);
                }
                catch(Exception exc)
                {
                    throw new ConverterException(ConverterCause.ConvertFailed, exc);
                }
            }
            throw new ConverterException(ConverterCause.ConverterFunctionDefaultNull, "A default value exists but no conversion function taking a default-value is defined");
        }


        /// <summary> The converter function that needs to be overwritten as part of the <see cref="IConverter"/> interface support. </summary>
        /// <param name="valueTyped">The value of <see cref="Type"/> <see cref="TIn"/> to be converted.</param>
        /// <param name="defaultValueTyped">The optional default value of <see cref="Type"/> <see cref="TOut"/>to be passed if the conversion fails or is `null`.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <returns>The value converted to <see cref="Type"/> of <see cref="TOut"/> </returns>
        /// <exception cref="ConverterException">Throws an exception of <see cref="ConverterCause.ConverterNotImplemented"/> if the parent class
        ///  does not implement `public override TOut ` <see cref="Convert(object,object)"/>
        /// </exception>
        public TOut Convert(TIn valueTyped, TOut defaultValueTyped = default(TOut))
        {
            return (TOut)this.Convert(value: valueTyped, defaultValue: defaultValueTyped);
        }

        /// <summary> Checks the types being in the correct source and target format, if not exceptions are thrown. </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="defaultValue">The optional default value to be passed if the conversion fails or is `null`.</param>
        /// <exception cref="ConverterException">Throws exceptions based on mismatching types or null references</exception>
        protected override void CheckConvertTypes(object value, object defaultValue)
        {
            if(value == null)
            {
                throw new ConverterException(ConverterCause.ConverterArgumentNull);
            }

            if((value is TIn) == false)
            {
                throw new ConverterException(ConverterCause.ConverterArgumentWrongType, $"Object is not of the type {this.From.FullName}.");
            }

            if(defaultValue != null && this.DefaultValueAnyType == false && (defaultValue is TOut) == false)
            {
                throw new ConverterException(ConverterCause.ConverterArgumentWrongType, $"Object is not of the type {this.To.FullName}.");
            }
        }
    }
}