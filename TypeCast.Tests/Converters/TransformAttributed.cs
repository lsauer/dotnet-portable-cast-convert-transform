// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Core.TypeCast.Test
{
    using System;

    using Core.TypeCast;
    using Core.TypeCast.Base;
    using Core.Extension;

    /// <summary>
    /// This class implements converting from string type to byte type as loose coupled, custom converter demonstration example
    /// </summary>
    [Converter(loadOnDemand: false, nameSpace: nameof(System), dependencyInjection: false)]
    public class TransformAttributed
    {
        /// <summary>
        /// An simple transformer aliased and thus uniquely referenced by the <see cref="Enum"/> <see cref="Transform"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ConverterMethod(name: nameof(Transform.MatrixReturnUnit))]
        public float AttributeNameAlias_MatrixReturnUnit_ForTransform(float[,] input)
        {
            return 1f;
        }

        /// <summary>
        /// An transformer aliased and thus uniquely referenced by the <see cref="Enum"/> <see cref="Transform"/>
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [ConverterMethod(name: nameof(Transform.MatrixMulDiagonal))]
        public float[,] AttributeNameAlias_MatrixMulDiagonal_ForTransform(float[,] a)
        {
            return new float[,] { { a[0, 0] * a[1, 1] }, { a[0, 1] * a[1, 0] } };
        }

        /// <summary>
        /// Add an ambiguity to the source/target type-space first occupied by MatrixMulDiagonal
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [ConverterMethod]
        public float[,] AttributeNameAlias_MatrixMulDiagonal_Like_Arguments(float[,] a)
        {
            return default(float[,]);
        }
    }
}