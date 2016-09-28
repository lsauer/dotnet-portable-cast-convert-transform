// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example8
{
    using System;
    using System.Drawing;

    using Core.TypeCast;
    using Core.TypeCast.Base;

    /// <summary>
    /// This class implements converting from string type to byte type as loose coupled, custom converter demonstration example
    /// </summary>
    [Converter(loadOnDemand: false, nameSpace: nameof(System), dependencyInjection: true)]
    public class BoolToString : ConverterCollectionDependency<bool, string>
    {
        protected string someValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringToByte"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <exception cref="ConverterException">
        /// </exception>
        public BoolToString(IConverterCollection collection)
            : base(collection)
        {
            //this.converter = this.Convert;
            this.someValue = "I got Instanced!";
        }

        #region Overrides of ConverterCollectionDependency<string,Image>

        [ConverterMethod]
        public string Bool2StringAndHello(bool str)
        {
            return str.ToString() + "Hello World!";
        }
        
        [ConverterMethod]
        public string Bool2StringAndInt(bool str, object defval)
        {
            return str.ToString() + defval;
        }

        public override string Convert(bool value, string defaultValue = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}