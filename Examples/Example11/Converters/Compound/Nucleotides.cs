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
    using System.Text.RegularExpressions;

    using Core.TypeCast;
    using Core.TypeCast.Base;
    using System.Text;
    using System.Collections;
    using System.Linq.Expressions;

    public class Nucleotides : MonomerCollection<Monomer>
    {
        public Alphabet Alphabet;
        public PolymerType Type;

        /// <summary>
        /// Abbreviation Reference: http://www.pnas.org/site/misc/table1.pdf
        /// </summary>
        public Nucleotides(PolymerType? type = null)
            : base()
        {
            this.AddRange(new List<Monomer> {
                new Nucleotide('A', 313.2, "Ade", "Adenine"),
                new Nucleotide('C', 289.2, "Cyt", "Cytosine"),
                new Nucleotide('G', 329.2, "Gua", "Guanine"),
            });
            if(type == PolymerType.RNA)
            {
                this.Add(new Nucleotide('U', 228.2, "Ura", "Uracil"));
                this.Alphabet = new Alphabet("ACGU", nameof(PolymerType.RNA), $"{nameof(Nucleotides)}_{nameof(PolymerType.RNA)}");
                this.Type = PolymerType.RNA;
            }
            else if(type == PolymerType.DNA)
            {
                this.Add(new Nucleotide('T', 304.2, "Thy", "Thymine"));
                this.Alphabet = new Alphabet("ACGT", nameof(PolymerType.DNA), $"{nameof(Nucleotides)}_{nameof(PolymerType.DNA)}");
                this.Type = PolymerType.DNA;
            }else if(type == PolymerType.DNARNA || type == null)
            {
                this.Add(new Nucleotide('U', 228.2, "Ura", "Uracil"));
                this.Add(new Nucleotide('T', 304.2, "Thy", "Thymine"));
                this.Alphabet = new Alphabet("ACGTU", nameof(PolymerType.DNARNA), $"{nameof(Nucleotides)}_{nameof(PolymerType.DNARNA)}");
                this.Type = PolymerType.DNARNA;
            }

            base.Cache();
        }

        public Nucleotides(string alphabet)
            : this(GetPolymerType(alphabet))
        {
        }

        public static PolymerType GetPolymerType(string alphabet)
        {
            var alphabetOrdered = new string((from c in alphabet?.ToUpperInvariant() orderby c select c).ToArray());
            if(alphabetOrdered == "ACGTU")
            {
                return PolymerType.DNARNA;
            }
            else if(alphabetOrdered == "ACGT")
            {
                return PolymerType.DNA;
            }
            else
            {
                return PolymerType.RNA;
            }
        }
    }
}