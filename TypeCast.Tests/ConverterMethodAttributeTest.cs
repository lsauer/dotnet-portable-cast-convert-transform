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
    using System.Text;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

    using NUnit.Framework;

    using Core.Singleton;
    using Core.TypeCast;
    using Core;
    using Core.Extension;

    [Category(nameof(Core.TypeCast))]
    [TestFixture]
    [Converter(dependencyInjection: false, loadOnDemand: false, name: nameof(ConverterMethodAttributeTest), nameSpace: nameof(Core.TypeCast))]
    public class ConverterMethodAttributeTest
    {
        [Converter]
        [Description("A Test class containing methods to be discovered and added within a nested class, so long as decorated by a `Converter` attribute")]
        public class SomeNestedClass
        {
            [ConverterMethod]
            [Description("A simple method that returns its own declaring class, taking a string-cutoff size as parameter")]
            public string SomeNestedClassMethod(int cutoff)
            {
                var str = nameof(SomeNestedClassMethod).ToString();
                return str.Substring(0, Math.Min(cutoff, str.Length));
            }
        }

        [Description("A simple method to test the ConverterMethod attribute being passed and instantiated correctly. Returns true upon passing its own instance.")]
        [ConverterMethod(isStatic: false, loadOnDemand: false, name: nameof(ConverterMethodAttributeTest), passInstance: true)]
        public bool TestAttributeMethod(ConverterMethodAttributeTest instance)
        {
            return Object.ReferenceEquals(this, instance);
        }

        [Test(Description = "Check that the attribute can be instantiated parameterless")]
        public void ConverterMethodAttribute_ExistsAndInstantiatesWithoutParameters()
        {
            var name = nameof(ConverterMethodAttribute);
            Assert.AreEqual("ConverterMethodAttribute", name);
            Assert.That(() => new ConverterMethodAttribute(), Is.TypeOf<ConverterMethodAttribute>());

            var converterMethodAttribute = new ConverterMethodAttribute();
            Assert.IsNull(converterMethodAttribute.BaseType);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.IsStatic);
            Assert.AreEqual(true, converterMethodAttribute.IsStatic);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.LoadOnDemand);
            Assert.AreEqual(true, converterMethodAttribute.LoadOnDemand);

            Assert.IsInstanceOf<string>(converterMethodAttribute.Name);
            Assert.AreEqual(string.Empty, converterMethodAttribute.Name);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.PassInstance);
            Assert.AreEqual(default(bool), converterMethodAttribute.PassInstance);

            Assert.IsInstanceOf<Type>(converterMethodAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterMethodAttribute), converterMethodAttribute.TypeId);

            Assert.GreaterOrEqual(converterMethodAttribute.ToString().Length, 30);
        }

        [Test(Description = "Check that instantiation is okay with parameters in specific order")]
        public void ConverterMethodAttribute_ExistsAndInstantiatesWithParametersSequential()
        {
            var name = nameof(ConverterMethodAttribute);
            Assert.AreEqual("ConverterMethodAttribute", name);
            Assert.That(() => new ConverterMethodAttribute(), Is.TypeOf<ConverterMethodAttribute>());

            // Without named parameters, i.e. sequential
            var converterMethodAttribute = new ConverterMethodAttribute(
                                               false,
                                               false,
                                               nameof(ConverterMethodAttributeTest),
                                               true);
            Assert.IsNull(converterMethodAttribute.BaseType);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.IsStatic);
            Assert.AreEqual(false, converterMethodAttribute.IsStatic);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.LoadOnDemand);
            Assert.AreEqual(false, converterMethodAttribute.LoadOnDemand);

            Assert.IsInstanceOf<string>(converterMethodAttribute.Name);
            Assert.AreEqual(nameof(ConverterMethodAttributeTest), converterMethodAttribute.Name);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.PassInstance);
            Assert.AreEqual(true, converterMethodAttribute.PassInstance);

            Assert.IsInstanceOf<Type>(converterMethodAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterMethodAttribute), converterMethodAttribute.TypeId);

            Assert.GreaterOrEqual(converterMethodAttribute.ToString().Length, 30);
        }

        [Test(Description = "Check that instantiation is okay with named parameters ")]
        public void ConverterMethodAttribute_ExistsAndInstantiatesWithParametersNamed()
        {
            var name = nameof(ConverterMethodAttribute);
            Assert.AreEqual("ConverterMethodAttribute", name);
            Assert.That(() => new ConverterMethodAttribute(), Is.TypeOf<ConverterMethodAttribute>());

            // Without named parameters, i.e. sequential
            var converterMethodAttribute = new ConverterMethodAttribute(
                                               isStatic: false,
                                               loadOnDemand: false,
                                               name: nameof(ConverterMethodAttributeTest),
                                               passInstance: true);
            Assert.IsNull(converterMethodAttribute.BaseType);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.IsStatic);
            Assert.AreEqual(false, converterMethodAttribute.IsStatic);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.LoadOnDemand);
            Assert.AreEqual(false, converterMethodAttribute.LoadOnDemand);

            Assert.IsInstanceOf<string>(converterMethodAttribute.Name);
            Assert.AreEqual(nameof(ConverterMethodAttributeTest), converterMethodAttribute.Name);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.PassInstance);
            Assert.AreEqual(true, converterMethodAttribute.PassInstance);

            Assert.IsInstanceOf<Type>(converterMethodAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterMethodAttribute), converterMethodAttribute.TypeId);

            Assert.GreaterOrEqual(converterMethodAttribute.ToString().Length, 30);
        }

        [Test(Description = "retrieve attribute from this instance and test the property values")]
        public void ConverterMethodAttribute_ExistsAndGetCustomAttributeFromThis()
        {
            var converterMethodAttribute =
                this.GetType()
                    .GetMethod(nameof(this.TestAttributeMethod))
                    .GetCustomAttribute<ConverterMethodAttribute>();
            Assert.IsNotNull(converterMethodAttribute);

            var name = converterMethodAttribute.Name;
            Assert.AreEqual(nameof(ConverterMethodAttributeTest), name);
            Assert.That(() => new ConverterMethodAttribute(), Is.TypeOf<ConverterMethodAttribute>());
            Assert.IsNull(converterMethodAttribute.BaseType);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.IsStatic);
            Assert.AreEqual(false, converterMethodAttribute.IsStatic);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.LoadOnDemand);
            Assert.AreEqual(false, converterMethodAttribute.LoadOnDemand);

            Assert.IsInstanceOf<string>(converterMethodAttribute.Name);
            Assert.AreEqual(nameof(ConverterMethodAttributeTest), converterMethodAttribute.Name);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.PassInstance);
            Assert.AreEqual(true, converterMethodAttribute.PassInstance);

            Assert.IsInstanceOf<Type>(converterMethodAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterMethodAttribute), converterMethodAttribute.TypeId);

            Assert.GreaterOrEqual(converterMethodAttribute.ToString().Length, 30);
        }

        [Test(Description = "Check which properties are readonly, writeonly or are fully mutable.")]
        public void ConverterMethodAttribute_AssertWriteAndReadProperties()
        {
            var converterMethodAttribute =
                this.GetType()
                    .GetMethod(nameof(this.TestAttributeMethod))
                    .GetCustomAttribute<ConverterMethodAttribute>();
            Assert.IsNotNull(converterMethodAttribute);

            var type = typeof(ConverterMethodAttribute);

            Assert.IsTrue(type.GetProperty(nameof(ConverterMethodAttribute.BaseType)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(ConverterMethodAttribute.BaseType)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(ConverterMethodAttribute.IsStatic)).CanRead);
            Assert.IsFalse(type.GetProperty(nameof(ConverterMethodAttribute.IsStatic)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(ConverterMethodAttribute.LoadOnDemand)).CanRead);
            Assert.IsFalse(type.GetProperty(nameof(ConverterMethodAttribute.LoadOnDemand)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(ConverterMethodAttribute.Name)).CanRead);
            Assert.IsFalse(type.GetProperty(nameof(ConverterMethodAttribute.Name)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(ConverterMethodAttribute.TypeId)).CanRead);
            Assert.IsFalse(type.GetProperty(nameof(ConverterMethodAttribute.TypeId)).CanWrite);
        }

        [Test(Description = "Check that the property values are changed correctly")]
        public void ConverterMethodAttribute_ExistsAndChangeProperties()
        {
            var converterMethodAttribute =
                this.GetType()
                    .GetMethod(nameof(this.TestAttributeMethod))
                    .GetCustomAttribute<ConverterMethodAttribute>();
            Assert.IsNotNull(converterMethodAttribute);

            var name = converterMethodAttribute.GetType().Name;
            Assert.AreEqual("ConverterMethodAttribute", name);
            Assert.That(() => new ConverterMethodAttribute(), Is.TypeOf<ConverterMethodAttribute>());

            converterMethodAttribute.BaseType = typeof(ConverterMethodAttributeTest).GetTypeInfo();
            Assert.IsNotNull(converterMethodAttribute.BaseType);
            Assert.AreEqual(typeof(ConverterMethodAttributeTest), converterMethodAttribute.BaseType);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.LoadOnDemand);
            Assert.AreEqual(false, converterMethodAttribute.LoadOnDemand);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.PassInstance);
            Assert.AreEqual(true, converterMethodAttribute.PassInstance);

            Assert.IsInstanceOf<string>(converterMethodAttribute.Name);
            Assert.AreEqual(nameof(ConverterMethodAttributeTest), converterMethodAttribute.Name);

            Assert.IsInstanceOf<Type>(converterMethodAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterMethodAttribute), converterMethodAttribute.TypeId);

            Assert.GreaterOrEqual(converterMethodAttribute.ToString().Length, 30);
        }

        [Test(Description = "Check that the overriden ToString method contains all expected values")]
        public void ConverterMethodAttribute_ToStringContainstAll()
        {
            var converterMethodAttribute =
                this.GetType()
                    .GetMethod(nameof(this.TestAttributeMethod))
                    .GetCustomAttribute<ConverterMethodAttribute>();
            Assert.IsNotNull(converterMethodAttribute);

            Assert.IsTrue(converterMethodAttribute.ToString().IndexOf(Boolean.TrueString) == -1);

            Assert.IsTrue(converterMethodAttribute.ToString().IndexOf(Boolean.FalseString) != -1);

            Assert.IsTrue(converterMethodAttribute.ToString().IndexOf(nameof(ConverterMethodAttributeTest)) != -1);
        }

        [Test(Description = "Check that all converter methods in the class are loaded and are set to their expected values, respectively")]
        public void ConverterMethodAttributeTest_InitializeAndGetMethodAttributeFromThis()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var someNumber = 100.CastTo<string>();
            var cc = ConverterCollection.CurrentInstance;

            cc.Initialize(this.GetType());

            // discovered `SomeNestedClassMethod` alright?
            Assert.IsTrue(cc.WithFunctionName(nameof(SomeNestedClass.SomeNestedClassMethod)).Count() == 1);

            // fetch the declared converter test Method in this class
            var converterMethod = cc.Get<ConverterMethodAttributeTest, bool>();

            Assert.IsNull(converterMethod.FunctionAttribute);

            Assert.IsNotNull(converterMethod.FunctionDefaultAttribute);

            var converterMethodAttribute = converterMethod.FunctionDefaultAttribute;

            var name = converterMethodAttribute.Name;
            Assert.AreEqual(nameof(ConverterMethodAttributeTest), name);
            Assert.That(() => new ConverterMethodAttribute(), Is.TypeOf<ConverterMethodAttribute>());
            Assert.IsNull(converterMethodAttribute.BaseType);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.IsStatic);
            Assert.AreEqual(false, converterMethodAttribute.IsStatic);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.LoadOnDemand);
            Assert.AreEqual(false, converterMethodAttribute.LoadOnDemand);

            Assert.IsInstanceOf<string>(converterMethodAttribute.Name);
            Assert.AreEqual(nameof(ConverterMethodAttributeTest), converterMethodAttribute.Name);

            Assert.IsInstanceOf<bool>(converterMethodAttribute.PassInstance);
            Assert.AreEqual(true, converterMethodAttribute.PassInstance);

            Assert.IsInstanceOf<Type>(converterMethodAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterMethodAttribute), converterMethodAttribute.TypeId);

            Assert.GreaterOrEqual(converterMethodAttribute.ToString().Length, 30);
        }
    }
}
