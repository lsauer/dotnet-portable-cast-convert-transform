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

    /// <summary>
    /// This class implements the assignment and operation on a single <see cref="RNA"/> <see cref="PolyNucleotide"/> 
    /// sequence of a simple canonical RNA alphabet of `GCAU`. 
    /// </summary>
    /// <remarks>Represents a complementary <see cref="RNA"/> sequence</remarks>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    public class cRNA : RNA
    {
        public cRNA(string sequence)
            : base(sequence)
        {
            this.IsComplementary = true;
        }

        [ConverterMethod(isStatic: false)]
        public RNA Complementary(cRNA seq = null)
        {
            return (RNA)base.Complementary(seq ?? this);
        }

        [ConverterMethod(isStatic: false)]
        public DNA ComplementaryDNA(cRNA seq = null)
        {
            return (DNA) base.Convert((RNA)base.Complementary(seq ?? this));
        }
    }
}