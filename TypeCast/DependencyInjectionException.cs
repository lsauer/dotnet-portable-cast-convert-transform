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

    using Core.Extensions;
    using Base;

    /// <summary>
    ///     The Exception-type which is raised exclusively by the <see cref="DependencyInjection{TDependency}" />  class
    /// </summary>
    public class DependencyInjectionException : Exception, IException
    {
        /// <summary>
        ///     Initializes a new instance of the  <see cref="DependencyInjectionException" /> class. Allows passing 
        ///     an optional user <paramref name="message" /> and <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">
        ///     The error message that explains the reason for the exception. 
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public DependencyInjectionException(string message = null, Exception innerException = null)
            : base(message: message, innerException: innerException)
        {
        }

        /// <summary>
        ///     Gets the exception message.
        /// </summary>
        public string GetMessage()
        {
            return Message;
        }
    }
}