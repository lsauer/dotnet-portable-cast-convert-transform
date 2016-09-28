// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example2
{
    using System;
    using System.Reflection;

    using System.Globalization;
    using System.IO;
    using System.Text;

    using Core.TypeCast;
    using Converters;
    using System.Drawing;
    using System.Drawing.Imaging;

    using Core.Extensions;

    /// <summary>
    /// Demonstrate the `ConverterCollection` class and advanced attribute usage
    /// </summary>
    [Converter(loadOnDemand: false)]
    [Description("Demonstrate the `ConverterCollection` class and advanced attribute usage")]
    class Program
    {
        [ConverterMethod(isStatic: true, loadOnDemand: true, name: nameof(Program))]
        public static byte[] ASCIIToBytes(string input, object model = null)
        {
            return Encoding.ASCII.GetBytes(input);
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            var cc = new ConverterCollection(
                application: typeof(Program),
                converterClass: typeof(CustomConverter),
                numberFormatDefault: new NumberFormatInfo()
                {
                    NumberGroupSeparator = ",",
                    NumberDecimalDigits = 3
                }
            );

            var image = "Some text to be saved...".CastTo<Image>();

            Console.WriteLine($"Image properties: {image.CastTo<string>()}");

            image.Save("convertedText.png", ImageFormat.Png);


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