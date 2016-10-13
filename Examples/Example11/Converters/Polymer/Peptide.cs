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
    using System.Reflection;

    using Core.Extensions;
    using Core.TypeCast;
    using Core.TypeCast.Base;
    using System.Text;

    /// <summary>
    /// This class implements the assignment and operation on a short, cannonical alpha-Aminoacid sequence through an assignable aminoacid alphabet. 
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    [Converter(loadOnDemand: false, nameSpace: nameof(Converters), dependencyInjection: false)]
    public class Peptide : BioPolymer,
                        IEnumerable<AminoAcid>,
                        IConverter<string, Peptide>,
                        IConverter<Peptide, string>
    {

        protected static AminoAcids aminoAcids = new AminoAcids();

        /// <summary>Initializes a new instance of the <see cref="Peptide"/> class.</summary>
        public Peptide()
            : this(string.Empty, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Peptide"/> class.</summary>
        /// <param name="sequence">An aminoacid sequence</param>
        /// <param name="monomers">An optional <see cref="MonomerCollection{TMonomer}"/>.</param>
        public Peptide(string sequence, MonomerCollection<Monomer> monomers = null)
            : base(sequence: sequence, monomers: monomers ?? new AminoAcids())
        {
            this.Type = PolymerType.Peptide;
        }


        /// <summary>
        /// Indicates a Peptidomimetic with other than (alpha)-aminoacids in its sequence, or in case of a <see cref="Protein"/> a non-functional version.
        /// </summary>
        public bool Pseudo
        {
            get
            {
                return this.Type.HasFlag(PolymerType.Pseudo);
            }
            set
            {
                if (value == true)
                {
                    this.Type |= PolymerType.Pseudo;
                }
                else
                {
                    this.Type &= ~PolymerType.Pseudo;
                }
            }
        }

        /// <summary>
        /// Accepted standard name for this feature.
        /// </summary>
        public string StandardName { get; set; }

        #region IEnumerable<AminoAcid> support

        public IEnumerable<AminoAcid> AminoAcids
        {
            get
            {
                foreach (var letter in this.Letters)
                {
                    yield return (AminoAcid)aminoAcids[letter];
                }
            }
        }

        IEnumerator<AminoAcid> IEnumerable<AminoAcid>.GetEnumerator()
        {
            return this.AminoAcids.GetEnumerator();
        }
        #endregion

        [ConverterMethod]
        public override string ToString()
        {
            return this.Sequence;
        }

        #region IConverter<,> support

        [ConverterMethod]
        public Peptide Convert(string value, Peptide defaultValue = null)
        {
            return new Peptide(value);
        }

        [ConverterMethod]
        public string Convert(Peptide value, string defaultValue = null)
        {
            return value?.Sequence;
        }

        public string[][] ToCodonCandidates()
        {
            List<string[]> ret = new List<string[]>();
            for (var i = 0; i < this.Count; i++)
            {
                var threeLetterAbbreviation = aminoAcids[this.Sequence[i]]?.Name;

                if (threeLetterAbbreviation != null)
                {
                    var codons = Codon.CodonsReverse[threeLetterAbbreviation];
                    ret.Add(codons);
                }
            }
            return ret.ToArray();
        }

        #endregion
    }
}