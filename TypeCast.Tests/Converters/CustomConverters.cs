// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example1.Converters
{
    using Core.TypeCast;

    /// <summary>
    /// This class demonstrates how to implement custom converters converting from `object` as well as `string` to `decimal`
    /// </summary>
    [Converter(loadOnDemand: false, nameSpace: nameof(System))]
    public class CustomConverter
        : IConverter, 
          IConverter<object, decimal>, 
          IConverter<string, decimal>
    {
        /// <summary>
        /// Unboxes an input object to a decimal value. Won't be registered since the converter is already included in the library
        /// </summary>
        /// <param name="value">The boxed input value</param>
        /// <param name="defaultValue">A default output decimal value if the conversion failed</param>
        /// <returns>Returns the converted decimal value</returns>
        [ConverterMethod]
        public decimal Convert(object value, decimal defaultValue = 0M)
        {
            return this.Convert(value != null ? value.ToString() : string.Empty, defaultValue);
        }


        /// <summary>
        /// Parses an input string to a decimal value
        /// </summary>
        /// <param name="value">The string input value</param>
        /// <param name="defaultValue">A default output decimal value if the conversion failed</param>
        /// <returns>Returns the converted decimal value</returns>
        [ConverterMethod]
        public decimal Convert(string value, decimal defaultValue = 0M)
        {
            if (value != null)
            {
                var s = value.Trim();
                decimal n;
                if (decimal.TryParse(s, out n))
                {
                    return n;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// A simple static converter method example
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [ConverterMethod]
        public static int Weight(int number)
        {
            return 100 + number;
        }

        /// <summary>
        /// stump to satisfy declaring support for IConverter
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual object Convert(object value, object defaultValue = null)
        {
            throw new System.NotImplementedException();
        }
    }
}