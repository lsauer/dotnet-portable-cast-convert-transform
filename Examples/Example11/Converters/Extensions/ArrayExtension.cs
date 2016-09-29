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
    public static partial class ArrayExtension
    {
        public static IEnumerable<Letter> ToLetters(this char[] self)
        {
            for(int i = 0; i < self.Length; i++)
            {
                yield return self[i];
            }
        }

        public static List<char> ToCharList(this IEnumerable<Letter> self)
        {
            var list = new List<char>();
            foreach(var letter in self)
            {
                list.Add(letter);
            }
            return list;
        }

        public static char[] ToCharArray(this IEnumerable<Letter> self)
        {
            return self.ToCharList().ToArray();
        }

        //public static bool IsAllPurine(this IEnumerable<Letter> self)
        //{
        //    var candidates = self.ToCharList().Distinct().ToArray();
        //    Array.Sort<char>(candidates);
        //    var purines = new string(AminoAcids.Purines);

        //    var cmp = new string(candidates.ToArray());
        //    return purines.Contains(cmp.ToUpperInvariant());
        //}

        public static bool IsAllPurine(this IEnumerable<Letter> self)
        {
            foreach(var letter in self)
            {
                if(letter == 'A' || letter == 'G' || letter == 'a' || letter == 'g')
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsAllPyrimidine(this IEnumerable<Letter> self)
        {
            foreach(var letter in self)
            {
                if(letter == 'C' || letter == 'T' || letter == 'U' || letter == 'c' || letter == 't' || letter == 'u')
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }

}