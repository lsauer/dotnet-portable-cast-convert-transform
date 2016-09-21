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
    using System.Reflection;
    using Base;

    /// <summary>
    /// The common <see cref="IConvertContext"/> interface.
    /// </summary>
    public interface IConvertContext
    {
        Type From { get; }
        Type To { get; }
        Type Argument { get; }
        object Value { get; }
        Converter Converter { get; }
        string Caller { get; }
        bool? Nullable { get; }
        bool? ThrowExceptions { get; }
        object Method { get; }
        MethodInfo MethodInfo { get; }
    }
}