// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example6.Converters
{
    using System;
    using System.Drawing;
    using Core.TypeCast;
    using Core.TypeCast.Base;

    /// <summary>
    /// This class implements converting from string type to byte type as loose coupled, custom converter demonstration example
    /// </summary>
    public struct Perimeter
    {
        public double Length;

        public Perimeter(Rectangle rect)
        {
            this.Length = rect.Width * 2 + rect.Height * 2;
        }

        public Perimeter(Point[] points)
        {
            this.Length = 0;
            if(points.Length <= 1)
            {
                return;
            }
            for(var i = 1; i < points.Length; i++)
            {
                this.Length += Math.Sqrt(Math.Pow(points[i].X - points[i - 1].X, 2) + Math.Pow(points[i].Y - points[i - 1].Y, 2));
            }
            // close with last to first point
            this.Length += Math.Sqrt(Math.Pow(points[points.Length - 1].X - points[0].X, 2) + Math.Pow(points[points.Length - 1].Y - points[0].Y, 2));
        }
    }
}