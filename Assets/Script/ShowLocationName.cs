using UnityEngine;
using TMPro;
using System.Collections;

public class ShowLocationNameTMP : MonoBehaviour
{
    public string locationName; // Nama tempat
    public TMP_Text locationText; // Referensi ke TextMeshPro UI
    public float displayTime = 3.0f; // Waktu tampil teks
    public float fadeDuration = 1.0f; // Durasi fade in dan fade out

    void Start()
    {
        locationText.enabled = false;
        locationText.text = locationName;
        StartCoroutine(FadeText());
    }

    IEnumerator FadeText()
    {
        yield return StartCoroutine(FadeIn());

        yield return new WaitForSeconds(displayTime);

        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        locationText.enabled = true;
        Color originalColor = locationText.color;
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            locationText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        locationText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    IEnumerator FadeOut()
    {
        Color originalColor = locationText.color;
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            locationText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        locationText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        locationText.enabled = false;
    }
}
