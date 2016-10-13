// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.Extension
{
    using System;
    using System.Reflection;

    /// <summary>
    /// extension methods for <see cref="Type"/> used for comprehensive tests
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Gets the Minimum value of most common system types
        /// </summary>
        /// <typeparam name="T">The generic type parameter</typeparam>
        /// <param name="self">The own <see cref="Type"/> instance</param>
        /// <returns>The value of the `MinValue` field</returns>
        public static T MinValue<T>(this Type self)
        {
            return (T)self.GetField(nameof(MinValue), BindingFlags.Public | BindingFlags.Static)?.GetValue(self);
        }

        public static object MinValue(this Type self)
        {
            return self.MinValue<object>();
        }

        /// <summary>
        /// Gets the Maximum value of most common system types
        /// </summary>
        /// <typeparam name="T">The generic type parameter</typeparam>
        /// <param name="self">The own <see cref="Type"/> instance</param>
        /// <returns>The value of the `MaxValue` field</returns>
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