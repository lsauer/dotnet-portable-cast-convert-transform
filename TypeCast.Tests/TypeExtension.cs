// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.Extension
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using Core.TypeCast;
    using Core.TypeCast.Base;

    public static class TypeExtension
    {
        public static T MinValue<T>(this Type self)
        {
            return (T)self.GetField(nameof(MinValue), BindingFlags.Public | BindingFlags.Static)?.GetValue(self);
        }

        public static object MinValue(this Type self)
        {
            return self.MinValue<object>();
        }

        public static T MaxValue<T>(this Type self)
        {
            return (T)self.GetField(nameof(MaxValue), BindingFlags.Public | BindingFlags.Static)?.GetValue(self);
        }

        public static object MaxValue(this Type self)
        {
            return self.MaxValue<object>();
        }
    }
}