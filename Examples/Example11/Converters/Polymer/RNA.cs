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
    /// This class implements the assignment and operation on a single <see cref="RNA"/> sequence. 
    /// It should be used for demonstrative purposes only!
    /// </summary>
    /// <remarks>Represents a complementary DNA sequence</remarks>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    [Converter(loadOnDemand: false, nameSpace: nameof(Converters), dependencyInjection: false)]
    public class RNA : PolyNucleotide, 
                       IEnumerable<Codon>,
                       IConverter<string, RNA>,
                       IConverter<RNA, string>
    {
        public RNA()
            : this(string.Empty)
        {
        }

        public RNA(string sequence)
            : base(sequence: sequence, monomers: new RibonucleicAcids())
        {
            this.Type = PolymerType.RNA;
        }

        #region IConverter<,> support

        [ConverterMethod]
        public RNA Convert(string value, RNA defaultValue = null)
        {
            return new RNA(value);
        }

        [ConverterMethod]
        public string Convert(RNA value, string defaultValue = null)
        {
            return value?.Sequence;
        }

        [ConverterMethod]
        public DNA Convert(RNA value)
        {
            return new DNA(value?.Sequence.ToUpperInvariant().Replace('U', 'T'));
        }
        #endregion

        [ConverterMethod(isStatic: false)]
        public cRNA Complementary(RNA seq = default(RNA))
        {
            return base.Complementary<RNA, cRNA>(seq);
        }

        [ConverterMethod(isStatic: false)]
        public cDNA ComplementaryDNA(RNA seq = default(RNA))
        {
            return this.Convert(seq).Complementary();
        }
    }
}