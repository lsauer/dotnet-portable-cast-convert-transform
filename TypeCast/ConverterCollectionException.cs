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

    /// <summary>
    ///     The Exception-type which is raised exclusively by the <see cref="Converter{TIn, TOut, TArg}"/> Library
    /// </summary>
    /// <seealso cref="ConverterCollectionCause" />
    public class ConverterCollectionException : Exception, IException<ConverterCollectionCause>
    {
        /// <summary>
        ///     Initializes a new instance of the  <see cref="ConverterCollectionException" /> class. Requires a
        ///     <see cref="ConverterCollectionCause" /> and optional user <paramref name="message" />.
        /// </summary>
        /// <param name="cause">The coded reason for the Exception</param>
        /// <param name="message">
        ///     The message for the Exception.  If left empty, the description of the cause will be used as
        ///     exception message
        /// </param>
        public ConverterCollectionException(ConverterCollectionCause cause, string message = null)
            : this(cause, null, message ?? cause.GetDescription())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the  <see cref="ConverterCollectionException" /> class. Requires a
        ///     <see cref="ConverterCollectionCause" /> and <paramref name="innerException" /> message
        /// </summary>
        /// <param name="cause">The coded reason for the Exception</param>
        /// <param name="innerException">The wrapped exception within the <see cref="ConverterCollectionException" /></param>
        public ConverterCollectionException(ConverterCollectionCause cause, Exception innerException)
            : this(cause, innerException, cause.GetDescription())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the  <see cref="ConverterCollectionException" /> class. Requires a
        ///     <see cref="ConverterCollectionCause" /> an <paramref name="innerException" /> and an exception <paramref name="message" />
        /// </summary>
        /// <param name="cause">The coded reason for the Exception of the value <see cref="ConverterCollectionCause" /></param>
        /// <param name="innerException">The wrapped exception within the <see cref="ConverterCollectionException" /></param>
        /// <param name="message">The <paramref name="message" /> for the Exception</param>
        public ConverterCollectionException(ConverterCollectionCause cause, Exception innerException, string message)
            : base(message: message, innerException: innerException)
        {
            this.Cause = cause;
        }

        /// <summary>
        ///     Gets or sets the exception's <see cref="ConverterCollectionCause" />, which is set through the parameterized exception constructor
        ///     <see cref="ConverterCollectionException(ConverterCollectionCause, string)" />
        /// </summary>
        /// <returns>The enumeration value for the raised Exception of the type <see cref="ConverterCollectionCause" /></returns>
        /// <remarks>To get a detailed description of the value, use <see cref="EnumExtension.GetDescription(Enum,string)" /></remarks>
        public ConverterCollectionCause Cause { get; set; }

        /// <summary>
        ///     Override this method for custom formatting of the unformatted exception <see cref="Exception.Message" />
        /// </summary>
        /// <returns>The string containing the formatted exception message</returns>
        public virtual string GetMessage()
        {
            return this.Cause.GetType().Name + " '" + this.Cause + "': " + this.Cause.GetDescription() + base.Message;
        }

        /// <summary>
        ///     Override this method for custom formatting of the unformatted exception <see cref="Exception.Message" />
        /// </summary>
        /// <returns>The string containing the formatted exception message</returns>
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