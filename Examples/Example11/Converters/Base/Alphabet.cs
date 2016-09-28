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

    public struct Alphabet : IValue<IEnumerable<Letter>>, IEnumerable<Letter>, IEquatable<Alphabet>
    {

        public static implicit operator Alphabet(string val)
        {
            return new Alphabet(val);
        }

        public static implicit operator List<Letter>(Alphabet val)
        {
            return val.value;
        }
        public readonly string FullName;
        public readonly Guid Id;
        public readonly string Name;
        private List<Letter> value;

        public Alphabet(string alphabet, string name = null, string fullName = null)
        {
            if(String.IsNullOrWhiteSpace(alphabet) == true)
            {
                throw new ArgumentException($"Argument `{nameof(alphabet)}` must not be null or empty");
            }

            this.Id = Guid.NewGuid();
            this.Name = name ?? $"{nameof(Alphabet)}_{alphabet}";
            this.FullName = fullName ?? $"{nameof(Alphabet)}_{alphabet}[{Id}]";

            this.value = new List<Letter>();
            var alphabetOrdered = (from c in alphabet?.ToUpperInvariant() orderby c select c).ToArray();

            for(int i = 0; i < alphabetOrdered.Length; i++)
            {
                var c = alphabetOrdered[i];
                value.Add(new Letter(c, name, fullName));
            }
        }

        public IEnumerable<Letter> Value
        {
            get
            {
                return this.value;
            }
        }

        public static bool operator !=(Alphabet a, Alphabet b)
        {
            return !(a == b);
        }

        public static bool operator ==(Alphabet a, Alphabet b)
        {
            // both are null, or both are the same instance
            if(ReferenceEquals(a, b))
            {
                return true;
            }

            // one side is null
            if((a.value == null) || (b.value == null))
            {
                return false;
            }
            if(a.value.Count != b.value.Count)
            {
                return false;
            }
            return a.value.SequenceEqual(b.value, new LetterComparer());
        }

        #region IEquatable implementation

        public bool Equals(Alphabet b)
        {
            return this == b;
        }

        public override bool Equals(object b)
        {
            return this == (Alphabet)b;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public IEnumerator<Letter> GetEnumerator()
        {
            return this.value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.value.GetEnumerator();
        }

        #endregion

    }

}