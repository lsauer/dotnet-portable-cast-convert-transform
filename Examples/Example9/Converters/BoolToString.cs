// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example9.Converters
{
    using System;
    using System.Drawing;

    using Core.TypeCast;
    using Core.TypeCast.Base;

    /// <summary>
    /// This class implements converting from string type to byte type as loose coupled, custom converter demonstration example
    /// </summary>
    [Converter(loadOnDemand: false, nameSpace: nameof(System), dependencyInjection: false)]
    public class BoolToString
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="StringToByte"/> class.
        /// </summary>
        /// <exception cref="ConverterException">
        /// </exception>
        public BoolToString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringToByte"/> class using dependency injection.
        /// </summary>
        /// <param name="collection">
        /// The collection to be injected as a optional dependency.
        /// </param>
        /// <exception cref="ConverterException">
        /// </exception>
        public BoolToString(IConverterCollection collection)
        {
            // ..operate on the collection here
        }

        #region Overrides of ConverterCollectionDependency<string,Image>
        
        [ConverterMethod]
        public string Bool2String(bool str)
        {
            return str.ToString();
        }

        [ConverterMethod]
        public string Bool2StringWithGeneric(bool str, object defval)
        {
            return str.ToString() + ", Default: " + defval;
        }

        [ConverterMethod]
        public decimal Bool2StringWithContext(bool str, IConvertContext context)
        {
            return decimal.Parse(context.Value.ToString());
        }
        
        #endregion
    }
}