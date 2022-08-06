using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Face_Recognition_Training_V1.Helper_Classes;

namespace Face_Recognition_Training_V1
{
    class Program
    {

        static List<int> PredictedLabels = new List<int>();
        static List<int> ActualLabels = new List<int>();
        static List<FaceData> TestingData = new List<FaceData>();
        static EigenFaceRecognizer eigenFaceRecognizer = new EigenFaceRecognizer();
        static FisherFaceRecognizer fisherFaceRecognizer = new FisherFaceRecognizer();
        static LBPHFaceRecognizer lbphFaceRecognizer = new LBPHFaceRecognizer();
        private static void testModel(int choice)
        {
            FaceRecognizer.PredictionResult result;
            result.Label = -1;

            PredictedLabels.Clear();
            ActualLabels.Clear();

            foreach (var item in TestingData)
            {
                foreach (var img in item.Images)
                {
                    if(choice == 1) result = eigenFaceRecognizer.Predict(img.Resize(224, 224, Inter.Cubic));
                    else if (choice == 2) result = fisherFaceRecognizer.Predict(img.Resize(224, 224, Inter.Cubic));
                    else if (choice == 3) result = lbphFaceRecognizer.Predict(img.Resize(224, 224, Inter.Cubic));
                    PredictedLabels.Add(result.Label);
                    ActualLabels.Add(item.Label);
                }
            }

            var cm = HelperClass.ComputeConfusionMatrix(ActualLabels.ToArray(), PredictedLabels.ToArray());
            var metrics = HelperClass.CalculateMetrics(cm, ActualLabels.ToArray(), PredictedLabels.ToArray());
            string results = $"EIGENFACES MODEL TESTED \n Test Samples = {ActualLabels.Count} \n Accuracy = {metrics[0] * 100}% " +
                $"\nPrecision = {metrics[1] * 100}% \n Recall = {metrics[2] * 100}%";
            
        }

        static void Main(string[] args)
        {
            List<FaceData> DataSet = new List<FaceData>();
            List<FaceData> TrainingData = new List<FaceData>();
            VectorOfMat imageList = new VectorOfMat();
            VectorOfInt labelList = new VectorOfInt();

            string[] subDirectories = Directory.GetDirectories(Config.dataDirectory);
            int count = 0;
            foreach (string subDirectory in subDirectories)
            {
                var directory = new DirectoryInfo(subDirectory);
                FileInfo[] files = directory.GetFiles();

                List<int> labels = new List<int>();
                VectorOfMat images = new VectorOfMat();
                int i = 0;
                foreach (FileInfo file in files)
                {
                    var image = new Image<Gray, byte>(directory + "\\" + file.Name).Resize(224, 224, Inter.Cubic);
                    int label = count;
                    var index = DataSet.FindIndex(x => x.Label == label);
                    if (index > -1)
                    {
                        
                        DataSet[index].Images.Add(image);
                    }
                    else
                    {
                        FaceData face = new FaceData();
                        face.Images = new List<Image<Gray, byte>>();
                        face.Images.Add(image);
                        if (file.Directory is not null) face.Name = file.Directory.Name;
                        face.Label = label;
                        DataSet.Add(face);
                    }
                    images.Push(image.Mat);
                    labels.Add(label);
                }
                imageList.Push(images);
                labelList.Push(labels.ToArray());
                count++;
            }

            (TrainingData, TestingData) = HelperClass.TestTrainSplit(DataSet);

            eigenFaceRecognizer = new EigenFaceRecognizer(imageList.Size);
            eigenFaceRecognizer.Train(imageList, labelList);
            eigenFaceRecognizer.Write(@"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Recognition Training V1\Models\trainedEigenFaceModel");
            testModel(1);

            fisherFaceRecognizer = new FisherFaceRecognizer(imageList.Size);
            fisherFaceRecognizer.Train(imageList, labelList);
            fisherFaceRecognizer.Write(@"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Recognition Training V1\Models\trainedFisherFaceModel");
            testModel(2);

            lbphFaceRecognizer.Train(imageList, labelList);
            lbphFaceRecognizer.Write(@"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Recognition Training V1\Models\trainedLBPHModel");
            testModel(3);
        }
    }
}