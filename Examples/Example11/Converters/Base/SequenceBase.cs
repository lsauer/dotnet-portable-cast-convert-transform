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

    public abstract class SequenceBase : IEquatable<SequenceBase>, IEnumerable<Letter>
    {
        public readonly Guid Id;

        private Alphabet alphabet;
        protected StringBuilder sequence;
        protected Dictionary<char, int> frequency;

        public readonly string Name;
        public readonly string FullName;

        public int Count { get; private set; } = 0;

        protected SequenceBase(string sequence, string alphabet = null, string name = null, string fullName = null)
            : this(sequence: sequence, alphabet: new Alphabet(alphabet, name, fullName))
        {
        }

        protected SequenceBase(string sequence, Alphabet alphabet)
        {
            this.Alphabet = alphabet;
            this.Sequence = sequence;
        }

        public Alphabet Alphabet
        {
            get
            {
                return this.alphabet;
            }

            set
            {
                if(value == default(Alphabet))
                {
                    return;
                }
                this.alphabet = value;
                this.frequency = new Dictionary<char, int>();

                foreach(var letter in this.alphabet)
                {
                    this.frequency.Add(letter, 0);
                }
            }
        }

        public string Sequence
        {
            get
            {
                return sequence.ToString();
            }

            set
            {
                if(string.IsNullOrWhiteSpace(value) == false)
                {
                    var val = value.ToUpperInvariant();
                    val = Regex.Replace(val, @"(\s*)", "", RegexOptions.Multiline);
                    this.sequence = new StringBuilder(val);

                    this.Frequency();
                    this.Count = value.Length;
                }
            }
        }

        private Dictionary<char, int> Frequency()
        {
            if(this.Alphabet != default(Alphabet))
            {
                foreach(var chr in this.Alphabet)
                {
                    this.frequency[chr] = this.Sequence.Count(c => c == chr);
                }
            }
            return this.frequency;
        }

        #region IEnumerable support
        public IEnumerable<Letter> Letters
        {
            get
            {
                for(int i = 0; i < sequence.Length; i++)
                {
                    yield return sequence[i];
                }
            }
        }

        public IEnumerator<Letter> GetEnumerator()
        {
            return Letters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IEquatable implementation

        public bool Equals(SequenceBase b)
        {
            return this == b;
        }

        public override bool Equals(object b)
        {
            return this == b as SequenceBase;
        }

        // instances with identical sequences should be treated as same
        public override int GetHashCode()
        {
            return (int)this.sequence.GetHashCode();
        }

        public static bool operator ==(SequenceBase a, SequenceBase b)
        {
            // both are null, or both are the same instance
            if(ReferenceEquals(a, b))
            {
                return true;
            }

            // one side is null
            if(((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.sequence.Equals(b.sequence)
                && a.alphabet == b.alphabet;
        }


        public static bool operator !=(SequenceBase a, SequenceBase b)
        {
            return !(a == b);
        }

        #endregion
    }
}