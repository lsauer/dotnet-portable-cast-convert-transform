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
    using System.Linq;
    using System.Reflection;

    using Core.TypeCast.Base;

    /// <summary>
    /// The <see cref="Converter"/> extension methods
    /// </summary>
    public static class ConverterExtension
    {
        /// <summary>
        /// Wraps the converter function <see cref="Converter.Function"/> to allow passing of a default-value, which is returned when the conversion yields `null`
        /// </summary>
        /// <typeparam name="TIn">The source / from <see cref="Type"/>from which to convert</typeparam>
        /// <typeparam name="TOut">The target / to <see cref="Type"/> to which to converter</typeparam>
        /// <param name="self">the current instance of the <see cref="Core.TypeCast.Base.Converter"/></param>
        /// <returns>Returns a function wrapper which takes a default-value of type <typeparamref name="TOut"/> as a second parameter </returns>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException"/>if the conversion fails</exception>
        /// <example>  **Example:** Get a default-value accepting wrapper function from a converter and invoke it.
        /// <code>
        /// ```cs
        ///     var converter = new Converter&lt;int?, float>((int? a) => {
        ///                return (float)(a ?? 0);
        ///     });
        ///     Func&lt;int?, float, float> intNullToSingleWithDefaultValue = converter.FunctionDefaultWrapper&lt;int?, float>();
        ///     var singleValue = intNullToSingleWithDefaultValue(new int?(5), 5.0f);
        /// ```
        /// </code></example>
        /// 
        /// <remarks>Use <see cref="Converter{TIn,TOut}"/> instead for the typed converter implementation.</remarks>
        public static Func<TIn, TOut, TOut> FunctionDefaultWrapper<TIn, TOut>(this Converter self)
        {
            if (self.Function == null)
            {
                return null;
            }
            Func<TIn, TOut> converter = (Func<TIn, TOut>)self.Function;
            Func<TIn, TOut, TOut> converterDefault = (value, defaultValue) =>
                {
                    TOut retvalue = default(TOut);
                    try
                    {
                        var result = converter(value);
                        retvalue = Equals(default(TOut), result) == true ? defaultValue : result;
                    }
                    catch (Exception exc)
                    {
                        throw new ConverterException(ConverterCause.ConverterWrapperError, exc);
                    }
                    return retvalue;
                };
            return converterDefault;
        }

        /// <summary>
        /// Wraps the converter function <see cref="Converter.Function"/> to allow passing of a default-value, which is returned when the conversion yields `null`
        /// </summary>
        /// <typeparam name="TIn">The source / from <see cref="Type"/>from which to convert</typeparam>
        /// <typeparam name="TOut">The target / to <see cref="Type"/> to which to converter</typeparam>
        /// <param name="self">the current instance of the <see cref="Core.TypeCast.Base.Converter"/></param>
        /// <returns>Returns a function wrapper which takes a default-value of type <typeparamref name="TOut"/> as a second parameter </returns>
        /// <exception cref="ConverterException">Throws an exception of type <see cref="ConverterException"/>if the conversion fails</exception>
        /// <remarks>Use <see cref="Converter"/> instead for the untyped base-converter implementation.</remarks>
        /// See <seealso cref="FunctionDefaultWrapper{TIn, TOut}(Converter)"/> for the logical implementation
        public static Func<TIn, TOut, TOut> FunctionDefaultWrapper<TIn, TOut>(this Converter<TIn, TOut> self)
        {
            return (self as Converter)?.FunctionDefaultWrapper<TIn, TOut>();
        }

        /// <summary>
        /// If possible, the method merges a second Converter instance <paramref name="converter"/> into the own instance, and returns <see langword="bool"/> `true` if successful, else `false`
        /// </summary>
        /// <param name="self">the current instance of the <see cref="Core.TypeCast.Base.Converter"/></param>
        /// <param name="converter">the other converter instance of which to look up the function and assign it to the own instance</param>
        /// <exception cref="ConverterCollectionException">Raises an exception if both converters have either their <see cref="Converter.Function"/> or 
        /// <see cref="Converter.FunctionDefault"/> assigned, with the cause: <see cref="ConverterCollectionCause.ConverterExists"/></exception>
        /// <returns>Returns <see langword="bool"/> `true` if successful in merging, else `false`.</returns>
        public static bool MergeStandard(this Converter self, Converter converter)
        {
            // check that either converter has a Function and DefaultFunction set
            if(self != null && self.Standard == true)
            {
                if((self.HasFunction == converter.HasFunction) && (self.HasDefaultFunction == converter.HasDefaultFunction))
                {
                    throw new ConverterCollectionException(ConverterCollectionCause.ConverterExists, self.ToString());
                }

                // merge the two converters together by using the existing and discarding the other
                if(self.HasDefaultFunction == false)
                {
                    self.FunctionDefault = converter.FunctionDefault;
                }
                else
                {
                    self.Function = converter.Function;
                }

                self.Attribute = self.Attribute ?? converter.Attribute;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets default settings derived from a <see cref="ConverterCollection"/> instance whose reference is acquired via <see cref="Converter.Collection"/>
        /// </summary>
        /// <param name="self">the current instance of the <see cref="Core.TypeCast.Base.Converter"/></param>
        /// <param name="collection">An instance reference to a <see cref="ConverterCollection"/></param>
        /// <returns><see langword="bool"/> `true` if successful, `false` if <see cref="Converter.Collection"/> is null.</returns>
        public static bool SetCollectionDefaults(this Converter self, ConverterCollection collection = null)
        {
            if(collection != null)
            {
                // add a collection reference to the converter
                self.Collection = collection;
            }

            if(self.Collection == null)
            {
                // set common collection settings according to the ConverterCollectionSettings
                self.DefaultValueAnyType |= self.Collection.Settings.DefaultValueAnyType;
                self.UseFunctionDefaultWrapper |= self.Collection.Settings.UseFunctionDefaultWrapper;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to set the <see cref="Converter.BaseType"/> as the declaring-type (<see cref="Type.DeclaringType"/> of the underlying converter-function.
        /// </summary>
        /// <param name="self">the current instance of the <see cref="Core.TypeCast.Base.Converter"/></param>
        /// <param name="baseType">The declaring-type (<see cref="Type.DeclaringType"/> of the underlying converter-function.</param>
        public static void SetBaseType(this Converter self, Type baseType)
        {
            if(self.BaseType == null)
            {
                self.BaseType = baseType.GetTypeInfo();
            }
        }

        /// <summary>
        /// Looks up converters with the same <see cref="Converter.From"/> Type
        /// </summary>
        /// <param name="self"></param>
        /// <returns>Returns an <see cref="IQueryable{Converter}"/> of converters with the same <see cref="Converter.To"/> Type</returns>
        public static IQueryable<Converter> WithSameFromType(this Converter self)
        {
            if(self.Collection != null)
            {
                return self.Collection.Items.AsQueryable().WithFrom(self.From);
            }
            return null;
        }

        /// <summary>
        /// Looks up converters with the same <see cref="Converter.To"/> Type
        /// </summary>
        /// <param name="self">the current instance of the <see cref="Core.TypeCast.Base.Converter"/></param>
        /// <returns>Returns an <see cref="IQueryable{Converter}"/> of converters with the same <see cref="Converter.To"/> Type</returns>
        public static IQueryable<Converter> WithSameToType(this Converter self)
        {
            if(self.Collection != null)
            {
                return self.Collection.Items.AsQueryable().WithTo(self.To);
            }
            return null;
        }

        /// <summary>
        /// Creates and sets a new <see cref="ConverterAttribute"/> with the information provided by the attributed parent class as well as <see cref="ConverterMethodAttribute"/> 
        /// </summary>
        /// <param name="self">the current instance of the <see cref="Core.TypeCast.Base.Converter"/></param>
        /// <param name="methodInfo">A <see cref="MethodInfo"/> of the converter function to be added, which is attributed by a <see cref="ConverterMethodAttribute"/> </param>
        /// <remarks>
        /// Used to set a function-alias or Base-Type delegate for <see cref="ObjectExtension.Transform{TBase, TOut}(object, object, string, bool, bool)"/> lookup
        /// </remarks>
        public static void MergeFromMethodAttribute(this Converter self, MethodInfo methodInfo)
        {
            var classAttribute = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<ConverterAttribute>();
            var methodAttribute = methodInfo.GetCustomAttribute<ConverterMethodAttribute>();

            // set function-alias for unique transformation function lookup
            self.Attribute = new ConverterAttribute(loadOnDemand: classAttribute?.LoadOnDemand ?? false, 
                name: String.IsNullOrWhiteSpace(classAttribute?.Name) ? methodAttribute?.Name : classAttribute.Name,
                nameSpace: methodInfo.DeclaringType.Namespace, 
                dependencyInjection: classAttribute?.DependencInjection ?? false);

            // set base-type delegate type used for unique transformation-lookup
            if(methodAttribute?.BaseType != null)
            {
                self.BaseType = self.BaseType ?? methodAttribute.BaseType.GetTypeInfo();
            }
        }
    }
}