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

    using Core.TypeCast;
    using Core.TypeCast.Base;

    /// <summary>
    /// This class implements converting from string type to byte type as loose coupled, custom converter demonstration example
    /// </summary>
    [Converter(loadOnDemand: false, nameSpace: nameof(System), dependencyInjection: false)]
    public class BoolToString : ConverterCollectionDependency<string, bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringToByte"/> class using dependency injection.
        /// </summary>
        /// <param name="collection">
        /// The collection to be injected as a optional dependency.
        /// </param>
        /// <exception cref="ConverterException">
        /// </exception>
        public BoolToString(IConverterCollection collection)
            : base(collection)
        {
            // ..operate on the collection here
        }

        #region Overrides of ConverterCollectionDependency<string, bool>

        /// <summary>
        /// A test method converting a <see langword="bool"/> to a <see cref="decimal"/> value
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A string representation of the input boolean value</returns>
        [ConverterMethod]
        public string Bool2String(bool str)
        {
            return str.ToString();
        }

        /// <summary>
        /// A test method converting a <see langword="bool"/> to a <see cref="decimal"/> value, with a <see cref="IConvertContext"/>
        /// </summary>
        /// <param name="str">The  input value of type bool</param>
        /// <param name="context"></param>
        /// <returns>A parsed <see cref="decimal"/> value, without exception handling</returns>
        [ConverterMethod]
        public decimal Bool2StringWithContext(bool str, IConvertContext context)
        {
            throw new CaptureDataException(context);

            return decimal.Parse(context.Value.ToString());
        }

        /// <summary>
        /// A test method converting a bool to a string value
        /// </summary>
        /// <param name="str">The  input value of type bool</param>
        /// <param name="defval">A default value of the same or different type as the input value</param>
        /// <returns>A dummy string value stump</returns>
        [ConverterMethod]
        public string Bool2StringWithGeneric(bool str, object defval)
        {
            throw new CaptureDataException(defval);

            return str.ToString() + ", Default: " + defval;
        }

        /// <summary>
        /// converts a <see cref="string"/> to a bool value,as part of the <see cref="IConverter{TIn, TOut}"/> support for testing purposes
        /// </summary>
        /// <param name="value">The input value of <see cref="Type"/> <see cref="string"/></param>
        /// <param name="defaultValue">A default value of the same type as the input</param>
        /// <returns>A <see langword="bool"/> value</returns>
        public override bool Convert(string value, bool defaultValue = default(bool))
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}