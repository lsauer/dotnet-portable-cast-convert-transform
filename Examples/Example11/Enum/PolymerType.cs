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
    public enum PolymerType
    {
        /// <summary>
        /// polypeptide(L)
        /// </summary>
        [Description("Polypeptide(L)")]
        Peptide,

        /// <summary>
        /// polypeptide(D)
        /// </summary>
        [Description("Polypeptide(D)")]
        DPeptide,

        /// <summary>
        /// polydeoxyribonucleotide
        /// </summary>
        [Description("Polydeoxyribonucleotide")]
        DNA,

        /// <summary>
        /// polyribonucleotide
        /// </summary>
        [Description("Polyribonucleotide")]
        RNA,

        /// <summary>
        ///  polydeoxyribonucleotide/polyribonucleotide hybrid
        /// </summary>
        [Description("Polydeoxyribonucleotide/Polyribonucleotide hybrid")]
        DNARNA,

        /**
         * polysaccharide(D)
         */
        [Description("Polysaccharide(D)")]
        Polysaccharide,

        /// <summary>
        /// polysaccharide(L)
        /// </summary>
        [Description("Polysaccharide(L)")]
        LPolysaccharide,

        /// <summary>
        /// other
        /// </summary>
        [Description(nameof(Other))]
        Other,

        /// <summary>
        /// if the type is not determinable or unknown
        /// </summary>
        [Description(nameof(Unknown))]
        Unknown,
    }
}