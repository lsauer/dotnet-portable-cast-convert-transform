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
    using System.Drawing;

    using Core.TypeCast;
    using Core.TypeCast.Base;
    using System.Text.RegularExpressions;
    /// <summary>
    /// Extension methods to facilitate the discovery of open reading frames <see cref="ORF"/> on a single <see cref="PolyNucleotide"/> sequence 
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    public static class DNAExtension
    {
        public static readonly RegexOptions RegexOptions = 
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant;

        public static IEnumerable<int> StartIndex(this DNA self)
        {
            var sequence = self.Sequence;
            var matches = Regex.Matches(sequence, $@"(?=({Codon.Start}))", RegexOptions);
            return matches.Cast<Match>().Select(m => m.Index).ToList();
        }

        public static IEnumerable<int> StopIndex(this DNA self)
        {
            var sequence = self.Sequence;
            var matches = Regex.Matches(sequence, $@"(?=({string.Join("|", Codon.Stop)}))", RegexOptions);
            return matches.Cast<Match>().Select(m => m.Index).ToList();
        }

        public static IEnumerable<int> StopIndex(this DNA self, int startIndex)
        {
            var seq = self.Sequence.Substring(startIndex);
            for(int i = 0; i < seq.Length - 2; i += 3)
            {
                var frame = seq.Substring(i, 3);
                if(Codon.Stop.Contains(frame) == true)
                {
                    yield return startIndex + i + 3;
                }
            }
        }

        public static IEnumerable<Converters.ORF> FindORFs(this DNA self, int minLength = 15, int maxLength = 0)
        {
            var list = new List<Converters.ORF>();
            list.AddRange(self.FindORFsUnidirectional(minLength, maxLength));
            list.AddRange(self.Complementary().FindORFsUnidirectional(minLength, maxLength));
            return list;
        }

        public static IEnumerable<Converters.ORF> FindORFsUnidirectional(this DNA self, int minLength = 15, int maxLength = 0, bool continueAfterStopCodon = false)
        {
            var sequence = self.Sequence;
            var list = new List<Converters.ORF>();
            foreach(var start in self.StartIndex())
            {
                foreach(var stop in self.StopIndex(start))
                {
                    var len = stop - start;
                    if(len < 0)
                    {
                        continue;
                    }
                    if(len % 3 != 0)
                    {
                        var mod = len % 3;
                        throw new Exception($"Out of reading-Frame. Length must be modulo 3! actual: {mod}");
                    }
                    if((len < minLength) || ((maxLength > 0) && (len > maxLength)))
                    {
                        break;
                    }
                    // check that the stop-codon occurs within a reading frame, and that the lengths are within bounds
                    //if( (len >= minLength) && (maxLength == 0 || (len < maxLength)))
                    {
                        var seq = sequence.Substring(start, len);
                        list.Add(new Converters.ORF(seq));
                        if(continueAfterStopCodon == false)
                        {
                            break;
                        }
                    }
                }
            }
            return list;
        }
    }

}