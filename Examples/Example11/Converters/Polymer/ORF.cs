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

    /// <summary>
    /// This class implements the assignment and operation on an Open-Reading Frame <see cref="PolyNucleotide"/> sequence 
    /// of a simple canonical <see cref="DNA"/> alphabet of GCAT. 
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    [Converter]
    public class ORF : DNA
    {
        public ORF()
            : base()
        {
        }

        public ORF(string sequence)
            : base(sequence)
        {
            if(sequence.Length > 6)
            {
                var seqHead = sequence.ToUpperInvariant().Substring(0, 3);
                var seqTail = sequence.ToUpperInvariant().Substring(sequence.Length - 3, 3);
                if(seqHead != Codon.Start && Codon.Stop.Contains(seqTail) == false)
                {
                    throw new Exception("Invalid ORF sequence!");
                }
            }
        }

        [ConverterMethod]
        public Protein ToProtein()
        {
            var protein = new StringBuilder();
            foreach(var codon in this.Codons)
            {
                protein.Append((Letter)codon);
            }
            return new Protein(protein.ToString());
        }

        public static implicit operator ORF(string val)
        {
            return new ORF(val);
        }

        public static implicit operator string(ORF val)
        {
            return val.ToString();
        }

        public override string ToString()
        {
            return $"{nameof(ORF)}: [{this.Sequence}]";
        }

    }
}