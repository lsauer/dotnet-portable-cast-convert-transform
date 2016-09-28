// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example10
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Reflection;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization.Json;
    using System.Text;

    using Core.TypeCast;
    using Core.Extensions;

    using System.Runtime.CompilerServices;

    // Declare transformer delegates
    public delegate float[,] Transpose1xN(float[] matrix);

    public delegate float[,] Transpose2x2(float[,] matrix, object model);

    public delegate float[,] Transpose4x4(float[,] matrix, object model);

    // Declare unique transformer aliases
    public enum Transform
    {
        MatrixReturnUnit,
        MatrixMulDiagonal,
    }

    /// <summary>
    /// Demonstrate using `Transform` and `TryTransform` in the context of matrix operations
    /// </summary>
    [Description("Demonstrate using `Transform` and `TryTransform` in the context of matrix operations")]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            //transpose example using transform
            // { { a, b}, { c, d } } -> 1/(ad -bc)* { {d, - b }, {- c,  a} }

            ConverterCollection.CurrentInstance
                .Add<float[], float[,], Transpose1xN>((a) =>
                {
                    var result = new float[a.Length, 1];
                    for(int i = 0; i < a.Length; result[i, 0] = a[i], i++)
                        ;
                    return result;
                });

            ConverterCollection.CurrentInstance
                .Add<float[,], float[,], Transpose2x2>((a) =>
                {
                    float _f = 1 / (a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0]);
                    return new float[,] { { _f * a[1, 1], -_f * a[0, 1] }, { -_f * a[1, 0], _f * a[0, 0] } };
                });

            // add an disambiguate: Transpose4x4 is a disambiguate to  Transpose2x2
            ConverterCollection.CurrentInstance
                .Add<float[,], float[,], Transpose4x4>((a) =>
                {
                    float _f = 1 / (a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0]);
                    return new float[,] { { _f * a[1, 1], -_f * a[0, 1] }, { -_f * a[1, 0], _f * a[0, 0] } };
                });

            // Add cast to pretty print a float[*,*] matrix
            ConverterCollection.CurrentInstance
                .Add((float[,] a) =>
                {
                    var ret = new StringBuilder(Environment.NewLine + "[");
                    int rowLength = a.GetLength(0);
                    int colLength = a.GetLength(1);

                    for(int i = 0; i < rowLength; i++)
                    {
                        for(int j = 0; j < colLength; j++)
                        {
                            ret.Append(string.Format(" {0} ", a[i, j]));
                        }
                        ret.Append(colLength > 1 ? Environment.NewLine : null);
                    }

                    return ret.ToString() + "]";
                });

            // Output

            var matrixTransposed = (float[,])matrix.Transform<Transpose4x4>();
            Console.WriteLine($"{nameof(matrixTransposed)}: {matrixTransposed.CastTo<string>()}");

            var matrixTransposed1xEx1 = (float[,])new[] { 1f, 2f, 3f, 4f }.Transform<Transpose1xN>();
            Console.WriteLine($"{nameof(matrixTransposed1xEx1)}: {matrixTransposed1xEx1.CastTo<string>()}");

            var matrixTransposed1xEx2 = new[] { 1f, 2f, 3f, 4f }.Transform<Transpose1xN, float[,]>();
            Console.WriteLine($"{nameof(matrixTransposed1xEx2)}: {matrixTransposed1xEx2.CastTo<string>()}");

            var matrixTransposed1xEx3 = new[] { 1f, 2f, 3f, 4f }.Transform<Transpose1xN>().CastTo<float[,]>();
            Console.WriteLine($"{nameof(matrixTransposed1xEx3)}: {matrixTransposed1xEx3.CastTo<string>()}");

            var matrixTransposedEx4 = matrix.Transform<Transpose2x2>();
            Console.WriteLine($"{nameof(matrixTransposedEx4)}: {matrixTransposedEx4.CastTo<string>()}");

            var matrixTransposedEx4_ = matrix.Transform(typeof(Transpose2x2));
            Console.WriteLine($"{nameof(matrixTransposedEx4_)}: {matrixTransposedEx4_.CastTo<string>()}");

            var matrixTransposedEx5 = matrix.Transform<Transpose2x2>().CastTo<float[,]>();
            Console.WriteLine($"{nameof(matrixTransposedEx5)}: {matrixTransposedEx5.CastTo<string>()}");

            var matrixTransposedEx6 = matrix.Transform((a) => 1f, Transform.MatrixReturnUnit);
            Console.WriteLine($"{nameof(matrixTransposedEx6)}: {matrixTransposedEx6}");

            var matrixTransposedEx7 = matrix.Transform<float>(Transform.MatrixReturnUnit);
            Console.WriteLine($"{nameof(matrixTransposedEx7)}: {matrixTransposedEx7}");

            var matrixTransposedEx8 = matrix.Transform<float>(functionAlias: Transform.MatrixReturnUnit);
            Console.WriteLine($"{nameof(matrixTransposedEx8)}: {matrixTransposedEx8}");

            var matrixTransposedEx9 = matrix.Transform((a) => new float[,] { { a[0, 0] * a[1, 1] }, { a[0, 1] * a[1, 0] } }, Transform.MatrixMulDiagonal);
            Console.WriteLine($"{nameof(matrixTransposedEx9)}: {matrixTransposedEx9.CastTo<string>()}");

            var matrixTransposedEx10 = matrix.Transform<float[,]>(Transform.MatrixMulDiagonal);
            Console.WriteLine($"{nameof(matrixTransposedEx10)}: {matrixTransposedEx10.CastTo<string>()}");

            float matrix2x2Det;
            if(matrix.TryTransform(out matrix2x2Det) == true)
            {
                //... perform further steps on result
                Console.WriteLine($"{nameof(matrix2x2Det)}: {matrix2x2Det}");
            }

            //throw an error by passing a delegate that does not match
            float matrix2x2Det2;
            try
            {
                if(matrix.TryTransform<Transpose1xN, float>(out matrix2x2Det2) == true)
                {
                    //... perform further steps on result
                    Console.WriteLine($"{nameof(matrix2x2Det2)}: {matrix2x2Det2}");
                }
            }
            catch(ConverterException exc)
            {
                Console.WriteLine($"{nameof(matrix2x2Det2)}: { exc.Cause}: {exc.Message}");
            }

            // End
            Console.WriteLine("List all Custom Converters? (y/n)");

            var key = Console.ReadKey(true);

            if(key.KeyChar == 'y' || key.KeyChar == 'Y')
            {
                // enumerate all custom converters in "Program"
                foreach(var item in ConverterCollection.CurrentInstance.WithBaseType(typeof(Program).GetTypeInfo()))
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine($"Total Number of CONVERTERS: {ConverterCollection.CurrentInstance.Count}");

                Console.ReadKey(true);
            }
        }
    }

}