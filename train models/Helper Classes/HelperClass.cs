
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face_Recognition_Training_V1.Helper_Classes
{
    public class HelperClass
    {
        public static T[,] To2D<T>(T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }

        public static (List<FaceData>, List<FaceData>) TestTrainSplit(List<FaceData> data, float split = 0.8f)
        {
            try
            {
                if (data.Count < 1)
                {
                    throw new Exception("Data is not found.");
                }

                int numTrainSamples = (int)Math.Floor(data[0].Images.Count * split);
                int numTestSamples = data[0].Images.Count - numTrainSamples;

                if (numTrainSamples == 0 || numTestSamples == 0)
                {
                    throw new Exception("Insufficient training or testing data.");
                }

                List<FaceData> TestData = (from d in data
                                           select new FaceData
                                           {
                                               Images = d.Images.Take(numTestSamples).ToList(),
                                               Label = d.Label
                                           }).ToList();

                List<FaceData> TrainData = (from d in data
                                            select new FaceData
                                            {
                                                Images = d.Images.Skip(numTestSamples)
                                                .Take(numTrainSamples).ToList(),
                                                Label = d.Label
                                            }).ToList();
                return (TrainData, TestData);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int[,] ComputeConfusionMatrix(int[] actual, int[] predicted)
        {
            try
            {
                if (actual.Length != predicted.Length)
                {
                    throw new Exception("Vectors lengths not matched");
                }

                int NoClasses = actual.Distinct().Count();
                int[,] CM = new int[NoClasses, NoClasses];
                for (int i = 0; i < actual.Length; i++)
                {
                    int r = predicted[i];
                    int c = actual[i];
                    CM[r, c]++;
                }
                return CM;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int[] GetDiagonal(int[,] matrix)
        {
            return Enumerable.Range(0, matrix.GetLength(0)).Select(i => matrix[i, i]).ToArray();
        }

        public static int[] GetSumCols(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[] colSum = new int[cols];

            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    colSum[col] += matrix[row, col];
                }
            }
            return colSum;
        }

        public static int[] GetSumRows(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[] rowSum = new int[cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    rowSum[row] += matrix[row, col];
                }
            }
            return rowSum;
        }

        public static double[] CalculateMetrics(int[,] CM, int[] actual, int[] predicted)
        {
            try
            {
                double[] metrics = new double[3];
                int samples = actual.Length;
                int classes = (int)CM.GetLongLength(0);
                var diagonal = GetDiagonal(CM);
                var diagonalSum = diagonal.Sum();

                int[] ColTotal = GetSumCols(CM);
                int[] RowTotal = GetSumRows(CM);

                // Accuracy
                var accuracy = diagonalSum / (double)samples;

                // predicion
                var precision = new double[classes];
                for (int i = 0; i < classes; i++)
                {
                    precision[i] = diagonal[i] == 0 ? 0 : (double)diagonal[i] / ColTotal[i];
                }

                // Recall
                var recall = new double[classes];
                for (int i = 0; i < classes; i++)
                {
                    recall[i] = diagonal[i] == 0 ? 0 : (double)diagonal[i] / RowTotal[i];
                }

                metrics[0] = accuracy;
                metrics[1] = precision.Average();
                metrics[2] = recall.Average();

                return metrics;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Image<Bgr, byte> HConcatenateImages(Image<Bgr, byte> img1, Image<Bgr, byte> img2)
        {
            try
            {
                int MaxRows = img1.Rows > img2.Rows ? img1.Rows : img2.Rows;
                int totalCols = img1.Cols + img2.Cols;

                Image<Bgr, byte> imgOutput = new Image<Bgr, byte>(totalCols, MaxRows, new Bgr(0, 0, 0));


                imgOutput.ROI = new Rectangle(0, 0, img1.Width, img1.Height);
                img1.CopyTo(imgOutput);
                imgOutput.ROI = Rectangle.Empty;

                imgOutput.ROI = new Rectangle(img1.Width, 0, img2.Width, img2.Height);
                img2.CopyTo(imgOutput);
                imgOutput.ROI = Rectangle.Empty;
                return imgOutput;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
