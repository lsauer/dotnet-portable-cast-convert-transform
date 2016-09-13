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
    /// <summary>
    /// The Base Converter interface.
    /// </summary>
    /// <example>
    /// <code>
    /// ```cs
    ///    [Converter]
    ///    public class CustomConverters : IConverter, IConverter<object, decimal>, IConverter<string, decimal>
    ///    {
    ///         public decimal Convert(object value, decimal defaultValue)
    ///         {
    ///            return this.Convert(value != null ? value.ToString() : string.Empty, defaultValue);
    ///         }
    ///    }
    /// ```
    /// </code>
    /// </example>
    /// <seealso cref="Converter"/>
    /// <remarks>It is not required, but recommended for a custom converter to support the common <see cref="IConverter"/> interface 
    /// - depending on the converter design guidelines followed, as laid out in the project documentation </remarks>
    public interface IConverter
    {
        /// <summary>
        /// The convert method for converting a boxed <see cref="value"/> to another boxed value-<see cref="object"/>.
        /// </summary>
        /// <param name="value">The boxed value of the underlying source-<see cref="System.Type"/> which is to be converted to a boxed value of the target-<see cref="System.Type"/>.</param>
        /// <param name="defaultValue"> The default value which may be used by the <see cref="Convert(object, object)"/>function if the conversion fails or is `null`</param>
        /// <returns>The boxed <see cref="object"/> of the target-type.</returns>
        /// <remarks>It is not required for a custom converter to support the common <see cref="IConverter"/> interface </remarks>
        object Convert(object value, object defaultValue = null);
    }

    /// <summary>
    /// This interface defines strictly the conversion and methods between two specified types. 
    /// </summary>
    /// <typeparam name="TIn">The Source- / From- <see cref="System.Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
    /// <typeparam name="TOut">The Target / To- <see cref="System.Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
    /// <remarks>It is not necessary for a custom converter implementation to support the generic <see cref="IConverter{TIn,TOut}"/> interface, but is highly 
    /// recommended for readability future-proofing, and automatic attribute assignment through <see cref="System.Reflection"/>
    /// </remarks>
    /// <seealso cref="ConverterCollection.GetConverterAttributeFromIConverter(System.Reflection.TypeInfo,Core.TypeCast.ConverterAttribute,bool)"/>
    public interface IConverter<in TIn, TOut>
    {
        /// <summary>
        /// The method to converts between two specified types
        /// </summary>
        /// <param name="value">The value of the underlying source-<see cref="System.Type"/> <typeparamref name="TIn"/> which is to be converted to a value of the 
        /// target-<see cref="System.Type"/> <typeparamref name="TOut"/>.</param>
        /// <param name="defaultValue"> The default value which may be used by the <see cref="Convert(TIn, TOut)"/> function if the conversion fails or is `null`</param>
        /// <returns>Returns destination value type <typeparamref name="TOut"/></returns>
        TOut Convert(TIn value, TOut defaultValue = default(TOut));
    }
}