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

    /// <summary>
    /// This class implements the assignment and operation on a single BioPolymer comprised out of defined, sequential units
    /// </summary>
    /// <remarks>Biopolymers are a hyperclass of polymers which entail a biological context for polymerization i.e. polymeric biomolecules.</remarks>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    public class BioPolymer : Polymer
    {
        public BioPolymer()
            : this(sequence: string.Empty, monomers: null)
        {
        }

        protected BioPolymer(string sequence, MonomerCollection<Monomer> monomers)
           : base(sequence: sequence, monomers: monomers)
        {
        }

        public bool IsComplementary { get; protected set; }
    }
}