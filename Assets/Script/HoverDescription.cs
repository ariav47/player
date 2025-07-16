using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Ganti referensi dari GameObject ke CanvasGroup
    public CanvasGroup descriptionCanvasGroup;
    
    // Atur durasi fade di Inspector
    public float fadeDuration = 0.3f; 

    private Coroutine fadeCoroutine;

    private void Start()
    {
        // Pastikan deskripsi tidak terlihat di awal
        if (descriptionCanvasGroup != null)
        {
            descriptionCanvasGroup.alpha = 0;
            descriptionCanvasGroup.blocksRaycasts = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Mulai animasi fade-in
        StartFade(1f, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Mulai animasi fade-out
        StartFade(0f, false);
    }

    private void StartFade(float targetAlpha, bool blocksRaycasts)
    {
        // Hentikan coroutine yang sedang berjalan jika ada, untuk menghindari konflik
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        
        // Atur agar bisa/tidak bisa diklik sebelum animasi dimulai
        if (descriptionCanvasGroup != null)
        {
            descriptionCanvasGroup.blocksRaycasts = blocksRaycasts;
        }

        // Mulai coroutine yang baru
        fadeCoroutine = StartCoroutine(FadeCanvasGroup(targetAlpha));
    }

    private IEnumerator FadeCanvasGroup(float targetAlpha)
    {
        if (descriptionCanvasGroup == null) yield break;

        float startAlpha = descriptionCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Hitung nilai alpha baru secara bertahap menggunakan Lerp
            descriptionCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null; // Tunggu frame berikutnya
        }

        // Pastikan nilai alpha berakhir tepat di target
        descriptionCanvasGroup.alpha = targetAlpha;
    }
}