// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example2.Converters
{
    using System;
    using System.Drawing;

    using Core.TypeCast;
    using Core.TypeCast.Base;

    /// <summary>
    /// Custom converter class to convert from string type to an image
    /// </summary>
    [Converter(loadOnDemand: false, nameSpace: nameof(System), dependencyInjection: true)]
    public class StringToImage : ConverterCollectionDependency<string, Image>
    {
        protected readonly int fontSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringToImage"/> class.
        /// </summary>
        /// <param name="collection">
        /// The converter collection to be injected into the constructor.
        /// </param>
        /// <exception cref="ConverterException">
        /// </exception>
        public StringToImage(IConverterCollection collection)
            : base(collection)
        {
            this.fontSize = 14;
        }

        #region Overrides of ConverterCollectionDependency<string,Image>

        /// <summary> The converter function that needs to be overwritten as part of the <see cref="IConverter" /> interface support. </summary>
        /// <param name="valueTyped">The value of <see cref="System.Type" /> <see cref="TIn" /> to be converted.</param>
        /// <param name="defaultValueTyped">The optional default value of <see cref="System.Type" /> <see cref="TOut" />to be passed if the conversion fails or is `null`.</param>
        /// <returns>The value converted to <see cref="System.Type" /> of <see cref="TOut" /> </returns>
        /// <exception cref="ConverterException">Throws an exception of <see cref="ConverterCause.ConverterNotImplemented" /> if the parent class does not implement
        ///     <code>`public override TOut `</code> <see cref="ConverterCollectionDependency{TIn,TOut}.Convert" />
        /// </exception>
        public override Image Convert(string valueTyped, Image defaultValueTyped = null)
        {
            var font = new Font(FontFamily.GenericSerif, this.fontSize);
            Image img = new Bitmap(400, 100);
            Graphics drawing = Graphics.FromImage(defaultValueTyped ?? img);
            SizeF textSize = drawing.MeasureString(valueTyped, font, 400);

            StringFormat sf = new StringFormat();
            sf.Trimming = StringTrimming.Word;

            drawing.Clear(Color.Transparent);
            Brush textBrush = new SolidBrush(Color.Black);

            drawing.DrawString(valueTyped, font, textBrush, new RectangleF(0, 0, textSize.Width, textSize.Height), sf);

            textBrush.Dispose();
            drawing.Dispose();
            return img;
        }

        /// <summary>
        /// Gets the basic parameters like width and height of an image instance
        /// </summary>
        /// <param name="img">An image instance</param>
        /// <returns>A string containing the basic image properties</returns>
        [ConverterMethod(isStatic: false)]
        public string ImageProperties(Bitmap img)
        {
            if(img == null)
            {
                return string.Empty;
            }
            return $"{nameof(img.HorizontalResolution)}: {img.HorizontalResolution},"
                 + $"{nameof(img.VerticalResolution)}: {img.VerticalResolution},"
                 + $"{nameof(img.Width)}: {img.Width},"
                 + $"{nameof(img.Height)}: {img.Height},"
                 + $"{nameof(this.fontSize)}: {this.fontSize}";
        }

        #endregion
    }
}