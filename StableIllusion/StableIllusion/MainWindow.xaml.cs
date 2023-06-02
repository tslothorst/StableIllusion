using StableDiffusion.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StableIllusion.Extensions;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace StableIllusion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var strPositivePromptInput = tbPositivePromptInput.Text;

                if (string.IsNullOrWhiteSpace(strPositivePromptInput))
                {
                    MessageBox.Show("The prompt field cannot be empty or only white spaces", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes);
                    return;
                }

                var config = new StableDiffusionConfig
                {
                    // Number of denoising steps
                    NumInferenceSteps = 15,
                    // Scale for classifier-free guidance
                    GuidanceScale = 7.5,
                    // Set your preferred Execution Provider. Currently (GPU, DirectML, CPU) are supported in this project.
                    // ONNX Runtime supports many more than this. Learn more here: https://onnxruntime.ai/docs/execution-providers/
                    // The config is defaulted to CUDA. You can override it here if needed.
                    // To use DirectML EP intall the Microsoft.ML.OnnxRuntime.DirectML and uninstall Microsoft.ML.OnnxRuntime.GPU
                    ExecutionProviderTarget = StableDiffusionConfig.ExecutionProvider.Cuda,
                    // Set GPU Device ID.
                    DeviceId = 0,
                    // Update paths to your models
                    TextEncoderOnnxPath = @"D:\Development\Github\StableDiffusion\StableDiffusion\text_encoder\model.onnx",
                    UnetOnnxPath = @"D:\Development\Github\StableDiffusion\StableDiffusion\unet\model.onnx",
                    VaeDecoderOnnxPath = @"D:\Development\Github\StableDiffusion\StableDiffusion\vae_decoder\model.onnx",
                    SafetyModelPath = @"D:\Development\Github\StableDiffusion\StableDiffusion\safety_checker\model.onnx",
                };

                var imgResult = await Task.FromResult(UNet.Inference(strPositivePromptInput, config));

                if (imgResult == null) 
                {
                    MessageBox.Show("The image could not be created", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes);
                    return;
                }

                imgOutput.Source = imgResult.ToBitmapImage();                
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnClearInput_Click(object sender, RoutedEventArgs e)
        {
            tbPositivePromptInput.Clear();
        }

        private void btnClearOutputImage_Click(object sender, RoutedEventArgs e)
        {
            if (imgOutput.Source != null)
            {
                imgOutput.Source = null;                
            }
        }
    }
}
