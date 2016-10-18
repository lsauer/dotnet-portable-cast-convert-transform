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

    using Core.TypeCast.Base;

    using NUnit.Framework;

    [Category(nameof(TypeCast))]
    [TestFixture]
    [Converter(dependencyInjection: false, loadOnDemand: false, name: nameof(ConverterMethodAttributeTest),
         nameSpace: nameof(TypeCast))]
    public class ConverterContextTest
    {
        [Test(Description = "Check which properties are readonly, writeonly or are fully mutable.")]
        public void ConvertContext_AssertWriteAndReadProperties()
        {
            var valueToPass = new { number = 100, key = "dismiss" };
            var convertContext = new ConvertContext(valueToPass);
            Assert.IsNotNull(convertContext);

            var type = typeof(ConvertContext);

            Assert.IsTrue(type.GetProperty(nameof(convertContext.Argument)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(convertContext.Argument)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(convertContext.Caller)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(convertContext.Caller)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(convertContext.Converter)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(convertContext.Converter)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(convertContext.From)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(convertContext.From)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(convertContext.To)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(convertContext.To)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(convertContext.Method)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(convertContext.Method)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(convertContext.MethodInfo)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(convertContext.MethodInfo)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(convertContext.Nullable)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(convertContext.Nullable)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(convertContext.ThrowExceptions)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(convertContext.ThrowExceptions)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(convertContext.Value)).CanRead);
            Assert.IsFalse(type.GetProperty(nameof(convertContext.Value)).CanWrite);
        }

        // Console.WriteLine();
        [Test(Description = "Check that the invoked method receives the context object with expected values.")]
        public void ConvertContext_ConvertToInvokeWithConvertContextArgumenty()
        {
            ConvertContext convertContext = null;
            int inputValue = 1377;
            var cc = ConverterCollection.CurrentInstance;
            cc.Initialize(typeof(ConverterContextTest));
            try
            {
                false.ConvertTo<decimal>(new ConvertContext(inputValue));
            }
            catch (CaptureDataException exc)
            {
                convertContext = exc.CaptureData as ConvertContext;
            }

            Assert.IsNotNull(convertContext);

            var type = typeof(ConvertContext);

            var name = convertContext.GetType().Name;
            Assert.AreEqual(typeof(ConvertContext<object, decimal>).Name, name);
            Assert.That(() => convertContext, Is.TypeOf<ConvertContext<object, decimal>>());

            Assert.IsNull(convertContext.Method);
            Assert.IsNull(convertContext.MethodInfo);

            Assert.IsNotNull(convertContext.Nullable);
            Assert.IsNotNull(convertContext.ThrowExceptions);
            Assert.IsNotNull(convertContext.From);
            Assert.IsNotNull(convertContext.To);
            Assert.IsNotNull(convertContext.Value);
            Assert.IsNotNull(convertContext.Caller);
            Assert.IsNotNull(convertContext.Argument);

            // check the values
            Assert.IsFalse(convertContext.Nullable);
            Assert.IsTrue(convertContext.ThrowExceptions);
            Assert.AreEqual(typeof(object), convertContext.From);
            Assert.AreEqual(typeof(decimal), convertContext.To);
            Assert.AreEqual(typeof(int), convertContext.Argument);
            Assert.AreEqual(inputValue, convertContext.Value);
            Assert.AreEqual(nameof(ObjectExtension.TryConvert), convertContext.Caller);
        }

        [Test(Description = "Check that the invoked method receives as the context object with expected values.")]
        public void ConvertContext_ConvertToInvokeWithNamedContextArgument()
        {
            ConvertContext convertContext = null;
            int inputValue = 1377;
            var cc = ConverterCollection.CurrentInstance;
            cc.Initialize(typeof(ConverterContextTest));
            try
            {
                true.ConvertTo<string>(inputValue, withContext: true);
            }
            catch (CaptureDataException exc)
            {
                convertContext = exc.CaptureData as ConvertContext;
            }

            Assert.IsNotNull(convertContext);

            var type = typeof(ConvertContext);

            var name = convertContext.GetType().Name;
            Assert.AreEqual(typeof(ConvertContext<object, string>).Name, name);
            Assert.That(() => convertContext, Is.TypeOf<ConvertContext<object, string>>());

            Assert.IsNull(convertContext.Method);
            Assert.IsNull(convertContext.MethodInfo);

            Assert.IsNotNull(convertContext.Nullable);
            Assert.IsNotNull(convertContext.ThrowExceptions);
            Assert.IsNotNull(convertContext.From);
            Assert.IsNotNull(convertContext.To);
            Assert.IsNotNull(convertContext.Value);
            Assert.IsNotNull(convertContext.Caller);
            Assert.IsNotNull(convertContext.Argument);

            // check the values
            Assert.IsFalse(convertContext.Nullable);
            Assert.IsTrue(convertContext.ThrowExceptions);
            Assert.AreEqual(typeof(object), convertContext.From);
            Assert.AreEqual(typeof(string), convertContext.To);
            Assert.AreEqual(typeof(int), convertContext.Argument);
            Assert.AreEqual(inputValue, convertContext.Value);
            Assert.AreEqual(nameof(ObjectExtension.TryConvert), convertContext.Caller);
        }

        [Test(Description = "Check that the property values are changed correctly")]
        public void ConvertContext_ExistsAndChangeProperties()
        {
            var valueToPass = new { number = 100, key = "dismiss" };
            var convertContext = new ConvertContext(valueToPass)
                                     {
                                         Argument = this.GetType(),
                                         From = typeof(int),
                                         To = typeof(ConvertContext),
                                     };
            Assert.IsNotNull(convertContext);

            var type = typeof(ConvertContext);

            var name = convertContext.GetType().Name;
            Assert.AreEqual(nameof(ConvertContext), name);
            Assert.That(() => convertContext, Is.TypeOf<ConvertContext>());
            Assert.That(() => new ConvertContext(null), Is.TypeOf<ConvertContext>());

            Assert.IsNull(convertContext.Converter);

            Assert.AreEqual(default(bool?), convertContext.Nullable);

            Assert.IsInstanceOf<Type>(convertContext.Argument);
            Assert.AreEqual(this.GetType(), convertContext.Argument);

            Assert.AreEqual(default(string), convertContext.Caller);

            Assert.IsInstanceOf<Type>(convertContext.From);
            Assert.AreEqual(typeof(int), convertContext.From);

            Assert.IsInstanceOf<Type>(convertContext.To);
            Assert.AreEqual(typeof(ConvertContext), convertContext.To);

            Assert.IsNull(convertContext.Method);
            Assert.IsNull(convertContext.MethodInfo);
            Assert.IsNull(convertContext.ThrowExceptions);
            Assert.IsTrue(ReferenceEquals(valueToPass, convertContext.Value));
        }

        [Test(Description = "Check that the context exists and can be instantiated with a null argument.")]
        public void ConvertContext_ExistsAndInstantiatesWithParameterNull()
        {
            var convertContext = new ConvertContext(null);

            Assert.IsNotNull(convertContext);

            var type = typeof(ConvertContext);

            var name = convertContext.GetType().Name;
            Assert.AreEqual(nameof(ConvertContext), name);
            Assert.That(() => convertContext, Is.TypeOf<ConvertContext>());

            Assert.IsNull(convertContext.Converter);

            Assert.AreEqual(default(bool?), convertContext.Nullable);
            Assert.IsNull(convertContext.Argument);
            Assert.IsNull(convertContext.Caller);
            Assert.IsNull(convertContext.From);
            Assert.IsNull(convertContext.To);
            Assert.IsNull(convertContext.Value);
            Assert.IsNull(convertContext.ThrowExceptions);
            Assert.IsNull(convertContext.Method);
            Assert.IsNull(convertContext.MethodInfo);
        }
    }
}