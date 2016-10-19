// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast.Test
{
    using System;

    using Core.Extensions;
    using Base;

    /// <summary>
    ///     The Exception-type which is raised exclusively by the <see cref="Core.TypeCast.Test" />  namespace
    /// </summary>
    public class CaptureDataException : Exception, IException
    {
        public object CaptureData { get; private set; }

        public Type Type { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the  <see cref="CaptureDataException" /> class. Allows passing 
        ///     an optional user data object <paramref name="captureData" /> and <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">
        ///     The error message that explains the reason for the exception. 
        /// </param>
        /// <param name="captureData">The data object to be thrown and passed outside of the method at a given point for analysis.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public CaptureDataException(object captureData, string message = null, Exception innerException = null)
            : base(message: message, innerException: innerException)
        {
            if (captureData != null)
            {
                this.CaptureData = captureData;
                this.Type = captureData.GetType();
            }
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