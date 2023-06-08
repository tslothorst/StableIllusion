using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableIllusion
{
    public class AppSettings
    {
        public string TextEncoderOnnxPath { get; set; }
        public string UnetOnnxPath { get; set; }
        public string VaeDecoderOnnxPath { get; set; }
        public string SafetyModelPath { get; set; }
    }
}
