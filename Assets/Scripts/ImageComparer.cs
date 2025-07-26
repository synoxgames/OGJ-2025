using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ImageMagick;

public class ImageComparer : MonoBehaviour
{
    [SerializeField]
    Texture2D referenceInput;

    [SerializeField]
    Texture2D paintedInput;

    // Start is called before the first frame update
    void Start()
    {
        if (paintedInput.width != referenceInput.width || paintedInput.height != referenceInput.height)
        {
            Debug.LogError("reference and painting were not the same size");
            return;
        }

        MagickImage reference = ImageConverter.ConvertToMagickImage(referenceInput);
        MagickImage painted = ImageConverter.ConvertToMagickImage(paintedInput);

        //reference.Write("C:/Users/ZZetho/Desktop/game 2025/OGJ-2025/Assets/Textures/gaming.png", MagickFormat.Png);
        //painted.Write("C:/Users/ZZetho/Desktop/game 2025/OGJ-2025/Assets/Textures/gaming2.png", MagickFormat.Png);

        // TODO: score images

        IPixelCollection<byte> referencePixels = reference.GetPixels();
        IPixelCollection<byte> paintedPixels = painted.GetPixels();

        MagickImage badnessMap = new MagickImage(MagickColors.Black, reference.Width, reference.Height);
        var badnessPixels = badnessMap.GetPixels();

        // how far to search for similar pixels
        int searchRadius = 2;
        long totalBadness = 0;

        for (int y = 0; y < reference.Height; y ++)
        {
            for (int x = 0; x < reference.Width; x ++)
            {
                // compare a pixel in the reference image to nearby pixels in the painted image
                // four bytes per pixel (rgba)
                byte[] referncePixel = referencePixels.GetPixel(x, y).ToColor().ToByteArray();

                // find the corners of the search box
                int xMin = Math.Max(x - searchRadius, 0);
                int yMin = Math.Max(y - searchRadius, 0);

                int xMax = Math.Min(x + searchRadius, (int)reference.Width);
                int yMax = Math.Min(y + searchRadius, (int)reference.Height);

                uint searchWidth = (uint)(xMax - xMin);
                uint searchHeight = (uint)(yMax - yMin);

                // three bytes per pixel (rgb)
                byte[] nearbyPaintedPixels = paintedPixels.GetArea(xMin, yMin, searchWidth, searchHeight);

                // conpare the reference pixel to nearby pixels
                int colourClosenessSympathy = 10;
                int bestNearbyPixel = int.MaxValue;
                byte[] badnessPixel = { 0, 0, 0 };
                for (int byteIndex = 0; byteIndex < nearbyPaintedPixels.Length; byteIndex += 3)
                {
                    int deltaR = Math.Max(Math.Abs(nearbyPaintedPixels[byteIndex] - referncePixel[0]) - colourClosenessSympathy, 0);
                    int deltaG = Math.Max(Math.Abs(nearbyPaintedPixels[byteIndex + 1] - referncePixel[1]) - colourClosenessSympathy, 0);
                    int deltaB = Math.Max(Math.Abs(nearbyPaintedPixels[byteIndex + 2] - referncePixel[2]) - colourClosenessSympathy, 0);

                    // how bad this pixel is compared to the refernce pixel
                    int badness = deltaR + deltaG + deltaB;

                    if (badness < bestNearbyPixel)
                    {
                        bestNearbyPixel = badness;
                        badnessPixel[0] = (byte)deltaR;
                        badnessPixel[1] = (byte)deltaG;
                        badnessPixel[2] = (byte)deltaB;
                    }

                    //Debug.Log("best pixel: " + bestNearbyPixel);
                }

                totalBadness += bestNearbyPixel;
                badnessPixels.SetPixel(x, y, badnessPixel);
            }
        }

        badnessMap.Write("C:/Users/zzzze/OneDrive/Desktop/game 2025/OGJ-2025/Assets/Textures/badnessMap.png", MagickFormat.Png);

        Debug.Log("badness per pixel: " + totalBadness / (reference.Width * reference.Height));
    }
}
