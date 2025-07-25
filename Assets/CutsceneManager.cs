using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image blackScreen;
    public Image walkieTalkieImage;
    public Image textBubbleImage;
    public TextMeshProUGUI textBubbleText;

    [Header("Cutscene Settings")]
    public float fadeDuration = 2f; // duration for fade in/out
    public float textBubbleEnlargeDuration = 1f; // duration for text bubble enlarge
    public float shakeDuration = 1f; // total shake time
    public float shakeMagnitude = 0.1f; //max degrees to rotate
    public float zoomDuration = 1f; // duration for zoom in/out of walkie talkie
    public float textTypingSpeed = 0.05f; // speed of text typing effect

    public bool debug = false;
    void Start()
    {
        StartCoroutine(PlayCutscene());
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
        yield return StartCoroutine(textTypingEffect());
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

        // Placeholder for walkie talkie zoom in logic
        yield return new WaitForSeconds(1f); // Simulate zoom in duration
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
        // Placeholder for text bubble enlarge logic
        yield return new WaitForSeconds(1f); // Simulate enlarge duration
    }

    IEnumerator textTypingEffect()
    {
        if (debug) Debug.Log("Text Typing Effect");
        // Placeholder for text typing effect logic
        yield return new WaitForSeconds(1f); // Simulate typing duration
    }

    IEnumerator textBubbleShrink()
    {
        if (debug) Debug.Log("Text Bubble Shrink");
        // Placeholder for text bubble shrink logic
        yield return new WaitForSeconds(1f); // Simulate shrink duration
    }

    IEnumerator walkieTalkieZoomOut()
    {
        if (debug) Debug.Log("Walkie Talkie Zoom Out");
        // Placeholder for walkie talkie zoom out logic
        yield return new WaitForSeconds(1f); // Simulate zoom out duration
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
    }
}
