using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.ColorReduction;
using System.IO;
using AForge.Math.Random;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace Image_Augmentation_Grayscale
{
    class Program
    {
        static string outputDirectory = @"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Augmented Face Dataset (Grayscale)\";
        static string dataDirectory = @"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Dataset\";
        static int count = 0;

        public static Bitmap toGrayscale(Bitmap image)
        {
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            return filter.Apply(image);
        }

        public static Bitmap AddRotateFlip(Bitmap image, RotateFlipType type)
        {
            image.RotateFlip(type);
            return image;
        }
        public static Bitmap useRotateNearestNeighbor(Bitmap image, double angle)
        {
            RotateNearestNeighbor filter = new RotateNearestNeighbor(angle, true);
            return filter.Apply(image);
        }
        public static List<Bitmap> imageRotateAndFlip(Bitmap image)
        {
            var imageVariations = new List<Bitmap>() {
            { AddRotateFlip((Bitmap)image.Clone(), RotateFlipType.RotateNoneFlipNone) },
            { AddRotateFlip((Bitmap)image.Clone(), RotateFlipType.Rotate90FlipNone)   },
            {AddRotateFlip((Bitmap)image.Clone(), RotateFlipType.Rotate180FlipNone)  },
            { AddRotateFlip((Bitmap)image.Clone(), RotateFlipType.Rotate270FlipNone)  },
            { AddRotateFlip((Bitmap)image.Clone(), RotateFlipType.RotateNoneFlipX)    },
            {  AddRotateFlip((Bitmap)image.Clone(), RotateFlipType.Rotate90FlipX)      },
            {  AddRotateFlip((Bitmap)image.Clone(), RotateFlipType.Rotate180FlipX)     },
            {  AddRotateFlip((Bitmap)image.Clone(), RotateFlipType.Rotate270FlipX)     },
            {  useRotateNearestNeighbor((Bitmap)image.Clone(), 30)     },
            {  useRotateNearestNeighbor((Bitmap)image.Clone(), 120)     },
            {  useRotateNearestNeighbor((Bitmap)image.Clone(), 210)     },
            {  useRotateNearestNeighbor((Bitmap)image.Clone(), 300)     },
            };
            return imageVariations;
        }

        public static Bitmap changeBrightness(Bitmap image, int adjustValue)
        {
            BrightnessCorrection brightnessCorrection = new BrightnessCorrection(adjustValue);
            return brightnessCorrection.Apply(image);
        }

        public static Bitmap changeContrast(Bitmap image, int factor)
        {
            ContrastCorrection contrastCorrection = new ContrastCorrection(factor);
            return contrastCorrection.Apply(image);
        }

        public static List<Bitmap> imageFilters(Bitmap image)
        {
            var imageVariations = new List<Bitmap>() {
            { (Bitmap)image.Clone() },
            { changeBrightness((Bitmap)image.Clone(), -60) },
            { changeBrightness((Bitmap)image.Clone(), -30) },
            { changeBrightness((Bitmap)image.Clone(), 30) },
            { changeBrightness((Bitmap)image.Clone(), 60) },
            { changeContrast((Bitmap)image.Clone(), -60) },
            { changeContrast((Bitmap)image.Clone(), -30) },
            { changeContrast((Bitmap)image.Clone(), 30) },
            { changeContrast((Bitmap)image.Clone(), 60) },
            };
            return imageVariations;
        }

        public static Bitmap useAdditiveNoise(Bitmap image)
        {
            IRandomNumberGenerator generator = new UniformGenerator(new AForge.Range(-50, 50));
            AdditiveNoise filter = new AdditiveNoise(generator);
            return filter.Apply(image);
        }

        public static Bitmap randomCutout(Mat image)
        {
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                int x = rand.Next(0, 224);
                int y = rand.Next(0, 224);
                Cv2.Rectangle(image, new Point(x, y), new Point(x + 40, y + 40), Scalar.Black, -1);
            }
            Mat n = new Mat();

            return image.ToBitmap();
        }
        public static Bitmap addGaussianBlur(Bitmap image, double sigma, int size)
        {
            GaussianBlur gaussianBlur = new GaussianBlur(sigma, size);
            return gaussianBlur.Apply(image);
        }
        public static Bitmap addSaltAndPepperNoise(Bitmap image, float noiseAmount)
        {
            SaltAndPepperNoise saltAndPepperNoise = new SaltAndPepperNoise(noiseAmount);
            return saltAndPepperNoise.Apply(image);
        }

        public static List<Bitmap> addNoises(Bitmap image)
        {
            var imageVariations = new List<Bitmap>() {
                { randomCutout(image.ToMat().Clone()) },
                { addSaltAndPepperNoise((Bitmap)image.Clone(), 4f) },
                { addGaussianBlur((Bitmap)image.Clone(), 4, 8) },
                { useAdditiveNoise((Bitmap)image.Clone()) },
            };
            return imageVariations;
        }

        public static void saveImage(Bitmap image, string filename)
        {
            if (!Directory.Exists(outputDirectory + filename)) Directory.CreateDirectory(outputDirectory + filename);
            image.Save(outputDirectory + filename + "\\" + count++ + ".jpg");
        }

        static void Main(string[] args)
        {
            string[] subDirectories = Directory.GetDirectories(dataDirectory);

            foreach (string subDirectory in subDirectories)
            {
                var directory = new DirectoryInfo(subDirectory);
                FileInfo[] files = directory.GetFiles();
                count = 0;
                foreach (FileInfo file in files)
                {
                    Bitmap image = new Bitmap(directory + "\\" + file.Name);
                    image = toGrayscale(image);
                    var imageVariations1 = imageRotateAndFlip(image);

                    foreach (Bitmap v1 in imageVariations1)
                    {
                        var imageVariations2 = imageFilters(v1);
                        foreach (Bitmap v2 in imageVariations2)
                        {
                            saveImage(v2, directory.Name);
                            var imageVariations3 = addNoises(v2);
                            foreach (Bitmap v3 in imageVariations3)
                            {
                                saveImage(v3, directory.Name);
                            }
                        }
                    }
                }
            }
        }
    }
}
