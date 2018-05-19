using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace CropToFace
{
    internal class AzureCongServFaceAPI_Cropper : IFaceCropper
    {
        public IConfiguration Config { get; set; }
        public string InputFile { get; set; }
        public string OutputFile { get; set; }

        public bool Crop()
        {
            try
            {
                Face foundFace = UploadAndDetectFaces().Result;
                OutputImage(foundFace);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<Face> UploadAndDetectFaces()
        {
            IFaceServiceClient faceServiceClient = new FaceServiceClient(Config["key"], Config["endpoint"]);

            // Call the Face API.
            try
            {
                using (Stream imageFileStream = File.OpenRead(InputFile))
                {
                    Face[] faces = await faceServiceClient.DetectAsync(imageFileStream, returnFaceId: true, returnFaceLandmarks: false);
                    return faces[0];
                }
            }
            // Catch and display Face API errors.
            catch (FaceAPIException f)
            {
                return new Face();
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                return new Face();
            }
        }

        private void OutputImage(Face f)
        {
            var img = Image.FromFile(InputFile);

            Rectangle crop = new Rectangle(
                f.FaceRectangle.Left,
                f.FaceRectangle.Top,
                f.FaceRectangle.Width,
                f.FaceRectangle.Height
                );

            using (Bitmap bmp = new Bitmap(crop.Width, crop.Height))
            {
                using (var gr = Graphics.FromImage(bmp))
                {
                    gr.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
                }

                bmp.Save(OutputFile, ImageFormat.Jpeg);
            }
        }
    }
}