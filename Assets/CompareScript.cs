using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CompareScript : MonoBehaviour
{
    [SerializeField] GameObject imageContainer; // The container that holds reference and drawn images
    private Image imageReference; // The reference image component
    private Image imageDrawn; // The drawn image component

    [SerializeField] TMP_Text accuracyText; // The text component to display accuracy
    [SerializeField] TMP_Text moneyText; // The text component to display money earned
    private int moneyEarned = 0; // The amount of money earned from the comparison

    private int accuracy = 0; // The accuracy of the painted image compared to the reference image
    // Start is called before the first frame update
    void Start()
    {
        imageReference = imageContainer.transform.Find("ReferenceImage").GetComponent<Image>();
        imageDrawn = imageContainer.transform.Find("DrawnImage").GetComponent<Image>();

        if (imageReference == null || imageDrawn == null)
        {
            Debug.LogError("Image components not found in the container.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This method will be called to set the reference image and drawn image
    public void SetReferenceImage(Texture2D referenceTexture)
    {
        Sprite referenceSprite = Sprite.Create(referenceTexture, new Rect(0, 0, referenceTexture.width, referenceTexture.height), new Vector2(0.5f, 0.5f));
        imageReference.sprite = referenceSprite;
    }

    public void SetDrawnImage(Texture2D drawnTexture)
    {
        Sprite drawnSprite = Sprite.Create(drawnTexture, new Rect(0, 0, drawnTexture.width, drawnTexture.height), new Vector2(0.5f, 0.5f));
        imageDrawn.sprite = drawnSprite;
    }

    IEnumerator DropDownImages()
    {
        // This method will be used to display the images, dropping from the ceiling to a given position
        yield return null;
    }

    IEnumerator PrintAccuracy(int accuracy)
    {
        // This method will be used to display the accuracy of the painted image compared to the reference image
        // it will start from 0 up to the given accuracy value
        yield return null;
    }
}
