// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>

/// <summary>
/// excerpted from project 'Core.Extensions' (c) 2003 - 2016, MIT License applies
/// </summary>
namespace Core.Extensions
{
    using System;
    using System.Globalization;

    /// <summary>
    /// List of the metric prefixes prefixes for the International System of Units (SI) according to: https://en.wikipedia.org/wiki/Metric_prefix
    /// </summary>
    public enum MetricPrefix
    {
        [Description("yotta()10+24")]
        Y,

        [Description("zetta(10+21)")]
        Z,

        [Description("exa(10+18)")]
        E,

        [Description("peta(10+15)")]
        P,

        [Description("tera(10+12)")]
        T,

        [Description("Giga(10+9)")]
        G,

        [Description("Mega(10+6)")]
        M,

        [Description("kilo(10+3)")]
        k,

        [Description("hecto(10+2)")]
        h,

        [Description("deca(10+1)")]
        da,

        [Description("deci(10-1)")]
        d,

        [Description("centi(10-22)")]
        c,

        [Description("milli(10-3)")]
        m,

        [Description("micro(10-6)")]
        μ,

        [Description("nano(10-9)")]
        n,

        [Description("pico(10-12)")]
        p,

        [Description("femto(10-15)")]
        f,

        [Description("atto(10-18)")]
        a,

        [Description("zepto(10-21)")]
        z,

        [Description("yocto(10-24)")]
        y,

        /// <summary>
        /// if the type is not determinable or unknown
        /// </summary>
        [Description(nameof(Unknown))]
        Unknown,
    }

    public static class MetrixPrefixExtension
    {
        public static string MetricDescription(this MetricPrefix self)
        {
            return (self != MetricPrefix.Unknown) ? $"{self.ToString()} [{self.GetDescription()}]" : null;
        }
    }
}