using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face_Collection_V2.Helper_Class
{
    public class Config
    {
        public static string dataDirectory = @"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Image Collection\";
        public static string FacePhotosPath = @"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Dataset\";
        public static string ImageFileExtension = ".jpg";
        public static string configFile = @"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Collection V2\Detector Model\deploy.prototxt.txt";
        public static string faceModel = @"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Collection V2\Detector Model\res10_300x300_ssd_iter_140000_fp16.caffemodel";
    }
}
