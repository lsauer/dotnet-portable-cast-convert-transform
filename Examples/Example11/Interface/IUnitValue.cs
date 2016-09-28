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
    /// <summary>
    /// The physical unit interface for scalar values
    /// </summary>
    public interface IUnitValue : IUnit, IValue
    {
    }

    /// <summary>
    /// The physical scalar value interface
    /// </summary>
    public interface IValue
    {
        double Value { get; }
    }

    public interface IValue<TPhysical>
    {
        TPhysical Value { get; }
    }

    /// <summary>
    /// The interface for scalar values
    /// </summary>
    public interface IUnit
    {
        PhysicalUnit Unit { get; }
    }

}