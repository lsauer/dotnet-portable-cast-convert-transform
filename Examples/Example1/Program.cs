// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example1
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Reflection;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;

    using Core.TypeCast;
    using Core.Extensions;
    using Converters;
    using System.Globalization;

    /// <summary>
    /// Demonstrate implementing custom converters and using the `ConverterAttribute` class in a simple scenario
    /// </summary>
    [Description("Demonstrate implementing custom converters and using the `ConverterAttribute` class in a simple scenario")]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            var s = int.MinValue.ToString(CultureInfo.InvariantCulture);
            var cond = s.CastTo<int>() == int.MinValue;
            var cond2 = ((object)"42").CastTo<object, uint>() ==  42;
            // Do not access ConverterCollection.CurrentInstance before invoking convert, unless you explicitly want to invoke Initialize

            if("500".CanConvertTo(1337M))
            {
                decimal loadinstant = "500".CastTo(1337M);
            }

            if("500".CanConvertTo<decimal>())
            {
                decimal loadinstant = "500".CastTo<decimal>();
            }

            var cc = ConverterCollection.CurrentInstance;

            var canConvertFromDecimal = cc.CanConvertFrom<decimal>();

            Console.WriteLine($" {nameof(ConverterCollection)} {nameof(canConvertFromDecimal)}: {canConvertFromDecimal}");

            var canConvertToDecimal = ConverterCollection.CurrentInstance.CanConvertTo<decimal>();

            Console.WriteLine($" {nameof(ConverterCollection)} {nameof(canConvertToDecimal)}: {canConvertToDecimal}");

            // possible
            if("500".CanConvertTo(1.337M))
            {
                var result = "500".CastTo(1337);

                Console.WriteLine($" {nameof(ObjectExtension.CanConvertTo)} {nameof(Decimal)}: {result} (Typeof: {result.GetType().Name})");
            }

            // possible
            if("500".CanConvertTo<decimal>())
            {
                var result = "500".ConvertTo<decimal>(0.0M);

                Console.WriteLine($" {nameof(ObjectExtension.CanConvertTo)}: {result}, Typeof: {result.GetType().Name}");
            }

            // not possible! No converter defined: CanConvertTo(desired result, model value)
            if("500".CanConvertTo(0M, Point.Empty))
            {
                var result = "500".ConvertTo<decimal>(Point.Empty);

                Console.WriteLine($" {nameof(ObjectExtension.CanConvertTo)}{nameof(Decimal)}: {result} (Typeof: {result.GetType().Name})");
            }

            // not possible! No converter defined: CanConvertTo(desired result, model value)
            if("500".CanConvertTo<decimal, Point>())
            {
                var result = "500".ConvertTo<decimal>(Point.Empty);

                Console.WriteLine($" {nameof(ObjectExtension.CanConvertTo)}: {result}, Typeof: {result.GetType().Name}");
            }

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