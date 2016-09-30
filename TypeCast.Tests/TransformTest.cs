using Core.TypeCast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit;

namespace Core.TypeCast.Test
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Text;
    using NUnit.Framework;

    // common test declarations
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

    [TestFixture]
    public class TransformTests
    {
        private static void Add_Five_Matrix_Transformers()
        {
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
            ConverterCollection.CurrentInstance
                .Add<float[], float[,], Transpose1xN>((a) =>
                {
                    var result = new float[a.Length, 1];
                    for(int i = 0; i < a.Length; result[i, 0] = a[i], i++)
                        ;
                    return result;
                });
            //add an disambiguate:
            //Transpose4x4 is a disambiguate to  Transpose2x2
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
        }

        [Test(Description = "Initialize the tests")]
        public void ConverterCollection_Add_Transformers_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            var cc = new ConverterCollection();
            var count = cc.Count;
            Add_Five_Matrix_Transformers();
            Assert.AreEqual(cc.Count, count + 5);
        }

        [Test(Description = "Initialize the tests")]
        public void Transform_Transpose4x4_Float_Matrix_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();

            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            var matrixTransposed = (float[,])matrix.Transform<Transpose4x4>();

            Assert.AreEqual(matrixTransposed.CastTo<string>(), "\r\n[ -1,5  0,5 \r\n 1  0 \r\n]");

            Assert.AreEqual(matrixTransposed, new float[,] { { -1.5f, 0.5f }, { 1f, 0f } });
        }

        [Test(Description = "Initialize the tests")]
        public void Transform_Transpose1xN_Float_Matrix_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            var vector = new[] { 1f, 2f, 3f, 4f };
            var matrixTransposed1xEx1 = (float[,])vector.Transform<Transpose1xN>();
            Assert.AreEqual(matrixTransposed1xEx1.CastTo<string>(), "\r\n[ 1  2  3  4 ]");

            Assert.AreEqual(matrixTransposed1xEx1, new float[,] { { 1f }, { 2f }, { 3f }, { 4f } });
        }

        [Test(Description = "Initialize the tests")]
        public void Transform_Transpose1xN_Float_Matrix_Typed_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            var vector = new[] { 1f, 2f, 3f, 4f };

            var matrixTransposed1xEx2 = vector.Transform<Transpose1xN, float[,]>();

            Assert.AreEqual(matrixTransposed1xEx2.CastTo<string>(), "\r\n[ 1  2  3  4 ]");

            Assert.AreEqual(matrixTransposed1xEx2, new float[,] { { 1f }, { 2f }, { 3f }, { 4f } });
        }

        [Test(Description = "Initialize the tests")]
        public void Transform_Transpose2x2_Float_Matrix_Typed_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            var matrixTransposed = matrix.Transform<Transpose2x2>();

            Assert.AreEqual(matrixTransposed.CastTo<string>(), "\r\n[ -1,5  0,5 \r\n 1  0 \r\n]");

            Assert.AreEqual(matrixTransposed, new float[,] { { -1.5f, 0.5f }, { 1f, 0f } });
        }

        [Test(Description = "Initialize the tests")]
        public void ConverterCollection_CanConvertTo_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();

            Assert.IsFalse(ConverterCollection.CurrentInstance.CanConvertTo<float>());

            Assert.IsTrue(ConverterCollection.CurrentInstance.CanConvertTo<object, float>(0f));

            Assert.IsTrue(ConverterCollection.CurrentInstance.CanConvertTo<float[,]>());

            Assert.IsFalse(ConverterCollection.CurrentInstance.CanConvertTo<float[][]>());

            Assert.IsFalse(ConverterCollection.CurrentInstance.CanConvertTo<float[]>());

            Assert.IsFalse(ConverterCollection.CurrentInstance.CanConvertTo<float[,,]>());
        }

        [Test(Description = "Initialize the tests")]
        public void ConverterCollection_CanConvertFrom_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();

            Assert.IsFalse(ConverterCollection.CurrentInstance.CanConvertFrom<float>());

            Assert.IsFalse(ConverterCollection.CurrentInstance.CanConvertFrom<float[][]>());

            Assert.IsTrue(ConverterCollection.CurrentInstance.CanConvertFrom<float[,]>());

            Assert.IsTrue(ConverterCollection.CurrentInstance.CanConvertFrom<float[]>());
        }

        [Test(Description = "Initialize the tests")]
        public void Transform_Alias_ByTypeOf_Argument_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            var matrixTransposedEx4 = matrix.Transform(typeof(Transpose2x2));
            Assert.AreEqual(matrixTransposedEx4.CastTo<string>(), "\r\n[ -1,5  0,5 \r\n 1  0 \r\n]");
        }

        [Test(Description = "Initialize the tests")]
        public void Transform_Alias_By_Delegate_Typed_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            var matrixTransposedEx5 = matrix.Transform<Transpose2x2>().CastTo<float[,]>();
            Assert.AreEqual(matrixTransposedEx5, new float[,] { { -1.5f, 0.5f }, { 1f, 0f } });

            Assert.AreEqual(matrixTransposedEx5.CastTo<string>(), "\r\n[ -1,5  0,5 \r\n 1  0 \r\n]");
        }

        [Test(Description = "Initialize the tests")]
        public void Transform_Alias_By_Enum_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            var matrixTransposedEx6 = matrix.Transform((a) => 1f, Transform.MatrixReturnUnit);
            Assert.AreEqual(matrixTransposedEx6, 1f);

            Assert.AreEqual(matrixTransposedEx6.CastTo<string>(), 1f.ToString());
        }


        [Test(Description = "Initialize the tests")]
        public void Transform_Alias_Add_And_Invoke_Typed_By_Enum_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            var matrixTransposedEx6 = matrix.Transform((a) => 1f, Transform.MatrixReturnUnit);
            var matrixTransposedEx7 = matrix.Transform<float>(Transform.MatrixReturnUnit);

            Assert.AreEqual(matrixTransposedEx7, 1f);
            Assert.AreEqual(matrixTransposedEx7, matrixTransposedEx6);

            Assert.AreEqual(matrixTransposedEx7.CastTo<string>(), 1f.ToString());
        }

        [Test(Description = "Initialize the tests")]
        public void Transform_Alias_Add_By_Attribute_Scan_And_Invoke_Typed_By_Named_Argument_Enum_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            ConverterCollection.CurrentInstance.Initialize(this.GetType().Assembly);
            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            var matrixTransposedEx8 = matrix.Transform<float>(functionAlias: Transform.MatrixReturnUnit);

            Assert.AreEqual(matrixTransposedEx8, 1f);

            Assert.AreEqual(matrixTransposedEx8.CastTo<string>(), 1f.ToString());
        }

        [Test(Description = "Initialize the tests")]
        public void Transform_Alias_Add_And_Invoke_Typed_By_Non_Ambiguity_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            var matrixTransposedEx9 = matrix.Transform((a) => new float[,] { { a[0, 0] * a[1, 1] }, { a[0, 1] * a[1, 0] } }, Transform.MatrixMulDiagonal);

            Assert.AreEqual(matrixTransposedEx9, new float[,] { { 0f }, { 2f } });

            Assert.AreEqual(matrixTransposedEx9.CastTo<string>(), "\r\n[ 0  2 ]");
        }

        [Test(Description = "Initialize the tests")]
        public void Transform_Matrix_Alias_Add_And_Invoke_Typed_By_Enum_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            ConverterCollection.CurrentInstance.Initialize(this.GetType().Assembly);

            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            var matrixTransposedEx10 = matrix.Transform<float[,]>(Transform.MatrixMulDiagonal);

            Assert.AreEqual(matrixTransposedEx10, new float[,] { { 0f }, { 2f } });

            Assert.AreEqual(matrixTransposedEx10.CastTo<string>(), "\r\n[ 0  2 ]");
        }

        [Test(Description = "Initialize the tests")]
        public void TryTransform_Okay()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();
            var matrix = new float[,] { { 0, 1 }, { 2, 3 } };

            float matrix2x2Det;
            var count = ConverterCollection.CurrentInstance.Count;
            // false, as no transformer for float[,] -> float exists
            Assert.IsFalse(matrix.TryTransform(out matrix2x2Det));

            //after attribute scan a converter exists
            ConverterCollection.CurrentInstance.Initialize(this.GetType().Assembly);

            Assert.That(ConverterCollection.CurrentInstance.Count, Is.GreaterThan(count));

            Assert.IsTrue(matrix.TryTransform(out matrix2x2Det));
            if(matrix.TryTransform(out matrix2x2Det) == true)
            {
                Assert.AreEqual(matrix2x2Det, 1.CastTo<float>());
            }

            Assert.AreEqual(matrix2x2Det.CastTo<string>(), 1f.CastTo<string>());
        }


        [Test(Description = "Throw an error by passing a delegate that does not match the argument types")]
        public void TryTransform_Alias_By_Delegate_Typed_Exception()
        {
            ConverterCollection.CurrentInstance.Dispose();

            Add_Five_Matrix_Transformers();

            TestDelegate testDelegate = () => {
                var matrix = new float[,] { { 0, 1 }, { 2, 3 } };
                float[] matrix2x2Det2;
                if(matrix.TryTransform<Transpose1xN, float[]>(out matrix2x2Det2, throwException: true) == true)
                {
                    //... perform further steps on result
                    Console.WriteLine($"{nameof(matrix2x2Det2)}: {matrix2x2Det2}");
                }
            };

            Assert.That(testDelegate, Throws.TypeOf<ConverterException>().With.Property("Cause").EqualTo(ConverterCause.LogicError));
        }

    }
}
