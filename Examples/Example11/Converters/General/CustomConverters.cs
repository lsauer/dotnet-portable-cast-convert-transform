// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace BioCore.Converters
{
    using System;
    using System.Linq;
    using Core.TypeCast;

    /// <summary>
    /// Additional, general converters used throughout the project
    /// </summary>
    [Converter]
    public class CustomConverter
    {
        /// <summary>
        /// Parses an input string that represents a series of integers, separated by a single repeating character, into an array of integer numbers
        /// </summary>
        /// <param name="value">The string input value</param>
        /// <param name="separator">A default separator character. Default is a dot</param>
        /// <returns>Returns a series of integer values as an array</returns>
        [ConverterMethod]
        public int[] ParseToIntegerArray(string value, char separator = '.')
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            var numbers = value.Split(separator)?.Select(int.Parse).ToArray();
            return numbers;
        }
        
    }
}