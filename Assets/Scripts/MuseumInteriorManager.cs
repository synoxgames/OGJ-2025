using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// It will use the fade in and swipe out effects for scene transitions.
// It will also deal with the timing and speech bubble animations.

public class MuseumInteriorManager : MonoBehaviour
{
    [Header("Art Settings")]
    [SerializeField] public Image artCanvas;                // the canvas that displays art in the museum
    [SerializeField] public float displayTime = 5f;         // how long the use has to look at the art
    [SerializeField] public TMP_Text timerText;             // the text that displays the remaining time

    [Header("UI Elements")]
    [SerializeField] public TMP_Text speechBubbleText;      // Reference to the speech bubble text component
    [SerializeField] public Image speechBubbleImage;        // Reference to the speech bubble image component
    [SerializeField] public float enlargeDuration = 1f;     // Duration of the speech bubble enlarge effect
    [SerializeField] public float wobbleDuration = 1f;      // Duration of the speech bubble wobble effect
    [SerializeField] public float wobbleMagnitude = 1f;     // Magnitude of the speech bubble wobble effect
    [SerializeField] public float fadeDuration = 1f;        // Duration of the fade effect
    [SerializeField] public Image blackScreen;              // Reference to the black screen image for fade effects

    [Header("Debug Settings")]
    [SerializeField] public bool debug = false;             // Enable debug mode for testing purposes

    private string artPath = "Art";                         // Path to the art folder

    private bool hasStartedFinalAnimation = false; // Flag to prevent multiple final animations
    void Awake()
    {
        // Ensure the black screen is set up
        if (blackScreen == null)
        {
            GameObject found = GameObject.Find("BlackScreen");
            if (found != null)
            {
                blackScreen = found.GetComponent<Image>();
            }
            else
            {
                Debug.LogError("BlackScreen object not found in scene!");
            }
        }

        // Ensure the art image and speech bubble are set up
        speechBubbleImage.rectTransform.localScale = new Vector3(0, 1, 1);
        speechBubbleText.gameObject.SetActive(false);

        LoadAndDisplayRandomArt();
    }

    // Function to load and display a random art piece and save it to SelectedArt
    void LoadAndDisplayRandomArt()
    {
        // selects a random peice of art and displays it on the museum's art canvas
        int artIndex = ArtManager.SelectRandomArt(0, int.MaxValue);
        Sprite selectedArt = ArtManager.GetArtSprite();

        // Display the selected art in the scene
        artCanvas.sprite = selectedArt;
    }

    // Update is called once per frame
    void Update()
    {
        if (debug && Input.GetKeyDown(KeyCode.Space))
        {
            // For debugging purposes, loads the art display test scene 
            SceneManager.LoadScene("Art_Display_Test");
        }

        // update timer
        displayTime -= Time.deltaTime;
        timerText.text = Mathf.Max(Mathf.Ceil(displayTime), 0f).ToString();

        // if display time is zero, start the final animation
        if (displayTime <= 0 && !hasStartedFinalAnimation)
        {
            hasStartedFinalAnimation = true; // Prevent multiple final animations
            StartCoroutine(FinalAnimation());
        }
    }

    IEnumerator FinalAnimation()
    {
        // Fade out the black screen
        yield return StartCoroutine(GuardSpeechBubble());
        yield return StartCoroutine(FadeOut());

        // Load the next scene or perform final animation
        SceneManager.LoadScene("PaintingStudio");
    }

    IEnumerator GuardSpeechBubble()
    {
        // speech bubble starts with a width of 0, then enlarges to full size over enlargeDuration
        float elapsedTime = 0f;

        // Phase 1: Enlarge the speech bubble
        while (elapsedTime < enlargeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / enlargeDuration;
            float easedWidth = Mathf.Lerp(0, 1, progress * progress); // Quadratic ease-out

            speechBubbleImage.rectTransform.localScale = new Vector3(easedWidth, 1, 1);
            yield return null;
        }

        // Show speech bubble text
        speechBubbleText.gameObject.SetActive(true);

        // Phase 2: wobble by enlarging to the left
        elapsedTime = 0f;
        while (elapsedTime < wobbleDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / wobbleDuration;
            float easedWobble = Mathf.Sin(progress * Mathf.PI * 2) * wobbleMagnitude; // Sinusoidal wobble

            speechBubbleImage.rectTransform.localScale = new Vector3(1 + easedWobble, 1, 1);
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.5f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;
            float easedAlpha = 1 - (progress * progress); // Quadratic ease-out

            blackScreen.color = new Color(0, 0, 0, easedAlpha);
            yield return null;
        }

        // Ensure alpha is exactly 0 at the end
        blackScreen.color = new Color(0, 0, 0, 0);
    }

    IEnumerator FadeOut()
    {
        if (blackScreen == null)
        {
            Debug.LogError("Black screen image is not assigned!");
            yield break;
        }

        // ensure black screen object is active
        if (!blackScreen.gameObject.activeSelf)
        {
            // Make sure black screen is clear 
            blackScreen.color = new Color(0, 0, 0, 0);
            blackScreen.gameObject.SetActive(true);
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;
            float easedAlpha = progress * progress; // Quadratic ease-in

            blackScreen.color = new Color(0, 0, 0, easedAlpha);
            yield return null;
        }

        // Ensure alpha is exactly 1 at the end
        blackScreen.color = new Color(0, 0, 0, 1);
    }
}
