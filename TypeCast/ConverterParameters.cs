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
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Linq;

    /// <summary>
    /// Container with sequentially assigned type-parameters of a strictly typed Converter-Function, in the sequence of Types for: In, Out, Argument
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ConverterParameters
    {
        public int Count;
        public Type In;
        public Type Out;
        public Type Arg;

        /// <summary>
        /// Creates a new instance of <see cref="ConverterParameters"/>
        /// </summary>
        /// <param name="parameters">A sequential list of function parameters in order of In, Out, Argument</param>
        /// <example>
        /// <code>
        /// ```cs
        ///     Func&lt;bool> method = () => true;
        ///     var methodInfo = method.Method;
        ///     var parameterInfos = methodInfo.GetParameters();
        ///     var parameterTypes = new ConverterParameters(methodInfo.ReturnParameter, parameterInfos);
        ///     Console.WriteLine( parameterTypes );
        /// ```
        /// </code>
        /// </example>
        /// <seealso cref="Converter{TIn, TOut, TArg}"/>
        public ConverterParameters(ParameterInfo parameterOut, ParameterInfo[] parameters)
            : this(parameters: new[] { parameters?.First(), parameterOut, parameters?.Length > 1 ? parameters[1] : parameterOut } )
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ConverterParameters"/>
        /// </summary>
        /// <param name="parameters">A sequential list of function parameters in order of In, Out, Argument</param>
        /// <remarks>
        /// In preparation of creating strongly typed standard containers of <see cref="Converter{TIn, TOut, TArg}"/>
        /// the argument type is assigned to the Out-Type, when no second parameter type is provided i.e. <code>`Count == 2`</code>  
        /// </remarks>
        /// <seealso cref="Converter{TIn, TOut, TArg}"/>
        public ConverterParameters(params ParameterInfo[] parameters)
        {
            this.Count = parameters.Length;
            this.In = Count > 0 ? parameters[0].ParameterType : null;
            this.Out = Count > 1 ? parameters[1].ParameterType : null;
            // with only In/Out parameters assigned i.e. the argument type is null, use `Out` as an argument-type
            this.Arg = Count > 2 ? parameters[2].ParameterType : this.Out;
        }

        /// <summary>
        /// Returns an Array of <see cref="Type"/> in the order of the Converter generic arguments <seealso cref="Converter{TIn, TOut, TArg}"/>
        /// </summary>
        /// <returns>an array containing <paramref name="In"/>, <paramref name="Out"/>, <paramref name="Arg"/></returns>
        public Type[] ToArray()
        {
            return new[] { this.In, this.Out, this.Arg };
        }

        /// <summary>
        /// Returns a string representation of the <see cref="struct"/> contents
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(ConverterParameters)}: (In:{this.In}, Out:{this.Out}, Arg:{this.Arg})";
        }
    }
}