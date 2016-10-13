// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace BioCore.Converters
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Core.TypeCast;
    using Core.TypeCast.Base;

    /// <summary>
    /// This class implements the assignment and operation on a single <see cref="DNA"/> <see cref="PolyNucleotide"/> 
    /// sequence of an simple canonical DNA alphabet of `GCAT`. 
    /// </summary>
    /// <remarks>Represents a complementary <see cref="DNA"/> sequence</remarks>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    public class cDNA : DNA
    {
        public cDNA(string sequence)
            : base(sequence)
        {
            this.IsComplementary = true;
        }

        [ConverterMethod(isStatic: false)]
        public DNA Complementary(cDNA seq = null)
        {
            return (DNA)base.Complementary(seq ?? this);
        }

        [ConverterMethod(isStatic: false)]
        public RNA ComplementaryRNA(cDNA seq = null)
        {
            return (RNA) base.Convert((DNA)base.Complementary(seq ?? this));
        }
    }
}