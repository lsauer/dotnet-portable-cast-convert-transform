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
    using System.Reflection;

    using Core.Extensions;
    using Core.TypeCast;
    using Core.TypeCast.Base;

    using System.Text;

    /// <summary>
    /// This class implements the assignment and operation of alpha-aminoacid sequences with genomic structural and referential background, using an assignable aminoacid alphabet,
    /// and existing data from databanks. 
    /// </summary>
    /// <remarks>Whereas a <see cref="Protein"/> generally requires an <see cref="ORF"/>, a <see cref="Peptide"/> does not require structural genomic information</remarks>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    [Converter(loadOnDemand: false, nameSpace: nameof(Converters), dependencyInjection: false)]
    public class Protein : Peptide,
                            IConverter<string, Protein>, 
                            IConverter<Protein, string>
    {
        public enum Transform
        {
            ToVerbose,
        }

        public Protein()
            : this(string.Empty, null)
        {
        }

        public Protein(string sequence, MonomerCollection<Monomer> monomers = null)
            : base(sequence: sequence, monomers: monomers ?? new AminoAcids())
        {
            this.Type = PolymerType.Protein;
        }

        /// <summary>
        /// Enzyme Commission number that is assigned to the peptide.
        /// </summary>
        public EnzymeComissionNumber ECNumber { get; set; }

        /// <summary>
        /// Reference to the <see cref="ORF"/> which created the protein, if any
        /// </summary>
        public string ORF { get; set; }

        /// <summary>
        /// Synonymous, replaced, obsolete or former gene symbol.
        /// </summary>
        public string GeneSymbol { get; set; }

        /// <summary>
        /// Genomic map position
        /// </summary>
        public string GenomicPosition { get; set; }

        /// <summary>
        /// Indicates that this feature is a non-functional version of the element named by the feature key.
        /// </summary>
        public bool Pseudo { get; set; }

        /// <summary>
        /// Accepted standard name for this feature.
        /// </summary>
        public string StandardName { get; set; }

        [ConverterMethod(passInstance: true, name: nameof(ToVerbose))]
        public string ToVerbose(bool fullName)
        {
            var seq = new StringBuilder();
            for (var i = 0; i < this.Count; i++)
            {
                var aa = aminoAcids[this.Sequence[i]];
                if (aa != null)
                {
                    seq.Append((fullName ? aa.FullName : aa.Name) + (this.Count - 1 == i ? "" : "-"));
                }
                else
                {
                    seq.Append("?");
                }
            }
            return seq.ToString();
        }

        [ConverterMethod]
        public override string ToString()
        {
            return this.Sequence;
        }

        #region IConverter<,> support

        [ConverterMethod]
        public Protein Convert(string value, Protein defaultValue = null)
        {
            return new Protein(value);
        }

        [ConverterMethod]
        public string Convert(Protein value, string defaultValue = null)
        {
            return value?.Sequence;
        }

        /// <summary>
        /// Generates a consensus <see cref="PolyNucleotide"/> sequence of the notation `A[CT]N{A}YR` such that A means that an A is always found in that position; 
        /// [CT] stands for either C or T; N stands for any base; and {A} means any base except A. Y represents any pyrimidine, and R indicates any purine.
        /// </summary>
        /// <param name="codons">One or multiple multivalent codons which code for a given AminoAcid or Operation</param>
        /// <param name="withCount">Whether to show the count of each base, which is useful for sequence logo creation</param>
        /// <returns></returns>
        private string ToConsensusCode(string[] codons, bool withCount = true)
        {
            if (codons == null || codons.Length == 0)
            {
                return string.Empty;
            }

            Array.Sort(codons);
            var consensus = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                var bases = codons[0].ToUpperInvariant().ToCharArray();
                var cRef = bases[i];
                var altBases = new Dictionary<char, int> { { cRef, 1 } };
                for (int j = 1; j < codons.Length; j++)
                {
                    var cCmp = codons[j].ToUpperInvariant()[i];

                    if (cRef != cCmp)
                    {
                        if (altBases.ContainsKey(cCmp) == false)
                        {
                            altBases.Add(cCmp, 1);
                        }
                        else if (withCount == true)
                        {
                            altBases[cCmp] += 1;
                        }
                    }
                }
                if (altBases.Count == 1)
                {
                    consensus.Append(cRef);
                }
                else
                {
                    // order
                    var altBasesArray = altBases.Keys.ToArray();
                    if (altBases.Count >= 4)
                    {
                        // any base
                        consensus.Append("N");
                    }
                    else if (altBases.Count == 3)
                    {
                        consensus.Append(
                            "{" + new string(new[] { 'A', 'C', 'G', 'T' }.Except(altBasesArray).ToArray()) + "}");
                    }
                    else if (altBasesArray.ToLetters().IsAllPurine() == true)
                    {
                        consensus.Append('Y');
                    }
                    else if (altBasesArray.ToLetters().IsAllPyrimidine() == true)
                    {
                        consensus.Append('R');
                    }
                    else if (withCount == true)
                    {
                        consensus.Append("[");
                        foreach (var item in altBases)
                        {
                            consensus.Append($"{(item.Value > 1 ? item.Value.ToString() : "")}{item.Key}");
                        }
                        consensus.Append("]");
                    }
                    else
                    {
                        consensus.Append($"[{new string(altBasesArray)}]");
                    }

                }
            }
            return consensus.ToString().ToUpperInvariant();
        }

        [ConverterMethod(passInstance: true, BaseType = typeof(DNA.ConsensusSequence))]
        public string ToConsensus()
        {
            var list = base.ToCodonCandidates();
            var seq = new StringBuilder();
            for (int i = 0; i < list.Length; i++)
            {
                var codons = list[i];
                var codon = this.ToConsensusCode(codons);

                seq.Append(codon);
            }
            return seq.ToString().ToUpperInvariant();
        }

        #endregion
    }
}
