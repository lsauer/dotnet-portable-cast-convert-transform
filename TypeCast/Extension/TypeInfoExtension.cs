// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A custom excerpt of the Core.Extensions library. Not for reuse!  </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/dotnet-core.extensions                 </project>
namespace Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    /// <summary>
    /// The  <see cref="TypeInfo"/> extension.
    /// </summary>
    public static partial class TypeInfoExtension
    {

        /// <summary>
        /// Get a an <seealso cref="IEnumerable{System.Reflection.ConstructorInfo}"/> of all constructors that match the argument <paramref name="parameterType"/>
        /// </summary>
        /// <param name="type">The own instance of the <see cref="TypeInfo"/> which invokes the method. </param>
        /// <param name="parameterType">The required parameter type which has to be present in the constructor's parameter-list.</param>
        /// <returns>The <see cref="IEnumerable{System.Reflection.ConstructorInfo}"/> of <see cref="System.Reflection.ConstructorInfo"/> that match the criteria. </returns>
        /// <remarks>In most cases the criteria should be narrowed to return one potential <see cref="System.Reflection.ConstructorInfo"/>. </remarks>
        public static IEnumerable<ConstructorInfo> GetConstructorsByParameterType(this TypeInfo type, Type parameterType)
        {
            return from constructorInfo in type.DeclaredConstructors
                   where constructorInfo.GetParameters().Any(p => p.IsOptional == false && p.ParameterType == parameterType)
                   select constructorInfo;
        }

        /// <summary>
        /// Get a a single <seealso cref="System.Reflection.ConstructorInfo"/> of the first constructors that match the argument <paramref name="parameterTypes"/> otherwise returns `null`
        /// </summary>
        /// <param name="type">The own instance of the <see cref="TypeInfo"/> which invokes the method. </param>
        /// <param name="parameterTypes">The required parameter types which have to be present in the constructor's parameter-list.</param>
        /// <returns>The <see cref="System.Reflection.ConstructorInfo"/> of <see cref="System.Reflection.ConstructorInfo"/> that match the criteria. </returns>
        /// <remarks>Make sure the criteria are narrowed down to yield only one potential <see cref="System.Reflection.ConstructorInfo"/> to avoid errant disambiguates. </remarks>
        public static ConstructorInfo GetConstructorByParameterTypes(this TypeInfo type, Type[] parameterTypes)
        {
            return (from constructorInfo in type.DeclaredConstructors
                   let parameterInfos = constructorInfo.GetParameters()
                   where
                        parameterInfos.Length == parameterTypes.Length
                        && parameterInfos.Select(p => p.ParameterType.Name).SequenceEqual(parameterTypes.Select(t => t.Name))
                   select constructorInfo)?.
                   FirstOrDefault();
        }

        /// <summary>
        /// Yields true if a <see cref="class"/> or <see cref="struct"/> contain a constructor of a <see cref="Type"/> "<paramref name="interfaceType"/>"
        /// </summary>
        /// <param name="type">The own instance of the <see cref="TypeInfo"/> which invokes the method. </param>
        /// <param name="interfaceType">The parameter argument <see cref="Type"/> which should be, but does not strictly has to be, an <see cref="interface"/>.</param>
        /// <returns>Returns true if <paramref name="type"/> has a constructor that can be used for Dependency Injection of <paramref name="interfaceType"/>.</returns>
        /// <remarks>The method-extension can be used to dynamically detect if a constructor is present for Dependency Injection via <paramref name="interfaceType"/>. </remarks>
        public static bool IsDependencyInjectable(this TypeInfo type, Type interfaceType)
        {
            return type.GetConstructorsByParameterType(interfaceType).Any();
        }


        /// <summary>
        /// Checks if a delegate <see cref="Type"/> matches the <see cref="MethodInfo.ReturnParameter"/> and argument types in order and <see cref="Type"/>
        /// </summary>
        /// <param name="method">A delegate Type from which a MethodInfo is looked up to compare argument-types with the arguments provided</param>
        /// <param name="returnParameter">The `return parameter` to compare with the return-parameter of the method</param>
        /// <param name="argumentParameters">The parameters to compare in sequence and <see cref="Type"/> to the method's arguments</param>
        /// <returns>Return `true` if the parameter all match in sequence order and type</returns>
        public static bool IsInvokableWithParameters(this TypeInfo method, Type returnParameter, params Type[] argumentParameters)
        {
            var methodInfo = method.GetDeclaredMethod("Invoke");
            if(methodInfo == null)
            {
                return false;
            }
            var parameters = methodInfo.GetParameters();
            bool trueForAll = true;
            for(int i = 0;  i < argumentParameters.Length; i++)
            {
                if(argumentParameters[i] == null)
                {
                    continue;
                }
                if(i <= parameters.Length)
                {
                    trueForAll &= (parameters[i] != null && parameters[i].ParameterType == argumentParameters[i]);
                }
            }
            return methodInfo.ReturnParameter.ParameterType == returnParameter && trueForAll;
        }

        /// <summary>
        /// Yields null for any reference <see cref="Type"/> or the default value of any value type <see cref="TypeInfo.IsValueType"/> that has a parameterless constructor 
        /// </summary>
        /// <param name="type">The own instance of the <see cref="TypeInfo"/> which invokes the static extension method. </param>
        /// <returns>Returns an object that is either null or containing the default value of the underlying value type.</returns>
        public static object GetDefault(this TypeInfo type)
        {
            if(type.GetConstructorsByParameterType(null) != null && type.IsAbstract == false && type.IsValueType == true)
            {
                return Activator.CreateInstance(type.AsType());
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the Return Parameter <see cref="Type"/> of any Delegate type instance else returns  `null`
        /// </summary>
        /// <param name="type">The own instance of the <see cref="TypeInfo"/> which invokes the static extension method. </param>
        /// <param name="defaultType">A default <see cref="Type"/> to be passed out in case of no parameter being present. 
        /// If the input type does not inherit from <see cref="Delegate"/> the result will still be `null`</param>
        /// <returns>Returns an <see cref="Type"/> instance of the delegate's return-parameter or null `null`.</returns>
        public static Type GetReturnParameterType(this TypeInfo type, Type defaultType = null)
        {
            if(typeof(Delegate).GetTypeInfo().IsAssignableFrom(type) == true)
            {
                return type.GetDeclaredMethod("Invoke")?.ReturnParameter?.ParameterType ?? defaultType;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Compares the nature between two types and returns a <see cref="TypeMatch"/> <see cref="Enum"/> value.
        /// If the parameter <paramref name="checkIsSame"/> is set to <see cref="true"/>, a <see cref="TypeMatch"/> value of <see cref="TypeMatch.Same"/> is returned if the Types match else <see cref="TypeMatch.None"/> is returned. 
        /// If the parameter <paramref name="checkIsSame"/> is set to <see cref="false"/>,  the <see cref="Type.Namespace"/> of the input <paramref name="self"/> and output type <typeparamref name="TOut"/> is compared. 
        /// If the parameter <paramref name="checkIsSame"/> is set to <see cref="null"/> all values of <see cref="TypeMatch"/> are possible as return value.
        /// </summary>
        /// <typeparam name="TOut">The Target / To- <see cref="Type" /> to which to <see cref="Converter{TIn,TOut}.Convert(object,object)" /></typeparam>
        /// <param name="self">The current instance holding the boxed value to convert from</param>
        /// <param name="checkIsSame"> If the parameter is set to <see cref="true"/>, <see cref="TypeMatch.None"/> means they types do not match. 
        /// If set to <see cref="false"/>, the result <see cref="TypeMatch.None"/> means the types are not similar. 
        /// If set to <see cref="null"/>,  the result <see cref="TypeMatch.None"/> means that the types do not share a relationship.</param>
        /// <returns>Returns the nature of the compared types as an <see cref="TypeMatch"/> <see cref="Enum"/> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeMatch IsSameOrSimilar<TOut>(this TypeInfo self, bool? checkIsSame = null)
        {
            var typeOut = typeof(TOut).GetTypeInfo();
            if((checkIsSame  == null || checkIsSame == true) && self == typeOut)
            {
                return TypeMatch.Same;
            }

            if((checkIsSame == null || checkIsSame == false) && (self.Namespace == typeOut.Namespace) && (self.BaseType == typeOut.BaseType))
            {
                return TypeMatch.Similar;
            }
            return TypeMatch.None;
        }
    }

    /// <summary>
    /// An enumerating of values containing a rough 2bit classification of relationship between two types
    /// </summary>
    /// <remarks>The enumeration relates to the type change classification of Cast, Convert and Transform</remarks>
    public enum TypeMatch
    {

        /// <summary>
        /// Unknown or no relationship between the types
        /// </summary>
        [Description("Unknown or no relationship between the types")]
        None,

        /// <summary>
        /// Identical or same relationship between the types
        /// </summary>
        [Description("Identical or same relationship between the types")]
        Same,

        /// <summary>
        /// Similar or same base relationship between the types
        /// </summary>
        [Description("Similar or same base relationship between the types")]
        Similar,
    }
}