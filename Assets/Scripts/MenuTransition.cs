using UnityEngine;
using System.Collections;

public class MenuTransition : MonoBehaviour
{
    public CanvasGroup mainMenu;
    public CanvasGroup settingsMenu;
    public float transitionTime = 0.3f;

    public void ShowSettings()
    {
        StartCoroutine(SwapMenus(mainMenu, settingsMenu));
    }

    public void ShowMain()
    {
        StartCoroutine(SwapMenus(settingsMenu, mainMenu));
    }

    IEnumerator SwapMenus(CanvasGroup from, CanvasGroup to)
    {
        to.gameObject.SetActive(true);
        from.interactable = false;
        from.blocksRaycasts = false;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / transitionTime;
            from.alpha = Mathf.Lerp(1, 0, t);
            to.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        // Final cleanup
    from.alpha = 0;
    from.gameObject.SetActive(false); // HIDE old menu

    to.alpha = 1;
    to.interactable = true;
    to.blocksRaycasts = true;
    }
}
