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
    using System.Collections;

    /// <summary>
    /// This class implements the assignment and operation on a single Polynucleotide sequence of an simple canonical DNA alphabet of GCATU. 
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    [Converter]
    public class PolyNucleotide : BioPolymer,
                                IEnumerable<Nucleotide>,
                                IEnumerable<Codon>,
                                IPolyNucleotide,
                                IValue<IEnumerable<Codon>>
    {

        protected static Nucleotides nucleotides = new Nucleotides();

        public PolyNucleotide()
            : this(string.Empty, null)
        {
        }

        protected PolyNucleotide(string sequence, MonomerCollection<Monomer> monomers)
            : base(sequence: sequence, monomers: monomers)
        {
        }

        public bool HasUracil
        {
            get
            {
                return this.Alphabet.Contains('U');
            }
        }

        #region IEnumerable<Nucleotide> support

        public IEnumerable<Nucleotide> Nucleotides
        {
            get
            {
                foreach(var letter in this.Letters)
                {
                    yield return (Nucleotide)nucleotides[letter];
                }
            }
        }

        IEnumerator<Nucleotide> IEnumerable<Nucleotide>.GetEnumerator()
        {
            return this.Nucleotides.GetEnumerator();
        }
        #endregion

        #region IEnumerable<Codon> support

        IEnumerator<Codon> IEnumerable<Codon>.GetEnumerator()
        {
            return this.Codons.GetEnumerator();
        }

        public IEnumerable<Codon> Codons
        {
            get
            {
                return (this as IValue<IEnumerable<Codon>>).Value;
            }
        }
        #endregion

        #region IValue<> support

        IEnumerable<Codon> IValue<IEnumerable<Codon>>.Value
        {
            get
            {
                var list = new List<Codon>();
                for(int i = 0; i < this.Sequence.Length - 2; i += 3)
                {
                    yield return new Codon(this.Sequence.Substring(i, 3));
                }
            }
        }
        #endregion

        [ConverterMethod]
        public MeltingTemperature Temperature()
        {
            try
            {
                if(this.Sequence.Length < 13)
                {
                    return 4.0d * (this.frequency['G'] + this.frequency['C']) + 2 * (this.frequency['A'] + this.frequency[HasUracil ? 'U' : 'T']);
                }
                else
                {
                    return 64.9d + 41 * ((this.frequency['G'] + this.frequency['C'] - 16.4) / (this.frequency['G'] + this.frequency['C'] + this.frequency['A'] + this.frequency[HasUracil ? 'U' : 'T']));
                }
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public void Add(Codon codon)
        {
            this.sequence.Append(codon.Value);
        }

        public virtual string Complementary()
        {
            if(string.IsNullOrWhiteSpace(this.Sequence) == true)
            {
                return null;
            }
            var cseq = this.Sequence
                      .Replace('T', 'X').Replace('A', 'T').Replace('X', 'A')
                      .Replace('C', 'X').Replace('G', 'C').Replace('X', 'G');
            return cseq;
        }

        protected TcSeq Complementary<TSeq, TcSeq>(TSeq seq = default(TSeq))
            where TSeq : class
            where TcSeq : class
        {
            return (TcSeq)Activator.CreateInstance(typeof(TcSeq), (seq as PolyNucleotide)?.Complementary() ?? this.Complementary());
        }
    }
}