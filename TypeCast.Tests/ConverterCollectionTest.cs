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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using NUnit.Framework;

    using Core;
    using Core.TypeCast;
    using Core.TypeCast.Base;

    using Example1.Converters;

    [Category(nameof(TypeCast))]
    [TestFixture]
    [Description("Basic and advanced tests that sample the sanity and integrity of the ConverterCollection")]
    public class ConverterCollectionTest
    {
        [Test(Description = "Check that instantiation without parameters succeeds, and the property values are as expected")]
        private void AddConvertersToCurrentInstance()
        {
            var cc = ConverterCollection.CurrentInstance;
            // add simple converter  with external dependencies for demonstration
            ConverterCollection.CurrentInstance.Add(
                (int[] a, string b) => new MemoryStream(Encoding.ASCII.GetBytes(a.ToString() + b.ToString()))
            );

        }

        [Test(Description = "Check that instantiation without parameters succeeds, and the property values are as expected")]
        public void ConverterCollectionTest_InstantiateWithoutParameters_TestGeneralParameters()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var cc = new ConverterCollection();

            Assert.IsNotNull(cc);

            Assert.IsInstanceOf<ConverterCollection>(cc);

            Assert.IsTrue(cc.Count == 0);

            Assert.IsNotNull(cc.Items);

            Assert.IsInstanceOf<BlockingCollection<TypeCast.Base.Converter>>(cc.Items);

            Assert.IsTrue(cc.Items.Count == 0);

            Assert.IsNotNull(cc.Settings);

            Assert.IsNull(cc.ApplicationNameSpace);

            Assert.IsNotNull(cc.ConstructorAddedClasses);

            Assert.IsNull(cc.CurrentConverterByAttribute);

            Assert.IsNotNull(cc.AssemblyInitialized);

            Assert.IsNotNull(cc.ConverterClassInitialized);

            Assert.IsNotNull(cc.FactoryBaseClass);

            Assert.IsNotNull(cc.Factory);

            // test singleton properties
            Assert.IsNull(cc.Manager);
        }

        [Test(Description = "Check that instantiation without parameters succeeds, and that the IQueryable interface is ready.")]
        public void ConverterCollectionTest_InstantiateWithoutParameters_CheckIQueryableProperties()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var cc = new ConverterCollection();

            Assert.IsNotNull(cc);

            Assert.IsInstanceOf<ConverterCollection>(cc);

            Assert.IsTrue(cc.Count == 0);

            Assert.IsNotNull(cc.Items);

            Assert.IsFalse(cc.IsAddingCompleted);

            Assert.IsNotNull(cc.ElementType);

            Assert.IsNotNull(cc.Expression);

            Assert.IsNotNull(cc.Provider);

        }

        [Test(Description = "Check that instantiation without parameters succeeds, and that the IQueryable interface is null.")]
        public void ConverterCollectionTest_InstantiateWithoutParameters_CheckIEnumerableProperties()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var cc = new ConverterCollection();

            Assert.IsNotNull(cc);

            Assert.IsInstanceOf<ConverterCollection>(cc);

            Assert.IsTrue(cc.Count == 0);

            Assert.IsNotNull(cc.Items);

            Assert.IsInstanceOf<IEnumerator<Converter>>(cc.GetEnumerator());
        }

        [Test(Description = "Check that instantiation without parameters succeeds okay, and that the IEnumerable interface is ready.")]
        public void ConverterCollectionTest_InstantiateWithParameter_Assembly()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var cc = new ConverterCollection(assembly: this.GetType().Assembly);

            Assert.IsNotNull(cc);

            Assert.IsInstanceOf<ConverterCollection>(cc);

            Assert.IsTrue(cc.Count >= 0);

            Assert.IsTrue(cc.Count == cc.Items.Count);

            Assert.IsNotNull(cc.Items);

            Assert.IsInstanceOf<Converter>(cc[0]);

            Assert.IsInstanceOf<Converter>(cc[cc.Count-1]);
        }

        [Test(Description = "Check that instantiation with parameters succeeds, and the collection shows expected values.")]
        public void ConverterCollectionTest_InstantiateWithParameter_AssemblyNameSpace()
        {
            ConverterCollection.CurrentInstance.Dispose();
            var cc = new ConverterCollection(assemblyNameSpace: typeof(ConverterCollection));

            Assert.IsNotNull(cc);

            Assert.IsInstanceOf<ConverterCollection>(cc);

            Assert.IsTrue(cc.Count >= 0);

            Assert.IsTrue(cc.Count == cc.Items.Count);

            Assert.IsNotNull(cc.Items);

            Assert.IsNull(cc[0]);
        }

        [Test(Description = "Check that instantiation with parameters succeeds, and the collection shows expected values.")]
        public void ConverterCollectionTest_InstantiateWithParameter_Types()
        {
            ConverterCollection.CurrentInstance.Dispose();
            var cc = new ConverterCollection(
                         assemblyNameSpace: null,
                         numberFormatDefault: null,
                         converterClasses: new[] { typeof(ConverterCollection) });

            Assert.IsNotNull(cc);

            Assert.IsInstanceOf<ConverterCollection>(cc);

            Assert.IsTrue(cc.Count >= 0);

            Assert.IsTrue(cc.Count == cc.Items.Count);

            Assert.IsNotNull(cc.Items);

            Assert.IsNull(cc[0]);
        }

        [Test(Description = "Check that instantiation with parameters succeeds, and the collection shows expected values.")]
        public void ConverterCollectionTest_InstantiateWithParameter_TypesSpecific()
        {
            ConverterCollection.CurrentInstance.Dispose();
            var cc = new ConverterCollection(
                         assemblyNameSpace: null,
                         numberFormatDefault: null,
                         converterClasses: new[] { typeof(BoolToString), typeof(ConverterStringToDecimal) });

            Assert.IsNotNull(cc);

            Assert.IsInstanceOf<ConverterCollection>(cc);

            Assert.IsTrue(cc.Count >= 4);

            Assert.IsTrue(cc.Count == cc.Items.Count);

            Assert.IsNotNull(cc.Items);

            Assert.IsNotNull(cc[0]);

            Assert.IsTrue(cc.CanConvertFrom<bool>());

            Assert.IsTrue(cc.CanConvertTo<decimal>());
        }

        [Test(Description = "Check that instantiation without parameters succeeds okay, and that the iqueryable interface is null.")]
        public void ConverterCollectionTest_InstantiateAndSampleConverters()
        {
            ConverterCollection.CurrentInstance.Dispose();
            ConverterCollection.CurrentInstance.Initialize(this.GetType().Assembly);

            var cc = ConverterCollection.CurrentInstance;

            Assert.IsNotNull(cc);

            Assert.IsInstanceOf<ConverterCollection>(cc);

            Assert.IsNotNull(cc.Items);

            var matrixConverter = cc.Get<float[,], float>();

            Assert.IsNotNull(matrixConverter);

            Assert.AreEqual(typeof(float[,]), matrixConverter.From);

            Assert.AreEqual(typeof(float), matrixConverter.To);

            Assert.AreEqual(typeof(object), matrixConverter.Argument);

            Assert.IsFalse(matrixConverter.AllowDisambiguates);

            Assert.IsInstanceOf<Converter>(matrixConverter);

            var matrixConverterRef = cc[typeof(float[,]), typeof(float)];

            Assert.AreEqual(matrixConverter, matrixConverterRef);
        }
    }
}