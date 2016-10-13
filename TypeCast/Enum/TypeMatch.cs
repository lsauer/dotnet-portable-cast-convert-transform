// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.Extensions
{
    using System;

    /// <summary>
    /// An enumerating of values containing a rough 2bit classification of relationship between two types
    /// </summary>
    /// <remarks>The enumeration relates to the type change classification of Cast, Convert and Transform</remarks>
    public enum TypeMatch : sbyte
    {
        /// <summary>
        /// Unknown or no relationship between the types
        /// </summary>
        [Description("Unknown or no relationship between the types")]
        None,

        /// <summary>
        /// Identical or same relationship between the types
        /// </summary>
        [Description("Identical or same relationship between the types")]
        Same,

        /// <summary>
        /// Similar or same base relationship between the types
        /// </summary>
        [Description("Similar or same base relationship between the types")]
        Similar,
    }
}