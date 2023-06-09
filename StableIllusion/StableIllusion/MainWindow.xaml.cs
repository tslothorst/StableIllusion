using StableDiffusion.ML.OnnxRuntime;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using StableIllusion.Extensions;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using System.IO;

namespace StableIllusion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppSettings settings;

        public MainWindow(IOptions<AppSettings> settings)
        {
            this.settings = settings.Value;
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
                    TextEncoderOnnxPath = settings.TextEncoderOnnxPath,
                    UnetOnnxPath = settings.UnetOnnxPath,
                    VaeDecoderOnnxPath = settings.VaeDecoderOnnxPath,
                    SafetyModelPath = settings.SafetyModelPath,
                };

                int.TryParse(cbInferenceSteps.Text, out config.NumInferenceSteps);
                double.TryParse(cbGuidanceScale.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out config.GuidanceScale);

                if (cbExecutionProvider.Text == "CPU")
                {
                    config.ExecutionProviderTarget = StableDiffusionConfig.ExecutionProvider.Cpu;
                }

                var imgResult = await Task.Run(()=> UNet.Inference(strPositivePromptInput, config));

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

        private void btnCopyImageToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (imgOutput.Source != null)
            {
                Clipboard.SetImage(imgOutput.Source as BitmapImage);
            }

            if (!Clipboard.ContainsImage())
            {
                MessageBox.Show("The image could not be copied to clipboard", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes);
            }
        }

        private void btnSaveImageToDisk_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog() { 
                Title = "Save picture as ",
                DefaultExt = "jpg",
                AddExtension = true,
                Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp" 
            };

            if (imgOutput.Source != null)
            {
                if (save.ShowDialog() == true)
                {
                    JpegBitmapEncoder jpg = new JpegBitmapEncoder();
                    jpg.Frames.Add(BitmapFrame.Create(imgOutput.Source as BitmapImage));

                    using (Stream mem = File.Create(save.FileName))
                    {
                        jpg.Save(mem);
                    }
                }
            }

        }
    }
}
