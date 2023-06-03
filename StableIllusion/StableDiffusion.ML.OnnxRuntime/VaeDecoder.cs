using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Drawing;

namespace StableDiffusion.ML.OnnxRuntime
{
    public static class VaeDecoder
    {
        public static Tensor<float> Decoder(List<NamedOnnxValue> input, string VaeDecoderOnnxPath)
        {
            var sessionOptions = new SessionOptions();
            sessionOptions.EnableMemoryPattern = false;
            // Create an InferenceSession from the Model Path.
            var vaeDecodeSession = new InferenceSession(VaeDecoderOnnxPath, sessionOptions);

           // Run session and send the input data in to get inference output. 
            var output = vaeDecodeSession.Run(input);
            var result = (output.ToList().First().Value as Tensor<float>);

            vaeDecodeSession.Dispose();

            return result;
        }

        /// <summary>
        /// Convert float array to a Win32 Bitmap image
        /// </summary>
        /// <param name="output"> The float array containing output</param>
        /// <param name="config">StableDiffusion config</param>
        /// <param name="width">Bitmap width</param>
        /// <param name="height">Bitmap height</param>
        /// <returns>A Bitmap based on the contents of the float array</returns>
        public static Bitmap ConvertToBitmapImage(Tensor<float> output, StableDiffusionConfig config, int width = 512, int height = 512)
        {
            var bmpImage = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var clrNewColor = System.Drawing.Color.FromArgb(
                        (int)(Math.Round(Math.Clamp((output[0, 0, y, x] / 2 + 0.5), 0, 1) * 255)),
                        (int)(Math.Round(Math.Clamp((output[0, 1, y, x] / 2 + 0.5), 0, 1) * 255)),
                        (int)(Math.Round(Math.Clamp((output[0, 2, y, x] / 2 + 0.5), 0, 1) * 255))
                        );

                    bmpImage.SetPixel(x, y, clrNewColor);
                }
            }

            return bmpImage;
        }
    }
}
