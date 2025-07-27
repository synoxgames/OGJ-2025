using UnityEngine;
using System;
using ImageMagick;

public static class ImageComparer
{
    // returns the average badness per pixel of a painted image compared to a reference image
    // reference image = the image that the painted image gets compared to,
    // search radius = how many pixels away will the comparer check for simmilar colours
    // colour discount = reduces the badness of simmilar colours
    // every n pixels = only compare every n pixels, increases speed by n^2 but reduces accuracy
    public static int CompareImages(Texture2D referenceInput, Texture2D paintedInput, int searchRadius, int colourDiscount, int everyNPixels)
    {
        Debug.Log("reference: " + referenceInput.width + "," + referenceInput.height + " painted: " + paintedInput.width + "," + paintedInput.height);

        if (paintedInput.width != referenceInput.width || paintedInput.height != referenceInput.height)
        {
            Debug.LogError("refernce and painting are not the same size");
            return -1;
        }

        // convert the images to imageMagick images
        MagickImage reference = ImageConverter.ConvertToMagickImage(referenceInput);
        MagickImage painted = ImageConverter.ConvertToMagickImage(paintedInput);

        // get the pixels of the images to comapre them
        IPixelCollection<byte> referencePixels = reference.GetPixels();
        IPixelCollection<byte> paintedPixels = painted.GetPixels();

        MagickImage badnessMap = new MagickImage(MagickColors.Black, reference.Width, reference.Height);
        var badnessPixels = badnessMap.GetPixels();

        long totalBadness = 0;
        for (int y = 0; y < reference.Height; y += everyNPixels)
        {
            for (int x = 0; x < reference.Width; x += everyNPixels)
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

                    int deltaR = Math.Max(Math.Abs(nearbyPaintedPixels[byteIndex] - referncePixel[0]) - colourDiscount, 0);
                    int deltaG = Math.Max(Math.Abs(nearbyPaintedPixels[byteIndex + 1] - referncePixel[1]) - colourDiscount, 0);
                    int deltaB = Math.Max(Math.Abs(nearbyPaintedPixels[byteIndex + 2] - referncePixel[2]) - colourDiscount, 0);

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
                        badnessPixel[0] = (byte)Math.Min(deltaR, 255);    // red
                        badnessPixel[1] = (byte)Math.Min(deltaG, 255);    // green
                        badnessPixel[2] = (byte)Math.Min(deltaB, 255);    // blue
                    }
                    //Debug.Log("best pixel: " + bestNearbyPixel);
                }

                totalBadness += bestNearbyPixel;
                badnessPixels.SetPixel(x, y, badnessPixel);
            }
        }
        // create the debug map
        badnessMap.Flip();
        badnessMap.Write("Assets/Textures/badnessMap.png", MagickFormat.Png);

        // normalize badness per pixel
        int averageBadness = (int)(totalBadness / (reference.Width * reference.Height) * Math.Pow(everyNPixels, 2));

        // use whatever normalized cross correlation is to compare the images
        double crossCorrelation = reference.Compare(painted, ErrorMetric.NormalizedCrossCorrelation);
        //Debug.Log(error);

        // discount badness depending on how close cross correlation thinks the images are
        float multiplier = (float)(0.5f + (0.5f * crossCorrelation));

        Debug.Log("badness: " + averageBadness * multiplier);

        return (int)(averageBadness * multiplier);
    }
}
