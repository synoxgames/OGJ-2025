using UnityEngine;

public class TitleShine : MonoBehaviour
{
    [Header("Shine Object")]
    public Transform shine; // The thing that moves (can be a sprite, UI image in world space, etc.)

    [Header("Target Points")]
    public Transform startPoint; // Empty GameObject on the left
    public Transform endPoint;   // Empty GameObject on the right

    [Header("Timing")]
    public float duration = 1.5f;
    public float delay = 0f;

    void Start()
    {
        shine.gameObject.SetActive(false);
        Invoke(nameof(DoShine), delay);
    }

    public void DoShine()
    {
        StartCoroutine(Slide());
    }

    System.Collections.IEnumerator Slide()
    {
        shine.position = startPoint.position;
        shine.gameObject.SetActive(true);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            shine.position = Vector3.Lerp(startPoint.position, endPoint.position, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        shine.gameObject.SetActive(false);
    }
}
