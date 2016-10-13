// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast.Test
{
    using Core.TypeCast;

    /// <summary>
    /// This class demonstrates loading converters through the `Initialize` interface via explicit instancing of the `ConverterCollection`
    /// </summary>
    public class ConverterStringToDecimal
    {
        /// <summary>
        /// Parses an input string to a decimal value
        /// </summary>
        /// <param name="value">The string input value</param>
        /// <param name="defaultValue">A default output decimal value if the conversion failed</param>
        /// <returns>Returns the converted decimal value</returns>
        [ConverterMethod]
        public decimal StringToDecimal(string value, decimal defaultValue = 0M)
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
        /// A dummy non-converting method, which will not be added since it is not attributed by `ConverterMethodAttribute`
        /// </summary>
        /// <param name="status"></param>
        public static void NonConverterMethod(bool status)
        {
            throw new System.NotImplementedException();
        }
    }
}