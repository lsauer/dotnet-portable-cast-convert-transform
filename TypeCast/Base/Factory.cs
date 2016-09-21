// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
using System.Reflection;
using System.Linq;
using System;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Core.TypeCast.Base
{
    using System.Collections.Generic;
    using Extensions;

    /// <summary>
    /// The abstract generic factory for creating arbitrary instances requiring up to two arguments. Use a container type such as <see cref="Tuple"/> or <see cref="struct"/> 
    /// as second parameter type <typeparamref name="TIn2"/> if more parameters are required.
    /// </summary>
    /// <typeparam name="TInstance">The <see cref="Type"/> of the instances to create and return by the factory method <see cref="Create(TIn1)"/>
    /// and <see cref="Create(TIn1, TIn2)"/>.</typeparam>
    /// <typeparam name="TIn1">The parameter type used for defining the instance creation process in the factory method <see cref="Create(TIn1)"/></typeparam>
    /// <typeparam name="TIn2">The 2. parameter type used for defining the instance creation process in the factory method <see cref="Create(TIn1)"/> 
    /// and <see cref="Create(TIn1, TIn2)"/></typeparam>
    public abstract class Factory<TInstance, TIn1, TIn2> : IFactory<TInstance, TIn1, TIn2> where TInstance : class
    {
        /// <summary>
        /// Gets a string representation of the Base-Factory using <see cref="ToString()"/>
        /// </summary>
        public string Name
        {
            get
            {
                return this.ToString();
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{Type}"/> of the interfaces supported by the factory instance.
        /// </summary>
        public IEnumerable<Type> GetInterfaces()
        {
            var type = this.GetType();
            yield return type;

            foreach(var interfaceType in type.GetTypeInfo().ImplementedInterfaces)
            {
                yield return interfaceType;
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{Type}"/> of the generic parameters that constructed the base-factory instance.
        /// </summary>
        public IEnumerable<Type> GetParameters()
        {
            var type = this.GetType();
            yield return type;

            foreach(var parameterType in type.GetTypeInfo().GenericTypeParameters)
            {
                yield return parameterType;
            }
        }

        /// <summary>
        /// Abstract method for creating instances of <typeparamref name="TInstance"/> defined only through <paramref name="parameter"/>
        /// </summary>
        /// <param name="parameter">The parameter to define the instance creation process</param>
        /// <returns>an instance of <see cref="Type"/> <typeparamref name="TInstance"/></returns>
        public abstract TInstance Create(TIn1 parameter);

        /// <summary>
        /// Abstract method for creating instances of <typeparamref name="TInstance"/> defined only through <paramref name="parameter"/> 
        /// and <paramref name="parameter2"/> 
        /// </summary>
        /// <param name="parameter">The parameter to define the instance creation process</param>
        /// <param name="parameter2">The 2. parameter to define the instance creation process</param>
        /// <returns>an instance of <see cref="Type"/> <typeparamref name="TInstance"/></returns>
        public abstract TInstance Create(TIn1 parameter, TIn2 parameter2 = default(TIn2));

        /// <summary>
        /// Internal method for object instantiation by a passed type <paramref name="type"/>
        /// </summary>
        /// <param name="type">the type of the <see cref="class"/> or <see cref="struct"/> which to instance</param>
        /// <param name="parameters">the parameters passed to the constructor of the <see cref="class"/> or <see cref="struct"/></param>
        /// <returns>Returns an instance object of type <typeparamref name="TOut"/> or `null`</returns>
        protected static object Instantiate(Type type, params object[] parameters)
        {
            return Instantiate<object>(type: type, args: parameters);
        }

        /// <summary>
        /// Internal method for object instantiation following the "Try" convention of returning a bool `true` upon success and passing the result with `out`
        /// </summary>
        /// <param name="type">The type of the <see cref="class"/> or <see cref="struct"/> which to instance</param>
        /// <param name="parameters">The parameters passed to the constructor of the <see cref="class"/> or <see cref="struct"/></param>
        /// <param name="instance">The assigned instance reference upon instancing of type <typeparamref name="TOut"/> or `null` upon failure</param>
        /// <returns>Returns <see cref="bool"/> `true` upon success or `false` upon failure and assigning `null` to <paramref name="instance"/></returns>
        protected static bool TryInstantiate(Type type, out object instance, params object[] parameters)
        {
            instance = null;
            try { 
                instance = Instantiate<object>(type: type, args: parameters);
            } catch(Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Internal method object instantiation of a generic type , with the generic-parameters passed as the second argument
        /// </summary>
        /// <typeparam name="TOut">The type of the instance to pass out</typeparam>
        /// <param name="type">The type of the <see cref="class"/> or <see cref="struct"/> which to instance</param>
        /// <param name="parameters">the generic types that comprise the generic type</param>
        /// <param name="args">The parameters passed to the constructor of the <see cref="class"/> or <see cref="struct"/></param>
        /// <returns>Returns an instance object of type <typeparamref name="TOut"/> or `null`</returns>
        protected static TOut Instantiate<TOut>(Type type, Type[] parameters, params object[] args)
        {
            var constructed = type.GetTypeInfo().MakeGenericType(parameters);
            var instance = Instantiate<TOut>(type: constructed, args: args);
            return instance;
        }

        /// <summary>
        /// Creates an instance of the type designated by the specified generic type parameter
        /// </summary>
        /// <param name="type">The type of the <see cref="class"/> or <see cref="struct"/> which to instance</param>
        /// <param name="args">the parameters passed to the constructor of the <see cref="class"/> or <see cref="struct"/></param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="System.MissingMethodException">NoteIn the .NET for Windows Store apps or the Portable Class Library, catch the
        /// base class exception, System.MissingMemberException, instead. The type that specified for T does not have a parameterless constructor.</exception>
        /// <remarks>Centralize all runtime calls to <code>`Activator.Create(...)`</code> and <code>constructor.Invoke(...)`</code> </remarks>
        protected static TOut Instantiate<TOut>(Type type, object[] args = null)
        {
            if(args?.Length == 0)
            {
                return (TOut)Activator.CreateInstance(type);
            }
            return (TOut)Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// Returns a string representation of the Factory type.
        /// </summary>
        public override string ToString()
        {
            var type = this.GetType().GetTypeInfo();
            var genericArgs = type.GenericTypeArguments;
            if(genericArgs.Any())
            {
                var typeNames = genericArgs.Select(t => t.Name)
                    .Aggregate((a, b) => a + ',' + b);
                return $"{type.Name} <{typeNames}>";
            }
            else
            {
                return type.Name;
            }
        }

    }
}