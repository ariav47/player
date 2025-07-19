using UnityEngine;

// Attribute ini memastikan GameObject yang memiliki skrip ini PASTI punya AudioSource
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // Setup Singleton agar mudah diakses dari mana saja
    public static AudioManager Instance;

    private AudioSource audioSource;

    [Header("Sound Effect Clips")]
    public AudioClip diamondCollectSound;
    public AudioClip playerHurtSound;
    public AudioClip enemyDefeatedSound;
    public AudioClip damageBoostSound;
    // Tambahkan AudioClip lain di sini jika perlu

    private void Awake()
    {
        // Logic Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Ambil komponen AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // Method publik untuk memutar setiap suara
    public void PlayDiamondCollectSound()
    {
        if (diamondCollectSound != null)
        {
            audioSource.PlayOneShot(diamondCollectSound);
        }
    }

    // Anda bisa membuat method lain untuk suara lainnya
    public void PlayPlayerHurtSound()
    {
        if (playerHurtSound != null)
        {
            audioSource.PlayOneShot(playerHurtSound);
        }
    }
}