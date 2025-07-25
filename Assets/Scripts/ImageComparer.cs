using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImageComparer : MonoBehaviour
{
    [SerializeField]
    Texture2D referenceInput;

    [SerializeField]
    Texture2D paintedInput;

    // Start is called before the first frame update
    void Start()
    {
        // get the pixels from both images
        Color[] referncePixels = referenceInput.GetPixels();
        Color[] paintedPixels = paintedInput.GetPixels();

        int referenceWidth = referenceInput.width;
        int referenceHeight = referenceInput.height;

        int paintedWidth = paintedInput.width;
        int paintedHeight = paintedInput.height;

        // repackage the pixels into arrays of the same size as the original textures
        // the rows and columns are swapped from the original image

        //Color[,] reference = new Color[referenceHeight, referenceWidth];
        //Color[,] painted = new Color[referenceWidth, referenceHeight];

        // TODO: scale the images to match

        Debug.Log("pixels: " + referncePixels.Length);

        float badness = 0;
        int test = 0;

        for (int pixelIndex = 0; pixelIndex < referncePixels.Length; pixelIndex ++)
        {
            float deltaGreen = Mathf.Abs((referncePixels[pixelIndex] - paintedPixels[pixelIndex]).g);
            float deltaRed = Mathf.Abs((referncePixels[pixelIndex] - paintedPixels[pixelIndex]).r);
            float deltaBlue = Mathf.Abs((referncePixels[pixelIndex] - paintedPixels[pixelIndex]).b);
            float deltaAlpha = Mathf.Abs((referncePixels[pixelIndex] - paintedPixels[pixelIndex]).a);

            badness += (deltaAlpha + deltaBlue + deltaGreen + deltaRed);
        }

        Debug.Log("badness: " + badness + ", test: " + test);
    }
}
