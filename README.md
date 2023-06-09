# Stable Illusion
Stable Illusion is a WPF based graphical interface for use with Stable Diffusion in conjunction with ONNX Runtime. The StableDiffusion.ML.OnnxRuntime library has been reused from [this repo](https://github.com/cassiebreviu/StableDiffusion) with modifications by me to improve some performance and to make it play nice with WPF.

## Prerequisites

- [.NET 6 runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or above in order to run the application.
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or above to build the application.
- (Optional) A GPU enabled machine with CUDA on Windows.
- (Optional) Nvidia CUDA SDK and cuDNN installed and configured for use with Onnx Runtime. Follow [this tutorial](https://onnxruntime.ai/docs/tutorials/csharp/csharp-gpu.html) how to do this.
- Either the [v1.4](https://huggingface.co/CompVis/stable-diffusion-v1-4/tree/onnx) or [v1.5](https://huggingface.co/runwayml/stable-diffusion-v1-5/tree/onnx) models for Stable Diffusion from Hugging Face converted to Onnx format.

## Building the application
If you want to build this app yourself, simple clone the repo. Make sure you have the .NET 6 SDK (or above) installed. Open a PowerShell terminal in the repo where the `.sln` file is located and enter
```shell
dotnet build --configuration Release
```
Hit the enter key and wait for the app to build. You can find the result of the build process in the `.\StableIllusion\StableIllusion\bin\Release` folder. Run the application by double clicking the `stableillusion.exe` file.


## How to use
Before you can get creative you need to prepare a few other things first. 

Use git with lfs support to clone the models from Hugging Face. 
```shell
git lfs install
git clone git https://huggingface.co/runwayml/stable-diffusion-v1-5 -b onnx
```

The app provides directories for you to move the models to afterwards. These named for the models to make it easy for you to put them in the right place.

Before usage update the appsettings.json to point to the location of your Onnx runtime models:
```json
{
  "Models": {
    "TextEncoderOnnxPath": ".\\OnnxModels\\text_encoder\\model.onnx",
    "UnetOnnxPath": ".\\OnnxModels\\unet\\model.onnx",
    "VaeDecoderOnnxPath": ".\\OnnxModels\\vae_decoder\\model.onnx",
    "SafetyModelPath": ".\\OnnxModels\\safety_checker\\model.onnx"
  }
}
```
Paths can be absolute or relative.

Once the application is running things should be self explanatory. Write a prompt and click on the Generate Image button to create an image based off that prompt. You can also use the other buttons to copy the created image to the clipboard or save it to disk so you can share your artworks with others if you so please. 

The Execution Provider dropdown will let you pick between the GPU or the CPU to be used to create your images. The default is GPU, but this requires a Nvidia GPU with Cuda support. Otherwise chose the CPU to be the execution provider.

The Inference Steps dropdown will let you pick from a number of presets. The higher the number the better the image quality will be, however it will take longer to generate an image. The default setting is 15 which appears to be a happy medium for both quality and speed. 

The Guidance Scale dropdown will control how much creative freedom you will grant the application, you can chose from a number of presets. The lower the number is the more creative freedom you grant, the higher the number the stricter it will follow your prompts. Keep in mind that if you get very strict the image quality is going to suffer. Default is 7.5, again this looks like a happy medium.

Now you can unleash your inner artist! And remember the words of Bob Ross: We don't make mistakes, just happy little accidents.

## Caveats

- This app has been tested with a Nvidia RTX 3080 12GB, no guarantees for lower spec cards. It will probably run, but very slow.
- There is a CPU option available if you do not have a Nvidia dGPU or you don't want to use Cuda. I have not tested this yet, but it will probably be slow.
- Using Stable Diffusion requires significant resources from your computer, generating an image consumes about 16GB of RAM. Idle memory usage is 4GB of RAM after first image creation. Having 32GB of RAM is best, running this on systems with less memory might produce unforseen effects.