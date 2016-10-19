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
    using System.Globalization;

    using NUnit.Framework;

    [Category(nameof(TypeCast))]
    [TestFixture]
    public class ConvertNumberFormatInfoTest
    {
        [Test(Description = "Check that NumberFormatInfo instances can be set, leading to an expected parsed value.")]
        public void ConvertNumberFormatInfoTest_WrongFormatAndTryWithCustomConverter()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var inputValue = "500,500";
            decimal outputValue = default(decimal);
            // Hold a reference and pass it to the ConverterCollection constructor. 
            // Alternatively a custom field may be added to a given converter class to set a custom number-format
            var numberFormatInfoInstance = new NumberFormatInfo()
                                               {
                                                   NumberGroupSeparator = ".",
                                                   NumberDecimalSeparator = ".",
                                                   NumberDecimalDigits = 5
                                               };
            var cc = new ConverterCollection(this.GetType().Assembly, numberFormatDefault: numberFormatInfoInstance);

            Func<decimal> testDelegate = () =>
                {
                    // applying `NumberStyles.Any` will allow to ignore any potential separator character
                    outputValue = inputValue.CastTo<object, decimal>();
                    return outputValue;
                };
            Assert.AreEqual(numberFormatInfoInstance, ConverterCollection.CurrentInstance.Settings.NumberFormat);

            Assert.IsTrue(inputValue.IndexOf(",") != -1);

            Assert.IsTrue(numberFormatInfoInstance.NumberDecimalSeparator != ",");

            Assert.That(() => testDelegate(), Is.TypeOf<decimal>().And.EqualTo(500500M));

            Assert.AreEqual(500500M, outputValue);
        }

        [Test(Description = "Check that NumberFormatInfo instances can be set, and that an expected exception is thrown.")]
        public void ConvertNumberFormatInfoTest_WrongFormatAndThrowException()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var inputValue = "500,500";
            decimal outputValue = default(decimal);
            // Hold a reference and pass it to the ConverterCollection constructor. 
            // Alternatively a custom field may be added to a given converter class to set a custom number-format
            var numberFormatInfoInstance = new NumberFormatInfo()
                                               {
                                                   NumberGroupSeparator = ".",
                                                   NumberDecimalSeparator = ".",
                                                   NumberDecimalDigits = 5
                                               };
            var cc = new ConverterCollection(this.GetType().Assembly, numberFormatDefault: numberFormatInfoInstance);

            Func<decimal> testDelegate = () =>
                {
                    outputValue = inputValue.CastTo<string, decimal>();
                    return outputValue;
                };
            Assert.AreEqual(numberFormatInfoInstance, ConverterCollection.CurrentInstance.Settings.NumberFormat);

            Assert.IsTrue(inputValue.IndexOf(",") != -1);

            Assert.IsTrue(numberFormatInfoInstance.NumberDecimalSeparator != ",");

            Assert.That(
                () => testDelegate(),
                Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.BadInputFormat));
        }

        // Console.WriteLine();
        [Test(Description = "Check that NumberFormatInfo can be used to change the input value, leading to an expected parsed value.")]
        public void ConvertNumberFormatInfoTest_ChangeInputAndParse()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var inputValue = "500.500";
            decimal outputValue = default(decimal);
            // Hold a reference and pass it to the ConverterCollection constructor. 
            // Alternatively a custom field may be added to a given converter class to set a custom number-format
            var numberFormatInfoInstance = new NumberFormatInfo()
                                               {
                                                   NumberGroupSeparator = ".",
                                                   NumberDecimalSeparator = ".",
                                                   NumberDecimalDigits = 5
                                               };
            var cc = new ConverterCollection(this.GetType().Assembly);

            Func<decimal> testDelegate = () =>
                {
                    outputValue = inputValue.CastTo<string, decimal>();
                    return outputValue;
                };
            Assert.AreNotEqual(numberFormatInfoInstance, ConverterCollection.CurrentInstance.Settings.NumberFormat);
            var separator = ConverterCollection.CurrentInstance.Settings.NumberFormat.NumberDecimalSeparator;
            inputValue = inputValue.Replace(".", separator);

            Assert.IsTrue(inputValue.IndexOf(separator) != -1);

            Assert.IsTrue(numberFormatInfoInstance.NumberDecimalSeparator != ",");

            Assert.That(() => testDelegate(), Is.EqualTo(500.500M));

            Assert.AreEqual(500.500M, outputValue);

            ConverterCollection.CurrentInstance.Settings.NumberFormat = numberFormatInfoInstance;

            Assert.AreEqual(numberFormatInfoInstance, ConverterCollection.CurrentInstance.Settings.NumberFormat);
        }

        [Test(Description = "Check that NumberFormatInfo instances can be set, leading to an expected parsed value.")]
        public void ConvertNumberFormatInfoTest_ChangeFormatAndParse()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var inputValue = "500|500";
            decimal outputValue = default(decimal);
            // Hold a reference and pass it to the ConverterCollection constructor. 
            // Alternatively a custom field may be added to a given converter class to set a custom number-format
            var numberFormatInfoInstance = new NumberFormatInfo()
                                               {
                                                   NumberGroupSeparator = ".",
                                                   NumberDecimalSeparator = "|",
                                                   NumberDecimalDigits = 5
                                               };
            var cc = new ConverterCollection(this.GetType().Assembly);

            Func<decimal> testDelegate = () =>
                {
                    outputValue = inputValue.CastTo<string, decimal>();
                    return outputValue;
                };
            Assert.AreNotEqual(numberFormatInfoInstance, ConverterCollection.CurrentInstance.Settings.NumberFormat);

            ConverterCollection.CurrentInstance.Settings.NumberFormat = numberFormatInfoInstance;

            Assert.AreEqual(numberFormatInfoInstance, ConverterCollection.CurrentInstance.Settings.NumberFormat);

            Assert.That(() => testDelegate(), Is.EqualTo(500.500M));

            Assert.AreEqual(500.500M, outputValue);
        }

        [Test(Description = "Check that NumberFormatInfo instances is used to try and parse the input to an expected resulting value.")]
        public void ConvertNumberFormatInfoTest_TryParseWithSuccess()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var inputValue = "500|500";
            decimal outputValue = default(decimal);
            // Hold a reference and pass it to the ConverterCollection constructor. 
            // Alternatively a custom field may be added to a given converter class to set a custom number-format
            var numberFormatInfoInstance = new NumberFormatInfo()
                                               {
                                                   NumberGroupSeparator = ".",
                                                   NumberDecimalSeparator = "|",
                                                   NumberDecimalDigits = 5
                                               };
            var cc = new ConverterCollection(this.GetType().Assembly);

            Func<bool> testDelegate = () =>
                {
                    // applying `NumberStyles.Any` will allow to ignore any potential separator character
                    var success = inputValue.TryCast<object, decimal>(out outputValue);
                    return success;
                };
            Assert.AreNotEqual(numberFormatInfoInstance, ConverterCollection.CurrentInstance.Settings.NumberFormat);

            ConverterCollection.CurrentInstance.Settings.NumberFormat = numberFormatInfoInstance;

            Assert.AreEqual(numberFormatInfoInstance, ConverterCollection.CurrentInstance.Settings.NumberFormat);

            Assert.That(() => testDelegate(), Is.True);

            Assert.AreEqual(500.500M, outputValue);
        }

        [Test(Description = "Check that NumberFormatInfo instances is used to try and parse the input to result in a predetermined failure.")]
        public void ConvertNumberFormatInfoTest_TryParseWithFailure()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var inputValue = "500|500";
            decimal outputValue = default(decimal);

            var cc = new ConverterCollection(this.GetType().Assembly);

            Func<bool> testDelegate = () =>
                {
                    // applying `NumberStyles.Any` will allow to ignore any potential separator character
                    var failure = inputValue.TryCast<object, decimal>(out outputValue);
                    return failure;
                };

            Assert.That(() => testDelegate(), Is.False);

            Assert.AreEqual(0M, outputValue);
        }
    }
}