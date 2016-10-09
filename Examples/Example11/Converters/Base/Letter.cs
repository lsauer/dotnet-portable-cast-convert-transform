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

    public struct Letter : IValue<char>, IEquatable<Letter>
    {
        public readonly Guid Id;

        private char value;

        public readonly string Name;

        public readonly string FullName;

        public Letter(char value, string name = null, string fullName = null)
        {
            this.Id = Guid.NewGuid();
            this.value = value;
            this.Name = name ?? $"{nameof(Letter)}_{value}";
            this.FullName = fullName ?? $"{nameof(Letter)}_{value}[{Id}]";
        }

        char IValue<char>.Value
        {
            get
            {
                return value;
            }
        }

        public static implicit operator Letter(char val)
        {
            return new Letter(val);
        }

        public static implicit operator char(Letter val)
        {
            return val.value;
        }

        #region IEquatable implementation

        public override int GetHashCode()
        {
            return this.value;
        }

        public bool Equals(Letter b)
        {
            return ((IValue<char>)this).Value == ((IValue<char>)b).Value;
        }

        public override bool Equals(object b)
        {
            return this == (Letter)b;
        }

        public static bool operator ==(Letter a, Letter b)
        {
            if(ReferenceEquals(a, b))
            {
                return true;
            }

            return a.value == b.value;
        }

        public static bool operator !=(Letter a, Letter b)
        {
            return !(a == b);
        }
        #endregion
    }
}