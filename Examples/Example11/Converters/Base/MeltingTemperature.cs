// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace BioCore.Converters
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Core.TypeCast;
    using Core.TypeCast.Base;

    using Core.Extensions;

    /// <summary>
    /// The melting temperature of a <see cref="Polymer"/>
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    public struct MeltingTemperature : IUnitValue
    {
        private readonly double value;

        public MeltingTemperature(double value)
        {
            this.value = value;
        }

        public double Value { get { return value; } }

        public PhysicalUnit Unit { get { return PhysicalUnit.Cs; } }

        public static implicit operator MeltingTemperature(double val)
        {
            return new MeltingTemperature(val);
        }

        public static implicit operator double(MeltingTemperature val)
        {
            return val.Value;
        }

        public override string ToString()
        {
            return $"{this.Value} [{this.Unit.GetDescription()}]";
        }

    }

}