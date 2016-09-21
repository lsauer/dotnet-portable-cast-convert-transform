// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    using Core.Extensions;
    using Core.Singleton;
    using Core.TypeCast.Base;

    /// <summary>
    /// A set of <see cref="IQueryable{Converter}"/> extension methods for filtering <see cref="ConverterCollection"/> items using <see cref="System.Linq"/>
    /// </summary>
    public static class ConverterCollectionLookup
    {

        /// <summary>
        /// Looks up an object instance implementing the <see cref="IConverter"/> and <see cref="IConverter{TIn, TOut}"/> interface 
        /// by deriving the source and target type from the generic argument parameters of the interface
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="baseTypeInstance">an object instance implementing the <see cref="IConverter"/> and <see cref="IConverter{TIn, TOut}"/> interface </param>
        /// <returns>Returns a <see cref="Converter"/> matching the source and target types of the supported <see cref="IConverter{TIn, TOut}"/> interface, else `null` </returns>
        public static Converter Get(this IQueryable<Converter> query, IConverter baseTypeInstance)
        {
            return query.Get(baseTypeInstance?.GetType());
        }

        /// <summary>
        /// Looks up an object <see cref="class"/> or <see cref="struct"/> <see cref="Type"/> implementing the <see cref="IConverter"/> and <see cref="IConverter{TIn, TOut}"/> interface 
        /// by deriving the source and target type from the generic argument parameters of the interface to look up a possible candidate in <see cref="Items"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="baseType">an arbitrary class <see cref="Type"/> implementing the <see cref="IConverter"/> and <see cref="IConverter{TIn, TOut}"/> interface </param>
        /// <returns>Returns a <see cref="Converter"/> matching the source and target types of the supported <see cref="IConverter{TIn, TOut}"/> interface, else `null` </returns>
        public static Converter Get(this IQueryable<Converter> query, Type baseType)
        {

            var interfaceType = baseType?.GetTypeInfo()
                                .ImplementedInterfaces.ToList()
                                .FirstOrDefault(c => c.Name.StartsWith(nameof(IConverter)));
            if(interfaceType != null && interfaceType.IsConstructedGenericType)
            {
                var types = interfaceType.GenericTypeArguments;

                var converter = query.Get(types.First().GetTypeInfo(), types.Last().GetTypeInfo());
                return converter;
            }
            return null;
        }

        /// <summary>
        /// Gets the <see cref="Converter"/> for a given arbitrary conversion source-type <paramref name="typeFrom"/> and an arbitrary conversion target-type <paramref name="typeTo"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeFrom">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></param>
        /// <param name="typeTo">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></param>
        /// <returns>A converter instance if the query yielded a result, or `null` if no suitable <see cref="Converter"/> could be found.</returns>
        public static Converter Get(this IQueryable<Converter> query, Type typeFrom, Type typeTo)
        {
            return query.Get(typeFrom: typeFrom.GetTypeInfo(), typeTo: typeTo.GetTypeInfo());
        }

        /// <summary>
        /// Gets the <see cref="Converter"/> for a given arbitrary conversion source-type <paramref name="typeFrom"/> and an arbitrary conversion target-type <paramref name="typeTo"/>
        /// </summary>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="value">The Source- / From- object from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></param>
        /// <param name="hasDefault">The Boolean <see cref="Converter.HasDefaultFunction"/> Property to look for</param>
        /// <param name="loadOnDemand">Whether to look for any <see cref="Converter"/> in the Load-On-Demand list, if no results are yielded.</param>
        /// <returns>A converter instance if the query yielded a result, or `null` if no suitable <see cref="Converter"/> could be found.</returns>
        public static Converter Get<TOut>(this IQueryable<Converter> query, object value, bool? hasDefault = null, bool loadOnDemand = false)
        {
            return query.Get(value?.GetType().GetTypeInfo(), typeof(TOut).GetTypeInfo(), hasDefaultFunction: hasDefault, loadOnDemand: loadOnDemand);
        }

        /// <summary>
        /// Gets the <see cref="Converter"/> for a given arbitrary conversion source-type <paramref name="typeFrom"/> and an arbitrary conversion target-type <paramref name="typeTo"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="value">The Source- / From- object from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></param>
        /// <param name="typeArgument">The <see cref="Converter.Argument"/> Type to look for</param>
        /// <param name="hasDefault">The Boolean <see cref="Converter.HasDefaultFunction"/> Property to look for</param>
        /// <param name="loadOnDemand">Whether to look for any <see cref="Converter"/> in the Load-On-Demand list, if no results are yielded.</param>
        /// <returns>A converter instance if the query yielded a result, or `null` if no suitable <see cref="Converter"/> could be found.</returns>
        public static Converter Get<TIn, TOut>(this IQueryable<Converter> query, TIn value = default(TIn), Type typeArgument = null, bool? hasDefault = null, bool loadOnDemand = false)
        {
            return query.Get(typeof(TIn).GetTypeInfo(), typeof(TOut).GetTypeInfo(), typeArgument: typeArgument?.GetTypeInfo(), hasDefaultFunction: hasDefault, loadOnDemand: loadOnDemand);
        }

        /// <summary>
        /// Gets the <see cref="Converter"/> for a given arbitrary conversion source-type <paramref name="typeFrom"/> and an arbitrary conversion target-type <paramref name="typeTo"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="value">The Source- / From- object from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></param>
        /// <param name="argument">The <see cref="Converter.Argument"/> Type to look for</param>
        /// <param name="defaultValue">An optional default value from which the <see cref="Type"/> is derived</param>
        /// <param name="hasDefault">The Boolean <see cref="Converter.HasDefaultFunction"/> Property to look for</param>
        /// <param name="loadOnDemand">Whether to look for any <see cref="Converter"/> in the Load-On-Demand list, if no results are yielded.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <typeparam name="TArg">The Argument <see cref="Type"/> for generic converters using see <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object)"/>. 
        /// <returns>A converter instance if the query yielded a result, or `null` if no suitable <see cref="Converter"/> could be found.</returns>
        /// <returns>A converter instance if the query yielded a result, or `null` if no suitable <see cref="Converter"/> could be found.</returns>

        public static Converter Get<TIn, TArg, TOut>(this IQueryable<Converter> query, TIn value = default(TIn), TArg argument = default(TArg), TOut defaultValue = default(TOut), bool? hasDefault = null, bool loadOnDemand = false)
        {
            return query.Get(typeof(TIn).GetTypeInfo(), typeof(TOut).GetTypeInfo(), typeArgument: typeof(TArg).GetTypeInfo(), hasDefaultFunction: hasDefault, loadOnDemand: loadOnDemand);
        }

        /// <summary>
        /// Gets the <see cref="Converter"/> for a given arbitrary conversion source-type <paramref name="typeFrom"/> and an arbitrary conversion target-type <paramref name="typeTo"/>
        /// </summary>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)"/></typeparam>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeFrom">The <see cref="Converter.From"/> Type to look for</param>
        /// <param name="typeArgument">The <see cref="Converter.Argument"/> Type to look for</param>
        /// <param name="hasDefault">The Boolean <see cref="Converter.HasDefaultFunction"/> Property to look for</param>
        /// <param name="loadOnDemand">Whether to look for any <see cref="Converter"/> in the Load-On-Demand list, if no results are yielded.</param>
        /// <returns>A converter instance if the query yielded a result, or `null` if no suitable <see cref="Converter"/> could be found.</returns>
        public static Converter Get<TOut>(this IQueryable<Converter> query, Type typeFrom, Type typeArgument = null, bool? hasDefault = null, bool loadOnDemand = false)
        {
            return query.Get(typeFrom.GetTypeInfo(), typeof(TOut).GetTypeInfo(), typeArgument: typeArgument?.GetTypeInfo(), hasDefaultFunction: hasDefault, loadOnDemand: loadOnDemand);
        }

        /// <summary>
        /// Gets the <see cref="Converter"/> for a given arbitrary conversion source-type <paramref name="typeFrom"/> and an arbitrary conversion target-type <paramref name="typeTo"/>
        /// </summary>
        /// <param name="query">The own <see cref="IQueryable{Converter}"/> instance which invokes the static extension methods in <see cref="ConverterCollectionFilters"/></param>
        /// <param name="typeFrom">The <see cref="Converter.From"/> Type to look for</param>
        /// <param name="typeTo">The <see cref="Converter.To"/> Type to look for</param>
        /// <param name="typeArgument">The <see cref="Converter.Argument"/> Type to look for</param>
        /// <param name="typeBase">The <see cref="Converter.BaseType"/> Type to look for</param>
        /// <param name="hasDefaultFunction">The Boolean <see cref="Converter.HasDefaultFunction"/> Property to look for</param>
        /// <param name="isStandard">The Boolean <see cref="Converter.Standard"/> property to look for</param>
        /// <param name="loadOnDemand">Whether to look for any <see cref="Converter"/> in the Load-On-Demand list, if no results are yielded.</param>
        /// <param name="typeFromIsGenericType">Whether <paramref name="typeFrom"/> is generic, in cases of unboxing to a sub-type, as may happen with <see cref="Nullable{T}"/></param>
        /// <param name="typeToIsGenericType">Whether <paramref name="typeTo"/> is generic, in cases of unboxing to a sub-type, as may happen with <see cref="Nullable{T}"/></param>
        /// <param name="functionName">A search-string to be contained in the <see cref="Converter.Function"/> or<see cref="Converter.FunctionDefault"/> to filter through</param>
        /// <param name="attributeName">A search-string to be contained in the <see cref="ConverterAttribute.Name"/> to filter through</param>
        /// <param name="assignable">Whether to check via <see cref="TypeInfo.IsAssignableFrom(TypeInfo)"/> for supported interfaces or base-classes.r</param>
        /// <returns>A converter instance if the query yielded a result, or `null` if no suitable <see cref="Converter"/> could be found.</returns>
        /// <remarks>note that invocation of <see cref="Get"/> may instantiate and thus initializes any required converters referenced in <see cref="loadOnDemandConverters"/>
        /// </remarks>
        /// <remarks>`new Nullable&lt;int>(5).GetType().IsConstructedGenericType` will yield `false` due to boxing steps during the Reflection process. 
        /// As such use the strictly typed <seealso cref="Get{TOut}(object,bool,bool)"/></remarks>
        /// <exception cref="ConverterCollectionException">Throws an exception if the lookup yielded a null reference or an internal error.</exception>
        public static Converter Get(this IQueryable<Converter> query, 
            TypeInfo typeFrom,
            TypeInfo typeTo,
            TypeInfo typeArgument = null,
            TypeInfo typeBase = null,
            bool? hasDefaultFunction = null,
            bool loadOnDemand = false,
            bool? isStandard = null,
            bool? typeFromIsGenericType = null,
            bool? typeToIsGenericType = null,
            string functionName = null,
            string attributeName = null,
            bool assignable = false)
        {
            query = query.ApplyAllFilters(typeFrom: typeFrom, typeTo: typeTo, typeArgument: typeArgument, typeBase: typeBase, hasDefaultFunction: hasDefaultFunction,
                                            isStandard: isStandard, typeFromIsGenericType: typeFromIsGenericType, typeToIsGenericType: typeToIsGenericType, 
                                            functionName: functionName, attributeName: attributeName, assignableFrom: assignable, assignableTo: assignable, assignableArgument: assignable);

            if(loadOnDemand == true && query.Any() == false && (query as ConverterCollection)?.LoadOnDemandConverter(typeTo.AsType()) > 0)
            {
                query = query.WithFrom(typeFrom).WithTo(typeTo);
            }

            return query.FirstOrDefault();
        }

    }

}