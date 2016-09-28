// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example4
{
    using System;
    using System.Reflection;

    using System.Globalization;
    using System.IO;
    using System.Text;

    using Core.TypeCast;

    using Core.Extensions;

    /// <summary>
    /// Demonstrate the use of listening to global `ConverterCollection` events
    /// </summary>
    [Description("Demonstrate the use of listening to global `ConverterCollection` events")]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            // Listen to global collection events
            Console.WriteLine($"Listen to global collection events via handler: {nameof(ConverterCollection_PropertyChanged)}");

            ConverterCollection.PropertyChanged += ConverterCollection_PropertyChanged;

            // add simple converter for demonstration
            ConverterCollection.CurrentInstance.Add(
                (int[] a, string b) => new MemoryStream(Encoding.ASCII.GetBytes(a.ToString() + b.ToString()))
            );

            var memoryStream = new[] { 1, 3, 4, 5 }.ConvertTo<MemoryStream>("");

            Console.WriteLine($"{nameof(MemoryStream)} Buffer: {Encoding.ASCII.GetString(memoryStream.ToArray())}");

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

        /// <summary>
        /// Event Listener for <see cref="Singleton{ConverterCollection}"/> events
        /// </summary>
        /// <param name="sender">The event-sender instance</param>
        /// <param name="e">Event arguments containing the function name as <paramref name="e.Name"/> as well as the pertaining value as <paramref name="e.Value"/></param>
        private static void ConverterCollection_PropertyChanged(Core.Singleton.ISingleton sender, Core.Singleton.SingletonPropertyEventArgs e)
        {
            Console.WriteLine($"sender: {sender.ToString()}, event: {e.Name}, value: {e.Value.ToString()}");
        }
    }
}