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
        MagickImage reference = ImageConverter.ConvertToMagickImage(referenceInput);
        MagickImage painted = ImageConverter.ConvertToMagickImage(paintedInput);

        //reference.Write("C:/Users/ZZetho/Desktop/game 2025/OGJ-2025/Assets/Textures/gaming.png", MagickFormat.Png);
        //painted.Write("C:/Users/ZZetho/Desktop/game 2025/OGJ-2025/Assets/Textures/gaming2.png", MagickFormat.Png);

        // TODO: score images
    }
}
