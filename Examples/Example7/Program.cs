// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example7
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Reflection;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization.Json;
    using System.Text;

    using Core.TypeCast;
    using Core.Extensions;

    using System.Runtime.CompilerServices;

    /// <summary>
    ///  Demonstrate the use of converter generics and adding inter-converter pairs
    /// </summary>
    [Description("Demonstrate the use of converter generics and adding inter-converter pairs")]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            // Demonstrate adding an inter-converter pair, wherein the order does not matter
            ConverterCollection.CurrentInstance.Add(
                (string s) => new Point(int.Parse(s?.Split(',').First()), int.Parse(s?.Split(',').Last())),
                p => $"{p.X},{p.Y}"
            );

            var point = "100,200".CastTo<Point>();

            Console.WriteLine($"{nameof(Point)}:  {point}, X: {point.X}, Y: {point.Y}");

            var pointString = point.CastTo<string>();

            Console.WriteLine($"{nameof(pointString)}:  {pointString}");

            // Demonstrate using a generic converter
            ConverterCollection.CurrentInstance.Add((Point a, SomeGenericClass<Point, ConverterAttribute> b) =>
            {
                return new SomeGenericClass<Point, ConverterAttribute>();
            });

            var genericResult = point.CastTo<SomeGenericClass<Point, ConverterAttribute>>();

            Console.WriteLine($"{nameof(genericResult)}: Value: '{genericResult.Value}', T: '{genericResult.TGeneric.Name}', T2: '{genericResult.TGeneric2.Name}'");

            // End
            Console.WriteLine("List all Custom Converters? (y/n)");

            var key = Console.ReadKey(true);

            if(key.KeyChar == 'y' || key.KeyChar == 'Y')
            {
                // enumerate all custom converters in "Program"
                foreach(var item in ConverterCollection.CurrentInstance.WithBaseType(typeof(Program).GetTypeInfo()))
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine($"Total Number of CONVERTERS: {ConverterCollection.CurrentInstance.Count}");

                Console.ReadKey(true);
            }
        }

    }
}