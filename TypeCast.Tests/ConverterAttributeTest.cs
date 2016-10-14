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
    using System.Reflection;

    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

    using NUnit.Framework;

    using Core.TypeCast;

    [Category(nameof(Core.TypeCast))]
    [TestFixture]
    [Converter(dependencyInjection: true, loadOnDemand: true, name: nameof(ConverterAttributeTest), nameSpace: nameof(Core.TypeCast))]
    public class ConverterAttributeTest
    {
        [ConverterMethod]
        public bool TestMethod(ConverterAttributeTest instance)
        {
            return Object.ReferenceEquals(this, instance);
        }

        [Test(Description = "check that the attribute can be instantiated parameterless")]
        public void ConverterAttribute_ExistsAndInstantiatesWithoutParameters()
        {
            var name = nameof(ConverterAttribute);
            Assert.AreEqual("ConverterAttribute", name);
            Assert.That(() => new ConverterAttribute(), Is.TypeOf<ConverterAttribute>());

            var converterAttribute = new ConverterAttribute();
            Assert.IsNull(converterAttribute.BaseType);

            Assert.IsInstanceOf<bool>(converterAttribute.DependencyInjection);
            Assert.AreEqual(default(bool), converterAttribute.DependencyInjection);

            Assert.IsInstanceOf<Guid>(converterAttribute.Id);
            Assert.AreNotEqual(default(Guid), converterAttribute.Id);

            Assert.IsInstanceOf<bool>(converterAttribute.LoadOnDemand);
            Assert.AreEqual(default(bool), converterAttribute.LoadOnDemand);

            Assert.IsInstanceOf<string>(converterAttribute.Name);
            Assert.AreEqual(string.Empty, converterAttribute.Name);

            Assert.IsInstanceOf<string>(converterAttribute.NameSpace);
            Assert.AreEqual(string.Empty, converterAttribute.NameSpace);

            Assert.IsInstanceOf<Type>(converterAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterAttribute), converterAttribute.TypeId);

            Assert.GreaterOrEqual(converterAttribute.ToString().Length, 30);
        }

        [Test(Description = "check that instantiation is okay with parameters in specific order")]
        public void ConverterAttribute_ExistsAndInstantiatesWithParametersSequential()
        {
            var name = nameof(ConverterAttribute);
            Assert.AreEqual("ConverterAttribute", name);
            Assert.That(() => new ConverterAttribute(), Is.TypeOf<ConverterAttribute>());

            //without named parameters
            var converterAttribute = new ConverterAttribute(nameof(Core), nameof(ConverterAttribute_ExistsAndInstantiatesWithParametersSequential), true, true);
            Assert.IsNull(converterAttribute.BaseType);

            Assert.IsInstanceOf<bool>(converterAttribute.DependencyInjection);
            Assert.AreEqual(true, converterAttribute.DependencyInjection);

            Assert.IsInstanceOf<Guid>(converterAttribute.Id);
            Assert.AreNotEqual(default(Guid), converterAttribute.Id);

            Assert.IsInstanceOf<bool>(converterAttribute.LoadOnDemand);
            Assert.AreEqual(true, converterAttribute.LoadOnDemand);

            Assert.IsInstanceOf<string>(converterAttribute.Name);
            Assert.AreEqual(nameof(ConverterAttribute_ExistsAndInstantiatesWithParametersSequential), converterAttribute.Name);

            Assert.IsInstanceOf<string>(converterAttribute.NameSpace);
            Assert.AreEqual(nameof(Core), converterAttribute.NameSpace);

            Assert.IsInstanceOf<Type>(converterAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterAttribute), converterAttribute.TypeId);

            Assert.GreaterOrEqual(converterAttribute.ToString().Length, 30);
        }

        [Test(Description = "check that instantiation is okay with named parameters ")]
        public void ConverterAttribute_ExistsAndInstantiatesWithParametersNamed()
        {
            var name = nameof(ConverterAttribute);
            Assert.AreEqual("ConverterAttribute", name);
            Assert.That(() => new ConverterAttribute(), Is.TypeOf<ConverterAttribute>());

            //without named parameters
            var converterAttribute = new ConverterAttribute(
                                         nameSpace: nameof(Core),
                                         name: nameof(this.ConverterAttribute_ExistsAndInstantiatesWithParametersNamed),
                                         loadOnDemand: true,
                                         dependencyInjection: true);
            Assert.IsNull(converterAttribute.BaseType);

            Assert.IsInstanceOf<bool>(converterAttribute.DependencyInjection);
            Assert.AreEqual(true, converterAttribute.DependencyInjection);

            Assert.IsInstanceOf<Guid>(converterAttribute.Id);
            Assert.AreNotEqual(default(Guid), converterAttribute.Id);

            Assert.IsInstanceOf<bool>(converterAttribute.LoadOnDemand);
            Assert.AreEqual(true, converterAttribute.LoadOnDemand);

            Assert.IsInstanceOf<string>(converterAttribute.Name);
            Assert.AreEqual(nameof(ConverterAttribute_ExistsAndInstantiatesWithParametersNamed), converterAttribute.Name);

            Assert.IsInstanceOf<string>(converterAttribute.NameSpace);
            Assert.AreEqual(nameof(Core), converterAttribute.NameSpace);

            Assert.IsInstanceOf<Type>(converterAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterAttribute), converterAttribute.TypeId);

            Assert.GreaterOrEqual(converterAttribute.ToString().Length, 30);
        }

        [Test(Description = "retrieve attribute from this instance and test the property values")]
        public void ConverterAttribute_ExistsAndGetCustomAttributeFromThis()
        {
            var converterAttribute = this.GetType().GetCustomAttribute<ConverterAttribute>();
            Assert.IsNotNull(converterAttribute);
            
            var name = converterAttribute.GetType().Name;
            Assert.AreEqual("ConverterAttribute", name);
            Assert.That(() => new ConverterAttribute(), Is.TypeOf<ConverterAttribute>());
            Assert.IsNull(converterAttribute.BaseType);

            Assert.IsInstanceOf<bool>(converterAttribute.DependencyInjection);
            Assert.AreEqual(true, converterAttribute.DependencyInjection);

            Assert.IsInstanceOf<Guid>(converterAttribute.Id);
            Assert.AreNotEqual(default(Guid), converterAttribute.Id);

            Assert.IsInstanceOf<bool>(converterAttribute.LoadOnDemand);
            Assert.AreEqual(true, converterAttribute.LoadOnDemand);

            Assert.IsInstanceOf<string>(converterAttribute.Name);
            Assert.AreEqual(nameof(ConverterAttributeTest), converterAttribute.Name);

            Assert.IsInstanceOf<string>(converterAttribute.NameSpace);
            Assert.AreEqual(nameof(Core.TypeCast), converterAttribute.NameSpace);

            Assert.IsInstanceOf<Type>(converterAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterAttribute), converterAttribute.TypeId);

            Assert.GreaterOrEqual(converterAttribute.ToString().Length, 30);
        }

        [Test(Description = "Check which properties are readonly, writeonly or are fully mutable.")]
        public void ConverterAttribute_AssertWriteAndReadProperties()
        {
            var converterAttribute = this.GetType().GetCustomAttribute<ConverterAttribute>();
            Assert.IsNotNull(converterAttribute);

            var type = typeof(ConverterAttribute);

            Assert.IsTrue(type.GetProperty(nameof(ConverterAttribute.Id)).CanRead);
            Assert.IsFalse(type.GetProperty(nameof(ConverterAttribute.Id)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(ConverterAttribute.BaseType)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(ConverterAttribute.BaseType)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(ConverterAttribute.DependencyInjection)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(ConverterAttribute.DependencyInjection)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(ConverterAttribute.LoadOnDemand)).CanRead);
            Assert.IsFalse(type.GetProperty(nameof(ConverterAttribute.LoadOnDemand)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(ConverterAttribute.Name)).CanRead);
            Assert.IsTrue(type.GetProperty(nameof(ConverterAttribute.Name)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(ConverterAttribute.NameSpace)).CanRead);
            Assert.IsFalse(type.GetProperty(nameof(ConverterAttribute.NameSpace)).CanWrite);

            Assert.IsTrue(type.GetProperty(nameof(ConverterAttribute.TypeId)).CanRead);
            Assert.IsFalse(type.GetProperty(nameof(ConverterAttribute.TypeId)).CanWrite);
        }

        [Test(Description = "check that the property values are changed correctly")]
        public void ConverterAttribute_ExistsAndChangeProperties()
        {
            var converterAttribute = this.GetType().GetCustomAttribute<ConverterAttribute>();
            Assert.IsNotNull(converterAttribute);

            var name = converterAttribute.GetType().Name;
            Assert.AreEqual("ConverterAttribute", name);
            Assert.That(() => new ConverterAttribute(), Is.TypeOf<ConverterAttribute>());

            converterAttribute.BaseType = typeof(ConverterAttributeTest).GetTypeInfo();
            Assert.IsNotNull(converterAttribute.BaseType);
            Assert.AreEqual(typeof(ConverterAttributeTest), converterAttribute.BaseType.AsType());

            Assert.IsInstanceOf<bool>(converterAttribute.DependencyInjection);
            converterAttribute.DependencyInjection = false;
            Assert.AreEqual(false, converterAttribute.DependencyInjection);

            Assert.IsInstanceOf<Guid>(converterAttribute.Id);
            Assert.AreNotEqual(new ConverterAttribute().Id, converterAttribute.Id);

            Assert.IsInstanceOf<string>(converterAttribute.Name);
            converterAttribute.Name = nameof(ConverterAttribute_ExistsAndChangeProperties);
            Assert.AreEqual(nameof(ConverterAttribute_ExistsAndChangeProperties), converterAttribute.Name);

            Assert.IsInstanceOf<string>(converterAttribute.NameSpace);

            Assert.IsInstanceOf<Type>(converterAttribute.TypeId);
            Assert.AreEqual(typeof(ConverterAttribute), converterAttribute.TypeId);

            Assert.GreaterOrEqual(converterAttribute.ToString().Length, 30);
        }

        [Test(Description = "check that the overriden ToString method contains all expected values")]
        public void ConverterAttribute_ToStringContainsAll()
        {
            var converterAttribute = this.GetType().GetCustomAttribute<ConverterAttribute>();
            Assert.IsNotNull(converterAttribute);

            Assert.IsTrue(converterAttribute.ToString().IndexOf(Boolean.TrueString) != -1 );

            Assert.IsTrue(converterAttribute.ToString().IndexOf(Boolean.FalseString) == -1 );

            Assert.IsTrue(converterAttribute.ToString().IndexOf(nameof(ConverterAttributeTest)) != -1 );

            Assert.IsTrue(converterAttribute.ToString().IndexOf(nameof(Core.TypeCast)) != -1 );
            
        }

    }
}
