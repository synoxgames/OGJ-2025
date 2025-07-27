using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevealArt : MonoBehaviour
{

    [Header("Reveal Settings")]
    [SerializeField] public SelectedArt selectedArtRef; // Reference to the SelectedArt ScriptableObject
    [SerializeField] public Image revealArtImage; // The image that will be revealed to the player
    public float slideDuration = 1f; // Duration for the slide-in effect
    private Vector3 startPosition; // The starting position for the slide-in effect
    private Vector3 endPosition; // The target position for the slide-in effect

    private
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
        startPosition = revealArtImage.transform.position; // Set the target position where the images should drop
        endPosition = new Vector3(startPosition.x + Screen.width, startPosition.y, startPosition.z); // Start position above the screen
    }

    public void Reveal()
    {
        StartCoroutine(SlideInCoroutine());
        StartCoroutine(WaitCoroutine());
        StartCoroutine(SlideOutCoroutine());
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
            revealArtImage.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    private IEnumerator WaitCoroutine()
    {
        float waitTime = selectedArtRef.displayTime * 2; // Wait time is twice the difficulty display time
        yield return new WaitForSeconds(waitTime); // Wait for the calculated time before hiding
    }

    // This method will be used to slide the art out of view, dropping to the right to a given position
    private IEnumerator SlideOutCoroutine()
    {
        // Move image to start position and enable them

        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            float t = elapsedTime / slideDuration;
            revealArtImage.transform.position = Vector3.Lerp(endPosition, startPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        revealArtImage.gameObject.SetActive(false); // Disable the image after sliding out
        // Reset the position to the start position for the next reveal
        revealArtImage.transform.position = startPosition;
        yield return null;
    }
}
