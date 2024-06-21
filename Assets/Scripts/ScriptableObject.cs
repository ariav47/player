using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectChanger : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] scriptableObjects;
    [SerializeField] private AnimeDisplay animeDisplay;
    [SerializeField] private AudioSource audioSource; // Add this field for audio
    private int currentIndex;

    private void Awake()
    {
        ChangeScriptableObject(0);
    }

    public void ChangeScriptableObject(int _change)
    {
        currentIndex += _change;
        if (currentIndex < 0) currentIndex = scriptableObjects.Length - 1;
        else if (currentIndex > scriptableObjects.Length - 1) currentIndex = 0;

        if (animeDisplay != null) 
        {
            animeDisplay.DisplayAnime((Anime)scriptableObjects[currentIndex]);
            PlayAnimeAudio((Anime)scriptableObjects[currentIndex]); // Play audio
        }
    }

    public void OnBackButtonClicked()
    {
        ChangeScriptableObject(-1);
    }

    private void PlayAnimeAudio(Anime anime)
    {
        if (audioSource != null && anime.audioClip != null)
        {
            audioSource.clip = anime.audioClip;
            audioSource.Play();
        }
    }
}
