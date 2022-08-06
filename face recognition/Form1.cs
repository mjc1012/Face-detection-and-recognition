using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Face;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;
using Face_Recognition_Demo_V1.Helper_Class;

namespace Face_Recognition_Demo_V1
{
    public partial class Form1 : Form
    {
        VideoCapture videoCapture;
        Mat image;
        Thread cameraThread;
        Net faceNet;
        bool fps = false;
        bool runCamera = false;
        bool doFaceDetection = false;

        List<string> nameList = new List<string>();
        EigenFaceRecognizer eigenFaceRecognizer = EigenFaceRecognizer.Create();
        FisherFaceRecognizer fisherFaceRecognizer = FisherFaceRecognizer.Create();
        LBPHFaceRecognizer lbphFaceRecognizer = LBPHFaceRecognizer.Create();

        bool eigenFaceTrained = false;
        bool fisherFaceTrained = false;
        bool lbphTrained = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            faceNet = CvDnn.ReadNetFromCaffe(Config.configFile, Config.faceModel);
            string[] subDirectories = Directory.GetDirectories(Config.dataDirectory);
            foreach (string subDirectory in subDirectories)
            {
                var directory = new DirectoryInfo(subDirectory);
                FileInfo[] files = directory.GetFiles();
                Console.WriteLine(files[0].Directory.Name);
                nameList.Add(files[0].Directory.Name);
            }
            eigenFaceRecognizer.Read(@"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Recognition Demo V1\Models\trainedEigenFaceModel");
            fisherFaceRecognizer.Read(@"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Recognition Demo V1\Models\trainedFisherFaceModel");
            lbphFaceRecognizer.Read(@"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Recognition Demo V1\Models\trainedLBPHModel");
        }

        private void btnOpenCamera_Click(object sender, EventArgs e)
        {

            if (videoCapture != null) videoCapture.Dispose();
            videoCapture = new VideoCapture(0);
            image = new Mat();
            cameraThread = new Thread(new ThreadStart(CaptureCameraCallback));
            cameraThread.Start();
            runCamera = true;
        }

        private void CaptureCameraCallback()
        {
            while (runCamera && videoCapture != null)
            {
                var startTime = DateTime.Now;

                videoCapture.Read(image);
                if (image.Empty()) return;
                var newImage = new Mat();
                Cv2.Resize(image, newImage, new Size(320, 240));
                if (doFaceDetection)
                {
                    int frameHeight = newImage.Rows;
                    int frameWidth = newImage.Cols;

                    var blob = CvDnn.BlobFromImage(newImage, 1.0, new Size(300, 300),
                        new Scalar(104, 117, 123), false, false);
                    faceNet.SetInput(blob, "data");

                    var detection = faceNet.Forward("detection_out");
                    var detectionMat = new Mat(detection.Size(2), detection.Size(3), MatType.CV_32F,
                        detection.Ptr(0));
                    for (int i = 0; i < detectionMat.Rows; i++)
                    {
                        float detectionConfidence = detectionMat.At<float>(i, 2);

                        if (detectionConfidence > 0.7)
                        {
                            int x1 = (int)(detectionMat.At<float>(i, 3) * frameWidth);
                            int y1 = (int)(detectionMat.At<float>(i, 4) * frameHeight);
                            int x2 = (int)(detectionMat.At<float>(i, 5) * frameWidth);
                            int y2 = (int)(detectionMat.At<float>(i, 6) * frameHeight);

                            Rect roi = new Rect(x1, y1, x2 - x1, y2 - y1);
                            //Assign the face to the picture Box face pictureBox1
                            if (0 <= x1 && 0 <= x2 - x1 && x1 + (x2 - x1) <= frameWidth && 0 <= y1 && 0 <= y2 - y1 && y1 + (y2 - y1) <= frameHeight)
                            {
                                var detectedFace = newImage.Clone(roi);
                                Cv2.CvtColor(detectedFace, detectedFace, ColorConversionCodes.BGR2GRAY);
                                Cv2.Rectangle(newImage, new Point(x1, y1), new Point(x2, y2), Scalar.Green, 2);
                                Cv2.Resize(detectedFace, detectedFace, new Size(224, 224));

                                int predictedLabel = 0;
                                double recognitionConfidence = 0;

                                //predict result
                                if (eigenFaceTrained)
                                {
                                    eigenFaceRecognizer.Predict(detectedFace, out predictedLabel,out recognitionConfidence);
                                    // The dependence of the degree of similarity on the difference of images approximately has the form
                                    // 10 -> 99%; 100 -> 90%; 750 -> 50%, 10000 -> 1%
                                    // then you need to enter the scale (for example, linear):
                                    // Algorithm for calculating the degree of similarity.
                                    float threshold = 750;              // threshold equal to 50% similarity of images (set experimentally)
                                    float thresholdMismatch = 10000;    // threshold value of image mismatch (equal to 1%, experimentally established)    
                                    if (recognitionConfidence < threshold)
                                        recognitionConfidence = (100 - (recognitionConfidence * 50.0 / threshold));
                                    else
                                        recognitionConfidence = (50 - (recognitionConfidence * 50 / thresholdMismatch));
                                }
                                else if (fisherFaceTrained)
                                {
                                    fisherFaceRecognizer.Predict(detectedFace, out predictedLabel, out recognitionConfidence);
                                    // The dependence of the degree of similarity on the difference of images approximately has the form
                                    // 10 -> 99%; 100 -> 90%; 750 -> 50%, 10000 -> 1%
                                    // then you need to enter the scale (for example, linear):
                                    // Algorithm for calculating the degree of similarity.
                                    float threshold = 750;              // threshold equal to 50% similarity of images (set experimentally)
                                    float thresholdMismatch = 10000;    // threshold value of image mismatch (equal to 1%, experimentally established)    
                                    if (recognitionConfidence < threshold)
                                        recognitionConfidence = (100 - (recognitionConfidence * 50.0 / threshold));
                                    else
                                        recognitionConfidence = (50 - (recognitionConfidence * 50 / thresholdMismatch));
                                }

                                else if (lbphTrained)
                                {
                                    lbphFaceRecognizer.Predict(detectedFace, out predictedLabel, out recognitionConfidence);
                                }

                                //Display predicted name and image
                                if (recognitionConfidence > 0)
                                {
                                    Cv2.PutText(newImage, nameList[predictedLabel] + ": " + String.Format("{0:0.00}%", recognitionConfidence), new Point(x1 - 40, y2 + 20),
                                    HersheyFonts.HersheyComplexSmall, 1, Scalar.Orange, 2);
                                }
                                else
                                {
                                    Cv2.PutText(newImage, "Face Detected: " + String.Format("{0:0.00}%", detectionConfidence * 100), new Point(x1 - 60, y2 + 20),
                                  HersheyFonts.HersheyComplexSmall, 1, Scalar.Green, 2);
                                }
                            }
                        }
                    }

                }
                var bmpEffect = BitmapConverter.ToBitmap(newImage);

                picVideoCapture.Image = bmpEffect;
            }
        }

        private void btnStopCamera_Click(object sender, EventArgs e)
        {
            runCamera = false;
            cameraThread.Interrupt();
            videoCapture.Release();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            cameraThread.Interrupt();
            videoCapture.Dispose();
        }

        private void btnUseEigenFace_Click(object sender, EventArgs e)
        {
            eigenFaceTrained = !eigenFaceTrained;
            fisherFaceTrained = false;
            lbphTrained = false;
        }

        private void btnUseFisherFace_Click(object sender, EventArgs e)
        {
            eigenFaceTrained = false;
            fisherFaceTrained = !fisherFaceTrained;
            lbphTrained = false;
        }

        private void btnUseLBPH_Click(object sender, EventArgs e)
        {
            eigenFaceTrained = false;
            fisherFaceTrained = false;
            lbphTrained = !lbphTrained;
        }

        private void btnDetectFace_Click(object sender, EventArgs e)
        {
            doFaceDetection = !doFaceDetection;
        }
    }
}
