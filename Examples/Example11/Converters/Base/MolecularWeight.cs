// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace BioCore.Converters
{
    using System;
    using System.Linq;
    using System.Globalization;
    using System.Collections.Generic;
    using System.Drawing;

    using Core.TypeCast;
    using Core.TypeCast.Base;

    using Core.Extensions;

    /// <summary>
    /// The molecular weight of a substance, as the sum of the (average) atomic weights of every atom in a molecule
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    public struct MolecularWeight : IUnitValue
    {
        private readonly double value;

        public MolecularWeight(double value)
        {
            this.value = value;
        }

        public double Value { get { return value; } }

        public PhysicalUnit Unit { get { return PhysicalUnit.Da; } }

        public static implicit operator MolecularWeight(double val)
        {
            return new MolecularWeight(val);
        }

        public static implicit operator double(MolecularWeight val)
        {
            return val.Value;
        }

        public override string ToString()
        {
            var tuple = this.Value.SizeLiteral(numberFormat: new NumberFormatInfo { NumberDecimalDigits = 2});
            return $"{tuple.Item1} [{(tuple.Item2 != MetricPrefix.Unknown ? tuple.Item2.ToString() : "")}{this.Unit.GetDescription()}]";
        }
    }

}