// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast.Base
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    using Core.Extensions;

    /// <summary>
    /// The generic class used for deriving specific dependency-injection classes, used for reflection, filtering, and strict checks at compile time.
    /// </summary>
    /// <typeparam name="TDependency">The <see cref="Type"/> of the dependency</typeparam>
    /// <exception cref="DependencyInjectionException">
    ///     If the <typeparamref name="TDependency"/> instance is `null` a <see cref="DependencyInjectionException"/> is thrown
    /// </exception>
    /// <seealso cref="ConverterCollectionDependency"/>
    [System.Runtime.InteropServices.ComVisible(false)]
    public abstract class DependencyInjection<TDependency>
    {
        /// <summary>Initializes a new instance of the abstract <see cref="DependencyInjection{TDependency}" /> class.</summary>
        /// <param name="injector">
        ///     The <paramref name="injector" />  injection parameter is required as an argument of the parent class constructor for dependency injection.
        /// </param>
        /// <exception cref="DependencyInjectionException">
        ///     If the <paramref name="injector" /> is `null` a <see cref="DependencyInjectionException"/> is thrown
        /// </exception>
        protected DependencyInjection(TDependency injector)
        {
            if(injector == null)
            {
                throw new DependencyInjectionException(message: nameof(NullReferenceException), innerException: new NullReferenceException());
            }
        }
    }
}