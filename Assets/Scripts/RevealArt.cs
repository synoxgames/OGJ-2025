using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class RevealArt : MonoBehaviour
{

    [Header("Reveal Settings")]
    [SerializeField] public SelectedArt selectedArtRef; // Reference to the SelectedArt ScriptableObject
    [SerializeField] public GameObject revealImageContainer; // The container for the image that will be revealed to the player
    [SerializeField] public Button revealButton; // the button to reveal the art
    [SerializeField] public Image revealArtImage; // Reference to the Image component for the art being revealed
    public float slideDuration = 1f; // Duration for the slide-in effect
    public int revealCost = 10; // Cost to reveal the art
    private Vector3 startPosition; // The starting position for the slide-in effect
    private Vector3 endPosition; // The target position for the slide-in effect


    // Start is called before the first frame update
    void Start()
    {
        if (selectedArtRef == null)
        {
            Debug.LogError("SelectedArt reference is not set in RevealArt script.");
            return;
        }
        if (revealArtImage == null)
        {
            Debug.LogError("RevealArtImage reference is not set in RevealArt script.");
            return;
        }
        revealArtImage.sprite = ArtManager.GetArtSprite();
        endPosition = revealImageContainer.transform.position; // Set the target position where the images should drop
        startPosition = new Vector3(startPosition.x + Screen.width, startPosition.y, startPosition.z); // Start position above the screen
    }

    public void Reveal()
    {
        if (CoinManager.GetCoinCount() < revealCost)
        {
            StartCoroutine(ButtonShake());
            return; // Not enough coins to reveal the art
        }
        Debug.Log("Reveal method called in RevealArt script.");
        StartCoroutine(RevealCoroutine());
    }

    private IEnumerator RevealCoroutine()
    {
        revealButton.interactable = false; // Disable the button to prevent multiple clicks
        yield return StartCoroutine(SlideInCoroutine());
        yield return StartCoroutine(WaitCoroutine());
        yield return StartCoroutine(SlideOutCoroutine());
        revealButton.interactable = true; // Re-enable the button after the reveal is done
    }
    // This method will be used to slide the art in view, dropping from the right to a given position
    private IEnumerator SlideInCoroutine()
    {
        // Move image to start position and enable them
        revealArtImage.gameObject.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            float t = elapsedTime / slideDuration;
            revealImageContainer.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    private IEnumerator WaitCoroutine()
    {
        float waitTime = selectedArtRef.displayTime * 2; // Wait time is twice the difficulty display time

        // Change the button text to indicate the time remaining
        float remainingTime = waitTime;
        while (remainingTime > 0)
        {
            revealButton.GetComponentInChildren<TextMeshProUGUI>().text = $"({remainingTime:F1}s)";
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        // Reset the button text after waiting
        revealButton.GetComponentInChildren<TextMeshProUGUI>().text = "PEEK $10";
        yield return null; // Wait for the calculated time before hiding
    }

    // This method will be used to slide the art out of view, dropping to the right to a given position
    private IEnumerator SlideOutCoroutine()
    {
        // Move image to start position and enable them

        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            float t = elapsedTime / slideDuration;
            revealImageContainer.transform.position = Vector3.Lerp(endPosition, startPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        revealArtImage.gameObject.SetActive(false); // Disable the image after sliding out
        // Reset the position to the start position for the next reveal
        revealImageContainer.transform.position = startPosition;
        yield return null;
    }

    private IEnumerator ButtonShake()
    {
        revealButton.interactable = false; // Disable the button to prevent multiple clicks
        Vector3 originalPosition = revealButton.transform.position;
        float shakeDuration = 0.5f;
        float shakeMagnitude = 10f;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            revealButton.transform.position = new Vector3(originalPosition.x + xOffset, originalPosition.y + yOffset, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        revealButton.transform.position = originalPosition; // Reset to original position
        revealButton.interactable = true; // Re-enable the button after shaking
    }
}
