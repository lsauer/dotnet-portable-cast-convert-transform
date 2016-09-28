using System;
using System.IO;
using System.Collections.Generic; //for List
using System.Linq; //for Array.Sum, Array.Select, Contains...
using Core.Extensions;
using System.Runtime.CompilerServices;

namespace Core.Extensions
{
    public static partial class LongExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Tuple<string, MetricPrefix> SizeLiteral<T>(this T size, int unitBase = 10, int exponentStep = 3, IFormatProvider numberFormat = null)
            where T : struct, IConvertible, IComparable<T>
        {
            var num = Convert.ToInt64(size);
            return num.SizeLiteral(unitBase, exponentStep, numberFormat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Tuple<string, MetricPrefix> FormatSize(long number, int exponent = 0, int unitBase = 10, IFormatProvider numberFormat = null,
            MetricPrefix prefix = MetricPrefix.Unknown)
        {
            return new Tuple<string, MetricPrefix>((number / Math.Pow(unitBase, exponent)).ToString(numberFormat), prefix);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="unitBase"></param>
        /// <param name="exponentStep"></param>
        /// <param name="numberFormat"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Tuple<string, MetricPrefix> SizeLiteral(this long size, int unitBase = 10, int exponentStep = 3, IFormatProvider numberFormat = null)
        {
            if(size < Math.Pow(unitBase, exponentStep * 1))
            {
                return FormatSize(size, exponentStep * 0, unitBase, numberFormat);
            }
            else if(size < Math.Pow(unitBase, exponentStep * 2))
            {
                return FormatSize(size, exponentStep * 1, unitBase, numberFormat, MetricPrefix.k);
            }
            else if(size < Math.Pow(unitBase, exponentStep * 3))
            {
                return FormatSize(size, exponentStep * 2, unitBase, numberFormat, MetricPrefix.M);
            }
            else if(size < Math.Pow(unitBase, exponentStep * 4))
            {
                return FormatSize(size, exponentStep * 3, unitBase, numberFormat, MetricPrefix.G);
            }
            else if(size < Math.Pow(unitBase, exponentStep * 5))
            {
                return FormatSize(size, exponentStep * 4, unitBase, numberFormat, MetricPrefix.T);
            }
            else if(size < Math.Pow(unitBase, exponentStep * 6))
            {
                return FormatSize(size, exponentStep * 5, unitBase, numberFormat, MetricPrefix.P);
            }
            else if(size < Math.Pow(unitBase, exponentStep * 7))
            {
                return FormatSize(size, exponentStep * 6, unitBase, numberFormat, MetricPrefix.E);
            }
            else if(size < Math.Pow(unitBase, exponentStep * 8))
            {
                return FormatSize(size, exponentStep * 7, unitBase, numberFormat, MetricPrefix.Z);
            }
            else if(size < Math.Pow(unitBase, exponentStep * 9))
            {
                return FormatSize(size, exponentStep * 8, unitBase, numberFormat, MetricPrefix.Y);
            }
            else
            {
                return FormatSize(size, exponentStep * 0, unitBase, numberFormat);
            }
        }
    }

}