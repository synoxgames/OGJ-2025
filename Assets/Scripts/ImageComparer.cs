using UnityEngine;
using System;

public static class ImageComparer
{
    // returns the average badness per pixel of a painted image compared to a reference image
    // reference image = the image that the painted image gets compared to,
    // search radius = how many pixels away will the comparer check for simmilar colours
    // colour discount = reduces the badness of simmilar colours
    // every n pixels = only compare every n pixels, increases speed by n^2 but reduces accuracy
    public static int CompareImages(Texture2D reference, Texture2D painted, int searchRadius, float colourDiscount, int everyNPixels)
    {
        Debug.Log("reference: " + reference.width + "," + reference.height + " painted: " + painted.width + "," + painted.height);

        if (painted.width != reference.width || painted.height != reference.height)
        {
            Debug.LogError("refernce and painting are not the same size");
            return -1;
        }

        Color[] referencePixels = reference.GetPixels();
        Color[] paintedPixels = painted.GetPixels();

        Debug.Log("refrencePixels: " + referencePixels.Length);
        Debug.Log("paintedPixels: " + paintedPixels.Length);

        double totalBadness = 0;
        for (int y = 0; y < reference.height; y += everyNPixels)
        {
            for (int x = 0; x < reference.width; x += everyNPixels)
            {
                // compare a pixel in the reference image to nearby pixels in the painted image
                // four bytes per pixel (rgba)
                Color referncePixel = GetPixelAt(x, y, reference, referencePixels);

                // find the corners of the search box
                int xMin = Math.Max(x - searchRadius, 0);
                int yMin = Math.Max(y - searchRadius, 0);

                int xMax = Math.Min(x + searchRadius, reference.width);
                int yMax = Math.Min(y + searchRadius, reference.height);

                int searchWidth = xMax - xMin;
                int searchHeight = yMax - yMin;

                // three bytes per pixel (rgb)
                Color[] nearbyPaintedPixels = GetArea(xMin, yMin, searchWidth, searchHeight, painted, paintedPixels);

                //int nearbyPixelCount = nearbyPaintedPixels.Length / 3;
                //Debug.Log("nearby pixels: " + nearbyPixelCount);

                // conpare the reference pixel to nearby pixels
                float bestNearbyPixel = float.MaxValue;
                for (int pixelIndex = 0; pixelIndex < nearbyPaintedPixels.Length; pixelIndex ++)
                {
                    float deltaR = Mathf.Max(Mathf.Abs(nearbyPaintedPixels[pixelIndex].r - referncePixel.r) - colourDiscount, 0);
                    float deltaG = Mathf.Max(Mathf.Abs(nearbyPaintedPixels[pixelIndex].g - referncePixel.g) - colourDiscount, 0);
                    float deltaB = Mathf.Max(Mathf.Abs(nearbyPaintedPixels[pixelIndex].b - referncePixel.b) - colourDiscount, 0);

                    // find the position of the current pixel relative to the centre of the search area
                    int xPos = pixelIndex % searchWidth;
                    int yPos = pixelIndex / searchWidth;

                    int xRelative = (-searchWidth / 2) + xPos;
                    int yRelative = (-searchHeight / 2) + yPos;

                    // badness multipler for disance from the reference pixel. currently distacnce^2 + 1
                    float distanceMultipler = Mathf.Pow(xRelative, 2) + Mathf.Pow(yRelative, 2) + 1;

                    // how bad this pixel is compared to the refernce pixel
                    float badness = (deltaR + deltaG + deltaB) * distanceMultipler;

                    if (badness < bestNearbyPixel)
                    {
                        bestNearbyPixel = badness;
                    }
                    //Debug.Log("best pixel: " + bestNearbyPixel);
                }

                totalBadness += bestNearbyPixel;
            }
        }
        // normalize badness per pixel
        float averageBadness = (float)(totalBadness / (reference.width * reference.height) * Mathf.Pow(everyNPixels, 2));

        Debug.Log("badness: " + averageBadness);

        return (int)(averageBadness * 255);
    }

    private static Color GetPixelAt(int x, int y, Texture2D image, Color[] imagePixels)
    {
        Color ret = Color.black;
        try
        {
            int index = (y * image.width) + x;
            ret = imagePixels[index];
        }
        catch
        {
            Debug.Log("getting pixel at: " + x + "," + y);
        }
        return ret;
    }

    private static Color[] GetArea(int x, int y, int width, int height, Texture2D image, Color[] imagePixels)
    {
        Color[] foundPixels = new Color[width * height];
        int foundPixelsIndex = 0;

        for (int yPos = y; yPos < y + width; yPos ++)
        {
            for (int xPos = x; xPos < x + height; xPos ++)
            {
                foundPixels[foundPixelsIndex] = GetPixelAt(xPos, yPos, image, imagePixels);
                foundPixelsIndex++;
            }
        }

        return foundPixels;
    }
}
