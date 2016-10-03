// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
using System;

namespace Core.TypeCast
{
    /// <summary>
    /// The generic, common factory interface for declaring factories creating arbitrary object instances requiring up to two arguments. 
    /// Use a container type such as <see cref="Tuple"/> or <see langword="struct"/> as second parameter <typeparamref name="TIn2"/> if more parameters are required.
    /// </summary>
    /// <typeparam name="TInstance">The <see cref="Type"/> of the instances to create and return by the factory method <see cref="Create(TIn1)"/>
    /// and <see cref="Create(TIn1, TIn2)"/>.</typeparam>
    /// <typeparam name="TIn1">The parameter type used for defining the instance creation process in the factory method <see cref="Create(TIn1)"/></typeparam>
    /// <typeparam name="TIn2">The 2. parameter type used for defining the instance creation process in the factory method <see cref="Create(TIn1)"/> 
    /// and <see cref="Create(TIn1, TIn2)"/></typeparam>
    public interface IFactory<out TInstance, in TIn1, in TIn2> where TInstance : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        TInstance Create(TIn1 parameter);

        /// <summary>
        /// Creates new <typeparamref name="TInstance"/> instances dependent on the parameter type <typeparamref name="TIn1"/> and optionally <see cref="Type"/> <typeparamref name="TIn2"/>
        /// </summary>
        /// <param name="parameter">The 1. parameter of <see cref="Type"/> <typeparamref name="TIn1"/> defining instance creation.</param>
        /// <param name="parameterSet">The 2. optional set of parameters or single parameter of <see cref="Type"/> <typeparamref name="TIn2"/> defining instance creation.</param>
        /// <returns>Returns a new instance of <typeparamref name="TInstance"/> upon success</returns>
        TInstance Create(TIn1 parameter, TIn2 parameterSet = default(TIn2));
    }
}