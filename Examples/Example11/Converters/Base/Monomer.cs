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
    using System.Text;
    using System.Collections;

    public abstract class Monomer : IMonomer
    {
        private readonly Guid id;
        private Letter letter;
        private readonly string name;
        private readonly string fullName;
        private readonly MolecularWeight weight;
        private ICompound compound;

        protected Monomer(Letter value, MolecularWeight weight, string name = null, string fullName = null)
        {
            this.id = Guid.NewGuid();
            this.Letter = value;
            this.weight = weight;
            this.name = name ?? $"{nameof(Monomer)}_{value}";
            this.fullName = fullName ?? $"{nameof(Monomer)}_{value}[{id}]";
        }

        public char Value
        {
            get
            {
                return Letter;
            }
        }

        public Guid Id
        {
            get
            {
                return id;
            }
        }

        public Letter Letter
        {
            get
            {
                return letter;
            }

            set
            {
                this.letter = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string FullName
        {
            get
            {
                return fullName;
            }
        }

        public MolecularWeight Weight
        {
            get
            {
                return weight;
            }
        }

        public ICompound Compound
        {
            get
            {
                return compound;
            }

            set
            {
                this.compound = value;
            }
        }
    }
}