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
    using System.Text.RegularExpressions;

    using Core.TypeCast;
    using Core.TypeCast.Base;
    using System.Text;
    using System.Collections;


    public class AminoAcids : MonomerCollection<Monomer>
    {
        public Alphabet Alphabet;
        public PolymerType Type;

        /// <summary>
        /// The alphabetically ascending Pyrimidines _Cytosine_, _Thymine_ and _Uracil_ bonding to their complementary _Purines_ via two (T::A, U::A) or 
        /// three (C::G) hydrogen bridges respectively. (Watson-Crick base pairing)
        /// </summary>
        public static char[] Pyrimidines = new[] { 'C', 'T', 'U' };

        /// <summary>
        /// The alphabetically ascending Purines _Adenine_ and _Guanine_ bonding to their complementary Pyrimidines via two (A::T, A::U) or three (G::C) hydrogen bridges 
        /// (Watson-Crick base pairing)
        /// </summary>
        public static char[] Purines = new[] { 'A', 'G' };

        public AminoAcids()
            : base()
        {
            this.AddRange(new List<Monomer> {
                new AminoAcid('A',  89.10, "ALA", "Alanine"),
                new AminoAcid('R', 174.20, "ARG", "Arginine"),
                new AminoAcid('N', 132.12, "ASN", "Asparagine"),
                new AminoAcid('D', 133.10, "ASP", "Aspartame"),
                new AminoAcid('C', 121.16, "CYS", "Cysteine"),
                new AminoAcid('Q', 146.15, "GLN", "Glutamate"),
                new AminoAcid('E', 147.13, "GLU", "Glutamine"),
                new AminoAcid('G',  75.07, "GLY", "Glycine"),
                new AminoAcid('H', 155.16, "HIS", "Histidine"),
                new AminoAcid('I', 131.17, "ILE", "IsoLeucine"),
                new AminoAcid('L', 131.18, "LEU", "Leucine"),
                new AminoAcid('K', 146.19, "LYS", "Lysine"),
                new AminoAcid('M', 149.21, "MET", "Methionine"),
                new AminoAcid('F', 165.19, "PHE", "Phenylalanine"),
                new AminoAcid('P', 115.13, "PRO", "Proline"),
                new AminoAcid('S', 105.09, "SER", "Serine"),
                new AminoAcid('T', 119.12, "THR", "Threonine"),
                new AminoAcid('W', 204.23, "TRP", "Tryptophane"),
                new AminoAcid('Y', 181.19, "TYR", "Tyrosine"),
                new AminoAcid('V', 117.15, "VAL", "Valine"),
                new AminoAcid('_', 000.00, "END", "End"),
            });
            this.Alphabet = new Alphabet("_ACDEFGHIKLMNPQRSTVWY", nameof(PolymerType.RNA), $"{nameof(AminoAcids)}_{nameof(PolymerType.Peptide)}");
            this.Type = PolymerType.Peptide;

            base.Cache();
        }
        
    }
}