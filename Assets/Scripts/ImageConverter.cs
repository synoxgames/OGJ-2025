using UnityEngine;
using ImageMagick;

public class ImageConverter
{
    public static MagickImage ConvertToMagickImage(Texture2D inputTexture)
    {
        // convert both images into imagemagick images
        Color[] inputPixels = inputTexture.GetPixels();
        byte[] inputPixelsConverted = new byte[inputPixels.Length * 3];

        // convert to 24 bit per pixel rgb images, 3 bytes at a time
        int writeIndex = 0;
        for (int pixelIndex = 0; pixelIndex < inputPixels.Length; pixelIndex++)
        {
            // unity colours have r,g,b between zero and one, this converts them to being between 0 and 255
            byte r = (byte)Mathf.Lerp(0, 255, inputPixels[pixelIndex].r);
            byte g = (byte)Mathf.Lerp(0, 255, inputPixels[pixelIndex].g);
            byte b = (byte)Mathf.Lerp(0, 255, inputPixels[pixelIndex].b);

            byte[] rgb = { r, g, b };

            rgb.CopyTo(inputPixelsConverted, writeIndex);

            writeIndex += 3;
        }

        // set the settings for image conversion
        MagickReadSettings settings = new MagickReadSettings();
        settings.Width = (uint)inputTexture.width;
        settings.Height = (uint)inputTexture.height;
        settings.ColorType = ColorType.TrueColor;
        settings.Format = MagickFormat.Rgb;

        // return the converted image
        return new MagickImage(inputPixelsConverted, settings);
    }
}
