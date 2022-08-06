using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Face_Recognition_Training_V1.Helper_Classes
{
    public class FaceData
    {
        public int Label { get; set; }
        public string? Name { get; set; }
        public List<Image<Gray, byte>> Images { get; set; }
    }
}
