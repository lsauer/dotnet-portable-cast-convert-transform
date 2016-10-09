// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example8
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

    /// <summary>
    /// Demonstrate using <see cref="ConverterCollection.AddBuilder{TBase}"/> to add a set of related converters to the `ConverterCollection` 
    /// with shared properties or attributes. Secondly demonstrate using `LINQ` queries and `Get`. 
    /// </summary>
    [Description("Demonstrate using ConverterCollection.AddBuilder{TBase} to add a set of related converters to the `ConverterCollection`. " +
                 "Secondly demonstrate using `LINQ` queries and `Get`.")]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            ConverterCollection.CurrentInstance
               .AddStart<Program>(new ConverterCollectionSettings { UseFunctionDefaultWrapper = false, },
                                cancellationToken: cancellationToken.Token)
               .Add<string, bool>(s => s.ToLowerInvariant() == "true" ? true : false)
               .Add<string, byte[]>(s => System.Text.ASCIIEncoding.ASCII.GetBytes(s))
               //.Add<>(...)
               .End();


            // Demonstrate using LINQ queries
            var cc = ConverterCollection.CurrentInstance;
            Console.WriteLine($"Converters converting to {typeof(int).Name}");
            foreach(var cint in cc.WithTo(typeof(int).GetTypeInfo()).ToList())
            {
                Console.WriteLine($"Converter->Int: {cint}");
            }

            Console.WriteLine($"Converters converting to {typeof(int).Name} / using {nameof(ConverterExtension.WithSameFromType)}(...)");
            var someIntConv = cc.Get<object, int>();
            foreach(var cint in someIntConv.WithSameToType())
            {
                Console.WriteLine($"\t\tConverter->Int: {cint}");
            }

            // Demonstrate invocation without a converter function, yielding a default(T) value
            Size size = new Point(100, 300).ConvertTo<Point, Size>(new Rectangle(1, 1, 2, 2));
            Console.WriteLine($"{nameof(Point)} converted to {nameof(Size)} with lacking converter yielding 'default(Point)': {size}");

            // simple converter function involving three different data types
            Func<Point, Rectangle, Size> PointToSizeFromRectangle = (ap, br) =>
                 {
                     var rect2 = (Rectangle)br;
                     if(ap.X * ap.Y > rect2.X * rect2.Y)
                     {
                         return new Size(ap.X, ap.Y);
                     }
                     return new Size(rect2.X, rect2.Y);
                 };
            cc.Add(PointToSizeFromRectangle);

            // Demonstrate looking for the previously declared  function `Get`
            var pointConv = cc.Get<Point, Rectangle, Size>();
            // Compare and make sure the function reference are the same
            var condition = Object.ReferenceEquals(PointToSizeFromRectangle, pointConv.FunctionDefault);

            // Demonstrate invoking the previously declared converter
            Point somePoint = new Point(1, 2);
            Size someSize = somePoint.ConvertTo<Point, Size>(new Rectangle(1, 1, 2, 2));
            Console.WriteLine($"{nameof(somePoint)} converted to {nameof(Size)}: {someSize}");

            Size outsize;
            bool successSize = new Point(2, 4).TryConvert<Size>(out outsize, new Rectangle(1, 2, 3, 4));
            Console.WriteLine($"{nameof(Point)} converted to {nameof(Size)}: Success: {successSize}, Value: {outsize}");

            Console.WriteLine(true.CastTo<string>());
            Console.WriteLine(true.ConvertTo<string>(1337, withContext: true));

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