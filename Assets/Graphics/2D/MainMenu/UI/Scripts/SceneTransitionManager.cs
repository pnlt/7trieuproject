using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance;

    public float fadeDuration = 1.0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LoadScene(string sceneName)
    {
        if (instance != null)
        {
            instance.StartCoroutine(instance.Transition(sceneName));
        }
        else
        {
            Debug.LogError("SceneTransitionManager instance is null.");
        }
    }

    private IEnumerator Transition(string sceneName)
    {
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            RenderSettings.skybox.SetFloat("_Exposure", alpha); // Adjust as needed for your setup
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);

        timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            RenderSettings.skybox.SetFloat("_Exposure", alpha); // Adjust as needed for your setup
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
