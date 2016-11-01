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
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using NUnit.Framework;
    using Core.TypeCast;

    using Example2.Converters;

    using Example6.Converters;

    [Category(nameof(Core.TypeCast))]
    [TestFixture]
    [Description("Advanced CastTo test scenarios, that complement the basic tests in the ConvertTo test scenario ")]
    public class CastToTests
    {
        [Test(Description = "Check culture invariance, and minimal value equality for a basic CLR type")]
        public void CastTo_Int_MinValue_Okay()
        {
            var s = int.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(s.CastTo<int>() == int.MinValue);
        }

        [Test(Description = "Check casting using the build in set of cnverters.")]
        public void CastTo_ObjectToUInt_Typed_Okay()
        {
            var s = 42.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(((object)s).CastTo<object, uint>() == 42);
        }

        [Test(Description = "Check initializing of a converter invoked through CastTo, outputing an image")]
        public void CastTo_External_Type_Image()
        {
            ConverterCollection.CurrentInstance.Dispose();

            ConverterCollection.CurrentInstance.Initialize(typeof(StringToImage));

            var image = "Some text to be saved...".CastTo<Image>();

            Assert.IsNotNull(image);

            Assert.IsInstanceOf<Image>(image);

            Assert.IsInstanceOf<Bitmap>(image);

            Assert.AreEqual(new SizeF(400, 100), image.PhysicalDimension);
        }

        [Test(Description = "Check for a failure of parsing a string to a decimal due to an unknown decimal symbol")]
        public void CastTo_ParseDecimal_Exception()
        {
            TestDelegate testDelegate = () =>
                {
                    var decValue = "500|500";
                    var decString = decValue.CastTo<object, decimal>();
                };

            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));

        }

        [Test(Description = "Check that compliance to parse a string with an unknown decimal separator proceeds without any exceptions")]
        public void CastTo_ParseDecimal_UseTry_Okay()
        {
            Func<decimal> testDelegate = () =>
                {
                    decimal decOut;
                    var decValue = "500|500";
                    var decString = decValue.TryCast<object, decimal>(out decOut);
                    return decOut;
                };

            Assert.That(testDelegate(), Is.EqualTo(0d));

        }

        public delegate float[,] Transpose1xN(float[] matrix);

        [Test(Description = "Check that the result of a reference type using a custom converter previously added")]
        public void CastTo_AddConverter_CustomReferenceType_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();
            var cc = ConverterCollection.CurrentInstance;

            // demonstrate a converter returning a copy-by-value instance:
            cc.Add((Point[] pts) => new Perimeter(pts));

            Func<Perimeter> testDelegate = () =>
                {
                    var plen = new[] {
                        new Point(0, 0),
                        new Point(-3, 4),
                        new Point(2, 2),
                        new Point(10, -3),
                        new Point(-10, 2)
                    }.CastTo<Perimeter>();
                    return plen;
                };

            Assert.That(testDelegate().Length, Is.EqualTo(50.632713094464989));

            cc.Add((Rectangle re) => new Perimeter(re));

            Func<Perimeter> testDelegate2 = () =>
            {
                var plen = new Rectangle(1, 2, 3, 4)
                            .CastTo<Perimeter>();
                return plen;
            };

            Assert.That(testDelegate2().Length, Is.EqualTo(14));
        }

        [Test(Description = "Check that an expected value using a aliased converter function previously added to the collection")]
        public void CastTo_ComplexType()
        {
            ConverterCollection.CurrentInstance.Dispose();

            ConverterCollection.CurrentInstance
                .Add<float[], float[,], Transpose1xN>((a) =>
                {
                    var result = new float[a.Length, 1];
                    for (int i = 0; i < a.Length; result[i, 0] = a[i], i++)
                        ;
                    return result;
                });

            var matrix = new float[,] { { 1f }, { 2f }, { 3f }, { 4f } };

            var matrixTransposed1xEx3 = new[] { 1f, 2f, 3f, 4f }.Transform<Transpose1xN>().CastTo<float[,]>();

            Assert.AreEqual(matrix, matrixTransposed1xEx3);
        }
    }
}
