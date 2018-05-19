
using Microsoft.Extensions.Configuration;

namespace CropToFace
{
    interface IFaceCropper
    {
        IConfiguration Config { get; set; }
        string InputFile { get; set; }
        string OutputFile { get; set; }

       bool Crop();
    }
}
