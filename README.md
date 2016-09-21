
<img src="https://googledrive.com/host/0ByqWUM5YoR35QUxZY3hrTDZTWGs/type-converter-big.png" align="right" height="84" />

## Portable TypeCast -  <small>Cast, Convert & Transform without compromises.</small>    
##### &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Intuitive, easy-to-use, portable, tested, documented, serializable, thread-safe, strongly typed,...

---
**author** | <a href="http://stackexchange.com/users/485574/lo-sauer"><img src="http://stackexchange.com/users/flair/485574.png" width="208" height="58" alt="profile for Lo Sauer on Stack Exchange, a network of free, community-driven Q&amp;A sites" title="profile for Lo Sauer on Stack Exchange, a network of free, community-driven Q&amp;A sites" /></a>
:------------ | :------------- 
**website** | <a href="https://github.com/lsauer/dotnet-portable-cast-convert-transform" target="_blank">https://github.com/lsauer/dotnet-portable-cast-convert-transform</a>
**license** | <a href="http://lsauer.mit-license.org/" target="_blank">MIT license</a>   
**package** | `PM> Install-Package Core.TypeCast`  
**description** | An easy to use, portable library for changing between unrestricted, arbitrary data types
**documentation** |  <a href="https://dotnet-cast-convert-transform.firebaseapp.com/">complete reference v3.1.0.2</a>
**supported** | <ul><li><a href="https://msdn.microsoft.com/en-us/library/gg597391(v=vs.110).aspx" target="_blank">Portable Class Library (PLC)</a></li> <li><a href="https://en.wikipedia.org/wiki/.NET_Framework#.NET_Core" target="_blank">.NET Core</a></li> <li>.NET Framework 4.5</li> <li>Windows Phone 8.1</li>  <li>Windows 8, 10</li> <li><a href="https://developer.xamarin.com/guides/android/" target="_blank">Xamarin.Android</a></li> <li><a href="https://developer.xamarin.com/guides/ios/" target="_blank">Xamarin.iOS</a></li>  <li>Xamarin.iOS Classic</li><li>XBOX 360 XNA (req. adaptations)</li></ul>   


## Table of Contents

 - [Features at a glance](#features-at-a-glance)
 - [Download](#download)
 - [Install and Requirements](#install-and-requirements)
 - [Key Features](#key-features)
 - [Documentation](#documentation)
 - [Getting started in 4 steps](#getting-started-in-4-steps)
 - [Code Glance](#code-glance)
 - [Background](#background)
 - [What to do Next](#what-to-do-next)
 - [Definitions](#definitions)
 - [Features in Detail](#features-in-detail)
 - [Why this Project exists](#why-this-project-exists)
 - [Practical Code Example](#practical-code-example)
 - [Using this library](#using-this-library)
 - [Using ConverterCollection](#using-convertercollection)
 - [Using the ConvertContext](#using-the-convertcontext)
 - [LINQ Query](#linq-query)
 - [Best Practices](#best-practices)
 - [Other projects](#other-projects)
 - [Contact & Contracted Customization](#contact--contracted-customization)


### Download

Full Version | NuGet | NuGet Install
------------ | :-------------: | :-------------:
Core.TypeCast | <a href="https://www.nuget.org/packages/Core.TypeCast/" target="_blank"><img src="https://img.shields.io/nuget/v/Core.TypeCast.svg?maxAge=2592000"/></a> | ```PM> Install-Package Core.TypeCast```

Social:  [![Twitter](https://img.shields.io/twitter/url/https/github.com/lsauer/csharp-singleton.svg?style=social)](https://twitter.com/intent/tweet?text=Wow:&url=%5Bobject%20Object%5D) <a href="https://twitter.com/sauerlo/" target="_blank"><img src="https://googledrive.com/host/0ByqWUM5YoR35NGZiSEs4SXduTGM/gh_twitter_like.png" alt="Twitter Follow" height="18" /></a>
<a href="https://www.facebook.com/lorenz.lo.sauer/" target="_blank"><img src="https://googledrive.com/host/0ByqWUM5YoR35NGZiSEs4SXduTGM/gh_facebook_like.png" alt="Facebook Like" height="18" /></a>


### Install and Requirements
In order to use this library, your application needs .NET Framework 4.5 or higher. Install via two ways:
* Via Nuget: ```Install-Package Core.TypeCast```
* Via Github: ```git clone https://github.com/lsauer/dotnet-portable-cast-convert-transform.git```


### Key Features

- Central thread-safe pool of converting functions
- Data Encapsulation
- Add converters at runtime or compile-time
- Consistent exception behavior
- Optional functions following the "Try" convention of .NET 
- Instant improvement of source-code readability and maintainability
- Low overall performance impact
- Little to no learning curve 
- Independent, portable library
- Well tested, small NuGet package

### Documentation

Please visit <a href="https://dotnet-cast-convert-transform.firebaseapp.com/" target="_blank">here for a complete reference</a>, 
which is also included in the <a href="https://www.nuget.org/packages/Core.TypeCast/" target="_blank">NuGet package</a>.

### Getting started in 4 steps

1. *Install* with the <a href="http://goo.gl/iekUWH" target="_blank">NuGet Package manager</a>: `PM> Install-Package Core.TypeCast`.  
2. *Add the namespace* to your top-listed using directives: `using Core.TypeCast;`.
3. *Create a class* with one or more methods: `class MyConverter { ... int MyConverter(string argument) ... }`
4. *Attribute* any class with `[ConverterAttribute]`: _`public class MyConverter { ... }`_.    
    Subsequently, attribute the converting methods using `[ConverterMethodAttribute]`: _`public int MyConverter(string attribute){ ... }`__
5. **Done!** 

Now, invoke conversions in your code anywhere as follows: 
```cs
    var castedInt = "500s".CastTo<int>();
    var protein = "GAGTGCGCCCTCCCCGCACATGCGCCCTGACAGCCCAACAATGGCGGCGCCCGCGGAGTC".ConvertTo<IProtein>();
```
Use library functions which suit the change of type descriptively:

    `var complimentary = "GAGTGCGCCCTCCCCGCACATGCGCCCTGACAGCCCAACAATGGCGGCGCCCGCGGAGTC".Transform<Complementary>();`


### Code Glance

Provided below is an abbreviated example of what code may look like in your project: 

```cs
    using System.Runtime.CompilerServices;
    // IPolyNucleotide.cs
    public interface IPolyNucleotide { ... }

    // used for "Tranform-Aliasing"
    delegate DNA Complimentary(string dnaSequence, AModelClass arguments);

    // DNA.cs
    [Converter]
    public class DNA : IPolyNucleotide
    {
        [ConverterMethod]
        protected IProtein ToProtein(string dnaSequence, bool homologyLookup = false)
        {
            ... ...
        }

        [ConverterMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DNA Complimentary(string dnaSequence, AModelClass arguments)
        {
            ... ...
        }
        ...
    }
```

### Background

1. The .NET framework lacks a converter class that can be used flexibly for most applications, ranging from mobile to desktop and to servers. 
The implemented Converter functionality of `System.ComponentModel` is disliked among developers and overly complicated as is outlined in a code-excerpt
from MSDN for implementing a `System.String` to `System.Drawing.Point` converter:

```cs
    // example adapted from MSDN
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Drawing;

    public class PointConverter : TypeConverter {
       public override bool CanConvertFrom(ITypeDescriptorContext context, 
          Type sourceType) {
      
          if (sourceType == typeof(string)) {
             return true;
          }
          return base.CanConvertFrom(context, sourceType);
       }
       public override object ConvertFrom(ITypeDescriptorContext context, 
          CultureInfo culture, object value) {
          if (value is string) {
             string[] v = ((string)value).Split(new char[] {','});
             return new Point(int.Parse(v[0]), int.Parse(v[1]));
          }
          return base.ConvertFrom(context, culture, value);
       }
       public override object ConvertTo(ITypeDescriptorContext context, 
          CultureInfo culture, object value, Type destinationType) {  
          if (destinationType == typeof(string)) {
             return ((Point)value).X + "," + ((Point)value).Y;
          }
          return base.ConvertTo(context, culture, value, destinationType);
       }
    }
```

2. Additionally, `System.ComponentModel.TypeConverter` is absent in `Portable Class Libraries` (PCLs), with no immediate
portable alternative class filling in for the missing functionality.

3. By using this library in C# 6 parlance, the aforementioned example of a `String` to `Point` *Interconverter*, may be reduced to a few lines of code:

```cs
    using System.Drawing;

    ConverterCollection.CurrentInstance
                       .Add<string, Point>(s => string.IsNullOrWhiteSpace(s) 
                            ? default(Point)
                            : new Point(int.Parse(s?.Split(',').First()), int.Parse(s?.Split(',').Last()))
                       )
                       .Add<Point, string>(p => p?.X + "," + p?.Y);
```

The example above, may be written even more concise by adding an Interconverter pair at once:

```cs
    ConverterCollection.CurrentInstance.Add(
        (string s) => new Point(int.Parse(s?.Split(',').First()), int.Parse(s?.Split(',').Last())), 
                 p => $"{p?.X},{p?.Y}");

```

Indeed, writing this example comes with full IDE code-completion through strong type inference of the Integrated Developer Environment of choice, 
guiding the developer along the way. 
If required, culture and context information may be passed as a second function parameter, i.e. the model-argument, otherwise it can be safely omitted.

By the way, using the `C# Apply` library (See links), the example becomes even shorter, yet remains comprehensible at a single glance:
```cs 
    ConverterCollection.CurrentInstance
    .Add((string s) => s.Split(',').ApplyTo<Point>(int.Parse), p => $"{p?.X},{p?.Y}");
```

Concluding, this **library** attempts to bridge the gap, with a **portable, well developed, documented and tested converter base**.  

_Note: Assuming that for a high performance loop scenario, carefully in-lined code exceeds the benefit of any additional function invocations, 
this library is widely and generally applicable.__


### What to do Next

The library has three main functions called `Cast`, `Convert` and  `Transform`, which are all similar but not the same.  
Be advised to choose the library's function, which best describes the situation of desired type change between the source type and resulting target data type.   
Take a look at the [definitions](#definitons). You may also take a glance at a set of suggestions provided under [best-practices](#best-practices).   

Take note that the library gives plenty of leeway toward individual code style preferences and does not enforce any particular style,
other than *do not repeat yourself*, and *keep it short and simple* KISS under the moniker of adhering to a pragmatic 
*<a href="http://goo.gl/EcbTnm" target="_blank">single responsibility principle</a>* approach.

Converting methods must have a <a href="http://goo.gl/jygQ1T" target="_blank">maximum of one return type</a> and a maximum of two arguments, by design.   
Create complex, comprehensive converters out of smaller ones, declared using a graph-building library. 
Incidentally this also serves a purpose of potential parallelization.

### Definitions

By knowing the terminology and applying each library function appropriately to a given context, 
your code is more readable, entails better IDE code introspection and ultimately is more maintainable.   

For instance, if a peer narrowed down a bug to a major underlying type change, then a subsequent investigation can yield the corresponding code units faster.

 - **Casting [Type]** - a process that changes a value from one data type to another.
 - **Converting** - a process that changes the form, character, or function of something.
 - **Transforming** a process that changes a figure, expression, or function into another one of similar value.

 Following is a table listing the gravity of the type-change, the types involved and the corresponding library functions.   
 The library extends `object` with static extension methods. There are many method overloads making the invocations fit a given context 
 and individual coding style.


Type Change  |      Function Types        | Function           | _"Try"_-Function     | Overloads
------------ |--------------------------- | :----------------: | :------------------: | -----------
`[ ~  ]`    | `(TIn, [TIn2]) => TOut`  | `Transform`       | `TryTransform`      |  Weak, Optional model and _"Add"_ function
`[ ~  ]`    | `(TIn, [TIn2]) => TOut`  |`Transform<,>`     | `TryTransform<,>`   |  Strict, Optional model and _"Add"_ function
`[ +~ ]`    | `(TIn, [TOut]) => TOut`  |`CastTo`           | `TryCast`            |  Weak, Optional default-value
`[ +~ ]`    | `(TIn, [TOut]) => TOut`  |`CastTo<,>`        | `TryCast<,>`        |  Strict, Optional default-value
`[ ++ ]`    | `(TIn, [TArg]) => TOut`  |`ConvertTo`        | `TryConvert`        |  Weak, Optional model-value
`[ ++ ]`    | `(TIn, [TArg]) => TOut`  |`ConvertTo<,>`     | `TryConvert<,>`     |  Strict, Optional model-value

 
The `Try...` functions adhere to the _"Try"_ convention of .NET, returning a boolean value of `false` or `true` upon success or failure, 
whilst passing the result value as a reference via `out`.

Take a look at the <a href="https://dotnet-cast-convert-transform.firebaseapp.com/" target="_blank">documentation</a> 
or a glance at the section <a href="https://csharpguidelines.codeplex.com/" target="_blank">best practices</a>. 


### Features in Detail

- Unique, comprehensive converter library framework
- Serializable converters
- Thread-Safe
- Singleton pattern using the `Portable-Singleton` library. 
- Safe to Dispose with on-demand reinitialization, as a requirement for some constrained server environments.
- Strong Type Casting
- Custom Converter Exceptions
- Granular and transparent control over the conversion behavior
- Unsurpassed IDE Introspection / MS Intellisense integration
- Extremely low learning curve as a consequence of IDE introspection alongside precise and intuitive methods
- Safe conversion through try-methods such as `TryCast`, following the Try convention of .NET 
- Clear distinct method naming: e.g. as opposed to `To`, `As` and `Cast`, used already "occupied" by LINQ and several Fluid Interfaces
- Short, intuitive syntax that is strongly typed, yet concise and easy to comprehend
- Flexibility converter implementation
- Flexible on-demand loading, by Namespaces first encountered
- Grouping of converters into Namespaces aliases
- Optional, strongly typed default-value conversion function
- Automatic Dependency Injection based on attributes set and / or the constructor parameters
- Auto / lazy instantiation. Resources are loaded on an on-demand basis 
- Lazy instantiation of the converter-collection, upon the first request
- Intuitive support for LINQ queries
- Readable code, pragmatically following the single responsibility principle. No method exceeds a cyclomatic complexity of >11


### Why this Project exists

Projects, big or small, require casting or conversion of types in one way or another.  
Aside the option of declaring specific conversion methods in a class as in `ToSomeType()`, C# offers implicit and explicit operators, 
and static extension methods for a given Type, which as a compiler trick is incredibly efficient.  
This project attempts to provide a comprehensive, easy to use, well tested interface that follows best-practices of conversion through a centralized interface.

The problem with specific class declarations for type conversion is short and simple code maintainability.    
Whilst custom conventions can be enforced through the use of interfaces, the level of enforcement is insufficient with the lack of declaring non-static methods 
such as operators whilst offering nothing to prevent unnecessary code repetition, which implies additional code that needs to be maintained and tested.  

Say you are implementing `ToProtein()` in one class, an implicit operator in another 
and whilst a peer declared an explicit conversion operator in a third class. 
You may soon be left with the sobering conclusion that in another derived class that concept does 
not scale anymore as you continuously fix bugs or get compiler errors, necessitating you to adapt the code. 
Whilst this may be quickly facilitated at first, it is rarely done so in the required attentive fashion is desired to prevent new bugs.

The benefit of **adopting this library into your project** is the ability of 
  - scalability
  - unburdening the programmer of unnecessary pitfalls
  - plays well with interactive shells (REPLs)
  - rapid prototyping
  - drastically reduced code maintenance 
  - consistent strong typing 
  - consistent, assertive exceptions and consistent handling thereof as well as 
  - common conventions towards implementation, 

Moreover a common conversion interface allows easier testing through integration, unit and mocking tests, with many pre-baked examples 
that one can copy and paste with few custom adaptations.

Ultimately several threads may concurrently interact with the converter collection instance at runtime. 
To facilitate correct threading behavior a `BlockingCollection` is used.

All library method: `Cast`, `Convert` and `Transform` feature a sibling _"Try"_-method that follows the "Try" convention of .NET . 
It accepts an object _"value"_ and an _out_ parameter of type T, _"result"_. An attempt is made to cast the value into the _result_ as type T. 
If the process succeeds, `true` is returned, else the converter sets `result = default(T)` and returns `false`.


### Practical Code Example

Find below an example to provide a glimpse of what the code will look like in a practical example of converting a string to an image:

```cs
    
    using Core.TypeCast;
    
    [Converter(loadOnDemand: true, nameSpace: nameof(System), dependencyInjection: true)]
    public class StringToImage
    {
        [ConverterMethod]
        public override Image Convert(string valueTyped, Image defaultValueTyped = null)
        {
             
            var font = new Font(FontFamily.GenericSerif, 12);
            Image img = new Bitmap(400, 100);
            Graphics drawing = Graphics.FromImage(defaultValueTyped ?? img);
            SizeF textSize = drawing.MeasureString(valueTyped, font, 400);
           
            StringFormat sf = new StringFormat();
            sf.Trimming = StringTrimming.Word;
            drawing.Clear(Color.Transparent);
           
            Brush textBrush = new SolidBrush(Color.Black);
            drawing.DrawString(valueTyped, font, textBrush, new RectangleF(0, 0, textSize.Width, textSize.Height), sf);

            textBrush.Dispose();
            drawing.Dispose();
            return img;
        }
    }
```

Which would then be used in productive code as follows:

```cs
    var fileName = @"~/mydir/textout.png";
    var imagePng = $"Some variable Text {nameof(Program.StringToImage)}".ConvertTo<Image>();
    imagePng.Save(fileName);
```


## Using the library

Outlined in the following sections are examples of how to use the various library extension methods.  
Subsequently you may peek ahead at the [best practice](#best-practices) paragraph.

In addition, any Integrated Development Environment (IDE) aided with code Introspection such as [_Sharpdevelop_](http://www.icsharpcode.net/), 
[_Xamarin Studio_](https://www.xamarin.com/studio) or [_KDevelop_](https://www.kdevelop.org/), will let you discover other available method overloads 
in-tune with your individual code style.

Once the library has been added to your packages.list, you will have a lazy-instancing Singleton class of Type `ConverterCollection`.
The `ConverterCollection` is based on the library dependency <a href="https://www.nuget.org/packages/CSharp.Portable-Singleton/" target="_blank">`Portable-Singleton`</a>,
and support full access to `IQueryable` and `IEnumerable`, in addition to being disposable - a key aspect for supporting server Environments.

Following is an inheritance-graph of the aforementioned relations:

<img src="https://googledrive.com/host/0ByqWUM5YoR35QUxZY3hrTDZTWGs/ConverterCollection_inherit_graph.png" >    


### Using ConverterCollection


The converter collection instance is accessed through  `ConverterCollection.CurrentInstance`.  
The `ConverterCollection` is a static on-demand instanced _Singleton_ class containing all converters. The instance is:

- Indexed
- Disposable
- Enumerable
- Queryable via LINQ
- Easy Debugging
- Efficient

**Quick Pointers:**
 - Access anywhere via `ConverterCollection.CurrentInstance`. In case your instance is safe from disposal you may create a local reference. 
   - This should generally be case for any client desktop application.
 - Access the current number of converter instances in the Collection through `Count`. The count does not include any non-instanced converters attributed with 
  `LoadOndemand =  true`.
- Subscribe to `PropertyChange` Events such as `Count` via the static Singleton event invocator property `Singleton<ConverterCollection>.PropertyChanged` 
- Use `Initialize(...)` to scan and load any attributed converters from a class or assembly. Valid argument Types of the method overloads are:
    `Assembly`, `Assembly[]` , a `string` of the application's NameSpace, a class `Type` or any `IEnumerable<Type>`.
- Use `Add(...)` and `Add<,>(..)` to add new converters at runtime. Most strongly typed generic functions in the library have weakly typed counterparts.
- Use `Get(...)` and `Get<,>(..)` to lookup a converter. 
- Use `IConverterCollection` as a constructor argument to set any class up for converter *dependency injection*. Consult the examples for usage cases. 


#### Get

For instance, all static functions in `ObjectExtensions` delegate a converter-lookup using the following succinct query:
```cs
    converter = ConverterCollection.CurrentInstance.Get(
                        typeFrom: typeFrom.GetTypeInfo(), 
                        typeTo: typeTo?.GetTypeInfo(),
                        typeArgument: typeArgument?.GetTypeInfo(), 
                        typeBase: typeBase?.GetTypeInfo(), 
                        attributeName: attributeName, 
                        loadOnDemand: true
    );
```

Invoke anywhere via `ConverterCollection.CurrentInstance.Get(...)`. As is generally the case in this library, there are strong and weak overloads available for `Get`.

_Note: `Get` can facilitate the lookup and instantiation of Converters grouped into NameSpaces which are attributed as LoadOnDemand. The lookup logic resides in the file ConverterCollectionLookup.cs.
To prevent instantiation `Get`'s parameter `loadOnDemand` can be set to false, which is set to `false` by default._ 

### Indexing

Converters can be accessed directly through indexes with arguments of `Type` or any integer within the collection range, such that the following statements are all valid:

```cs    
    using System.Linq;

    Converter converter;
    if(ConverterCollection.CurrentInstance.Count > 3)
    {
        converter = ConverterCollection.CurrentInstance[2];
    }

    converter = ConverterCollection.CurrentInstance[typeof(int), typeof(string)];

    // Count the number of converters for a source-conversion type
    converter = ConverterCollection.CurrentInstance[typeof(int)].Count();

```

### CanConvertTo

The wrapper function `CanConvertTo` allows intuitively checking whether an converter exists for the given input and output types. 
The function returns true upon success, else false, and is parametrically identical to the usage of `CastTo`:

```cs
    if("500".CanConvertTo(1337))
    { 
        // ... CastTo
    }

    if("500".CanConvertTo<int>())
    {
        // ... CastTo
    }

    if("500".CanConvertTo(0f, Point.Empty))
    {
        // ... ConvertTo
    }

    if("500".CanConvertTo<int, Point>())
    {
        // ... ConvertTo
    }
```

### ConverterCollection's CanConvertFrom, CanConvertTo

Additionally `ConverterCollection` contains the methods `CanConvertFrom` and `CanConvertFrom` which take a single type argument, 
to look up and return `true` if any converter was found, else `false` as demonstrated bewlow:

```cs
    var resultCanConvertFromInt = ConverterCollection.CurrentInstance.CanConvertFrom<int>();
    var resultCanConvertToInt = ConverterCollection.CurrentInstance.CanConvertTo<int>();

    Console.WriteLine($"{nameof(resultCanConvertFromInt)}: {resultCanConvertFromInt}");
    Console.WriteLine($"{nameof(resultCanConvertToInt)}: {resultCanConvertToInt}");
```

### Dispose

Other than a minor performance hit owing to re-instancing, the `ConverterCollection` can be destroyed at any time, in accordance with occasional 
need of multi-threaded server environments e.g. in stateless web-scripts.

```cs
    Console.WriteLine(true.CastTo<string>());
    Console.WriteLine(true.ConvertTo<string>(1337, true));

    //dispose such as through a loss of application context or state
    ConverterCollection.CurrentInstance.Dispose();

    Console.WriteLine( ((double)200.3).CastTo<string>()) ;
    Console.WriteLine( 5 + 200.3 .CastTo<string>("1337") );
```


## LINQ Query

In Addition to the LINQ functions provided by the framework or additional packages, the library provides the following filters

* **List of LINQ filters:**
    *   `WithFrom(TypeInfo typeFrom)`
    *   `WithTo(TypeInfo typeTo)`
    *   `WithArgument(TypeInfo typeArgument)`
    *   `WithBaseType(TypeInfo typeBase)`
    *   `WithStandard(bool? isStandard)`
    *   `WithDefaultFunction(bool? hasDefaultFunction)`
    *   `WithFromIsGenericType(bool? typeFromIsGenericType)`
    *   `WithToIsGenericType(bool? typeToIsGenericType)`
    *   `WithFunctionName(string functionName)`
    *   `WithConverterAttributeName(string attributeName)`

Be advised that all `Get` functions are a Facade, which at its core wrap these aforementioned Query filters. These filter functions reside in the static class `ConverterCollectionFilters`.  
The Query function `ApplyAllFilters(typeTo:..., typeArgument:..., )` in turn, wraps all LINQ Query filter functions serving as a a central hub, with the `arguments` and Types provided 
in the list above.


## Enumerate

The following example will list all loaded and instanced converters currently residing within the collection, however none which are back-listed by means of a `ConverterAttribute` 
parameter set to lazy or on-demand Loading. Such converters are added to the collection only upon a request to their particular namespace collection set in the attribute.

```cs
    var cc = ConverterCollection.CurrentInstance;
    // list all converters
    foreach(var item in cc)
    {
        Console.WriteLine(item);
    }

```

This loop should yield a result that is similar to the abbreviated listing below, wherein `Converter`2` is a converter instance not specifying an Argument-Type (i.e. set to `object`):

```cs

    Converter`2[(Object, Object) => Int32] BaseType: ConverterDefaults, Attribute: [...]
    Converter`2[(Object, Object) => UInt32] BaseType: ConverterDefaults, Attribute: [...]
    Converter`2[(Object, Object) => Char] BaseType: ConverterDefaults, Attribute: [...]
    Converter`2[(Object, Object) => Boolean] BaseType: ConverterDefaults, Attribute: [...]
    Converter`2[(Object, Object) => Byte] BaseType: ConverterDefaults, Attribute: [...]
    Converter`2[(Object, Object) => SByte] BaseType: ConverterDefaults, Attribute: [...]
    ...
```

_LINQ_ is fully applicable to the collection. For instance, you may loop over all results at any time such as:
 `foreach(var item in cc.WithFrom(typeof(Point))).WithToIsGenericType()` 

 This will lookup and single out all converters with a source type of `Point` and any target type that is comprised out of generic parameters.

 _Note: Many queries are deferred and are not applied and executed until an Enumerator, Count, ToArray, etc is requested.__


### Using CastTo

Use _Cast_ whenever an unrestricted Type A is to be changed in an unrestricted Type B, without an inferred particular relationship or complexity.   
Use _TryCast_ to prevent raising exceptions during the invocation of the custom _conversion-logic_.

```cs
    string oneHundredOne = "101";
    int value = oneHundredOne.CastTo<int>(1337);
    Console.WriteLine(value);
```

Following are further examples, demonstrating that the `ConverterCollection` can be disposed at any time,  
as long as the converters are discoverable through attributes in the included assemblies.

```cs

    Console.WriteLine(100.2323.CastTo<string>());
    Console.WriteLine("100.2323".CastTo<float>());

    ConverterCollection.CurrentInstance.Dispose();

    Console.WriteLine(true.CastTo<string>());
    Console.WriteLine(true.ConvertTo<string>(1337));
```

```cs
    Console.WriteLine( ((double)200.3).CastTo<string>()) ;
    Console.WriteLine( 5 + 200.3 .CastTo<string>("1337") );
```

The libraries numerous method overloads allow flexibility towards personal code styles whilst remaining strongly descriptive.

```cs
    var aString = "255s";

    var aByte1 = aString.CastTo(255);
    var aByte2 = aString.CastTo(new int());
    var aByte3 = aString.CastTo<string, byte>();
    var aByte4 = aString.CastTo<byte>();
    var aByte5 = aString.CastTo<byte>(255);

```

Treatment of `Nullables<>` is complicated as boxing a non-null nullable value type boxes the value type itself (See References at the end).    
`CastTo` includes a specific generic overload just for non-null Nullable value-types, which is invoked in each of the following methods:

```cs
    int? nullInt = new Nullable<int>(5);

    var nullInt1 = nullInt.CastTo<int, float>();
    var nullInt2 = nullInt.CastTo(0.0);
    var nullInt3 = nullInt.CastTo(typeof(float));
    var nullInt4 = nullInt.CastTo("");

    // Converter: Nullable<int> --> float
    ConverterCollection.CurrentInstance.Add((int? a) =>
    {
        return (float)(a ?? 0);
    });
    var nullInt5 = new Nullable<int>(5).CastTo<int?, float>();
```
```

Be advised that in the following example invocation of the nullable extension method does not occur, due to the specific nature of generic declaration, not detailed further in here. 
You may need to resort to a `ConvertContext`. Consult the <a href="https://dotnet-cast-convert-transform.firebaseapp.com/" target="_blank">documentation</a> for details.

```cs
    int? nullInt = new Nullable<int>(10);

    var xy2 = nullInt.CastTo<int?, float>();
    var genericconv = nullInt.CastTo<Converter<int, float>>();

``` 

You can declare a custom NumberFormatter during adding, load serialized functions, and precede casting by a value-boxing step.

```cs

    ConverterCollection.CurrentInstance.Add<string, int>(
        s => int.Parse(s?.Trim(),  new NumberFormatInfo() { NumberGroupSeparator = ",", NumberDecimalDigits = 3 })
    );

    var stream = new FileStream(@"converters.dat", FileMode.Create);

    var dec = ((object)"6.5").CastTo<object, decimal>();
```


### TryCast


In analogy to the Try convention of the .NET framework, TryCast may be more appropriate for casting, 
which suppresses any exceptions during the casting process and returns a boolean success value instead, whilst passing the casting result by reference.

Following are two examples:

```cs
    int result = default(int);
    if(5f.TryCast<float, int>(out result) == true)
    {
        // ... process result
    }

    decimal resultDecimal;
    if("400.01M".TryCast(out resultDecimal, 0.000001M) == true)
    {
        // ... process result
    }
```


### Using ConvertTo


The convert methods `ConvertTo` and `TryConvert`, can involve up to three different types, to convert an arbitrary input type to another arbitrary output type,
using an optional second _model_ argument, which encapsulates all data required for the custom conversion function.

- Use _Convert_ whenever an unrestricted Type A is to be changed into an unrestricted Type B, with an implied convertible relationship 
    between the two and / or a complexity that requires additional arguments.   
- Use _TryConvert_ to prevent raising exceptions during the invocation of the custom _conversion-logic_.



Following is an example absent of accompanying notes:

```cs
    Func<Point, Rectangle, Size> delegateThreeTypes = (ap, br) =>
        {
            var rect2 = (Rectangle)br;
            if(ap.X * ap.Y > rect2.X * rect2.Y)
            {
                return new Size(ap.X, ap.Y);
            }
            return new Size(rect2.X, rect2.Y);
        };

    Point somePoint = new Point(1, 2);
    Size size = somePoint.ConvertTo<Point, Size>(new Rectangle(1, 1, 2, 2));

```

### Using TryConvert


In analogy to the _Try_ convention of the .NET framework, TryConvert may be more appropriate for converting, 
which suppresses any exceptions during the converting process and returns a boolean success value instead, whilst passing the conversion result by reference.

Following is an example, using the converter function from the example provided in the preceding section:

```cs
    Size outsize;
    bool successSize = new Point(2, 4).TryConvert<Size>(out outsize, new Rectangle(1, 2, 3, 4));
    if(successSize == true)
    {
        //... process result
    }
```

#### Model arguments and events

Provided below is a code example for a more involved and comprehensive converter example which take a `String` and converts it into a `MemoryStream` of an _Image_.  
The code is provided in full in the examples folder of the library.
For brevity's sake the `EventModelBase` is excluded from being listed herein.

Following, the _"model"_ class is declared, with a simplified inheritance model, along with events required. 

```cs 
    public class TextModel : EventModelBase, IModel
    {
        public Font Font { get; set; }
        public Brush Brush{ get; set; }
        public Point Point{ get; set; }
        public Size Size{ get; set; }
        public Color TextColor { get; set; }
        public Color CanvasColor { get; set; }

        public bool LeftToRight { get; set; } = true;
    }

    var textModel = new TextModel()
    {
        Size = new Size(400, 50),
        Font = new Font("Arial", 12),
        Brush = Brushes.Black,
        Point = new Point(40, 20),
    };

    textModel.StatusChanged += (object sender, EventModelBase.Status e) =>
    {
        Console.WriteLine($"{sender.GetType().Name} ....{e.ToString()}");
    };
```

Next, the actual converter logic is implemented and added. Please note that the example is written such as to purposefully demonstrate features of the library.

In an actual project, be advised to put a converter of this complexity in a separate class and split it up into several smaller converter-units, 
pragmatically adhering to the Single Responsibility Principle.

```cs
    ConverterCollection.CurrentInstance.Add(
        (int[] a, string b) => new MemoryStream(Encoding.ASCII.GetBytes(a.ToString() + b.ToString()))
    );
    new[] { 1, 3, 4, 5 }.ConvertTo<MemoryStream>("");

    // A more complex converter example: convert a string to text taking a Data-Model Object as second argument. 
    // The model encapsulates in a strict, declarative manner any further arguments
    // The parameter of the function may even be an interface!
    Func<string, IModel, MemoryStream> stringToPngImage = (text, dto) =>
        {
            var model = dto as TextModel;
            if (model == null)
            {
                return null;
            }
            model.OnStatusChanged(EventModelBase.Status.Started);
            Image image = new Bitmap(model.Size.Width, model.Size.Height);
            Graphics graphics = Graphics.FromImage(image);
            model.OnStatusChanged(EventModelBase.Status.Busy);
            graphics.DrawString(text, model.Font, model.Brush, model.Point);
            graphics.Dispose();
            var imageStream = new MemoryStream();
            image.Save(imageStream, ImageFormat.Png);
            image.Dispose();
            model.OnStatusChanged(EventModelBase.Status.Completed);
            return imageStream;
        };
    // creates a strictly typed Converter instance: Converter<string, MemoryStream, IModel> and adds it to the static, thread-safe collection
    ConverterCollection.CurrentInstance.Add(stringToPngImage);

    var streamPng = $"Draw Some variable Text with {nameof(stringToPngImage)}".ConvertTo<MemoryStream>(textModel);
    System.IO.File.WriteAllBytes(@"testpng.png", streamPng.ToArray());
```


#### Using the ConvertContext

On rare occasions, a contextual data structure may be required which provides meta-data about the converting-process. Such data is provided by the `ConvertContext`,
and passed to the Convert function in question, when the parameter `withContext` of `ConvertTo` or `TryConvert` is set to `true`:

```cs
    ConverterCollection.CurrentInstance.Add( (bool b, object defval) => b.ToString() + defval)
    Console.WriteLine(true.ConvertTo<string>(1337, withContext: true));
``` 

This will pass a `ConvertContext` instance to the converter similar to the following:

```cs
{Core.TypeCast.Base.ConvertContext<object, string>}
    Argument: {Name = "Int32" FullName = "System.Int32"}
    Caller: "TryConvert"
    Converter: Converter`3[(Boolean, Object) => String] BaseType: StringToImage, 
                Attribute: [LoD:False,Base:StringToImage,DepInj:True,NamSp:System,Name:]
    From: {Name = "Object" FullName = "System.Object"}
    Method: null
    MethodInfo: null
    Nullable: false
    ThrowExceptions: true
    To: {Name = "String" FullName = "System.String"}
    Value: 1337
```

_Note: `Argument` is set to `null`, which is the `default`  if the type is `object` (Boxed)_

In that case the model-value that is passed to the converter function upon invocation will be wrapped in a `ConvertContext` instance, which implements `IConvertContext`.
The field-names follow the names of the arguments of the extension methods in the class `ObjectExtension`.

```cs
    public interface IConvertContext
    {
        Type From { get;  }
        Type To { get; }
        Type Argument { get; }
        object Value { get; }
        Converter Converter { get; }
        string Caller { get; }
        bool? Nullable { get; }
        bool? ThrowExceptions { get; }
        object Method { get; }
        MethodInfo MethodInfo{ get; }
    }
```

**Important:** The requirement for the converter function that can be invoked through `ConvertTo` is that the second argument must be of type `object`, as follows: 

```cs
    [ConverterMethod]
    public string Bool2StringWithOptInConvertContext(bool self, object model)
    {
        var context = model as IConvertContext;
        if(context != null) {
            var nullable = model.Nullable;
            // ... do something contextual ...
        }
        return self > 0 ? "true" : "false";
    }
```

_Note: To simply get the Converter instance within the converting function, prefer using `ConverterCollection.CurrentInstance.Get(...)` over obtaining an `IConvertContext` instance_.

In case the converter function should always be invoked by `ConvertTo(...withContext: true )` or `TryConvert(...withContext: true )` 
you are advised to set the Type of the second function argument as `IConvertContext`. Thus ensuring that an exception is thrown if the converter is not called with a context
and avoiding type boxing / casts.

Otherwise a `System.InvalidCastException` exception will be thrown.


_Note: The decision for implementing a converting-context as an opt-in rather, than inherently into the architecture of the `Converter` base-classes, 
is separation from invocation time arguments for thread-safety, along with avoidance of additional overhead for a feature rarely required.__


### Using Transform

Transform is useful in situations wherein the input and output type are similar or the same. Aside from linguistics the implementation is similar.
All Types involved in the conversion must be from the same namespace.

If the optional parameter `strictTypeCheck` is set to `true`, an exception will be thrown if the input and output types do not match.
This does not hold true for the Try version, which has no optional `strictTypeCheck` argument.


In the following example, a converter transposing a 2x2 Square matrix is implemented. Aptly, `Transform` is invoked as the output and input types match.

Transformations are different from `Convert` and `Cast` operations in that there can be disambiguate functions, all of which operate on the same input-, 
output- and model parameter types, yet may yield completely different values.

As such, converters should be assigned by named aliases or delegates. Follow best-practices by using a transform enumerable. 
However a string argument may be passed as alias as well.

Additionally, best-practices recommend that an assertively named delegate is declared, as explicatively shown in the following code sample:


**Example for simple matrix operations**

```cs
    delegate float[,] Transpose2x2(float[,] matrix, object model);

    // Transpose 2x2 matrix converter:
    // { { a, b}, { c, d } } -> 1/(ad -bc)* { {d, - b }, {- c,  a} }
    ConverterCollection.CurrentInstance
        .Add<float[,], float[,], Transpose2x2>((a) => {
            float _f = 1 / (a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0]);
            return new float[,] { { _f * a[1, 1], -_f * a[0, 1] }, { -_f * a[1, 0], _f * a[0, 0] } };
        });

    var matrix2x2 = new float[,] 
    { 
        { 0, 1 }, 
        { 2, 3 } 
    };

```

After adding the custom transformer-algorithm, the given matrix may be transformed by passing the `delegate` type of the transformer function, 
and declaring the output Type as the second generic parameter, or alternatively following up with a subsequent cast operation as shown below:

```cs
    float[,] matrixTransposedExample1 = matrix2x2.Transform<Transpose2x2, float[,]>();
    
    // alernatively, you may follow up with a cast statement:
    float[,] matrixTransposedExample1 = matrix2x2.Transform<Transpose2x2>().CastTo<float[,]>();
```

Alternatively, the delegate may be passed as a type argument, in case the output and input types do match:
```cs
    float[,] matrixTransposedExample1 = matrix.Transform(typeof(Transpose2x2));

    // with a second "model" parameter
    float entropyValue = matrix.Transform("Shannon", typeof(Entropy));
    // the same example with named arguments
    float entropyValue = matrix.Transform(typeBase: typeof(Entropy), model: "Shannon");

```

The example using `Transpose2x2``  will yield the transposed of input `matrix2x2` as follows:

```cs
    matrixTransposedExmpl1 = {float[2, 2]}:
        [0, 0]: -1.5
        [0, 1]: 0.5
        [1, 0]: 1
        [1, 1]: 0
```



When the input and output types do not match, a subsequent implicit or explicit cast operation is required as follows:

```cs
    delegate float[,] Transpose1xN(float[] matrix);

    ConverterCollection.CurrentInstance
        .Add<float[], float[,], Transpose1xN>((a) => {
            var result = new float[a.Length, 1];
            for(int i = 0; i < a.Length; result[i, 0] = a[i], i++) ;
            return result;
        });

    var matrixTransposed1x = (float[,])new []{ 1f, 2f, 3f, 4f}.Transform<Transpose1xN>();

    // yielding:
    matrixTransposed1x = {float[4, 1]}
        [0, 0]: 1
        [1, 0]: 2
        [2, 0]: 3
        [3, 0]: 4
```


Additionally, particularly with interactive-shells in mind, `Transform` allows adding an aliased function and computing in one invocation, 
with the full benefit of strong typed IDE introspection, as follows:

```cs
    // list the tranform-function aliases here
    public enum Transform
    {
        Matrix2x2Determinant,
        MatrixAnotherOperation,
        ...
    }

    // declare and add a transformer, whilst computing the result, with a strongly typed result in both cases
    var matrixTransposedEx2 = matrix2x2.Transform( (a) => a[0,0]*a[1,1] - a[0,1]*a[1,0], Transform.Matrix2x2Determinant);

    var matrixTransposedEx2_ = matrix2x2.Transform<float>( Transform.Matrix2x2Determinant);
```


#### TryTransform


In analogy to the _"Try"_ convention of the .NET framework, `TryTransform` may be more appropriate for transformations, 
which suppresses any exceptions during the transforming process and returns a boolean success value instead, whilst passing the transformation result by reference.

Following is an example, using the transformation function from the example provided in the preceding section:

```cs
    float matrix2x2Det;
    if(matrix.TryTransform(out matrix2x2Det) == true)
    {
        //... perform further steps on result
    }

    float matrix2x2Det2;
    if(matrix.TryTransform<Transpose1xN, float>(out matrix2x2Det2) == true)
    {
        //... perform further steps on result
    }


```

### Creating and adding Converters

Any custom written converter logic is brought into the context of a strong-type generic converter container instance, with the abstract base-class `Converter`.
In general converters are instanced internally by a Factory during the process of adding custom-converters to the static collection `ConverterCollection` by various means.  
The easiest way of adding converters is by attributing methods throughout the assembly, through the attributes `ConverterAttribute` and `ConverterMethodAttribute`, 
in just this nesting order.

#### Using Add

Many intuitive and concise ways of adding converters to the `ConverterCollection` exists and will be provided in brief in the subsequent listings. 
Be advised that by design Converters cannot be removed, other than through the methods provided by the `BlockingCollection` using `ConsumingEnumerator`.

Following are the most common ways of adding converters at runtime:

- Through *Initialize*
    
        ConverterCollection.CurrentInstance.Initialize(Assembly assembly);
        ConverterCollection.CurrentInstance.Initialize(Type className);
        ...

- As an Interconverter pair:

       ConverterCollection.CurrentInstance.Add(
            (string s) => new Point(int.Parse(s?.Split(',').First()), int.Parse(s?.Split(',').Last())), 
            p => $"{p.X},{p.Y}"
       );

- As an Transformer associated by a delegate:

        delegate double Entropy(byte[] data, EntropySettings model);
        ConverterCollection.CurrentInstance.Add<byte[], EntropySettings, double, Entropy>((da, se) => {
            ...
        });

- As a a boxed delegate (`object`)

        var fileStream = new FileStream(@"converters.dat", FileMode.Open);
        var binaryFormatter = new BinaryFormatter();
        var func = binaryFormatter.Deserialize(fileStream );
        ConverterCollection.CurrentInstance.Add(func);

- As a (lambda) function
 
        ConverterCollection.CurrentInstance.Add((Point[] pts) => new Perimeter(pts));

       //with generics
       ConverterCollection.CurrentInstance.Add((Point a, someGenericClass<Point, ConverterAttribute> b) =>
       {
           return new someGenericClass<Point, ConverterAttribute>();
       });

- With an explicit baseType

        public class ShapeMath { .... }
        ConverterCollection.CurrentInstance.Add<Point[], Perimeter, ShapeMath>((Point[] pts) => new Perimeter(pts));

- With Constructor Dependency Injection

        [Converter (dependencyInjection: true)]
        public class ConverterDefaults
        {
           public ConverterDefaults(IConverterCollection collection) : base(collection)
           {
                this.NumberFormat = ConverterCollectionSettings.DefaultNumberFormat.Clone() as NumberFormatInfo;
                collection.Add<object, int>(o => int.Parse(o?.ToString() ?? string.Empty, this.NumberFormat), this.GetType())
            }
        }
        

#### Using the Add Builder

By invoking `AddStart(...)` on the `ConverterCollection` instance, a set of related converters can be added in a deferred manner,
such that a set of converters share specific `ConverterColllectionSettings`, and a mutual reference to a 
<a href="http://goo.gl/CPbouh" target="_blank">`CancellationToken`</a> for multi-threaded situations, as well as other parameters.   

As the method is deferred, the operation does not complete until the method `End()` is invoked, as shown in the following example:

```cs
    public class Program 
    {
        static void Main(string[] args)
        {
            ...
            // Add a set of releated converter delegates, explicitly setting `Program` as the BaseType. 
            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            cc.AddStart<Program>(new ConverterCollectionSettings { UseFunctionDefaultWrapper = false, },
                                cancellationToken: cancellationToken.Token)
               .Add<string, bool>(s => s.ToLowerInvariant() == "true" ? true : false)
               .Add<string, byte[]>(s => System.Text.ASCIIEncoding.ASCII.GetBytes(s))
               //.Add<>()
            .End();
            ...
        }
    }
```

## Best Practices

Reading the sections thus far you are already familiarized with the syntax and possibilities of this library. 
Generally speaking best practices for using this library and conversion logic at large, breaks down to:

- Prefer strong typed method invocations, containers, etc...
- Prefer library methods that best describes the underlying type change relationship i.e. how similar are the intput/output types
- Prefer readability over complexity
- Prefer separation for conversion logic: separate files, classes, structs, exceptions and interfaces
- Prefer simplicity: i.e. complex converters made out of smaller ones
- Prefer declaring converters through attributes
- Prefer adding converters early and apply On-Demand-Load if converter use is not guaranteed instead
- Prefer passing a model with custom parameters rather than obtaining a `ConvertContext`

## Class Structure

Following in brief are graphs and short descriptions of important classes of the project. 
Detailed descriptions are provided in full in the <a href="https://dotnet-cast-convert-transform.firebaseapp.com/" target="_blank">documentation</a>.

### Exceptions

<img src="https://googledrive.com/host/0ByqWUM5YoR35QUxZY3hrTDZTWGs/Exceptions_inherit_graph.png">    


### Attributes

The library uses two custom attributes. `ConverterAttribute` ascribable to a `class` and 
`ConverterMethodAttribute` ascribable to a `delegate`. The attributes must be nested in just this order.

<img src="https://googledrive.com/host/0ByqWUM5YoR35QUxZY3hrTDZTWGs/Attributes_inherit_graps.png">    



### The converter

Underlying the converters are a strongly typed container, declaring three types. `TIn`, `TOut`, `TArg`, with `TArg` 
being optionally used and set to `object` as a default value.

<img src="https://googledrive.com/host/0ByqWUM5YoR35QUxZY3hrTDZTWGs/ConverterDefaults_inherit_graph.png" >    


### Performance criticality

For higher performance, one can first directly fetch a converter through the common LINQ query interface and subsequently reference the converter function 
for direct invocation within a loop or other code parts which require attention towards performance.  
Additionally, for performance-critical code part you may want to explicitly invoke the converter, bypassing the library altogether and enable compiler-inlining via  
`[MethodImpl(MethodImplOptions.AggressiveInlining)]`.

### Contact & Contracted Customization

If you need assistance with your project codebase or custom implementations in regards to synchronization of the Converters across memory-contexts or other barriers,
eventing and notifications, parallel processing, profiling and performance reports or anything else tangential to this library, please do not hesitate further and get in touch.  


### ![Passed](http://i.imgur.com/rq5lf6x.png "Passed")  Tests

This library is being continously tested and improved using the NUnit Test Framework.   
If you run into a bug, please push a new issue [here](https://github.com/lsauer/dotnet-portable-cast-convert-transform/issues).

### Copyright and License
Copyright 2013-2016 by Lorenz Lo Sauer - <a href="http://lsauer.mit-license.org/" target="_blank">MIT License</a>

### Other projects

- <a href="https://github.com/congdongdotnet/SafeConvert" target="_blank">SafeCast</a>
- <a href="http://www.convertapi.com/" target="_blank">ConvertAPI</a>
- <a href="https://github.com/ledsun/ObjectTypeConvertExtentions" target="_blank">ObjectTypeConvertExtentions</a>
- <a href="https://www.nuget.org/packages/MyConvert/" target="_blank">MyConvert</a>
- <a href="https://github.com/deniszykov/typeconvert" target="_blank">typeconvert</a>
- <a href="https://github.com/darrencauthon/CastAs" target="_blank">CastAs</a>
- <a href="https://github.com/thomasgalliker/TypeConverter" target="_blank">TypeConverter</a>

Please let me know about your project if I missed to list it here, or push an edit yourself.


### References / Further reading: 

- <a href="https://msdn.microsoft.com/en-us/library/98bbex99(v=vs.110).aspx" target="_blank">Type Conversion in the .NET Framework</a> (MSDN)
- <a href="https://msdn.microsoft.com/en-us/library/ayybcxe5.aspx" target="_blank">How to: Implement a Type Converter</a>
- <a href="https://msdn.microsoft.com/en-us/library/ms228597.aspx" target="_blank">Boxing Nullable Types </a>
- <a href="https://msdn.microsoft.com/en-us/library/system.componentmodel.isynchronizeinvoke.aspx" target="_blank">ISynchronizeInvoke </a>
- <a href="http://stackoverflow.com/questions/17448562/blind-casting-with-reflection-for-thread-safe-event-call" target="_blank">Blind Casting for thread safe event call</a>
- <a href="https://en.wikipedia.org/wiki/Type_conversion" target="_blank">Type Conversion</a>
- <a href="https://msdn.microsoft.com/en-us/library/gg597391(v=vs.110).aspx" target="_blank">Cross-Platform Development with the Portable Class Library</a>
- <a href="https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/" target="_blank">Porting to .NET Core</a>    

### Caveats

- When using the serialization and deserialization in a productive setting, please ensure that you are using a virtualized, sandboxed environment such as 
<a href="https://en.wikipedia.org/wiki/Docker_(software)" target="_blank">Docker</a> or are executing the assemblies in a trusted environment. 
Furthermore ensure that the C# languages match for version and bytecode compatibility between the serializing and the deserializing platform.

- The thread-safety pertains to the internal handling of the converter collection, not the custom implementation of converter user-logic.
Implementers are not freed from the responsibility of thread-safety in multi-threaded applications. Simply adding a custom converter logic to the ConverterCollection
does not automatically confer immunity of common threading issues.
