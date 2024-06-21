using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimeDisplay : MonoBehaviour
{
    [SerializeField] private Text animeName;
    [SerializeField] private Text animeDescription;
    [SerializeField] private Image animeImage;
    [SerializeField] private AudioSource audioSource;

    public void DisplayAnime(Anime _anime)
    {
        animeName.text = _anime.animeName;

        string fullDescription = string.Join("\n", _anime.animeDescription);
        animeDescription.text = fullDescription;
        animeImage.sprite = _anime.animeImage;

        if (_anime.audioClip != null && audioSource != null)
        {
            audioSource.clip = _anime.audioClip;
            audioSource.Play();
        }
    }
}
