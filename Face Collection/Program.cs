using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using Face_Collection_V2.Helper_Class;

namespace Face_Collection_V2
{
    class Program
    {

        static void Main(string[] args)
        {
            Net? faceNet = CvDnn.ReadNetFromCaffe(Config.configFile, Config.faceModel);
            string[] subDirectories = Directory.GetDirectories(Config.dataDirectory);

            foreach (string subDirectory in subDirectories)
            {
                var directory = new DirectoryInfo(subDirectory);
                FileInfo[] files = directory.GetFiles();
                foreach (FileInfo file in files)
                {
                    Mat newImage = new Mat(directory + "\\" + file.Name);
                    int frameHeight = newImage.Rows;
                    int frameWidth = newImage.Cols;

                    var blob = CvDnn.BlobFromImage(newImage, 1.0, new Size(300, 300),
                        new Scalar(104, 117, 123), false, false);
                    
                    if(faceNet is not null) faceNet.SetInput(blob, "data");

                    var detection = (faceNet is not null)? faceNet.Forward("detection_out"): new Mat();
                    var detectionMat = new Mat(detection.Size(2), detection.Size(3), MatType.CV_32F,
                        detection.Ptr(0));
                    for (int i = 0; i < detectionMat.Rows; i++)
                    {
                        float confidence = detectionMat.At<float>(i, 2);

                        if (confidence > 0.7)
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

                                if(file.Directory is not null && !Directory.Exists(Config.FacePhotosPath + file.Directory.Name)) System.IO.Directory.CreateDirectory(Config.FacePhotosPath + file.Directory.Name);

                                int count = 0;
                                if (file.Directory is not null && Directory.EnumerateFileSystemEntries(Config.FacePhotosPath + file.Directory.Name).Any())
                                {
                                    directory = new DirectoryInfo(Config.FacePhotosPath + file.Directory.Name);
                                    var latestFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                                    var filename = latestFile.Name;
                                    count = Int32.Parse(filename.Split('.')[0]) + 1;
                                }

                                //Save detected face
                                Mat imageToSave = new Mat();
                                Cv2.Resize(detectedFace, imageToSave, new Size(224, 224));
                                if (file.Directory is not null) imageToSave.SaveImage(Config.FacePhotosPath + file.Directory.Name + "\\" + count + Config.ImageFileExtension);
                                
                            }
                        }
                    }
                }
            }
        }
    }
}