// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example3
{
    using System;
    using System.Reflection;

    using System.Globalization;
    using System.IO;
    using System.Text;

    using Core.TypeCast;

    using Core.Extensions;

    /// <summary>
    ///  Demonstrate using a custom  <see cref="NumberFormatInfo"/> provider instance for numeric `Converters`
    /// </summary>
    [Description("Demonstrate using a custom `NumberFormatInfo` provider instance for numeric `Converters`")]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            // Hold a reference and pass it to the ConverterCollection constructor. 
            // Alternatively a custom field may be added to a given converter class to set a custom number-format
            var numberFormatInfoInstance = new NumberFormatInfo()
            {
                NumberGroupSeparator = ",",
                NumberDecimalSeparator = ",",
                NumberDecimalDigits = 5
            };
            var cc = new ConverterCollection(typeof(Program), numberFormatInfoInstance);

            var decString = "500,500".CastTo<object, decimal>();

            Console.WriteLine($"Decimal Value with NumberGroupSeparator ({cc.Settings.NumberFormat.NumberDecimalSeparator}): {decString.ToString(numberFormatInfoInstance)}");

            // this will throw an exception...
            try
            {
                var decValue = "500.500";
                Console.WriteLine($"Casting Decimal Value '{decValue}' with {nameof(cc.Settings.NumberFormat.NumberDecimalSeparator)} "
                                + $"({cc.Settings.NumberFormat.NumberDecimalSeparator}): {decString.ToString(numberFormatInfoInstance)}");

                decString = decValue.CastTo<object, decimal>();

            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            // ...unless TryCast is used
            decimal result;
            var success = "500.500".TryCast<object, decimal>(out result);

            Console.WriteLine($"{nameof(ObjectExtension.TryCast)} success: {success}, result: {result.ToString(numberFormatInfoInstance)}");

            // change the separator and number of digits
            numberFormatInfoInstance.NumberDecimalSeparator = ".";
            numberFormatInfoInstance.NumberDecimalDigits = 2;
            Console.WriteLine($"Changed {nameof(numberFormatInfoInstance.NumberDecimalSeparator)} to: ({numberFormatInfoInstance.NumberDecimalSeparator })");

            var decValue2 = "500.500";
            var decString2 = decValue2.CastTo<object, decimal>();

            Console.WriteLine($"Decimal Value '{decValue2}' with {nameof(numberFormatInfoInstance.NumberDecimalSeparator)} ({cc.Settings.NumberFormat.NumberDecimalSeparator}): "
                            + $"{decString2.ToString(numberFormatInfoInstance)}");

            // End
            Console.WriteLine("List all Converters? (y/n)");

            var key = Console.ReadKey(true);

            if(key.KeyChar == 'y' || key.KeyChar == 'Y')
            {
                // enumerate all converters
                foreach(var item in ConverterCollection.CurrentInstance)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine($"Total Number of CONVERTERS: {ConverterCollection.CurrentInstance.Count}");

                Console.ReadKey(true);
            }

        }
    }
}