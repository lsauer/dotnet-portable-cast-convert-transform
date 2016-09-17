﻿// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.0.1.4                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Core.Extensions;
    using Core.Singleton;
    using Core.TypeCast.Base;

    /// <summary>
    /// A set of <see cref="IQueryable{Converter}"/> extension methods for filtering <see cref="ConverterCollection"/> items using <see cref="System.Linq"/>
    /// </summary>
    public static class ConverterCollectionFilters
    {

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.From"/> set equal to <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeFrom">The <see cref="Converter.From"/> Type to look for</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithFrom(this IQueryable<Converter> query, TypeInfo typeFrom)
        {
            if(typeFrom != null)
            {
                query = query.Where(c => c.From == typeFrom);
            }
            return query;
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.From"/> set equal to <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeFrom">The <see cref="Converter.From"/> Type to look for</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithFrom(this IQueryable<Converter> query, Type typeFrom)
        {
            return WithFrom(query, typeFrom?.GetTypeInfo());
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.To"/> set equal to <paramref name="typeTo"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeTo">The <see cref="Converter.To"/> Type to look for</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithTo(this IQueryable<Converter> query, TypeInfo typeTo)
        {
            if(typeTo != null)
            {
                query = query.Where(c => c.To == typeTo);
            }
            return query;
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.To"/> set equal to <paramref name="typeTo"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeTo">The <see cref="Converter.To"/> Type to look for</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithTo(this IQueryable<Converter> query, Type typeTo)
        {
            return WithTo(query, typeTo?.GetTypeInfo());
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.BaseType"/> set equal to <paramref name="typeBase"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeBase">The <see cref="Converter.BaseType"/> Type to look for</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithBaseType(this IQueryable<Converter> query, TypeInfo typeBase)
        {
            if(typeBase != null)
            {
                query = query.Where(c => c.BaseType == typeBase);
            }
            return query;
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.BaseType"/> set equal to <paramref name="typeBase"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeBase">The <see cref="Converter.BaseType"/> Type to look for</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithBaseType(this IQueryable<Converter> query, Type typeBase)
        {
            return WithBaseType(query, typeBase?.GetTypeInfo());
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.Argument"/> set equal to <paramref name="typeArgument"/>, or
        /// if that query yields no results, checks via <see cref="TypeInfo.IsAssignableFrom(TypeInfo)"/> for supported interfaces or base-classes.
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeArgument">The <see cref="Converter.Argument"/> Type to look for</param>
        /// <param name="assignableFrom">Whether to check via <see cref="TypeInfo.IsAssignableFrom(TypeInfo)"/> for supported interfaces or base-classes.r</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithArgument(this IQueryable<Converter> query, TypeInfo typeArgument, bool assignableFrom = true)
        {
            if(typeArgument != null && typeArgument.AsType() != typeof(object))
            {
                var tmpquery = query.Where(c => c.Argument == typeArgument);
                if(tmpquery.Any() || assignableFrom == false)
                {
                    query = tmpquery;
                }
                else
                {
                    // check for interfaces, sub-types ...
                    query = query.Where(c => c.Argument.IsAssignableFrom(typeArgument));
                }
            }
            return query;
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.Argument"/> set equal to <paramref name="typeArgument"/>, or
        /// if that query yields no results, checks via <see cref="TypeInfo.IsAssignableFrom(TypeInfo)"/> for supported interfaces or base-classes.
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeArgument">The <see cref="Converter.Argument"/> Type to look for</param>
        /// <param name="assignableFrom">Whether to check via <see cref="TypeInfo.IsAssignableFrom(TypeInfo)"/> for supported interfaces or base-classes.r</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithArgument(this IQueryable<Converter> query, Type typeArgument, bool assignableFrom = true)
        {
            return WithArgument(query, typeArgument?.GetTypeInfo(), assignableFrom);
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.Standard"/> set equal to <paramref name="isStandard"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="isStandard">The Boolean <see cref="Converter.Standard"/> property to look for</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithStandard(this IQueryable<Converter> query, bool? isStandard = true)
        {
            if(isStandard != null)
            {
                query = query.Where(c => c.Standard == isStandard);
            }
            return query;
        }


        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.From.IsGenericType"/> set equal to <paramref name="typeFromIsGenericType"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods 
        /// in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeFromIsGenericType">The Boolean <see cref="Converter.From.IsGenericType"/> Type-property to look for</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithFromIsGenericType(this IQueryable<Converter> query, bool? typeFromIsGenericType = true)
        {
            if(typeFromIsGenericType != null)
            {
                query = query.Where(c => c.From.IsGenericType == typeFromIsGenericType);
            }
            return query;
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.To.IsGenericType"/> set equal to <paramref name="typeToIsGenericType"/>
        /// </summary>
        /// <param name="query">The typeToIsGenericType <see cref="IQueryable{Converter}"/> instance which invokes the static extension 
        /// methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeToIsGenericType">The Boolean <see cref="Converter.To.IsGenericType"/> Type-property to look for</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithToIsGenericType(this IQueryable<Converter> query, bool? typeToIsGenericType = true)
        {
            if(typeToIsGenericType != null)
            {
                query = query.Where(c => c.From.IsGenericType == typeToIsGenericType);
            }
            return query;
        }

        //(converter.Function as Delegate).Method.Name
        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have their Delegate Name set to contain a case sensitive string of <paramref name="containedName"/>
        /// </summary>
        /// <param name="query">The typeToIsGenericType <see cref="IQueryable{Converter}"/> instance which invokes the static extension 
        /// methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="containedName">A search-string to be contained in the Method's Name to filter through</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithFunctionName(this IQueryable<Converter> query, string containedName = null)
        {
            if(containedName != null)
            {
                query = query.Where(c => (c.Function != null && (c.Function as Delegate).GetMethodInfo().Name.Contains(containedName) == true) ||
                                         (c.FunctionDefault != null && (c.FunctionDefault as Delegate).GetMethodInfo().Name.Contains(containedName) == true)
                );
            }
            return query;
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.Attribute.Name"/> set equal to <paramref name="containedName"/>
        /// </summary>
        /// <param name="query">The typeToIsGenericType <see cref="IQueryable{Converter}"/> instance which invokes the static extension 
        /// methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="containedName">A search-string to be contained in the <see cref="ConverterAttribute.Name"/> to filter through</param>
        /// <param name="caseSensitive">Whether to search case sensitive (`true`) or not (`false`)</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithConverterAttributeName(this IQueryable<Converter> query, string containedName = null, bool caseSensitive = false)
        {
            if(string.IsNullOrWhiteSpace(containedName) == false)
            {
                if(caseSensitive == true)
                {
                    query = query.Where(c => (c.Attribute != null && c.Attribute.Name.ToLowerInvariant().Equals(containedName.ToLowerInvariant()) == true));
                }
                else
                {
                    query = query.Where(c => (c.Attribute != null && c.Attribute.Name.Equals(containedName) == true));
                }
            }
            return query;
        }

        /// <summary>
        /// Filters items in <see cref="ConverterCollection"/> which have <see cref="Converter.HasDefaultFunction"/> set equal to <paramref name="hasDefaultFunction"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="hasDefaultFunction">The Boolean <see cref="Converter.HasDefaultFunction"/> Property to look for</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<Converter> WithDefaultFunction(this IQueryable<Converter> query, bool? hasDefaultFunction = true)
        {
            if(hasDefaultFunction != null)
            {
                query = query.Where(c => c.HasDefaultFunction == hasDefaultFunction);
            }
            return query;
        }

        /// <summary>Applies all filters in <see cref="ConverterCollectionFilters"/> to an <see cref="IQueryable{Converter}"/> instance</summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeFrom">The <see cref="Converter.From"/> Type to look for</param>
        /// <param name="typeTo">The <see cref="Converter.To"/> Type to look for</param>
        /// <param name="typeArgument">The <see cref="Converter.Argument"/> Type to look for</param>
        /// <param name="typeBase">The <see cref="Converter.BaseType"/> Type to look for</param>
        /// <param name="hasDefaultFunction">The Boolean <see cref="Converter.HasDefaultFunction"/> Property to look for</param>
        /// <param name="isStandard">The Boolean <see cref="Converter.Standard"/> property to look for</param>
        /// <param name="typeFromIsGenericType">The Boolean <see cref="Converter.From.IsGenericType"/> Type-property to look for</param>
        /// <param name="typeToIsGenericType">The Boolean <see cref="Converter.To.IsGenericType"/> Type-property to look for</param>
        /// <param name="functionName">A search-string to be contained in the <see cref="Converter.Function"/> or<see cref="Converter.FunctionDefault"/> to filter through</param>
        /// <param name="attributeName">A search-string to be contained in the <see cref="ConverterAttribute.Name"/> to filter through</param>
        /// <returns>Returns a new filtered query as <see cref="IQueryable{Converter}"/> </returns>
        /// <seealso cref="ConverterCollection.Get(TypeInfo, TypeInfo, TypeInfo, bool?, bool, bool?, bool?, bool?)"/>
        public static IQueryable<Converter> ApplyAllFilters(
            this IQueryable<Converter> query,
            TypeInfo typeFrom = null,
            TypeInfo typeTo = null,
            TypeInfo typeArgument = null,
            TypeInfo typeBase = null,
            bool? hasDefaultFunction = null,
            bool? isStandard = null,
            bool? typeFromIsGenericType = null,
            bool? typeToIsGenericType = null,
            string functionName = null,
            string attributeName = null)
        {
            // all converters are contained in Converter_T3's with the argument set to default: object or TOut with only a Converter.Function
            if(typeBase == null)
            {
                typeArgument = typeArgument ?? typeof(object).GetTypeInfo();
            }

            return query
                        .WithFrom(typeFrom)
                        .WithTo(typeTo)
                        .WithArgument(typeArgument)
                        .WithBaseType(typeBase)
                        .WithStandard(isStandard)
                        .WithDefaultFunction(hasDefaultFunction)
                        .WithFromIsGenericType(typeFromIsGenericType)
                        .WithToIsGenericType(typeToIsGenericType)
                        .WithFunctionName(functionName)
                        .WithConverterAttributeName(attributeName);
        }

    }

}