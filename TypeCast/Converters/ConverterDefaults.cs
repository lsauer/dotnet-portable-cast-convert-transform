// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.0.1.4                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast.Converters
{
    using System;
    using System.Globalization;
    using Base;

    /// <summary>
    /// Converts between <see cref="object"/> and the most common System types in the <see cref="System"/> namespace. 
    /// </summary>
    /// <remarks>
    /// The reverse, converting from common <see cref="System"/> Types to <see cref="object"/> is not sensible as a simple boxing operation suffices.
    /// </remarks>
    [Converter(loadOnDemand: false, nameSpace: nameof(System), dependencyInjection: false)]
    public class ConverterDefaults : ConverterCollectionDependency
    {
        /// <summary>
        /// The <see cref="Type"/> of the own custom converter class
        /// </summary>
        /// <remarks>for caching purposes</remarks>
        private readonly Type self;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterDefaults"/> class, and adds converters for the most common types of the <see cref="System"/> namespace. 
        /// </summary>
        public ConverterDefaults(IConverterCollection collection) : base(collection)
        {
            this.self = this.GetType();

            // use a custom number format, set in the ConverterCollection instance
            this.NumberFormat = ConverterCollectionSettings.DefaultNumberFormat.Clone() as NumberFormatInfo;

            // convert from `string` to `System.T`
            this.AddObjectConverter(collection: collection);
        }

        /// <summary>
        /// Gets or sets the Converters <see cref="NumberFormatInfo"/>, which is copied from <see cref="ConverterCollectionSettings.NumberFormat"/> upon 
        /// creating an new instance of <see cref="ConverterDefaults"/> 
        /// </summary>
        public NumberFormatInfo NumberFormat { get; set; }

        /// <summary>
        /// The converters to convert from a boxed <see cref="object"/> value to Types of the Common C# language <see cref="System"/>
        /// </summary>
        /// <param name="collection">The <see cref="IConverterCollection"/> collection instance to which the converters are added</param>
        /// <seealso cref="Core.TypeCast.Test"/>
        private void AddObjectConverter(IConverterCollection collection)
        {
            collection.Add<object, int>(o => int.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, uint>(o => uint.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, char>(o => (char)uint.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, bool>(o => o != null && (int)o != 0 && o.ToString() != "false", this.self)
                      .Add<object, string>(o => o != null ? o.ToString() : string.Empty, this.self)
                      .Add<object, byte>(o => byte.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, sbyte>(o => sbyte.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, decimal>(o => decimal.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, double>(o => double.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, float>(o => float.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, long>(o => long.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, ulong>(o => ulong.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, short>(o => short.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, ushort>(o => ushort.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self)
                      .Add<object, Enum>(o => (Enum)Enum.Parse(o?.GetType(), o != null ? o.ToString() : string.Empty), this.self)
                      .Add<object, DateTime>(o => DateTime.Parse(o != null ? o.ToString() : string.Empty, this.NumberFormat), this.self);
        }
    }
}