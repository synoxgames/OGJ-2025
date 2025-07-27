using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// this script compares the painted image to the selected reference image
public class ComparisonManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject canvasContainer; // The container that holds reference and drawn images
    [SerializeField] TMP_Text accuracyText; // The text component to display accuracy
    [SerializeField] TMP_Text moneyText; // The text component to display money earned
    [SerializeField] Image backgroundImage; // The reference image component for the black background

    // the canvases that display the drawn and reference images nest to one another
    private Image referenceCanvas;
    private Image drawnCanvas;

    [Header("Comparison Settings")]
    [SerializeField] float moneyPerAccuracy = 0.1f; // The amount of money earned per accuracy point
    [SerializeField] float dropDownDuration = 1f; // Duration for the dropdown effect

    private int moneyEarned = 0; // The amount of money earned from the comparison
    private int accuracy = 0; // The accuracy of the painted image compared to the reference image

    // Start is called before the first frame update
    void Start()
    {
        referenceCanvas = canvasContainer.transform.Find("ReferenceImage").GetComponent<Image>();
        drawnCanvas = canvasContainer.transform.Find("DrawnImage").GetComponent<Image>();

        if (referenceCanvas == null || drawnCanvas == null)
        {
            Debug.LogError("Image component RefernceImage or DrawnImage not found in the container " + canvasContainer.name);
            return;
        }
    }

    // This method will be called to set the reference image and drawn image
    public void SetReferenceImage(Texture2D referenceTexture)
    {
        Sprite referenceSprite = Sprite.Create(referenceTexture, new Rect(0, 0, referenceTexture.width, referenceTexture.height), new Vector2(0.5f, 0.5f));
        referenceCanvas.sprite = referenceSprite;
    }

    public void SetDrawnImage(Texture2D drawnTexture)
    {
        Sprite drawnSprite = Sprite.Create(drawnTexture, new Rect(0, 0, drawnTexture.width, drawnTexture.height), new Vector2(0.5f, 0.5f));
        drawnCanvas.sprite = drawnSprite;
    }

    public void StartAnimation(int accuracy)
    {
        backgroundImage.gameObject.SetActive(true);
        // This method will be used to start the animation of the images dropping down
        StartCoroutine(FinalAnimation(accuracy));
    }

    IEnumerator FinalAnimation(int accuracy)
    {
        // This method will be used to display the final animation of the art piece
        yield return StartCoroutine(DropDownImages());

        // Calculate the money based on the accuracy
        moneyEarned = Mathf.RoundToInt(accuracy * moneyPerAccuracy);

        // Display the accuracy and money earned
        yield return StartCoroutine(PrintAccuracy(accuracy));
        yield return StartCoroutine(PrintMoneyEarned(moneyEarned));
    }

    IEnumerator DropDownImages()
    {
        // This method will be used to display the images, dropping from the ceiling to a given position
        Vector3 targetPosition = canvasContainer.transform.position; // Set the target position where the images should drop
        Vector3 startPosition = new Vector3(targetPosition.x, targetPosition.y + Screen.height, targetPosition.z); // Start position above the screen

        // Move images to start position and enable them
        canvasContainer.transform.position = startPosition;
        referenceCanvas.gameObject.SetActive(true);
        drawnCanvas.gameObject.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < dropDownDuration)
        {
            float t = elapsedTime / dropDownDuration;
            canvasContainer.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    IEnumerator PrintAccuracy(int accuracy)
    {
        // This method will be used to display the accuracy of the painted image compared to the reference image
        // it will start from 0 up to the given accuracy value
        accuracyText.gameObject.SetActive(true);
        accuracyText.text = "Accuracy: 0%"; // Start with 0% accuracy
        int currentAccuracy = 0;
        while (currentAccuracy < accuracy)
        {
            currentAccuracy = Mathf.Min((int)(currentAccuracy * 1.1) + 1, accuracy);
            accuracyText.text = "Accuracy: " + currentAccuracy + "%";
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator PrintMoneyEarned(int money)
    {
        // This method will be used to display the money earned from the comparison
        // it will start from 0 up to the given money value
        moneyText.gameObject.SetActive(true);
        moneyText.text = "Money Earned: $0"; // Start with 0 money earned
        int currentMoney = 0;
        while (currentMoney < money)
        {
            currentMoney = Mathf.Min((int)(currentMoney * 1.1) + 1, money);
            moneyText.text = "Money Earned: $" + currentMoney;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
