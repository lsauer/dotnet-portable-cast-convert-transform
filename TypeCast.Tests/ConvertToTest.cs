// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast.Test
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using NUnit.Framework;
    using Core.TypeCast;

    [Category(nameof(Core.TypeCast))]
    [TestFixture]
    public class ConvertToTests
    {
        [Test(Description = "Initialize the tests")]
        public void CastTo_Int_MinValue_Okay()
        {
            var s = int.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(s.CastTo<int>() == int.MinValue);
        }

        [Test(Description = "Initialize the tests")]
        public void CastTo_ObjectToUInt_Typed_Okay()
        {
            var s = 42.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(((object)s).CastTo<object, uint>() == 42);
        }

        [Test(Description = "Initialize the tests")]
        public void CanConvertTo_Decimal_WithDefault_Okay()
        {
            var s = 500.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(s.CanConvertTo(1337M));
            if(s.CanConvertTo(1337M))
            {
                Assert.AreEqual(s.CastTo(1337M), 500M);
            }
        }

        [Test(Description = "Initialize the tests")]
        public void CanConvertTo_Decimal_Typed_Okay()
        {
            var s = 500.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(s.CanConvertTo<decimal>());
            if(s.CanConvertTo<decimal>())
            {
                Assert.AreEqual(s.CastTo<decimal>(), 500M);
            }
        }

        [Test(Description = "Initialize the tests")]
        public void ConverterCollection_CanConvertFrom_Decimal_Typed_False()
        {
            var cc = ConverterCollection.CurrentInstance;
            Assert.IsFalse(cc.CanConvertFrom<decimal>());
        }

        [Test(Description = "Initialize the tests")]
        public void ConverterCollection_CanConvertTo_Decimal_Typed_False()
        {
            var cc = ConverterCollection.CurrentInstance;
            Assert.IsTrue(cc.CanConvertTo<decimal>());
        }

        [Test(Description = "Initialize the tests")]
        public void CanConvert_StringToDecimal_WithDefault_Okay()
        {
            var defval = 1.337M;
            var s = 300.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(s.CanConvertTo(defval));

            // possible
            if(s.CanConvertTo(defval))
            {
                Assert.AreEqual(s.CastTo(defval), 300M);
            }
        }

        [Test(Description = "Initialize the tests")]
        public void ConverterCollection_CanConvertTo_StringToDecimal_Okay()
        {
            var cc = ConverterCollection.CurrentInstance;
            var s = 300.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(cc.CanConvertTo<decimal>());
            var defval = 0.0M;

            // possible
            if(cc.CanConvertTo<decimal>())
            {
                Assert.AreEqual(s.CastTo(defval), 300M);
            }
        }


        [Test(Description = "Initialize the tests")]
        public void CanConvert_StringToDecimal_Typed_Okay()
        {
            var defval = 0.0M;
            var s = 300.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(s.CanConvertTo<decimal>());

            // possible
            if(s.CanConvertTo<decimal>())
            {
                Assert.AreEqual(s.ConvertTo<decimal>(defval), 300M);
            }
        }


        [Test(Description = "Initialize the tests")]
        public void CanConvert_StringToDecimal_WithPoint_False()
        {
            var model = Point.Empty;
            var defval = 0M;
            var s = 42.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(s.CanConvertTo<decimal>());

            // not possible! No converter defined: CanConvertTo(desired result, model value)
            if(s.CanConvertTo(defval, model))
            {
                Assert.IsTrue(s.CanConvertTo<decimal>());
                Assert.AreEqual(s.ConvertTo<decimal>(Point.Empty), default(decimal));
                Assert.AreNotEqual(s.ConvertTo<decimal>(Point.Empty), 42M);
            }
        }
    }
}
