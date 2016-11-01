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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    using NUnit.Framework;

    using Core;
    using Core.TypeCast;
    using Core.TypeCast.Base;

    using Example1.Converters;

    [Category(nameof(TypeCast))]
    [TestFixture]
    [Description("Basic and advanced tests that cover the functionality of the addBuilder")]
    public class ConverterCollectionAddBuilderTest
    {
        [Test(Description = "Check that the defferred addding of converters to the collection proceeds as expected.")]
        public void ConverterCollectionTest_Instantiate_And_BuildCollection_WithSettings()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var cc = new ConverterCollection();

            Assert.IsNotNull(cc);

            Assert.IsInstanceOf<ConverterCollection>(cc);

            Assert.IsTrue(cc.Count == 0);

            var initCount = cc.Count;

            Assert.IsNotNull(cc.Items);

            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            var builder =
                cc.AddStart<ConverterCollectionAddBuilderTest>(
                    new ConverterCollectionSettings { UseFunctionDefaultWrapper = false, },
                    cancellationToken: cancellationToken.Token);

            Assert.IsNotNull(builder);

            Assert.IsInstanceOf<ConverterCollection.AddBuilder<ConverterCollectionAddBuilderTest>>(builder);

            // assert that the adding is indeed deferred
            Assert.AreEqual(initCount, cc.Count);

            builder.Add<string, bool>(s => s.ToLowerInvariant() == "true" ? true : false);

            // assert that the adding is indeed deferred
            Assert.AreEqual(initCount, cc.Count);

            builder.Add<string, byte[]>(s => System.Text.ASCIIEncoding.ASCII.GetBytes(s));

            var collection = builder.End();

            Assert.AreEqual(initCount + 2, cc.Count);

            Assert.IsTrue( cc.CanConvertFrom<string>());

            Assert.IsTrue( cc.CanConvertTo<byte[]>());

            Assert.IsTrue( cc.CanConvertTo<bool>());

        }

        [Test(Description = "Check that the return values when adding coverters to the set using the deferred addBuilder.")]
        public void ConverterCollectionTest_Instantiate_And_BuildCollection_Without_Finalizing()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var cc = new ConverterCollection();

            Assert.IsNotNull(cc);

            Assert.IsInstanceOf<ConverterCollection>(cc);

            Assert.IsTrue(cc.Count == 0);

            var initCount = cc.Count;

            Assert.IsNotNull(cc.Items);

            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            var builder =
                cc.AddStart<ConverterCollectionAddBuilderTest>(
                    new ConverterCollectionSettings { UseFunctionDefaultWrapper = false, },
                    cancellationToken: cancellationToken.Token);

            Assert.IsNotNull(builder);

            Assert.IsInstanceOf<ConverterCollection.AddBuilder<ConverterCollectionAddBuilderTest>>(builder);

            Assert.AreEqual(initCount, cc.Count);

            builder.Add<string, bool>(s => s.ToLowerInvariant() == "true" ? true : false);

            var iBuilder = builder.Cancel();

            Assert.IsInstanceOf<IConverterCollection>(iBuilder);

            Assert.AreEqual(initCount, cc.Count);
        }
    }
}