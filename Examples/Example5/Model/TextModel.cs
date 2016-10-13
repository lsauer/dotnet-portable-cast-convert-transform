// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example5.Model
{
    using System;
    using System.Drawing;
    using Core.Singleton;
    using System.ComponentModel;

    public class TextModel : EventModelBase, IModel
    {
        public Font Font { get; set; }
        public Brush Brush { get; set; }
        public Point Point { get; set; }
        public Size Size { get; set; }
        public Color TextColor { get; set; }
        public Color CanvasColor { get; set; }

        public bool LeftToRight { get; set; } = true;
    }
}