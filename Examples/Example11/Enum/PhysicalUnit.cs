// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace BioCore
{
    using System;

    using Core.TypeCast;
    using Core.Extensions;

    using Core.TypeCast.Base;

    /// <summary>
    /// Definitions of physical Units
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    public enum PhysicalUnit
    {
        /// <summary>
        /// Dalton, the unit used in expressing macromolecular weights of biopolymers, equivalent to atomic mass unit.
        /// </summary>
        [Description("Dalton")]
        Da,

        /// <summary>
        /// Degress Celcsius, equivalent to SI Unit Scale of Kelvin
        /// </summary>
        [Description("°Celsius")]
        Cs,

        /// <summary>
        /// Codon, the unit of the genetic code, comprised out of three nucleotides
        /// </summary>
        [Description("Codon")]
        Codon,

        /// <summary>
        /// Other
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