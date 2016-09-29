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
    using System.Collections.Generic;

    using Core.TypeCast;
    using Core.TypeCast.Base;
    using System.Text.RegularExpressions;
    using System.Text;

    /// <summary>
    /// Extension methods for <see cref="Alphabet"/>
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    public static class MonomerExtension
    {
        public static Alphabet GetAlphabet(this IEnumerable<Monomer> self, string name = null, string fullName = null)
        {
            var mAlphabet = new StringBuilder();
            foreach(var monomer in self)
            {
                mAlphabet.Append(monomer.Letter);
            }
            return new Alphabet(mAlphabet.ToString(), name, fullName);
        }
    }

}