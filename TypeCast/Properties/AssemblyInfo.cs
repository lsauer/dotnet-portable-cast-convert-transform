﻿// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>

using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Core.TypeCast - Portable Converter")]
[assembly: AssemblyDescription("A tested, generic, portable, runtime-extensible type conversion library for any types")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Lorenz Lo Sauer")]
[assembly: AssemblyProduct("Core.TypeCast")]
[assembly: AssemblyCopyright("Lo Sauer ©  2016 - MIT License")]
[assembly: AssemblyTrademark("MIT License")]
[assembly: AssemblyCulture("neutral")]
[assembly: NeutralResourcesLanguage("en")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("16EA9EC0-1CFC-4257-85DD-61FEC509945B")]

// Version information for an assembly consists of the following four values:
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.1.0.*")]
[assembly: AssemblyFileVersion("3.1.0.5")]

// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CC0057:Unused parameters", Justification = "<Pending>", Scope = "member", Target = "~M:Core.TypeCast.ConverterCollectionLookup.Get``3(System.Linq.IQueryable{Core.TypeCast.Base.Converter},``0,``1,``2,System.Nullable{System.Boolean},System.Boolean)~Core.TypeCast.Base.Converter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CC0057:Unused parameters", Justification = "<Pending>", Scope = "member", Target = "~M:Core.TypeCast.ObjectExtension.CanConvertTo``2(System.Object,``0,``1)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CC0057:Unused parameters", Justification = "<Pending>", Scope = "member", Target = "~M:Core.TypeCast.ObjectExtension.CanConvertTo``2(``0,System.Type,``1)~System.Boolean")]
