// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast
{
    using System;

    /// <summary>
    /// The common <see cref="Exception"/>interface.
    /// </summary>
    public interface IException
    {
        /// <summary>
        /// Gets the exception message.
        /// </summary>
        /// <returns>The <see cref="string"/> message containing a <see cref="string"/> representation of the <see cref="Exception"/></returns>
        string GetMessage();
    }

    /// <summary>
    /// The generic <see cref="Exception"/>interface.
    /// </summary>
    /// <typeparam name="TCause">The custom <see cref="Enum"/> type containing a coded representation of the exception cause</typeparam>
    public interface IException<out TCause>
    {
        /// <summary>
        /// Gets a custom <see cref="Enum"/> type containing exception causes.
        /// </summary>
        TCause Cause { get; }
    }
}