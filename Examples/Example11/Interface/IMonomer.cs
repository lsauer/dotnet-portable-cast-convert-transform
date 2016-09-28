using System;

namespace BioCore.Converters
{
    public interface IMonomer
    {
        ICompound Compound { get; set; }
        string FullName { get; }
        Guid Id { get; }
        Letter Letter { get; set; }
        string Name { get; }
        char Value { get; }
        MolecularWeight Weight { get; }
    }
}