// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.0.1.4                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast.Base
{
    using System;
    using Core.TypeCast;
    using Core.Extensions;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Creates instances from classes which are attributed by <see cref="ConverterAttribute"/>
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public class BaseClassFactoryRT : Factory<object, Type, ConverterCollection>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Type"/> <paramref name="converterClass"/>
        /// </summary>
        /// <param name="converterClass">The class which is attributed by <see cref="ConverterAttribute"/></param>
        /// <exception cref="ConverterCollectionException">An exception raised by the <see cref="Create"/> method pre-instance creation, if the <see cref="converterClass "/>
        /// was previously instanced by the <see cref="ConverterCollection"/>.</exception>
        /// <exception cref="TargetInvocationException">An exception-wrapper containing an <see cref="Exception.InnerException"/> of an error which occurred during or 
        /// at instance creation.</exception>
        public override object Create(Type converterClass)
        {
            return this.Create(converterClass, ConverterCollection.CurrentInstance);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Type"/> <paramref name="converterClass"/>
        /// </summary>
        /// <param name="converterClass">The class which is attributed by <see cref="ConverterAttribute"/></param>
        /// <param name="collection">A n optional <see cref="ConverterCollection"/> instance passed during instance creation for Dependency Injection</param>
        /// <returns>Returns a new instance of <paramref name="converterClass"/> upon success, else null</returns>
        /// <exception cref="ConverterCollectionException">An exception raised by the <see cref="Create"/> method pre-instance creation, if the <see cref="converterClass "/>
        /// was previously instanced by the <see cref="ConverterCollection"/>.</exception>
        /// <exception cref="TargetInvocationException">An exception-wrapper containing an <see cref="Exception.InnerException"/> of an error which occurred during or 
        /// at instance creation.</exception>
        public override object Create(Type converterClass, ConverterCollection collection = null)
        {
            if(collection?.ConverterClassInitialized.ContainsKey(converterClass.GetTypeInfo()) == true)
            {
                if(collection?.Settings.ConverterClassExistsException == true)
                {
                    throw new ConverterCollectionException(ConverterCollectionCause.ConverterClassExists);
                }
                return null;
            }

            object customConverter;
            try
            {
                if(typeof(ConverterCollectionDependency).GetTypeInfo().IsAssignableFrom(converterClass.GetTypeInfo())
                    || converterClass.GetTypeInfo().IsDependencyInjectable(typeof(IConverterCollection)))
                {
                    customConverter = Instantiate(type: converterClass, parameters: collection as IConverterCollection);
                }
                else
                {
                    customConverter = Instantiate(type: converterClass);
                }
            }
            catch(TargetInvocationException exc)
            {
                throw exc?.InnerException;
            }

            return customConverter;
        }
    }
}