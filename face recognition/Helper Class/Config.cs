using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face_Recognition_Demo_V1.Helper_Class
{
    public static class Config
    {
        public static string dataDirectory = @"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Augmented Face Dataset (Grayscale)";
        public static string configFile = @"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Recognition Demo V1\Resources\deploy.prototxt.txt";
        public static string faceModel = @"D:\Visual Studio Projects\FINAL VERSIONS FOR FACE RECOGNITION IMPLEMENTATION\Face Recognition Demo V1\Resources\res10_300x300_ssd_iter_140000_fp16.caffemodel";
    }
}
