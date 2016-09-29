﻿// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
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
    using System.Text.RegularExpressions;

    using Core.TypeCast;
    using Core.TypeCast.Base;
    using System.Text;
    using System.Collections;

    public class LetterComparer : IEqualityComparer<Letter>
    {
        public bool Equals(Letter a, Letter b)
        {
            //Check whether the objects are the same object. 
            if(Object.ReferenceEquals(a, b))
            {
                return true;
            }

            return a != null && b != null && a == b;
        }

        public int GetHashCode(Letter obj)
        {
            return (int)obj;
        }
    }
}