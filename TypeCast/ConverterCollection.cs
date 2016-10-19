// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Core.Extensions;
    using Core.Singleton;
    using Core.TypeCast.Base;


    /// <summary>
    /// The thread-safe, static collection of <see cref="Converter"/> items, using <see cref="Core.Singleton"/> and supporting <see cref="Core.Singleton.ISingleton"/>
    /// </summary>
    /// <remarks> See https://github.com/lsauer/csharp-singleton for more information.</remarks>
    /// <remarks> Building the project requires the Singleton-library: https://www.nuget.org/packages/CSharp.Portable-Singleton/ </remarks>
    /// <remarks>If <see cref="SingletonAttribute.CreateInternal"/> is set to `false`, <see cref="ConverterCollection"/> must explicitly be instantiated using 
    /// `new ConverterCollection(...)` and  not use <see cref="Singleton{ConverterCollection}.CurrentInstance"/> before instantiation. 
    /// If <see cref="SingletonAttribute.CreateInternal"/> is set to `true`, lazy-instantiation of the singleton is possible at any time, 
    /// after the program's assembly entry point is reached.</remarks>
    [Singleton(disposable: true, createInternal: true, initByAttribute: true)]
    [System.Runtime.InteropServices.ComVisible(false)]
    public partial class ConverterCollection : Singleton<ConverterCollection>, IConverterCollection, IDisposable, IEnumerable<Converter>, IQueryable<Converter>
    {
        /// <summary>
        /// `True` when the ConverterCollection has been loading assemblies through auto-discovery as implemented in <see cref="AutoInitialize"/>
        /// </summary>
        private static bool AutoInitialized;

        /// <summary>
        /// A list of <see cref="Converter"/> base-classes which have not yet been instantiated and added to <see cref="Items"/>. 
        /// Upon first look-up they will be instantiated and removed from the list.
        /// </summary>
        private readonly Dictionary<string, List<Type>> loadOnDemandConverters;

        /// <summary>Gets the count for <see cref="Count"/>.</summary>
        /// <seealso cref="Core.Singleton.Singleton{TClass}.OnPropertyChanged(object, string)"/>
        private int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterCollection"/> class with default-parameters.
        /// <para>Instantiation with `new ConverterCollection(...)` must only be once and before any functions 
        /// that require <see cref="Singleton{ConverterCollection}.CurrentInstance"/> </para>
        /// </summary>
        public ConverterCollection()
            : this(assemblyNameSpace: null, numberFormatDefault: null, converterClasses: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterCollection"/> class with default-parameters.
        /// <para>Instantiation with `new ConverterCollection(...)` must only be once and before any functions 
        /// that require <see cref="Singleton{ConverterCollection}.CurrentInstance"/> </para>
        /// </summary>
        /// <param name="assemblyNameSpace">The <see cref="TypeInfo.BaseType"/> type which serves as the assembly entry point of the assemblyNameSpace e.g. `Program`</param>
        public ConverterCollection(Type assemblyNameSpace)
            : this(assemblyNameSpace: assemblyNameSpace, numberFormatDefault: null, converterClasses: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterCollection"/> class with default-parameters.
        /// <para>Instantiation with `new ConverterCollection(...)` must only be once and before any functions 
        /// that require <see cref="Singleton{ConverterCollection}.CurrentInstance"/> </para>
        /// </summary>
        /// <param name="assemblyNameSpace">The <see cref="TypeInfo.BaseType"/> type which serves as the assembly entry point of the assemblyNameSpace e.g. `Program`</param>
        /// <param name="converterClass">The type of a converter class to look for converters and load into the collection</param>
        /// <param name="numberFormatDefault">An optional <see cref="NumberFormatInfo"/> to provide to any added converters for formatting.</param>
        public ConverterCollection(Type assemblyNameSpace, Type converterClass, NumberFormatInfo numberFormatDefault = null)
            : this(assemblyNameSpace: assemblyNameSpace, numberFormatDefault: numberFormatDefault, converterClasses: converterClass)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterCollection"/> class with default-parameters.
        /// <para>Instantiation with `new ConverterCollection(...)` must only be once and before any functions 
        /// that require <see cref="Singleton{ConverterCollection}.CurrentInstance"/> </para>
        /// </summary>
        /// <param name="assembly">An assembly to look for <see cref="ConverterAttribute"/> to discover and load converters into the collection</param>
        /// <param name="numberFormatDefault">An optional <see cref="NumberFormatInfo"/> to provide to any added converters for formatting.</param>
        public ConverterCollection(Assembly assembly, NumberFormatInfo numberFormatDefault = null)
            : this(assemblyNameSpace: null, numberFormatDefault: numberFormatDefault, converterClasses: null)
        {
            this.Initialize(assembly);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterCollection"/> class with default-parameters.
        /// </summary>
        /// <param name="assemblyNameSpace">The <see cref="TypeInfo.BaseType"/> type which serves as the assembly entry point of the assemblyNameSpace e.g. `Program`</param>
        /// <param name="converterClasses">The type of a converter class to look for converters and load into the collection  <see cref="Items"/></param>
        public ConverterCollection(Type assemblyNameSpace = null, params Type[] converterClasses)
            : this(assemblyNameSpace: assemblyNameSpace, numberFormatDefault: null, converterClasses: converterClasses)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterCollection"/> class with default-parameters.
        /// </summary>
        /// <example> **Example:** Initialize a new ConverterCollection with several custom parameters, before any invocation of a ConverterCollection instance
        /// <code>
        /// ```cs
        ///     var cc = new ConverterCollection(typeof(Program), typeof(CustomConverters), typeof(DNA))
        ///     {
        ///         AutoReset = true,
        ///         Settings = new ConverterCollectionSettings()
        ///         {
        ///             ConverterDefaultWrapperOrException = true
        ///         }
        ///    };
        /// ```
        /// </code>
        /// </example>
        /// <param name="assemblyNameSpace">The <see cref="TypeInfo.BaseType"/> type which serves as the assembly entry point of the assemblyNameSpace e.g. `Program`</param>
        /// <param name="numberFormatDefault">An optional <see cref="NumberFormatInfo"/> to provide to any added converters for formatting.</param>
        /// <param name="converterClasses">The types of a converter classes to encapsulate in <see cref="Converter"/> instances and add into the collection <see cref="Items"/></param>
        public ConverterCollection(Type assemblyNameSpace = null, NumberFormatInfo numberFormatDefault = null, params Type[] converterClasses)
        {
            this.Settings = new ConverterCollectionSettings();
            this.loadOnDemandConverters = new Dictionary<string, List<Type>>();

            this.Factory = new ConverterFactory();
            this.FactoryBaseClass = new BaseClassFactoryRT();

            this.AssemblyInitialized = new ConcurrentDictionary<Assembly, bool>();
            this.ConverterClassInitialized = new ConcurrentDictionary<TypeInfo, bool>();
            this.ConstructorAddedClasses = new List<Type>();
            this.Items = new BlockingCollection<Converter>(boundedCapacity: this.Settings.BoundedCapacity);

            if (assemblyNameSpace != null)
            {
                ApplicationNameSpace = assemblyNameSpace.Namespace;
                this.Initialize(ApplicationNameSpace);
            }

            if (numberFormatDefault as NumberFormatInfo != null)
            {
                this.Settings.NumberFormat = numberFormatDefault;
            }

            if (converterClasses != null)
            {
                foreach (var converterClass in converterClasses)
                {
                    if (converterClass != null && converterClass.GetTypeInfo().IsClass == true)
                    {
                        ConstructorAddedClasses.Add(converterClass);

                        this.AddAllAvailableConverters(converterClass.GetTypeInfo());
                    }
                }
            }

            // Load any declared converters from the own assembly
            this.Initialize(this.GetType().GetTypeInfo()?.Assembly.DefinedTypes);
        }

        /// <summary>
        /// References the current class and method that is being loaded.
        /// </summary>
        /// <remarks>Used for fine grained error-reporting during auto-initialize, etc...</remarks>
        public Tuple<TypeInfo, MethodInfo> CurrentConverterByAttribute;

        /// <summary>
        /// Reference to the <see cref="ConverterFactory"/> which facilitates the instantiation of any <see cref="Converter"/> 
        /// </summary>
        public ConverterFactory Factory { get; }

        /// <summary>
        /// Reference to the runtime-creation <see cref="BaseClassFactoryRT"/> which facilitates the instantiation of any <see langword="class"/> 
        /// which declares a converter function and has a <see cref="ConverterAttribute"/>
        /// </summary>
        public BaseClassFactoryRT FactoryBaseClass { get; }

        /// <summary>
        /// Stores the namespace of the application namespace. May be set during auto-initialization loading
        /// </summary>
        public string ApplicationNameSpace { get; set; }

        /// <summary>
        /// Stores a list of classes that were passed into the constructor as a <see crlangword="params"/> list of types and have been added to <see cref="Items"/>
        /// </summary>
        public List<Type> ConstructorAddedClasses { get; }

        /// <summary>
        /// Stores a list of assemblies which have been scanned for attributes and added to <see cref="Items"/>
        /// </summary>
        public ConcurrentDictionary<Assembly, bool> AssemblyInitialized { get; }

        /// <summary>
        /// Stores a list of the types of <see cref="Converter"/> function declaring-classes which have been instantiated and added to <see cref="Items"/>
        /// </summary>
        public ConcurrentDictionary<TypeInfo, bool> ConverterClassInitialized { get; }

        /// <summary>Gets the count of singletons in the list.</summary>
        /// <example>
        /// ```
        /// var singletonManager = new SingletonManager(new[]{typeof(ParentOfAClass), typeof(IndispensibleClass)});
        /// 
        /// singletonManager.PropertyChanged += (singleton, arg) => {
        ///     if(singletonManager.Count &lt; singletonManager.Pool.Count() ){
        ///         Console.WriteLine("A Singleton was added to the Manager");
        ///     }else{
        ///         Console.WriteLine("A Singleton was removed from the Manager");
        ///     }
        /// }
        /// ```
        /// </example>
        public int Count
        {
            get
            {
                return this.Items.Count;
            }

            private set
            {
                if (value != this.count)
                {
                    OnPropertyChanged();
                    this.count = value;
                }
            }
        }

        /// <summary>
        /// The <see cref="BlockingCollection{Converter}"/> class provides a thread-safe collection with full support of the Producer-Consumer pattern, 
        /// to store a collection of <see cref="Converter"/> instances.
        /// </summary>
        public BlockingCollection<Converter> Items { get; }

        /// <summary>
        /// Marks the <see cref="T:System.Collections.Concurrent.BlockingCollection`1"/> instances as not accepting any more additions.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection`1"/> has been disposed.</exception>
        public bool IsAddingCompleted
        {
            get
            {
                return this.Items.IsAddingCompleted;
            }
            set
            {
                if (value == true)
                {
                    this.Items.CompleteAdding();
                }

            }
        }

        /// <summary>
        /// The settings for the <see cref="ConverterCollection"/>.
        /// </summary>
        public ConverterCollectionSettings Settings { get; set; }

        #region Implementation of IQueryable<Converter>

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="System.Linq.IQueryable"/> is executed.
        /// </summary>
        /// <returns>A <see cref="Type"/> that represents the type of the element(s) that are returned  when the expression tree associated with this object is executed.</returns>
        public Type ElementType
        {
            get
            {
                return this.Items.AsQueryable().ElementType;
            }
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="System.Linq.IQueryable"/>.
        /// </summary>
        /// <returns>The <see cref="System.Linq.Expressions.Expression"/> that is associated with this instance of <see cref="System.Linq.IQueryable"/>.</returns>
        public Expression Expression
        {
            get
            {
                return this.Items.AsQueryable().Expression;
            }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns> The <see cref="System.Linq.IQueryProvider"/> that is associated with this data source.</returns>
        public IQueryProvider Provider
        {
            get
            {
                return this.Items.AsQueryable().Provider;
            }
        }

        #endregion

        /// <summary>
        /// Gets the value for a given <paramref name="index"/> using <see cref="System.Linq"/>
        /// </summary>
        /// <param name="index">The integer index of the elements starting at zero.</param>
        /// <returns>A single Converter instance if a <see cref="Type"/> match was found was found, else `null`.</returns>
        public Converter this[int index]
        {
            get
            {
                return this.Items?.Skip(index)?.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a <see cref="Converter"/> for a given Key by invoking <see cref="ConverterCollectionLookup.Get(IQueryable{Converter}, Type, Type)"/>. If none is found, `null` is returned.
        /// </summary>
        /// <param name="typeFrom">The source <see cref="Type"/> to look up in <see cref="Items"/></param>
        /// <param name="typeTo">The target <see cref="Type"/> to look up in <see cref="Items"/></param>
        /// <returns>A single Converter instance if a <see cref="Type"/> match was found was found, else `null`.</returns>
        public Converter this[Type typeFrom, Type typeTo]
        {
            get
            {
                return this.Get(typeFrom.GetTypeInfo(), typeTo.GetTypeInfo());
            }
        }

        /// <summary>
        /// Gets a <see cref="Converter"/> for a given Key by invoking <see cref="ConverterCollectionLookup.Get(IQueryable{Converter}, Type, Type)"/>. If none is found, `null` is returned.
        /// </summary>
        /// <param name="typeFrom">The source <see cref="Type"/> to look up in <see cref="Items"/></param>
        /// <param name="typeTo">The target <see cref="Type"/> to look up in <see cref="Items"/></param>
        /// <returns>A single Converter instance if a <see cref="Type"/> match was found was found, else `null`.</returns>
        public Converter this[TypeInfo typeFrom, TypeInfo typeTo]
        {
            get
            {
                return this.Get(typeFrom, typeTo);
            }
        }


        /// <summary>
        /// Looks up all converters that match the source-type argument <paramref name="typeFrom"/> and returns an <see cref="IEnumerable{Converter}"/>
        /// </summary>
        /// <param name="typeFrom">The source <see cref="Type"/> to look up in <see cref="Items"/></param>
        /// <returns>A IEnumerable of Converter instances if a <see cref="Type"/> match was found was found, else `null`.</returns>
        public IEnumerable<Converter> this[TypeInfo typeFrom]
        {
            get
            {
                return this.WithFrom(typeFrom);
            }
        }

        /// <summary>
        /// Allows to perform a deferred adding operation of multiple adds using a common Base-Class <see cref="Type"/> argument, 
        /// common <see cref="ConverterCollectionSettings"/> as well as a mutual <see cref="CancellationToken"/> for the added group of converters.
        /// The operation is applied upon invoking <see cref="AddBuilder{TBase}.End"/>, and can be explicitly canceled by invoking <see cref="AddBuilder{TBase}.Cancel"/>
        /// </summary>
        /// <typeparam name="TBase">The declaring <see cref="Type"/> of the converter-functions to add as a group.</typeparam>
        /// <param name="settings">Optional common <see cref="ConverterCollectionSettings"/> to be applied for the added group of converter-functions</param>
        /// <param name="cancellationToken">Optional mutual <see cref="CancellationToken"/> to be used for the grouped-adding of converter-functions</param>
        /// <returns>Returns a (nested-class) instance of <see cref="AddBuilder{TBase}"/> with strongly-typed generic methods for deferred grouped-adding.</returns>
        public AddBuilder<TBase> AddStart<TBase>(ConverterCollectionSettings settings = null,
            CancellationToken cancellationToken = default(CancellationToken)) where TBase : class
        {
            return new AddBuilder<TBase>(this, settings: settings, cancellationToken: ref cancellationToken);
        }

        /// <summary>
        /// Allows to perform a deferred adding operation of multiple adds using a common Base-Class <see cref="Type"/> argument, 
        /// common <see cref="ConverterCollectionSettings"/> as well as a mutual <see cref="CancellationToken"/> for the added group of converters.
        /// The operation is applied upon invoking <see cref="AddBuilder{TBase}.End"/>, and can be explicitly canceled by invoking <see cref="AddBuilder{TBase}.Cancel"/>
        /// </summary>
        /// <typeparam name="TBase">The declaring <see cref="Type"/> of the converter-functions to add as a group.</typeparam>
        /// <param name="settings">Common <see cref="ConverterCollectionSettings"/> to be applied for the added group of converter-functions</param>
        /// <param name="cancellationToken">Mutual <see cref="CancellationToken"/> to be used for the grouped-adding of converter-functions</param>
        /// <returns>Returns a (nested-class) instance of <see cref="AddBuilder{TBase}"/> with strongly-typed generic methods for deferred grouped-adding.</returns>
        public AddBuilder<TBase> AddStart<TBase>(ConverterCollectionSettings settings,
            ref CancellationToken cancellationToken) where TBase : class
        {
            return new AddBuilder<TBase>(this, settings: settings, cancellationToken: ref cancellationToken);
        }

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <param name="converterAction">A function delegate <see cref="Func{TIn, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        public IConverterCollection Add<TIn, TOut>(
            Func<TIn, TOut> converterAction,
            Type baseType = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var converter = Factory.Create<TIn, TOut>(converterAction);
            return this.Add(converter: converter, baseType: baseType, cancellationToken: cancellationToken);
        }


        /// <summary>
        /// Creates and adds a pair of <see cref="Converter"/> instances to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>, 
        /// which together form an _Inter-converter_, for mutual conversion between two types in either conversion direction.
        /// </summary>
        /// <param name="converterActionForward">A source-to-target <see cref="Type"/> converting function delegate <see cref="Func{TIn, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="converterActionBackward">A target-to-source <see cref="Type"/> converting function delegate <see cref="Func{TOut, TIn}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        public IConverterCollection Add<TIn, TOut>(
            Func<TIn, TOut> converterActionForward,
            Func<TOut, TIn> converterActionBackward,
            Type baseType = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            this.Add(converterAction: converterActionForward, baseType: baseType, cancellationToken: cancellationToken);
            return this.Add(converterAction: converterActionBackward, baseType: baseType, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TBase">The <see cref="Type"/> of the declaring and attributed custom converter class</typeparam>
        /// <param name="converterAction">A function delegate <see cref="Func{TIn, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        public IConverterCollection Add<TIn, TOut, TBase>(Func<TIn, TOut> converterAction, CancellationToken cancellationToken = default(CancellationToken))
            where TBase : class
        {
            var converter = Factory.Create<TIn, TOut>(converterAction);
            return this.Add(converter: converter, baseType: typeof(TBase), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TBase">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</typeparam>
        /// <param name="converterActionDefault">A function delegate <see cref="Func{TIn, TOut, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        public IConverterCollection Add<TIn, TOut, TBase>(
            Func<TIn, TOut, TOut> converterActionDefault,
            CancellationToken cancellationToken = default(CancellationToken)) where TBase : class
        {
            var converter = Factory.Create<TIn, TOut>(converterActionDefault);
            return this.Add(converter: converter, baseType: typeof(TBase), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <param name="converterActionAny">A function delegate <see cref="Func{TIn, TArg, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TArg">The Argument <see cref="Type"/> for generic converters using see <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object, bool)"/>.</typeparam>
        /// <typeparam name="TBase">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</typeparam>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        public IConverterCollection Add<TIn, TArg, TOut, TBase>(
            Func<TIn, TArg, TOut> converterActionAny,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (typeof(TOut) == typeof(TArg))
            {
                var converter = Factory.Create<TIn, TOut>((Func<TIn, TOut, TOut>)(object)converterActionAny);
                return this.Add(converter: converter, baseType: typeof(TBase), cancellationToken: cancellationToken);
            }
            return this.Add(converterDelegate: converterActionAny, baseType: typeof(TBase), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <param name="converterActionAny">A function delegate <see cref="Func{TIn, TArg, TOut}"/> to use as the <see cref="Converter.Function"/></param>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <typeparam name="TIn">The Source- / From- <see cref="Type"/>from which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TOut">The Target / To- <see cref="Type"/> to which to <see cref="Converter.Convert(object,object)"/></typeparam>
        /// <typeparam name="TArg">The Argument <see cref="Type"/> for generic converters using see <see cref="ObjectExtension.ConvertTo{TIn, TOut}(TIn, object, bool)"/>.</typeparam>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        public IConverterCollection Add<TIn, TArg, TOut>(
            Func<TIn, TArg, TOut> converterActionAny,
            Type baseType = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (typeof(TOut) == typeof(TArg))
            {
                var converter = Factory.Create<TIn, TOut>((Func<TIn, TOut, TOut>)(object)converterActionAny);
                return this.Add(converter: converter, baseType: baseType, cancellationToken: cancellationToken);
            }
            return this.Add(converterDelegate: converterActionAny, baseType: baseType, cancellationToken: cancellationToken);
        }


        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <param name="converterDelegate">An optional function delegate to use as the <see cref="Converter.Function"/></param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        public IConverterCollection Add<TBase>(object converterDelegate, CancellationToken cancellationToken = default(CancellationToken)) where TBase : class
        {
            return this.Add(converterDelegate: converterDelegate, baseType: typeof(TBase), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="Delegate"/>
        /// </summary>
        /// <param name="converterDelegate">An optional function delegate to use as the <see cref="Converter.Function"/></param>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <example> **Example:** Add a converter to the ConverterCollection `cc`, to converter `Nullable&lt;int>` to float 
        /// <code>```cs
        ///         var cc = new ConverterCollection(typeof(Program));
        ///         // Converter: [Nullable&lt;int>] --> [float]
        ///         cc.Add((int? a) =>
        ///         {
        ///             return (float)(a ?? 0);
        ///         });
        /// ```</code>
        /// </example>
        /// <remarks> Any lambda function or strictly declared function can be added as <paramref name="converterDelegate"/>. For more information visit the MSDN recommendations on 
        /// lambda expressions: https://msdn.microsoft.com/en-us/library/bb397687.aspx</remarks>
        /// <exception cref="ConverterException"> Throws a <see cref="ConverterCause.ConverterCollectionAddFailed"/> with a wrapped <see cref="Exception.InnerException"/>
        /// if an internal error occurred</exception>
        public IConverterCollection Add(object converterDelegate, Type baseType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (converterDelegate == null)
            {
                throw new ConverterCollectionException(ConverterCollectionCause.ConverterArgumentNull, $"{nameof(converterDelegate)} must not be null and of Type Delegate");
            }
            if ((converterDelegate is Delegate) == false)
            {
                throw new ConverterCollectionException(ConverterCollectionCause.ConverterArgumentWrongType, $"{nameof(converterDelegate)} must be a `Delegate` Type");
            }

            if (baseType == null)
            {
                baseType = ((Delegate)converterDelegate).Target.GetType().DeclaringType;
            }

            var methodInfo = ((Delegate)converterDelegate).GetMethodInfo(); //type.GetDeclaredMethod("Invoke");
            return this.Add(methodInfo: methodInfo, converterDelegate: converterDelegate, baseType: baseType, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates and adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/> using a <see cref="MethodInfo"/> instance
        /// </summary>
        /// <param name="methodInfo">A <see cref="MethodInfo"/> instance describing a method to create a delegate from and add to the converter</param>
        /// <param name="converterDelegate">An optional function delegate to use as the <see cref="Converter.Function"/></param>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="baseInstance">An optional base-class instance containing a method which is added to the converter and serves as `this` reference during invocation.</param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterException">Throws an exception if adding failed</exception>
        public IConverterCollection Add(MethodInfo methodInfo, object converterDelegate = null, Type baseType = null, object baseInstance = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            var converter = this.Factory.Create(methodInfo, converterDelegate);

            if (converter != null)
            {
                converter.Base = baseInstance;
                return this.Add(converter: converter, baseType: baseType, cancellationToken: cancellationToken);
            }
            else
            {
                throw new ConverterException(ConverterCause.ConverterCollectionAddFailed);
            }
        }

        /// <summary>
        /// Adds a <see cref="Converter"/> instance to the collection of <see cref="ConverterCollection.Items"/>
        /// </summary>
        /// <param name="converter">The <see cref="Converter"/> instance to add to the <see cref="ConverterCollection.Items"/></param>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class, if one exists</param>
        /// <param name="allowDisambiguates">Optional <see langword="bool"/> value to indicate whether to allow multiple converters with the 
        /// same return and parameter <see cref="Type"/>s in the collection of <see cref="Items"/>. Only set to `true` for transform functions.</param>
        /// <param name="cancellationToken">Optional token to propagate notification that operations should be canceled. From <see cref="System.Threading.Tasks"/>.</param>
        /// <returns>Returns a <see cref="IConverterCollection"/> for safe, constricted function chaining.</returns>
        /// <exception cref="ConverterCollectionException">Throws an exception if adding failed</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
        public IConverterCollection Add(Converter converter, Type baseType = null, bool allowDisambiguates = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Add references for detailed error-messages
            try
            {
                var function = (converter?.FunctionDefault ?? converter?.Function) as Delegate;
                var functionInfo = function?.GetMethodInfo();
                CurrentConverterByAttribute = new Tuple<TypeInfo, MethodInfo>(functionInfo?.DeclaringType.GetTypeInfo(), functionInfo);
            }
            // Not significant since CurrentConverterByAttribute itself is chiefly used for improving exception messages
            catch (Exception) { }

            if (converter == null)
            {
                throw new ConverterCollectionException(ConverterCollectionCause.ConverterArgumentNull, GetInitializeFailedErrorMessage());
            }

            if (converter.HasFunction == false)
            {
                throw new ConverterCollectionException(ConverterCollectionCause.ConverterFunctionsNull, GetInitializeFailedErrorMessage());
            }

            try
            {
                converter.SetCollectionDefaults(this);

                if (allowDisambiguates == false && baseType?.GetTypeInfo().BaseType == typeof(MulticastDelegate))
                {
                    converter.AllowDisambiguates = true;
                }
                // if there is already a standard converter try to merge them. The condition is that the converter argument type match or is not used
                if (converter.Standard)
                {
                    try
                    {
                        Converter converterLookup = null;
                        if (converter.Argument.AsType() == typeof(object))
                        {
                            converterLookup = this.Get(typeFrom: converter.From, typeTo: converter.To, typeArgument: converter.To, loadOnDemand: false, isStandard: true);
                        }
                        else
                        {
                            converterLookup = this.Get(typeFrom: converter.From, typeTo: converter.To, typeArgument: typeof(object).GetTypeInfo(), loadOnDemand: false, isStandard: true);
                        }

                        if (converterLookup?.MergeStandard(converter) == true)
                        {
                            return this;
                        }
                    }
                    catch (ConverterCollectionException exc)
                    {
                        throw new ConverterCollectionException(ConverterCollectionCause.AddFailed, exc, GetInitializeFailedErrorMessage(exc));
                    }
                }
                // update the base-type and thus the attribute access of the converter
                if (baseType != null)
                {
                    converter.SetBaseType(baseType);
                }

                if (this.Items.TryAdd(converter, -1, cancellationToken) == true)
                {
                    OnPropertyChanged(converter);
                }
            }
            catch (ConverterCollectionException)
            {
                throw;
            }
            catch (ConverterException)
            {
                throw;
            }
            catch (Exception exc)
            {
                throw new ConverterException(ConverterCause.ConverterCollectionAddFailed, exc, GetInitializeFailedErrorMessage(exc));
            }

            return this;
        }

        /// <summary>
        /// Returns <see langword="bool"/>  `true` if the <see cref="ConverterCollection"/> supports conversion for a given Type <paramref name="typeFrom"/>, else `false`.
        /// </summary>
        /// <param name="typeFrom">The source <see cref="Type"/> to look up in <see cref="Items"/></param>
        /// <remarks>The function is a wrapper for <see cref="ConverterCollectionFilters.WithFrom(IQueryable{Converter}, TypeInfo, bool)"/>.</remarks>
        /// <returns>`true` if the source <see cref="Type"/> is supported as source type by any converter in the <see cref="ConverterCollection"/>, else `false`</returns>
        public bool CanConvertFrom(Type typeFrom)
        {
            return this.WithFrom(typeFrom.GetTypeInfo()).FirstOrDefault() != null;
        }

        /// <summary>
        /// Returns <see langword="bool"/>  `true` if the <see cref="ConverterCollection"/> supports conversion for a given Type <typeparamref name="TIn"/>, else `false`.
        /// </summary>
        /// <typeparam name="TIn">The source <see cref="Type"/> to look up in <see cref="Items"/></typeparam>
        /// <remarks>The function is a wrapper for <see cref="ConverterCollectionFilters.WithFrom(IQueryable{Converter}, TypeInfo, bool)"/>.</remarks>
        /// <returns>`true` if the source <see cref="Type"/> is supported as source type by any converter in the <see cref="ConverterCollection"/>, else `false`</returns>
        public bool CanConvertFrom<TIn>()
        {
            return this.WithFrom(typeof(TIn).GetTypeInfo()).FirstOrDefault() != null;
        }

        /// <summary>
        /// Returns <see langword="bool"/>  `true` if the <see cref="ConverterCollection"/> supports conversion for a given Type <paramref name="typeTo"/>, else `false`.
        /// </summary>
        /// <param name="typeTo">The target <see cref="Type"/> to look up in <see cref="Items"/></param>
        /// <remarks>The function is a wrapper for <see cref="ConverterCollectionFilters.WithTo(IQueryable{Converter}, TypeInfo, bool)"/>.</remarks>
        /// <returns>`true` if the target <see cref="Type"/> is supported as source type by any converter in the <see cref="ConverterCollection"/>, else `false`</returns>
        public bool CanConvertTo(Type typeTo)
        {
            return this.WithTo(typeTo.GetTypeInfo()).FirstOrDefault() != null;
        }

        /// <summary>
        /// Returns <see langword="bool"/>  `true` if the <see cref="ConverterCollection"/> supports conversion for a given Type <typeparamref name="TOut"/>, else `false`.
        /// </summary>
        /// <typeparam name="TOut">The target <see cref="Type"/> to look up in <see cref="Items"/></typeparam>
        /// <remarks>The function is a wrapper for <see cref="ConverterCollectionFilters.WithTo(IQueryable{Converter}, TypeInfo, bool)"/>.</remarks>
        /// <returns>`true` if the target <see cref="Type"/> is supported as source type by any converter in the <see cref="ConverterCollection"/>, else `false`</returns>
        public bool CanConvertTo<TOut>()
        {
            return this.WithTo(typeof(TOut).GetTypeInfo()).FirstOrDefault() != null;
        }

        /// <summary>
        /// Adds or updates an attribute for a <see cref="Converter"/> if it exists, dependent only on the source and target conversion types
        /// </summary>
        /// <param name="baseType">The <see cref="Type"/> of the declaring and attributed custom converter class <see cref="Type"/> if one exists</param>
        /// <param name="attribute">The <see cref="ConverterAttribute"/> of the converter-class that is to be updated</param>
        /// <param name="update">Whether to update an existing <see cref="Converter.Attribute"/></param>
        /// <returns>Returns the updated <paramref name="attribute"/>, if <paramref name="update"/> was set to `true`</returns>
        public ConverterAttribute ConverterAttributeFromIConverter(TypeInfo baseType, ConverterAttribute attribute = null, bool update = false)
        {
            if (baseType == null || (attribute != null && update == false))
            {
                return attribute;
            }

            attribute = attribute ?? new ConverterAttribute();

            attribute.BaseType = baseType;
            // look up a possible converter candidate with generic IConverter<,> support
            var converter = this.Get(baseType.AsType());
            if (converter != null && (converter.Attribute == null || converter.Attribute != null && update == true))
            {
                converter.Attribute = attribute;
            }

            return attribute;
        }

        /// <summary>
        /// Tries to lookup the namespace obtained from <paramref name="typeTo"/> in <see cref="loadOnDemandConverters"/> and add the converters.
        /// If successful the number of Added Converters are returned, and the NameSpace is removed from the list of <see cref="loadOnDemandConverters"/>.
        /// </summary>
        /// <param name="typeTo"> The converter target <see cref="Type"/> to obtain the namespace from.</param>
        /// <remarks>Any compatible <see cref="Converter.Standard"/> converters will have merged after loading, as such  the returned number of loaded 
        /// converters does not have a direct relationship to the <see cref="Count"/>
        /// </remarks>
        /// <returns>Returns <see langword="bool"/> an integer number > `0` if the Converters of the namespace from <paramref name="typeTo"/> were successfully added, otherwise returns `0`</returns>
        public int LoadOnDemandConverter(Type typeTo)
        {
            var nameSpace = typeTo?.Namespace.Split('.').Last();
            if (nameSpace != null && this.loadOnDemandConverters.ContainsKey(nameSpace))
            {
                var loaded = this.loadOnDemandConverters[nameSpace].Select(this.CreateConverterClassInstance).Count();
                this.loadOnDemandConverters.Remove(nameSpace);
                return loaded;
            }
            return 0;
        }

        /// <summary>
        /// Provides details to the class and method where an <see cref="ConverterException"/> was thrown.
        /// </summary>
        /// <param name="exc"> An optional exception of the currently caught exception, from which to extract an <see cref="Exception.InnerException"/> message.</param>
        /// <returns>Astring with detail of the class and method at which any Exception occurred during the use of Initialize.</returns>
        private static string GetInitializeFailedErrorMessage(Exception exc = null)
        {
            var currentConverterByAttribute = CurrentInstance.CurrentConverterByAttribute;
            if (currentConverterByAttribute == null)
            {
                return null;
            }
            var innerMessage = exc?.InnerException?.InnerException?.Message ?? exc?.InnerException?.Message ?? exc?.Message;
            // check if the method has an attribute
            if (currentConverterByAttribute.Item2.GetCustomAttribute<ConverterMethodAttribute>() != null)
            {
                return
                    $"Please remove the {nameof(ConverterMethodAttribute)} from method '{currentConverterByAttribute.Item2?.Name}', in the class {currentConverterByAttribute.Item1?.Name}: '{innerMessage}'";
            }
            else
            {
                return
                    $"Please remove the {nameof(Add)} declaration of the method '{currentConverterByAttribute.Item2?.Name}', in the declaring class {currentConverterByAttribute.Item1?.Name}: '{innerMessage}'";
            }
        }

        /// <summary>
        /// Checks if the <see cref="ConverterCollection"/> is initialized. Attempts to initialize and load the user-assembly if <see cref="Singleton{ConverterCollection}.Initialized"/> is `false`
        /// </summary>
        /// <remarks>
        /// see also: http://stackoverflow.com/questions/35655726/system-reflection-assembly-does-not-contain-a-definition-for-getexecutingassemb/39192534#39192534
        /// </remarks>
        /// <exception cref="ConverterException"> If the Reflection required for the Initialization failed an exception is caused of 
        /// exception-cause: <see cref="ConverterCause.ConverterAutoInitializationFailed"/></exception>
        public static void AutoInitialize()
        {
            // is the ConverterCollection Initialized?
            if (AutoInitialized == false && CurrentInstance.Settings.AutoInitialize == true)
            {
                try
                {
                    var getEntryAssembly = typeof(Assembly).GetRuntimeMethods().FirstOrDefault(m => m.Name.Equals("GetEntryAssembly"));
                    if (getEntryAssembly != null)
                    {
                        var assembly = getEntryAssembly.Invoke(null, null) as Assembly;
                        CurrentInstance.Initialize(assembly);
                        CurrentInstance.ApplicationNameSpace = assembly?.GetNamespacesByLevel(0)?.FirstOrDefault();
                    }
                    // set as initialized regardless of success, to run only once
                    AutoInitialized = true;
                }
                catch (Exception exc)
                {
                    throw new ConverterException(ConverterCause.ConverterAutoInitializationFailed, exc, GetInitializeFailedErrorMessage(exc));
                }
            }
        }

        /// <summary>
        /// Initializes all attributed Converter classes, <see cref="Initialize(Assembly[])"/>
        /// </summary>
        /// <param name="applicationNameSpace">
        /// The class that holds the entry point of the application. Usually `public static Main(){...}`
        /// </param>
        /// <example> **Example:**
        /// ```
        ///     ...    
        ///     var ConverterManager = new ConverterManager();
        ///     ConverterManager.Initialize(typeof(Program));
        /// ```
        /// </example>
        public void Initialize(string applicationNameSpace)
        {
            try
            {
                var assembly = Assembly.Load(new AssemblyName(applicationNameSpace));
                var types = assembly.DefinedTypes;
                this.Initialize(types);
            }
            catch (FileNotFoundException exc)
            {
                throw new ConverterCollectionException(ConverterCollectionCause.AssemblyFileNotFound, exc, new AssemblyName(applicationNameSpace).FullName);
            }
        }

        /// <summary>
        /// Initializes all attributed Converter classes, <see cref="Initialize(Assembly[])"/>
        /// </summary>
        /// <param name="applicationClass">
        /// The class that holds the entry point of the application. Usually `public static Main(){...}`
        /// </param>
        /// <example> **Example:**
        /// ```
        ///     ...    
        ///     var ConverterManager = new ConverterManager();
        ///     ConverterManager.Initialize(typeof(Program));
        /// ```
        /// </example>
        public void Initialize(Type applicationClass)
        {
            if (applicationClass != null)
            {
                var assembly = applicationClass.GetTypeInfo().Assembly;
                this.Initialize(new[] { assembly });
            }
        }

        /// <summary>
        ///  Initializes all attributed Converter classes, <see cref="Initialize(Assembly[])"/>
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to look for attributed Converter classes</param>
        /// <example> **Example:**
        /// ```
        ///     ...    
        ///     var ConverterManager = new ConverterManager();
        ///     ConverterManager.Initialize(typeof(Program).Assembly);
        /// ```
        /// </example>
        public void Initialize(Assembly assembly)
        {
            this.Initialize(new[] { assembly });
        }

        /// <summary>
        /// Initializes all attributed Converter classes, unless the <see cref="ConverterAttribute"/> parameter <see cref="ConverterAttribute.LoadOnDemand"/> is `false`
        /// </summary>
        /// <param name="assemblies">An  <see cref="Array"/> of <see cref="Assembly"/> to look for <see cref="ConverterAttribute"/> to discover and add 
        /// converters into the collection</param>
        /// <example> **Example:**
        /// ```
        ///     ...   
        ///     var ConverterManager = new ConverterManager();
        ///     ConverterManager.Initialize(AppDomain.CurrentDomain.GetAssemblies());
        /// ```
        /// </example>
        public void Initialize(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                if (assembly == null || this.AssemblyInitialized.ContainsKey(assembly) == true)
                {
                    continue;
                }

                var types = assembly.GetAtributedTypes<ConverterAttribute>().ToArray();
                this.Initialize(types);
            }
        }

        /// <summary>
        /// Initializes all attributed Converter classes, unless the <see cref="ConverterAttribute"/> parameter <see cref="ConverterAttribute.LoadOnDemand"/> is `false`
        /// </summary>
        /// <param name="types">The <see cref="Array"/> of <see cref="Assembly"/> within to look for attributed Converter classes</param>
        /// <example> **Example:**
        /// ```
        ///     ...   
        ///     var ConverterManager = new ConverterManager();
        ///     ConverterManager.Initialize(AppDomain.CurrentDomain.GetAssemblies());
        /// ```
        /// </example>
        public void Initialize(IEnumerable<TypeInfo> types)
        {
            foreach (var type in types)
            {
                try
                {
                    this.AddAllAvailableConverters(type);
                }
                catch (Exception exc)
                {
                    throw new ConverterException(ConverterCause.ConverterInitializationFailed, exc, GetInitializeFailedErrorMessage(exc));
                }
            }
        }

        /// <summary>
        /// Dispose of the <see cref="Items"/> and underlying static <see cref="Singleton{TClass}"/> references.
        /// </summary>
        /// <param name="disposing">The state of the object disposal, to avoid unnecessary invocations during the disposing invocation.</param>
        /// <remarks>Invocation is only required in a few rare cases</remarks>
        /// <remarks>warning CA2213 can be ignored: http://stackoverflow.com/questions/34583417/code-analysis-warning-ca2213-call-dispose-on-idisposable-backing-field </remarks>
        /// <seealso cref="Singleton{TClass}.Disposed"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<Items>k__BackingField")]
        protected override void Dispose(bool disposing)
        {
            this.Items.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Sets the <see cref="Assembly"/> as initialized in <see cref="AssemblyInitialized"/>
        /// </summary>
        /// <param name="type">The <see cref="TypeInfo"/> instance from which to lookup the <see cref="TypeInfo.Assembly"/></param>
        /// <returns>Returns true if the concurrent dictionary update succeeded</returns>
        protected bool SetAssemblyInitialized(TypeInfo type)
        {
            return this.AssemblyInitialized.AddOrUpdate(type.Assembly, true, (oldkey, oldval) => true);
        }

        /// <summary>
        /// Sets the <see cref="Converter"/> of the <see cref="ConverterCollection"/> as initialized
        /// </summary>
        /// <param name="converterClass">The <see langword="class"/> instance of a custom converter implementation that is to be marked initialized</param>
        /// <returns>Returns `true` if the concurrent dictionary update succeeded</returns>
        /// <remarks>The class can support <see cref="IConverter"/> </remarks>
        protected bool SetConverterClassInitialized(TypeInfo converterClass)
        {
            return this.ConverterClassInitialized.AddOrUpdate(converterClass, true, (oldkey, oldval) => true);
        }

        /// <summary>
        /// Creates a custom converter instance
        /// </summary>
        /// <param name="converterClass">The <see langword="class"/> instance of a custom converter implementation that is to be marked initialized</param>
        /// <returns>Returns an instance of the <see langword="class"/> type argument provided. `Null` if unsuccessful.</returns>
        /// <remarks>Use <see cref="AddAllAvailableConverters"/> to instantiate a custom converter with regard to the <see cref="ConverterAttribute"/> properties</remarks>
        private object CreateConverterClassInstance(Type converterClass)
        {
            object customConverter;

            customConverter = this.FactoryBaseClass.Create(converterClass, this);
            this.SetConverterClassInitialized(converterClass.GetTypeInfo());

            return customConverter;
        }

        /// <summary>
        /// Looks for classes with a <see cref="ConverterAttribute"/> and Dependency Injectable constructors as well as classes which have an <see cref="IConverter{TIn,TOut}"/> interface.
        /// </summary>
        /// <param name="type">The <see cref="TypeInfo"/> instance from which to lookup the <see cref="TypeInfo.Assembly"/></param>
        /// <returns>Returns `true` if the type had a <see cref="ConverterAttribute"/> which could be added to the collection</returns>
        /// <remarks>Use the <see cref="CreateConverterClassInstance"/> method to instantiate a custom converter type without regard towards the <see cref="ConverterAttribute"/> properties</remarks>
        private bool AddAllAvailableConverters(TypeInfo type)
        {
            int count = this.Count;
            object converterCustom = null;

            if (ConstructorAddedClasses?.Contains(type.AsType()) == false)
            {
                var attribute = type.GetCustomAttribute<ConverterAttribute>();
                if (attribute == null)
                {
                    if (type.IsClass == true && type.ImplementedInterfaces.Contains(typeof(IConverter)) == true)
                    {
                        attribute = this.ConverterAttributeFromIConverter(type, attribute, update: true);
                    }
                    // the type is not a converter, let's return
                    return false;
                }

                if (attribute?.LoadOnDemand == true)
                {
                    if (this.loadOnDemandConverters?.ContainsKey(attribute.NameSpace) == false)
                    {
                        this.loadOnDemandConverters.Add(attribute.NameSpace, new List<Type>());
                    }

                    if (this.loadOnDemandConverters[attribute.NameSpace]?.Contains(type.AsType()) == false)
                    {
                        this.loadOnDemandConverters[attribute.NameSpace].Add(type.AsType());
                    }

                    return false;
                }

                if (attribute?.DependencyInjection == true)
                {
                    converterCustom = this.CreateConverterClassInstance(type.AsType());
                }
            }
            converterCustom = this.AddAllConvertersByAttribute(type, converterCustom);

            this.SetAssemblyInitialized(type);
            return count != this.Count;
        }

        /// <summary>
        /// Looks for a <see cref="ConverterAttribute"/> from the passed <see cref="Type"/> <paramref name="type"/> to discover and add converters into the collection
        /// </summary>
        /// <param name="type">The <see cref="TypeInfo"/> of a class in which to look for methods which habe an <see cref="ConverterMethodAttribute"/>.</param>
        /// <param name="converterCustom">A pre-existing instance of the type defined in <paramref name="type"/> for reuse.</param>
        /// <returns>Returns `true` if the type had a <see cref="ConverterAttribute"/> which could be added to the collection</returns>
        /// <remarks>Use the <see cref="CreateConverterClassInstance"/> method to instantiate a custom converter type without regard towards the <see cref="ConverterAttribute"/> properties</remarks>
        private object AddAllConvertersByAttribute(TypeInfo type, object converterCustom = null)
        {

            // discover attributed methods
            foreach (var declaredMethod in type.DeclaredMethods)
            {
                CurrentConverterByAttribute = new Tuple<TypeInfo, MethodInfo>(type, declaredMethod);
                var customAttribute = declaredMethod.GetCustomAttribute<ConverterMethodAttribute>() as ConverterMethodAttribute;
                if (customAttribute != null)
                {
                    var declaredMethodParams = declaredMethod.GetParameters();

                    // create a wrapper taking the own class-instance as first argument for methods that are attributed by `ConverterMethod`:
                    if (

                        // the method has no arguments, thus no input-type yet has a ConverterMethod attribute or has the specific attribute property `PassInstance` set to `true`
                        ((declaredMethod.IsStatic == false && declaredMethodParams.Length == 0) || customAttribute.PassInstance == true) ||

                        // the first argument is already of the containing class, e.g. static implicit or explicit operators
                        (declaredMethod.IsStatic == true && declaredMethodParams.Length == 1 && declaredMethodParams.First().ParameterType.GetTypeInfo() == type) ||

                        // the method is declared in a class requiring parameters to instantiate, which implies that the method requires a specific class instance
                        ((declaredMethod.IsStatic == false && declaredMethodParams.Length == 1) && type.GetConstructorWithoutParameters() == null))
                    {
                        Converter converter = this.Factory.CreateWrapper(type, declaredMethod);
                        this.Add(converter: converter, baseType: type.AsType());
                    }
                    else
                    {
                        if (declaredMethod.IsStatic == false && converterCustom == null)
                        {
                            converterCustom = this.CreateConverterClassInstance(type.AsType());
                        }
                        this.Add(methodInfo: declaredMethod, baseType: type.AsType(), baseInstance: converterCustom);
                    }
                }
            }

            return converterCustom;
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Converter> GetEnumerator()
        {
            var enumerator = this.Items as IEnumerable<Converter>;//.GetConsumingEnumerable();
            return enumerator.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Items).GetEnumerator();
        }

        #endregion
    }
}