using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuFadeIn : MonoBehaviour
{
    public CanvasGroup[] buttonGroups;
    public float fadeTime = 0.5f;
    public float delayBetween = 0.1f;

    void Awake()
    {
        // Ensure all button groups are initially hidden
        foreach (CanvasGroup group in buttonGroups)
        {
            group.alpha = 0f;
            group.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        StartCoroutine(FadeInButtons());
    }

    IEnumerator FadeInButtons()
    {
        foreach (CanvasGroup group in buttonGroups)
        {
            group.alpha = 0f;
            group.gameObject.SetActive(true);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / fadeTime;
                group.alpha = Mathf.SmoothStep(0, 1, t);
                yield return null;
            }

            yield return new WaitForSeconds(delayBetween);
        }
    }
}
