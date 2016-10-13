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
    /// List of meta-types around Proteins, not encompassed by the EC nomenclature
    /// </summary>
    public enum ProteinType
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
        /// A  intrinsically disordered protein (IDP) does not have a fixed ordered three-dimensional structure, to greater degree so than IUPs
        /// </summary>
        IntrinsicallyDisordered,

        /// <summary>
        /// A  intrinsically unstructured protein (IUP) does not have a fixed ordered three-dimensional structure throughout its length and / or lifetime 
        /// </summary>
        IntrinsicallyUnstructured,

        /// <summary>
        /// Indicates that the protein is known to have a well defined, known structure
        /// </summary>
        WellStructured,

        /// <summary>
        /// Indicates that the protein has an disordered conformation in thermodynamic equilibrium
        /// </summary>
        RandomCoil,
    }
}