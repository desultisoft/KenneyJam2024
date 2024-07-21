using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutOnLoad : MonoBehaviour
{
    public Image image; // Assign the Image component in the Inspector
    public float fadeDuration = 2f; // Duration of the fade effect
    public float delayBeforeFade = 1f; // Delay before starting the fade

    void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        StartFadeOut();
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        // Wait for the delay before starting the fade-out
        yield return new WaitForSeconds(delayBeforeFade);

        float startAlpha = image.color.a;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            Color tempColor = image.color;
            tempColor.a = Mathf.Lerp(startAlpha, 0, progress);
            image.color = tempColor;

            progress += rate * Time.deltaTime;

            yield return null;
        }

        Color finalColor = image.color;
        finalColor.a = 0;
        image.color = finalColor;
    }
}
