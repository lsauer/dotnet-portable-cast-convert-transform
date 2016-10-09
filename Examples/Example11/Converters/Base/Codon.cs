// <copyright file=mitlicense.md url=http,//lsauer.mit-license.org/ >
//             Lo Sauer}, {2013-2016
// </copyright>
// <summary>   A tested}, {generic}, {portable}, {runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https,//github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace BioCore.Converters
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Core.TypeCast;
    using Core.TypeCast.Base;

    using Core.Extensions;
    using System.Text;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The single unit of the genetic code comprising three nucleotides which is paramount for <see cref="Protein"/> synthesis .
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    public struct Codon : IValue<string>
    {
        private readonly string value;

        public const string Start = "ATG";

        public static string[] Stop = new[] { "TAG", "TGA", "TAA" };

        private static AminoAcids aminoAcids = new AminoAcids();

        /// <summary>
        /// The codon table
        /// </summary>
        public static Dictionary<string, char> Codons = new Dictionary<string, char> {
            {"ATA", 'I'}, {"ATC", 'I'}, {"ATT", 'I'}, {"ATG", 'M'},
            {"ACA", 'T'}, {"ACC", 'T'}, {"ACG", 'T'}, {"ACT", 'T'},
            {"AAC", 'N'}, {"AAT", 'N'}, {"AAA", 'K'}, {"AAG", 'K'},
            {"AGC", 'S'}, {"AGT", 'S'}, {"AGA", 'R'}, {"AGG", 'R'},
            {"CTA", 'L'}, {"CTC", 'L'}, {"CTG", 'L'}, {"CTT", 'L'},
            {"CCA", 'P'}, {"CCC", 'P'}, {"CCG", 'P'}, {"CCT", 'P'},
            {"CAC", 'H'}, {"CAT", 'H'}, {"CAA", 'Q'}, {"CAG", 'Q'},
            {"CGA", 'R'}, {"CGC", 'R'}, {"CGG", 'R'}, {"CGT", 'R'},
            {"GTA", 'V'}, {"GTC", 'V'}, {"GTG", 'V'}, {"GTT", 'V'},
            {"GCA", 'A'}, {"GCC", 'A'}, {"GCG", 'A'}, {"GCT", 'A'},
            {"GAC", 'D'}, {"GAT", 'D'}, {"GAA", 'E'}, {"GAG", 'E'},
            {"GGA", 'G'}, {"GGC", 'G'}, {"GGG", 'G'}, {"GGT", 'G'},
            {"TCA", 'S'}, {"TCC", 'S'}, {"TCG", 'S'}, {"TCT", 'S'},
            {"TTC", 'F'}, {"TTT", 'F'}, {"TTA", 'L'}, {"TTG", 'L'},
            {"TAC", 'Y'}, {"TAT", 'Y'}, {"TAA", '_'}, {"TAG", '_'},
            {"TGC", 'C'}, {"TGT", 'C'}, {"TGA", '_'}, {"TGG", 'W'},
        };

        public static Dictionary<string, string[]> CodonsReverse = new Dictionary<string, string[]> {
              {"PHE", new[] {"TTT", "TTC"} },
              {"LEU", new[] {"TTA", "TTG", "CTT", "CTC", "CTA", "CTG"} },
              {"ILE", new[] {"ATT", "ATC", "ATA"} },
              {"MET", new[] {"ATG"} },
              {"VAL", new[] {"GTT", "GTC", "GTA", "GTG"} },
              {"SER", new[] {"TCT", "TCC", "TCA", "TCG", "AGT", "AGC"} },
              {"PRO", new[] {"CCT", "CCC", "CCA", "CCG"} },
              {"THR", new[] {"ACT", "ACC", "ACA", "ACG"} },
              {"ALA", new[] {"GCT", "GCC", "GCA", "GCG"} },
              {"TYR", new[] {"TAT", "UAC"} },
              {"HIS", new[] {"CAT", "CAC"} },
              {"GLN", new[] {"CAA", "CAG"} },
              {"ASN", new[] {"AAT", "AAC"} },
              {"LYS", new[] {"AAA","AAG"} },
              {"ASP", new[] {"GAT", "GAC"} },
              {"GLU", new[] {"GAA", "GAG"} },
              {"CYS", new[] {"TGT", "TGC"} },
              {"TRP", new[] {"TGG"} },
              {"ARG", new[] {"CGT", "CGC", "CGA", "CGG", "AGA", "AGG"} },
              {"GLY", new[] {"GGT", "GGC", "GGA", "GGG"} },
              {"END", new[] {"UAA", "UAG", "TGA"} },
              {"___", new[] { "END" } },
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Letter AminoAcid(Codon key)
        {
            if(Codons.ContainsKey(key) == true)
            {
                return Codons[key];
            }
            return default(char);
        }

        public Codon(string tripleNucleotide)
        {
            if(tripleNucleotide.Length != 3)
            {
                throw new ConverterException(ConverterCause.BadInputFormat);
            }
            this.value = tripleNucleotide;
        }

        public string Value { get { return value; } }

        public PhysicalUnit Unit { get { return PhysicalUnit.Codon; } }

        public static implicit operator Codon(string val)
        {
            return new Codon(val);
        }

        public static implicit operator string(Codon val)
        {
            return val.Value;
        }

        public static implicit operator Letter(Codon val)
        {
            return Codon.AminoAcid(val);
        }

        public static implicit operator AminoAcid(Codon val)
        {
            return (AminoAcid)aminoAcids[(Letter)val];
        }

        //public override string ToString()
        //{
        //    return $"{this.Value} [{this.Unit.GetDescription()}]";
        //}
    }
}