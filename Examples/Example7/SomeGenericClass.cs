// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example7
{
    using System;

    public class SomeGenericClass<T, T2>
        where T : struct
        where T2 : Attribute
    {
        public readonly object Value;
        public readonly Type TGeneric;
        public readonly Type TGeneric2;

        public SomeGenericClass()
        {
            this.Value = nameof(SomeGenericClass<T, T2>);
            this.TGeneric = typeof(T);
            this.TGeneric2 = typeof(T2);
        }
    }
}