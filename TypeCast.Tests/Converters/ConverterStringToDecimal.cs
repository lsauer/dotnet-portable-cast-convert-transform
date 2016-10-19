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
    using System.Globalization;

    using Core.TypeCast;

    /// <summary>
    /// This class demonstrates loading converters through the `Initialize` interface via explicit instancing of the `ConverterCollection`
    /// </summary>
    [Converter]
    public class ConverterStringToDecimal
    {
        /// <summary>
        /// Parses an input string to a decimal value
        /// </summary>
        /// <param name="value">The string input value</param>
        /// <returns>Returns the converted decimal value</returns>
        [ConverterMethod]
        public decimal StringToDecimal(string value)
        {
            if (value != null)
            {
                var nf = ConverterCollection.CurrentInstance.Settings.NumberFormat;
                var s = value.Trim();
                return decimal.Parse(s, nf);
            }
            return default(decimal);
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