using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageTransition : MonoBehaviour
{
    public Image mainBackground;
    public Image extraBackground;

    public float transitionDuration = 2f;

    private void Start()
    {
        StartCoroutine(TransitionImages());
    }

    private IEnumerator TransitionImages()
    {
        while (true)
        {
            yield return new WaitForSeconds(transitionDuration);

            StartCoroutine(FadeImage(mainBackground, 0f));
            yield return new WaitForSeconds(transitionDuration);

            // Swap the images
            SwapImages();

            StartCoroutine(FadeImage(mainBackground, 1f));
        }
    }

    private void SwapImages()
    {
        Image temp = mainBackground;
        mainBackground = extraBackground;
        extraBackground = temp;
    }

    private IEnumerator FadeImage(Image image, float targetAlpha)
    {
        Color currentColor = image.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

        float elapsed_time = 0f;

        while (elapsed_time < transitionDuration)
        {
            image.color = Color.Lerp(currentColor, targetColor, elapsed_time / transitionDuration);
            elapsed_time += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;
    }
}
