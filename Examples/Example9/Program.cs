// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example9
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
    using System.Threading;

    using Core.TypeCast.Base;
    using Core.TypeCast.Converters;

    using Converters;

    /// <summary>
    /// Demonstrate using <see cref="ConvertContext"/> as an opt-in feature of this library
    /// </summary>
    [Description("Demonstrate using `ConvertContext` as an opt-in feature of this library")]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            var cc = ConverterCollection.CurrentInstance;

            // A simple converter function with an opt-in IConvertContext. The type of the argument has to be checked within the function 
            Func<Point, object, Size> PointToSizeFromRectangle = (ap, br) =>
            {
                Rectangle rect2;
                if(br is IConvertContext)
                {
                    Console.WriteLine($"{nameof(PointToSizeFromRectangle)} has a {nameof(IConvertContext)}: {br}");
                    rect2 = (Rectangle)(br as IConvertContext).Value;
                }
                else
                {
                    rect2 = (Rectangle)br;
                }

                Console.WriteLine($"{(nameof(Rectangle))} input is : {rect2}");
                if(ap.X * ap.Y > rect2.X * rect2.Y)
                {
                    return new Size(ap.X, ap.Y);
                }
                return new Size(rect2.X, rect2.Y);
            };
            cc.Add(PointToSizeFromRectangle);

            // invoke conversion with a ConvertContext using a named argument
            var result = new Point(3, 2).ConvertTo<Size>(new Rectangle(1, 2, 3, 4), withContext: true);

            Console.WriteLine(true.CastTo<string>());

            Console.WriteLine(false.CastTo<string>());

            Console.WriteLine(true.ConvertTo<string>(1337, withContext: true));

            Console.WriteLine(false.ConvertTo<string>(1337, true));

            // invoke explicit function with ConvertContext
            Console.WriteLine(false.ConvertTo<decimal>(new ConvertContext(1337)));

            // End
            Console.WriteLine("List all Custom Converters? (y/n)");

            var key = Console.ReadKey(true);

            if(key.KeyChar == 'y' || key.KeyChar == 'Y')
            {
                // enumerate all custom converters in "Program"
                foreach(var item in ConverterCollection.CurrentInstance.WithBaseType(typeof(Program)).
                             Concat(ConverterCollection.CurrentInstance.WithBaseType(typeof(Converters.BoolToString))))
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine($"Total Number of CONVERTERS: {ConverterCollection.CurrentInstance.Count}");

                Console.ReadKey(true);
            }
        }
    }
}