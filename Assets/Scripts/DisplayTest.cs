using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/**
 * DisplayTest is a test script for displaying the saved art from the SelectedArt ScriptableObject in the scene.
 */
public class DisplayTest : MonoBehaviour
{
    public Image artImage; // Reference to the image component for displaying art
    public SelectedArt selectedArtRef; // Reference to the SelectedArt ScriptableObject
    public TMP_Text artText; // Reference to the text component for displaying art image name
    // Start is called before the first frame update
    void Start()
    {
        // Get the SelectedArt ScriptableObject
        selectedArtRef = Resources.Load<SelectedArt>("SelectedArtData");

        if (selectedArtRef != null)
        {
            // Display the selected art in the scene
            artImage.sprite = selectedArtRef.selectedArt;
            // Display the name of the art image
            artText.text = selectedArtRef.artName;
        }
        else
        {
            Debug.LogError("SelectedArt ScriptableObject not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
