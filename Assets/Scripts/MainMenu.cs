using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public GameObject settingsWindow;

    public void startGame()
    {
        Debug.Log("Start game clicked");
        //Load the Meuseum cut scene timed
        SceneManager.LoadScene("MuseumExterior");
        new WaitForSeconds(1f);
        Debug.Log("Waited 1 sec");


        //SceneManager.LoadScene("Level1");
        //SceneManager.LoadSceneAsync("NextSvener", LoadSceneMode.Additive); // add it on top of the scene, "pause"
        //SceneManager.UnloadScene("NextSvener");

        // For other scenes.
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

}
