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
    /// <summary>
    /// Enumeration of peptide types based on their function (non-standard)
    /// </summary>
    public enum PeptideType
    {
        /// <summary>
        /// The undetermined value
        /// </summary>
        None,

        /// <summary>
        /// Determined to be not known at the point of determination
        /// </summary>
        Unknown,

        /// <summary>
        /// A 5-30 amino-acids long peptide, at the N-terminus of most nascent proteins, destined to be further processed in the secretory pathway
        /// </summary>
        Transit,

        /// <summary>
        /// An signal amino-acid sequence 
        /// </summary>
        Signal,
    }
}