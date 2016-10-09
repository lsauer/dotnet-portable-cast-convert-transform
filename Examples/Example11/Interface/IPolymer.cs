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
    /// <summary>
    /// The <see cref="Polymer"/> interface
    /// </summary>
    public interface IPolymer
    {
        //string Alphabet { get; set; }
        int Count { get; }
        //string Sequence { get; set; }

        MolecularWeight Weight();
    }
}