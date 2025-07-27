using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ArtManager is responsible for managing art-related functionalities in the game.
 * It will deal with loading the art from the "Art" folder and displaying it in the game.
 * It will also need to ensure that the loaded art is accessible across different scenes.
 */

public static class ArtManager
{
    public static Texture2D[] artTextures; // the art
    public static int selectedArtIndex = 0;    // the index of the currently selected pice of art        
    static int[] artDifficulty;     // how hard each pice of art is to copy

    static string artPath = "Art";                         // Path to the art folder

    // load the art from the art folder, under resources
    private static void LoadArt()
    {
        artTextures = Resources.LoadAll<Texture2D>("Art");
        artDifficulty = new int[artTextures.Length];

        // TODO: make the art have actual difficulty values
        // populate the difficulty array with default values
        for (int i = 0; i < artDifficulty.Length; i ++)
        {
            artDifficulty[i] = i;
        }

        if (artTextures.Length <= 0)
        {
            Debug.LogError("Art not found: " + artPath);
        }
    }

    // gets a peice of art as a sprite
    public static Sprite GetArtSprite()
    {
        if (artTextures == null)
        {
            LoadArt();
        }

        Texture2D selectedTexture = artTextures[selectedArtIndex];

        // Convert the texture to a sprite
        Sprite selectedSprite = Sprite.Create(
            selectedTexture,
            new Rect(0, 0, selectedTexture.width, selectedTexture.height),
            new Vector2(0.5f, 0.5f)
        );

        return selectedSprite;
    }

    // gets a peice of art as a texture2D
    public static Texture2D GetArtTexture()
    {
        if (artTextures == null)
        {
            LoadArt();
        }
        return artTextures[selectedArtIndex];
    }

    // gets the index of a random peice of art within a range of difficulties
    public static int SelectRandomArt(int difficultyMin, int difficultyMax)
    {
        // load in the art if is hasnt been done already
        if (artTextures == null)
        {
            LoadArt();
        }

        int tries = 0;

        while (true)
        {
            // get a random peice of art's index
            int index = Random.Range(0, artDifficulty.Length);

            // if it is within the difficulty range, return it
            if (artDifficulty[index] <= difficultyMax && artDifficulty[index] >= difficultyMin)
            {
                selectedArtIndex = index;
                return index;
            }

            // only try and find an image 100 times, otherwise return the first image in the array
            if (tries > 100)
            {
                selectedArtIndex = 0;
                return 0;
            }
        }
    }
}
