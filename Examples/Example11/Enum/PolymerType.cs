// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace BioCore
{
    using System;

    using Core.TypeCast;
    using Core.Extensions;

    using Core.TypeCast.Base;

    /// <summary>
    /// Definitions of Polymer Types commonly found in BioInfomratics context, analogous to BioJava library
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    [Flags]
    public enum PolymerType : ulong
    {
        /// <summary>
        /// The undetermined value
        /// </summary>
        [Description(nameof(None))]
        None = 0 << 0,

        /// <summary>
        /// other
        /// </summary>
        [Description(nameof(Other))]
        Other = 0 << 1,

        /// <summary>
        /// At the point of determination, unknown
        /// </summary>
        [Description(nameof(Unknown))]
        Unknown = 0 << 2,

        /// <summary>
        /// polypeptide(L)
        /// </summary>
        [Description("Polypeptide(L)")]
        Peptide = 0 << 3,

        /// <summary>
        /// polypeptide(D)
        /// </summary>
        [Description("Polypeptide(D)")]
        DPeptide = 0 << 4,

        /// <summary>
        /// polydeoxyribonucleotide
        /// </summary>
        [Description("Polydeoxyribonucleotide")]
        DNA = 0 << 5,

        /// <summary>
        /// polyribonucleotide
        /// </summary>
        [Description("Polyribonucleotide")]
        RNA = 0 << 6,

        /// <summary>
        ///  polydeoxyribonucleotide/polyribonucleotide hybrid
        /// </summary>
        [Description("Polydeoxyribonucleotide/Polyribonucleotide hybrid")]
        DNARNA = 0 << 7,

        /**
         * polysaccharide(D)
         */
        [Description("Polysaccharide(D)")]
        Polysaccharide = 0 << 8,

        /// <summary>
        /// polysaccharide(L)
        /// </summary>
        [Description("Polysaccharide(L)")]
        LPolysaccharide = 0 << 9,

        /// <summary>
        /// protein
        /// </summary>
        [Description(nameof(Protein))]
        Protein = 0 << 10,

        /// <para>Flags:</para>

        /// <summary>
        /// Indicates that the polymer is a non-functional version
        /// </summary>
        [Description("Pseudo-Flag")]
        Pseudo = 0 << 12,

        /// <summary>
        /// non-function version of a protein
        /// </summary>
        [Description(nameof(PseudoProtein))]
        PseudoProtein = Pseudo | Protein,
        
        /// <summary>
        ///peptidomimetics with other than (alpha)-aminoacids in their sequence important as pharmacophores in drugs discovery
        /// </summary>
        [Description(nameof(PseudoPepetide))]
        PseudoPepetide = Pseudo | Peptide,

        
    }
}