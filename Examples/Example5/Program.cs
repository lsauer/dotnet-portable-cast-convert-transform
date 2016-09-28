// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example5
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Core.TypeCast;
    using Core.Extensions;
    using System.Reflection;

    using System.Runtime.CompilerServices;
    using Model;

    /// <summary>
    /// Demonstrate the use of listening to custom `Converter` model events
    /// </summary>
    [Description("Demonstrate the use of listening to custom `Converter` model events")]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            // Add a converter which takes a string and a Data-Model Object as second argument. 
            // The model encapsulates in a strict, declarative manner any further arguments. 
            // Note that the second parameter may be an interface or inherited sub-type
            Func<string, IModel, MemoryStream> stringToPngImage = (text, dto) =>
            {
                var model = dto as TextModel;
                if(model == null)
                {
                    return null;
                }
                model.OnStatusChanged(EventModelBase.Status.Started);
                Image image = new Bitmap(model.Size.Width, model.Size.Height);
                Graphics graphics = Graphics.FromImage(image);
                model.OnStatusChanged(EventModelBase.Status.Busy);
                graphics.DrawString(text, model.Font, model.Brush, model.Point);
                graphics.Dispose();
                var imageStream = new MemoryStream();
                image.Save(imageStream, ImageFormat.Png);
                image.Dispose();
                model.OnStatusChanged(EventModelBase.Status.Completed);
                return imageStream;
            };

            // Add the strictly typed `Converter<string, MemoryStream, IModel>` function to the global collection pool 
            ConverterCollection.CurrentInstance.Add(stringToPngImage);

            // create a new model instance to be passed to the converter as model-parameter
            var textModel = new TextModel()
            {
                Size = new Size(400, 50),
                Font = new Font("Arial", 12),
                Brush = Brushes.Black,
                Point = new Point(40, 20),
            };

            // Subscribe to model events, which inform about conversion status
            textModel.StatusChanged += (object sender, EventModelBase.Status e) =>
            {
                Console.WriteLine($"{sender.GetType().Name} ....{e.ToString()}");
            };

            var streamPng = $"Some variable Text courtesy of {nameof(stringToPngImage)}".ConvertTo<MemoryStream>(textModel);
            // write the result as a file to the storage medium
            System.IO.File.WriteAllBytes(@"testpng.png", streamPng.ToArray());

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