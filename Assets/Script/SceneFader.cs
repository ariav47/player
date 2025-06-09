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
        else
        {
            fadeImage.gameObject.SetActive(false); // Nonaktifkan fadeImage saat game dimulai
        }
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        fadeImage.gameObject.SetActive(true); // Aktifkan fadeImage saat fade out dimulai
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
        fadeImage.gameObject.SetActive(false); // Nonaktifkan fadeImage setelah fade in selesai

        // Reset health after fade in
        HealthManager healthManager = FindObjectOfType<HealthManager>();
        if (healthManager != null)
        {
            healthManager.RespawnPlayer();
        }
    }
}
