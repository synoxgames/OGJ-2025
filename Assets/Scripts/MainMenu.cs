using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsWindow;
    public Image blackScreen; // Reference to the black screen image
    public float fadeDuration = 1f; // Duration of the fade effect


    void Awake()
    {
        if (blackScreen == null)
        {
            GameObject found = GameObject.Find("BlackScreen");
            if (found != null)
            {
                blackScreen = found.GetComponent<Image>();
                Debug.Log("Found blackScreen via Find().");
            }
            else
            {
                Debug.LogError("BlackScreen object not found in scene!");
            }
        }
    }
    public void startGame()
    {
        Debug.Log("Start game clicked");
        // Start fade out coroutine
        StartCoroutine(FadeOutAndLoadScene("MuseumExterior"));
    }

    //Function performs the features of pausing the game, it may pause the state in the game.
    public void pauseGame()
    {
        SceneManager.LoadSceneAsync("PauseMenuSettings", LoadSceneMode.Additive); // add it on top of the scene, "pause"
        Time.timeScale = 0; // pause time in the game.
    }
    public void unPauseGame()
    {
        SceneManager.UnloadSceneAsync("PauseMenuSettings");
        Time.timeScale = 1; // restore state/time.
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // Fade out the black screen
        yield return StartCoroutine(FadeOut());

        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOut()
    {
        if (blackScreen == null)
        {
            Debug.LogError("Black screen image is not assigned!");
            yield break;
        }

        Debug.Log("Fading out black screen");
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
