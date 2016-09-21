// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.0.1.4                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast
{
    using System;

    using Core.Extensions;

    /// <summary>
    ///     The Exception-type which is raised exclusively by the <see cref="Converter{TIn,TOut}" /> Library
    /// </summary>
    /// <seealso cref="ConverterCause" />
    public class ConverterException : Exception, IException, IException<ConverterCause>
    {
        /// <summary>
        ///     Initializes a new instance of the  <see cref="ConverterException" /> class. Requires a
        ///     <see cref="ConverterCause" /> and optional user <paramref name="message" />.
        /// </summary>
        /// <param name="cause">The coded reason for the Exception</param>
        /// <param name="message">
        ///     The message for the Exception.  If left empty, the description of the cause will be used as
        ///     exception message
        /// </param>
        public ConverterException(ConverterCause cause, string message = null)
            : this(cause, null, message ?? cause.GetDescription())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the  <see cref="ConverterException" /> class. Requires a
        ///     <see cref="ConverterCause" /> and <paramref name="innerException" /> message
        /// </summary>
        /// <param name="cause">The coded reason for the Exception</param>
        /// <param name="innerException">The wrapped exception within the <see cref="ConverterException" /></param>
        public ConverterException(ConverterCause cause, Exception innerException)
            : this(cause, innerException, cause.GetDescription())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the  <see cref="ConverterException" /> class. Requires a
        ///     <see cref="ConverterCause" /> an <paramref name="innerException" /> and an exception <paramref name="message" />
        /// </summary>
        /// <param name="cause">The coded reason for the Exception of the value <see cref="ConverterCause" /></param>
        /// <param name="innerException">The wrapped exception within the <see cref="ConverterException" /></param>
        /// <param name="message">The <paramref name="message" /> for the Exception</param>
        public ConverterException(ConverterCause cause, Exception innerException, string message)
            : base(message, innerException)
        {
            this.Cause = cause;
        }

        /// <summary>
        ///     Gets or sets the exception's <see cref="ConverterCause" />, which is set through the parameterized exception constructor
        ///     <see cref="ConverterException(ConverterCause, string)" />
        /// </summary>
        /// <returns>The enumeration value for the raised Exception of the type <see cref="ConverterCause" /></returns>
        /// <remarks>To get a detailed description of the value, use <see cref="EnumExtension.GetDescription(Enum,string)" /></remarks>
        public ConverterCause Cause { get; set; }

        /// <summary>
        ///     Override this method for custom formatting of the unformatted exception <see cref="Exception.Message" />
        /// </summary>
        /// <returns>The string containing the formatted exception message</returns>
        public virtual string GetMessage()
        {
            return this.Cause.GetType().Name + " '" + this.Cause + "': " + this.Cause.GetDescription();
        }

        /// <summary>
        ///     Gets the exception message.
        /// </summary>
        public override string Message
        {
            get
            {
                return GetMessage();
            }
        }

        /// <summary>
        /// A string representation of the current exception.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="string"/> representation of <see cref="GetMessage"/> concatenated to the underlying <see cref="Exception.ToString"/> method.
        /// </returns>
        public override string ToString()
        {
            return this.GetMessage() + Environment.NewLine + base.ToString();
        }
    }
}