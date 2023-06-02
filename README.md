# Stable Illusion
Stable Illusion is a WPF based graphical interface for use with Stable Diffusion in conjunction with ONNX Runtime. 

## Prerequisites

- [.NET 6 runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or above.
- A GPU enabled machine with CUDA on Windows

This app has been tested with an Nvidia RTX 3080 12GB, no guarantees for lower spec cards. It will probably run, but very slow. Using Stable Diffusion requires significant resources from your computer, generating an image consumes about 20GB of RAM at time of writing. Make sure your computer has at least 32GB of RAM total.