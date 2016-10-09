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
    using System.Text.RegularExpressions;

    using Core.TypeCast;
    using Core.TypeCast.Base;
    using System.Text;
    using System.Collections;

    /// <summary>
    /// This class implements the assignment and operation on a sequence of repeated molecular subunits or `meres` whose sequence order follows a canonical alphabet. 
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    [Converter]
    public class Polymer : SequenceBase, 
                            IPolymer, 
                            IEnumerable<Monomer>
    {
        private MonomerCollection<Monomer> monomers;

        protected PolymerType Type = PolymerType.Other;

        protected Dictionary<char, double> weights;

        protected Polymer(string sequence, MonomerCollection<Monomer> monomers, string name = null, string fullName = null)
            : base(sequence: sequence, alphabet: monomers.GetAlphabet(name, fullName))
        {
            this.monomers = monomers;
        }

        [ConverterMethod]
        public MolecularWeight Weight()
        {
            var weight = 0.0d;
            foreach(var monomer in this.monomers)
            {
                weight += monomer.Weight * this.frequency[monomer.Letter];
            }
            return weight;
        }

        #region IEnumerable support

        public new IEnumerator<Monomer> GetEnumerator()
        {
            return ((IEnumerable<Monomer>)this.monomers).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Monomer>)this.monomers).GetEnumerator();
        }

        #endregion
    }
}