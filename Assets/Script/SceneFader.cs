using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image is not set in the inspector.");
        }
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;
        SceneManager.LoadScene(sceneName);
        StartCoroutine(FadeIn(sceneName));
    }

    private IEnumerator FadeIn(string sceneName)
    {
        yield return null; // Wait for the scene to fully load
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 0f;
        fadeImage.color = color;

        // Reset health after fade in
        HealthManager healthManager = FindObjectOfType<HealthManager>();
        if (healthManager != null)
        {
            healthManager.ResetHealth();
        }
    }
}
