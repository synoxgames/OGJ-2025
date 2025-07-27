using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip walkieTalkieClip;
    public AudioClip textBubbleClip;

    [Header("UI Elements")]
    public Image blackScreen;
    public Image walkieTalkieImage;
    public Image textBubbleImage;
    public TextMeshProUGUI textBubbleText;
    public Button skipButton;

    [Header("Cutscene Settings")]
    public float fadeDuration = 2f; // duration for fade in/out
    public float textBubbleEnlargeDuration = 1f; // duration for text bubble enlarge
    public float wobbleDuration = 2f; // duration for text bubble wobble
    public float wobbleSpeed = 5f; // speed of wobble
    public float wobbleAmount = 0.1f; // amount of wobble
    public float shakeDuration = 1f; // total shake time
    public float shakeMagnitude = 0.1f; //max degrees to rotate
    public float zoomDuration = 1f; // duration for zoom in/out of walkie talkie
    public float textTypingSpeed = 0.05f; // speed of text typing effect
    private string greetingText = "This is it, make sure to get a good look before the guards recognise you!";

    public bool debug = false;
    private bool isTyping = false;
    private bool textFullyDisplayed = false;
    private Coroutine typingCoroutine;
    public bool userClickedToContinue = false;
    void Start()
    {
        // Ensure black screen is active at the start
        if (!blackScreen.gameObject.activeSelf)
        {
            blackScreen.gameObject.SetActive(true);
            blackScreen.color = new Color(0, 0, 0, 1); // Start with black screen
        }
        // Disable everything
        walkieTalkieImage.gameObject.SetActive(false);
        textBubbleImage.gameObject.SetActive(false);
        textBubbleText.gameObject.SetActive(false);

        // Set the initial text to whats from scene and remove stored text
        greetingText = textBubbleText.text;
        textBubbleText.text = "";

        // Start the cutscene
        StartCoroutine(PlayCutscene());
    }

    void Update()
    {
        if (!walkieTalkieImage.gameObject.activeSelf)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                SkipTyping();
                return; // Skip typing and continue
            }

            if (textFullyDisplayed)
            {
                if (debug) Debug.Log("Text fully displayed, user click to continue.");
                userClickedToContinue = true; // this lets PlayCutscene() move forward
            }
        }
    }

    IEnumerator PlayCutscene()
    {
        // setting the scene - walkie talkie and text should be disabled
        // start black screen fade in
        yield return StartCoroutine(FadeIn());
        // walkie talkie should zoom in from side and start shaking
        yield return StartCoroutine(walkieTalkieZoomIn());
        yield return StartCoroutine(walkieTalkieShake());
        // after a few seconds walkie talkie stops shaking and text bubble enlarges
        yield return StartCoroutine(textBubbleEnlarge());
        // text is then slowly typed out
        userClickedToContinue = false;
        yield return typingCoroutine = StartCoroutine(textTypingEffect());
        // Wait for click after full text
        if (debug) Debug.Log("Waiting for user click after text fully displayed.");
        yield return new WaitUntil(() => userClickedToContinue);
        if (debug) Debug.Log("User clicked to continue after text fully displayed.");
        // after text is done and user clicks, text bubble shrinks and walkie talkie zooms out
        yield return StartCoroutine(textBubbleShrink());
        yield return StartCoroutine(walkieTalkieZoomOut());
        // black screen fades out and next scene is loaded
        yield return StartCoroutine(FadeOut());

    }

    // This coroutine is called at the start of the game to fade in from black.
    public IEnumerator FadeIn()
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

    IEnumerator walkieTalkieZoomIn()
    {
        if (debug) Debug.Log("Walkie Talkie Zoom In");
        // Activate walkie talkie image
        walkieTalkieImage.gameObject.SetActive(true);

        Vector3 startPos = walkieTalkieImage.transform.position + new Vector3(-Screen.width, 0, 0); // Start from off camera
        Vector3 endPos = walkieTalkieImage.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / zoomDuration;
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            walkieTalkieImage.transform.position = Vector3.Lerp(startPos, endPos, easedProgress);
            yield return null;
        }

        walkieTalkieImage.transform.position = endPos;
    }

    IEnumerator walkieTalkieShake()
    {
        if (debug) Debug.Log("Walkie Talkie Shake");

        float elapsedTime = 0f;
        Quaternion originalRotation = walkieTalkieImage.transform.rotation;

        while (elapsedTime < shakeDuration)
        {
            float z = Random.Range(-shakeMagnitude, shakeMagnitude);
            walkieTalkieImage.transform.rotation = originalRotation * Quaternion.Euler(0f, 0f, z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        walkieTalkieImage.transform.rotation = originalRotation;
    }

    IEnumerator textBubbleEnlarge()
    {
        if (debug) Debug.Log("Text Bubble Enlarge");

        // Activate text bubble image and text
        textBubbleImage.gameObject.SetActive(true);
        textBubbleText.gameObject.SetActive(true);

        Vector3 finalScale = textBubbleImage.transform.localScale; // Final scale for the text bubble image
        Vector3 startScale = Vector3.zero; // Start from scale 0

        float elapsedTime = 0f;

        // Phase 1: expand
        while (elapsedTime < textBubbleEnlargeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / textBubbleEnlargeDuration;
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            textBubbleImage.transform.localScale = Vector3.Lerp(startScale, finalScale, easedProgress);
            yield return null;
        }
        textBubbleImage.transform.localScale = finalScale; // ensure final scale is exactly what we want

        // Phase 2: wobble
        elapsedTime = 0f;
        while (elapsedTime < wobbleDuration)
        {
            elapsedTime += Time.deltaTime;

            // Wobble scale along y (sinusoidal)
            float wobble = Mathf.Sin(elapsedTime * wobbleSpeed) * wobbleAmount;

            textBubbleImage.transform.localScale = new Vector3(
                finalScale.x,
                finalScale.y * (1f + wobble),
                finalScale.z
            );
            yield return null;
        }
        textBubbleImage.transform.localScale = finalScale; // Ensure final scale is exactly what we want
    }

    IEnumerator textTypingEffect()
    {
        isTyping = true;
        textFullyDisplayed = false;

        string displayedText = ""; // store whats been displayed so far

        for (int i = 0; i < greetingText.Length; i++)
        {
            if (!isTyping) break; // if typing is stopped, break out of the loop
            char currentChar = greetingText[i];

            // add current character
            displayedText += currentChar;

            // update the TMP text
            textBubbleText.text = displayedText;

            // wait for the next character
            yield return new WaitForSeconds(textTypingSpeed);
        }
        // Ensure final text and flags
        textBubbleText.text = greetingText;
        isTyping = false;
        textFullyDisplayed = true;
    }

    public void SkipTyping()
    {
        // if (typingCoroutine != null)
        // {
        //     StopCoroutine(typingCoroutine); // Stop any existing text animation
        //     typingCoroutine = null;
        // }
        textBubbleText.text = greetingText; // Display the full text
        isTyping = false;
        textFullyDisplayed = true;
    }

    IEnumerator textBubbleShrink()
    {
        if (debug) Debug.Log("Text Bubble Shrink");
        // Activate text bubble image and text
        textBubbleImage.gameObject.SetActive(true);
        textBubbleText.gameObject.SetActive(false); // Hide text bubble text

        Vector3 finalScale = Vector3.zero; // Final scale for the text bubble image (0,0,0)
        Vector3 startScale = textBubbleImage.transform.localScale; // Start from current scale

        float elapsedTime = 0f;

        // Phase 1: expand
        while (elapsedTime < textBubbleEnlargeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / textBubbleEnlargeDuration;
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            textBubbleImage.transform.localScale = Vector3.Lerp(startScale, finalScale, easedProgress);
            yield return null;
        }
        textBubbleImage.transform.localScale = finalScale; // ensure final scale is exactly what we want
    }

    IEnumerator walkieTalkieZoomOut()
    {
        if (debug) Debug.Log("Walkie Talkie Zoom Out");
        // Activate walkie talkie image
        walkieTalkieImage.gameObject.SetActive(true);

        Vector3 startPos = walkieTalkieImage.transform.position;
        Vector3 endPos = walkieTalkieImage.transform.position + new Vector3(-Screen.width, 0, 0); // Start from off camera

        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / zoomDuration;
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            walkieTalkieImage.transform.position = Vector3.Lerp(startPos, endPos, easedProgress);
            yield return null;
        }

        walkieTalkieImage.transform.position = endPos;
    }

    // This coroutine is called at the end of the game to fade out to black.
    public IEnumerator FadeOut()
    {
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

        //End of the cutscene move to the next scene.
        SceneManager.LoadScene("MuseumInterior");
    }
    
    public void SkipCutscene()
    {
        if (debug) Debug.Log("Cutscene skipped by user.");
        // If the cutscene is skipped, we can immediately fade out and load the next scene
        //StopAllCoroutines(); // Stop any ongoing coroutines
        StartCoroutine(FadeOut());
    }
}
