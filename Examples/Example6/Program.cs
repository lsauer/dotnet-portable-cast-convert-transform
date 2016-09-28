// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example6
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
    using System.Collections;
    using System.Collections.Generic;

    using Core.TypeCast;
    using Core.Extensions;
    using Converters;

    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    /// <summary>
    ///  Demonstrate using serialization for bootstrapping client-specific runtime limited converters in scenarios such as protecting 
    //// intellectual property, ensuring concurrency and / or for subscriber services
    /// </summary>
    [Converter]
    [Description("Demonstrate using serialization for bootstrapping client-specific runtime limited converters in scenarios such as protecting" +
                 "intellectual property, ensuring concurrency and / or for subscriber services")]
    class Program
    {
        public delegate string Serialize(object obj);

        [DataContract]
        public struct SomeDTO
        {
            [DataMember]
            public string Name;
            [DataMember]
            public int Value;
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            // Add a serialization transformer
            ConverterCollection.CurrentInstance
                .Add<object, string, Serialize>((obj) =>
                {
                    var serializer = new DataContractJsonSerializer(obj.GetType());
                    string json;
                    using(var memStream = new MemoryStream())
                    {
                        serializer.WriteObject(memStream, obj);
                        json = Encoding.UTF8.GetString(memStream.ToArray());
                    }
                    return json;
                });

            // test serialization to json. Since we convert from object, we have to specify the input and output types
            var dtoSerialized = new SomeDTO { Name = "test", Value = 100, }.Transform<Serialize, string>();

            Console.WriteLine($"JSON serialized output of {nameof(dtoSerialized)}: {dtoSerialized}");

            // use Binaryformatter for serialization. Heed the Caveats pointed out in the Readme!

            var cc = ConverterCollection.CurrentInstance;

            // demonstrate a converter returning a copy-by-value instance:
            cc.Add((Rectangle re) => new Perimeter(re));

            cc.Add((Point[] pts) => new Perimeter(pts));

            var rect = new Rectangle(1, 2, 3, 4);
            var len = rect.CastTo<Perimeter>().Length;
            Console.WriteLine($"{nameof(Perimeter)} of {nameof(Rectangle)}: {len}");

            var plen = new[] {
                new Point(0, 0),
                new Point(-3, 4),
                new Point(2, 2),
                new Point(10, -3),
                new Point(-10, 2)
            }.CastTo<Perimeter>().Length;

            Console.WriteLine($"Perimeter of Points: {plen}");


            // A simple converter with a secondary argument taking a default value to be passed out if the conversion fails
            Func<string, int, int> converter = (s, defaultValue) =>
                {
                    int value;
                    var success = int.TryParse(s, out value);
                    return success == true ? value : defaultValue;
                };

            var binaryFormatter = new BinaryFormatter();

            // serialize the converter to a binary
            var streamOut = new FileStream(@"converters.dat", FileMode.Create);
            binaryFormatter.Serialize(streamOut, converter);
            streamOut.Dispose();

            // open the binary and deserialize
            var streamIn = new FileStream(@"converters.dat", FileMode.Open);
            var loadedFunction = binaryFormatter.Deserialize(streamIn);

            // finally add the function to the ConverterCollection
            cc.Add(loadedFunction);

            // Invoke the converter
            int resultTry;
            var successTry = "500ss".TryCast(out resultTry);

            var converterLookup = cc.WithFrom(typeof(string)).WithTo(typeof(int)).WithArgument(typeof(int)).FirstOrDefault();
            Console.WriteLine($"Conversion using: {converterLookup}, Success: {successTry}, Result: {resultTry}");

            // End
            Console.WriteLine("List all Custom Converters? (y/n)");

            var key = Console.ReadKey(true);

            if(key.KeyChar == 'y' || key.KeyChar == 'Y')
            {
                // enumerate all converters in "Program"
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