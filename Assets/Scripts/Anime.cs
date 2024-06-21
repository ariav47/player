using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Anime", menuName = "Scriptable Object/Anime")]

public class Anime : ScriptableObject
{
    public int animeIndex;
    public string animeName;
    public string[] animeDescription; // Change to string[]
    public Color nameColor;
    public Sprite animeImage;
    public Object sceneToLoad;
    public AudioClip audioClip;
}
