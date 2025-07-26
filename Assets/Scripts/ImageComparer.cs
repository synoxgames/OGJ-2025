using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ImageMagick;

public class ImageComparer : MonoBehaviour
{
    [SerializeField]
    Texture2D referenceTest;
    [SerializeField]
    Texture2D paintedTest;

    // Start is called before the first frame update
    void Start()
    {
        CompareImages(referenceTest, paintedTest);
    }

    // returns the average badness per pixel of a painted image compared to a reference image
    int CompareImages(Texture2D referenceInput, Texture2D paintedInput)
    { 
        if (paintedInput.width != referenceInput.width || paintedInput.height != referenceInput.height)
        {
            Debug.LogError("reference and painting were not the same size");
            return -1;
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

        // how far (in terms of pixels) to search for simmilar pixels
        int searchRadius = 2;
        // how close a colour can be (in terms of combined difference in r g and b values) and still count as the same colour
        int colourClosenessSympathy = 0;

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

                //int nearbyPixelCount = nearbyPaintedPixels.Length / 3;
                //Debug.Log("nearby pixels: " + nearbyPixelCount);

                // conpare the reference pixel to nearby pixels
                int bestNearbyPixel = int.MaxValue;
                byte[] badnessPixel = { 0, 0, 0 };
                for (int byteIndex = 0; byteIndex < nearbyPaintedPixels.Length; byteIndex += 3)
                {
                    int pixelIndex = byteIndex / 3;
                    //Debug.Log("pixel index: " + pixelIndex);

                    int deltaR = Math.Max(Math.Abs(nearbyPaintedPixels[byteIndex] - referncePixel[0]) - colourClosenessSympathy, 0);
                    int deltaG = Math.Max(Math.Abs(nearbyPaintedPixels[byteIndex + 1] - referncePixel[1]) - colourClosenessSympathy, 0);
                    int deltaB = Math.Max(Math.Abs(nearbyPaintedPixels[byteIndex + 2] - referncePixel[2]) - colourClosenessSympathy, 0);

                    // find the position of the current pixel relative to the centre of the search area
                    int xPos = (int)(pixelIndex % searchWidth);
                    int yPos = (int)(pixelIndex / searchWidth);

                    int xRelative = (int)((-searchWidth / 2) + xPos);
                    int yRelative = (int)((-searchHeight / 2) + yPos);

                    //Debug.Log(xRelative + ", " + yRelative);

                    // badness multipler for disance from the reference pixel. currently distacnce^2 + 1
                    float distanceMultipler = Mathf.Pow(xRelative, 2) + Mathf.Pow(yRelative, 2) + 1;

                    // how bad this pixel is compared to the refernce pixel
                    int badness = (int)((deltaR + deltaG + deltaB) * distanceMultipler);

                    if (badness < bestNearbyPixel)
                    {
                        bestNearbyPixel = badness;

                        // make a map of how incrorrect the painting is
                        badnessPixel[0] = (byte)deltaR;    // red
                        badnessPixel[1] = (byte)deltaG;    // green
                        badnessPixel[2] = (byte)deltaB;    // blue
                    }

                    //Debug.Log("best pixel: " + bestNearbyPixel);
                }

                totalBadness += bestNearbyPixel;
                badnessPixels.SetPixel(x, y, badnessPixel);
            }
        }
        badnessMap.Write("Assets/Textures/badnessMap.png", MagickFormat.Png);

        Debug.Log("badness per pixel: " + totalBadness / (reference.Width * reference.Height));

        return (int)(totalBadness / reference.Width * reference.Height);
    }
}
