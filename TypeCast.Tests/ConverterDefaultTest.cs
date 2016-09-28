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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using NUnit.Framework;

    using Core.Extension;

    [TestFixture]
    public class ConverterDefaultTest
    {
        private List<Type> types = new List<Type> {
                typeof(int),
                typeof(uint),
                typeof(char),
                typeof(bool),
                typeof(byte),
                typeof(sbyte),
                typeof(decimal),
                typeof(double),
                typeof(float),
                typeof(long),
                typeof(ulong),
                typeof(short),
                typeof(ushort),
                typeof(DateTime),
        };

        // basic startup test
        [Test]
        public void Test_Basic_Initialization()
        {
            var cc = ConverterCollection.CurrentInstance;
            // invoke auto-discovery
            cc.Initialize(this.GetType().Assembly);
            var items = cc.Items;
            Assert.That(cc.Count, Is.GreaterThan(0));
            foreach(var converter in ConverterCollection.CurrentInstance)
            {
                Assert.That(converter, Is.InstanceOf<Base.Converter>() );
            }
        }


        // comprehensive tests
        [Test]
        public void CastTo_MinValue_With_Default_Equal()
        {
            foreach(var type in types)
            {
                var defValue = Activator.CreateInstance(type);
                var minValue = type.MinValue();
                Assert.AreEqual(minValue?.CastTo(defaultValue: defValue, typeTo: type, unboxObjectType: false), minValue);
            }
        }

        [Test]
        public void CastTo_MaxValue_With_Default_Equal()
        {
            foreach(var type in types)
            {
                var defValue = Activator.CreateInstance(type);
                var maxValue = type.MaxValue();
                Assert.AreEqual(maxValue?.CastTo(defaultValue: defValue, typeTo: type, unboxObjectType: false), maxValue);
            }
        }

        // int
        [Test]
        public void CastTo_ObjectToIntValue_Equal()
        {
            Assert.AreEqual(((object)"42").CastTo<object, int>(), 42);
        }

        [Test]
        public void CastTo_Int_MinValue_Equal()
        {
            var s = int.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<int>(), int.MinValue);
        }

        [Test]
        public void CastTo_ObjectToIntMinValue_Equal()
        {
            var s = int.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, int>(), int.MinValue);
        }

        [Test]
        public void CastTo_Int_OutOfRangeMaxValue_NonOkay()
        {
            var s = ((long)int.MaxValue + 1).ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<int>(), 0);
        }

        [Test]
        public void CastTo_ObjectToIntMaxValue_Equal()
        {
            var s = int.MaxValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, int>(), int.MaxValue);
        }

        [Test]
        public void CastTo_Int_OutOfRangeMinValue_NonOkay()
        {
            var s = ((long)int.MinValue - 1).ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<int>(), 0);
        }

        // uint
        [Test]
        public void CastTo_ObjectToUIntValue_Equal()
        {
            Assert.AreEqual(((object)"42").CastTo<object, uint>(), 42);
        }

        [Test]
        public void CastTo_ObjectToUIntMinValue_Equal()
        {
            var s = uint.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, uint>(), uint.MinValue);
        }

        [Test]
        public void CastTo_ObjectToUIntMaxValue_Equal()
        {
            var s = uint.MaxValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, uint>(), uint.MaxValue);
        }

        // char
        [Test]
        public void CastTo_ObjectToCharValue_Equal()
        {
            Assert.AreEqual(((object)42).CastTo<object, char>(), 42);
        }

        [Test]
        public void CastTo_ObjectToCharMinValue_Equal()
        {
            var s = char.MinValue;
            Assert.AreEqual(s.CastTo<object, char>(), char.MinValue);
        }

        [Test]
        public void CastTo_ObjectToCharMaxValue_Equal()
        {
            var s = uint.MaxValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, uint>(), uint.MaxValue);
        }

        // byte
        [Test]
        public void CastTo_Byte_MinValue_Okay()
        {
            var s = byte.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, byte>(), byte.MinValue);
        }

        [Test]
        public void CastTo_Byte_OutOfRangeMaxValue_NonOkay()
        {
            var n = (byte.MaxValue + 1).ToString(CultureInfo.InvariantCulture);

            TestDelegate testDelegate = () => n.CastTo<object, byte>();
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.LogicError) );
        }

        [Test]
        public void CastTo_Byte_OutOfRangeMinValue_NonOkay()
        {
            var n = (byte.MinValue - 1).ToString(CultureInfo.InvariantCulture);
            TestDelegate testDelegate = () => n.CastTo<object, byte>();
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.LogicError));
        }

        // sbyte
        [Test]
        public void CastTo_SByte_MinValue_Okay()
        {
            var s = sbyte.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, sbyte>(), sbyte.MinValue);
        }

        [Test]
        public void CastTo_SByte_OutOfRangeMaxValue_NonOkay()
        {
            var n = (sbyte.MaxValue + 1).ToString(CultureInfo.InvariantCulture);
            TestDelegate testDelegate = () => n.CastTo<object, sbyte>();
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.LogicError));
        }

        [Test]
        public void CastTo_SByte_OutOfRangeMinValue_NonOkay()
        {
            var n = (sbyte.MinValue - 1).ToString(CultureInfo.InvariantCulture);
            TestDelegate testDelegate = () => n.CastTo<object, byte>();
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.LogicError));
        }

        // decimal
        [Test(ExpectedResult = 6.5, Description = "use fallback `object` converter")]
        public decimal CastTo_Decimal_SpecifiedValue_Okay()
        {
            return ((object)"6.5").CastTo<decimal>();
        }

        [Test(ExpectedResult = 6.5, Description = "use type-source `object` converter lookup")]
        public decimal CastTo_Decimal_WithExplicitObjectType_SpecifiedValue_Okay()
        {
            return ((object)"6.5").CastTo<object, decimal>();
        }

        [Test]
        public void CastTo_Decimal_MinValue_Okay()
        {
            var s = decimal.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, decimal>(), decimal.MinValue);
        }

        [Test]
        public void CastTo_Decimal_OutOfRangeMaxValue_NonOkay()
        {
            TestDelegate testDelegate = () =>
            {
                var n = (decimal.MaxValue + new decimal(1)).ToString(CultureInfo.InvariantCulture);
                n.CastTo<object, decimal>();
            };
            Assert.That(testDelegate, Throws.TypeOf<OverflowException>());
        }

        [Test]
        public void CastTo_Decimal_OutOfRangeMinValue_NonOkay()
        {
            TestDelegate testDelegate = () => {
                var n = (decimal.MinValue - new decimal(1)).ToString(CultureInfo.InvariantCulture);
                n.CastTo<object, decimal>();
            };
            Assert.That(testDelegate, Throws.TypeOf<OverflowException>());
        }

        // double
        [Test]
        public void CastTo_Double_MinValue_Okay()
        {
            var s = double.MinValue;
            Assert.AreEqual(s.CastTo<object, double>(), double.MinValue);
        }

        [Test]
        public void CastTo_Double_OutOfRangeMaxValue_NonOkay()
        {
            var n = (double.MaxValue + 1d).ToString(CultureInfo.InvariantCulture);
            TestDelegate testDelegate = () => n.CastTo<object, decimal>();
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.LogicError));
        }

        [Test]
        public void CastTo_Double_OutOfRangeMinValue_NonOkay()
        {
            var n = (double.MinValue - 1d).ToString(CultureInfo.InvariantCulture);
            TestDelegate testDelegate = () => n.CastTo<object, double>();
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.LogicError));
        }


        // float
        [Test(ExpectedResult = 6.5, Description = "use fallback `object` converter")]
        public float CastTo_Single_SpecifiedValue_Okay()
        {
            return ((object)"6.5").CastTo<float>();
        }

        [Test(ExpectedResult = 6.5, Description = "use type-source `object` converter lookup")]
        public float CastTo_Single_WithExplicitObjectType_SpecifiedValue_Okay()
        {
            return ((object)"6.5").CastTo<object, float>();
        }

        [Test]
        public void CastTo_Single_MinValue_Okay()
        {
            var s = float.MinValue;
            Assert.AreEqual(s.CastTo<object, float>(), float.MinValue);
        }

        [Test]
        public void CastTo_Single_OutOfRangeMaxValue_NonOkay()
        {
            var n = (float.MaxValue + 1f);
            var s = n.ToString(CultureInfo.InvariantCulture);

            Assert.That(s.CastTo<object, float>(),  Is.LessThan(n) );
        }

        [Test]
        public void CastTo_Single_OutOfRangeMinValue_NonOkay()
        {
            var n = (float.MinValue - 1f);
            var s = n.ToString(CultureInfo.InvariantCulture);

            TestDelegate testDelegate = () => {
                n.CastTo<object, float>();
            };
            Assert.That(s.CastTo<object, float>(), Is.GreaterThan(n));
        }

        [Test]
        public void CastTo_Float_SpecifiedValue_Okay()
        {
            Assert.AreEqual("6.5".CastTo<float>(), 6.5f);
        }

        // DateTime
        [Test]
        public void CastTo_DateTime_NoValidFormatSpecifiedValue_NonOkay()
        {
            var n = "2014//12//02 11:00:00";
            TestDelegate testDelegate = () => n.CastTo<object, DateTime>();
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));
        }

        [Test]
        public void CastTo_DateTime_SpecifiedValue_Okay()
        {
            Assert.AreEqual("2014-12-02 11:00:00".CastTo<object, DateTime>(), new DateTime(2014, 12, 2, 11, 0, 0));
        }


        // long
        [Test(ExpectedResult = +653234324234234, Description = "use fallback `object` converter")]
        public long CastTo_Long_SpecifiedValue_Okay()
        {
            return ((object)"653234324234234").CastTo<long>();
        }

        [Test(ExpectedResult = -653234324234234, Description = "use type-source `object` converter lookup")]
        public long CastTo_Long_WithExplicitObjectType_SpecifiedValue_Okay()
        {
            return ((object)"-653234324234234").CastTo<object, long>();
        }

        [Test]
        public void CastTo_Long_MinValue_Okay()
        {
            var s = long.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, long>(), long.MinValue);
        }

        [Test]
        public void CastTo_Long_OutOfRangeMaxValue_NonOkay()
        {
            TestDelegate testDelegate = () =>
            {
                var n = (long.MaxValue + 1f).ToString(CultureInfo.InvariantCulture);
                n.CastTo<object, long>();
            };
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));
        }

        [Test]
        public void CastTo_Long_OutOfRangeMinValue_NonOkay()
        {
            TestDelegate testDelegate = () => {
                var n = (long.MinValue - 1f).ToString(CultureInfo.InvariantCulture);
                n.CastTo<object, long>();
            };
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));
        }

        [Test]
        public void CastTo_Long_MaxValue_Okay()
        {
            var s = long.MaxValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<long>(), long.MaxValue);
        }


        // ulong
        [Test(ExpectedResult = 18446744073709551615, Description = "use fallback `object` converter")]
        public ulong CastTo_ULong_SpecifiedValue_Okay()
        {
            return ((object)"18446744073709551615").CastTo<ulong>();
        }

        [Test(ExpectedResult = 18446744073709551615, Description = "use type-source `object` converter lookup")]
        public ulong CastTo_ULong_WithExplicitObjectType_SpecifiedValue_Okay()
        {
            return ((object)"18446744073709551615").CastTo<object, ulong>();
        }

        [Test]
        public void CastTo_ULong_MinValue_Okay()
        {
            var s = ulong.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, ulong>(), ulong.MinValue);
        }

        [Test]
        public void CastTo_ULong_OutOfRangeMaxValue_NonOkay()
        {
            TestDelegate testDelegate = () =>
            {
                var n = (ulong.MaxValue + 1f).ToString(CultureInfo.InvariantCulture);
                n.CastTo<object, ulong>();
            };
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));
        }

        [Test]
        public void CastTo_ULong_OutOfRangeMinValue_NonOkay()
        {
            TestDelegate testDelegate = () => {
                var n = (ulong.MinValue - 1f).ToString(CultureInfo.InvariantCulture);
                n.CastTo<object, ulong>();
            };
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));
        }

        [Test]
        public void CastTo_ULong_MaxValue_Okay()
        {
            var s = ulong.MaxValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<ulong>(), ulong.MaxValue);
        }



        // short
        [Test(ExpectedResult = 32767, Description = "use fallback `object` converter")]
        public short CastTo_Short_SpecifiedValue_Okay()
        {
            return ((object)"32767").CastTo<short>();
        }

        [Test(ExpectedResult = -32768, Description = "use type-source `object` converter lookup")]
        public short CastTo_Short_WithExplicitObjectType_SpecifiedValue_Okay()
        {
            return ((object)"-32768").CastTo<object, short>();
        }

        [Test]
        public void CastTo_Short_MinValue_Okay()
        {
            var s = short.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, short>(), short.MinValue);
        }

        [Test]
        public void CastTo_Short_OutOfRangeMaxValue_NonOkay()
        {
            TestDelegate testDelegate = () =>
            {
                var n = (short.MaxValue + 1f).ToString(CultureInfo.InvariantCulture);
                n.CastTo<object, short>();
            };
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));
        }

        [Test]
        public void CastTo_Short_OutOfRangeMinValue_NonOkay()
        {
            TestDelegate testDelegate = () => {
                var n = (short.MinValue - 1f).ToString(CultureInfo.InvariantCulture);
                n.CastTo<object, short>();
            };
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));
        }

        [Test]
        public void CastTo_Short_MaxValue_Okay()
        {
            var s = short.MaxValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<short>(), short.MaxValue);
        }


        // ushort
        [Test(ExpectedResult = 65535, Description = "use fallback `object` converter")]
        public ushort CastTo_UShort_SpecifiedValue_Okay()
        {
            return ((object)"65535").CastTo<ushort>();
        }

        [Test(ExpectedResult = 65535, Description = "use type-source `object` converter lookup")]
        public ushort CastTo_UShort_WithExplicitObjectType_SpecifiedValue_Okay()
        {
            return ((object)"65535").CastTo<object, ushort>();
        }

        [Test]
        public void CastTo_UShort_MinValue_Okay()
        {
            var s = ushort.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<object, ushort>(), ushort.MinValue);
        }

        [Test]
        public void CastTo_UShort_OutOfRangeMaxValue_NonOkay()
        {
            TestDelegate testDelegate = () =>
            {
                var n = (ushort.MaxValue + 1f).ToString(CultureInfo.InvariantCulture);
                n.CastTo<object, ushort>();
            };
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));
        }

        [Test]
        public void CastTo_UShort_OutOfRangeMinValue_NonOkay()
        {
            TestDelegate testDelegate = () => {
                var n = (ushort.MinValue - 1f).ToString(CultureInfo.InvariantCulture);
                n.CastTo<object, ushort>();
            };
            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));
        }

        [Test]
        public void CastTo_UShort_MaxValue_Okay()
        {
            var s = ushort.MaxValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(s.CastTo<ushort>(), ushort.MaxValue);
        }

    }
}