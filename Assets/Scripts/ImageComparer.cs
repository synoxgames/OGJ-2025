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

        Debug.Log("pixels: " + referncePixels.Length);

        float badness = 0;

        for (int pixelIndex = 0; pixelIndex < referncePixels.Length; pixelIndex ++)
        {
            Color deltaColour = (referncePixels[pixelIndex] - paintedPixels[pixelIndex]);

            badness += (deltaColour.r + deltaColour.g + deltaColour.b + deltaColour.a);
        }

        Debug.Log("score: " + 1/badness);
    }
}
