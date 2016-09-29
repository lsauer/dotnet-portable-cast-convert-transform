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

    using Core.TypeCast;
    using Core.TypeCast.Base;

    /// <summary>
    /// This class implements the assignment and operation on a single <see cref="PolyNucleotide"/> sequence of a simple canonical DNA alphabet of `ACGT`. 
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    [Converter(loadOnDemand: false, nameSpace: nameof(Converters), dependencyInjection: false)]
    public class DNA : PolyNucleotide,
                        IEnumerable<Codon>,
                        IConverter<string, DNA>,
                        IConverter<DNA, string>
    {
        public delegate string ConsensusSequence(Protein protein);

        public DNA()
            : this(string.Empty)
        {
        }

        public DNA(string sequence, MonomerCollection<Monomer> monomers = null)
            : base(sequence: sequence, monomers: monomers ?? new DeoxyribonucleicAcids())
        {
            this.Type = PolymerType.DNA;
        }

        #region IConverter<,> support

        [ConverterMethod]
        public DNA Convert(string value, DNA defaultValue = null)
        {
            return new DNA(value);
        }

        [ConverterMethod]
        public string Convert(DNA value, string defaultValue = null)
        {
            return value?.Sequence;
        }

        [ConverterMethod]
        public RNA Convert(DNA value)
        {
            return new RNA(value?.Sequence.ToUpperInvariant().Replace('T', 'U'));
        }
        #endregion

        [ConverterMethod(isStatic: false)]
        public cDNA Complementary(DNA seq = default(DNA))
        {
            return base.Complementary<DNA, cDNA>(seq);
        }

        [ConverterMethod(isStatic: false)]
        public cRNA ComplementaryDNA(DNA seq = default(DNA))
        {
            return this.Convert(seq).Complementary();
        }

    }
}