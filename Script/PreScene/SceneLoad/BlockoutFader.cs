using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlockoutFader : MonoBehaviour
{
    public Image blockout;
    public float fadeDuration = 1f;

    void Awake()
    {
        if (blockout == null) blockout = GetComponent<Image>();
    }

    public IEnumerator FadeIn()
    {
        float t = 0;
        Color c = blockout.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            blockout.color = c;
            yield return null;
        }
        c.a = 0f;
        blockout.color = c;
    }

    public IEnumerator FadeOut()
    {
        float t = 0;
        Color c = blockout.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            blockout.color = c;
            yield return null;
        }
        c.a = 1f;
        blockout.color = c;
    }
}
