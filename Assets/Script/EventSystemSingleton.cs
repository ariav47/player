using UnityEngine;
using UnityEngine.EventSystems; // Penting

public class EventSystemSingleton : MonoBehaviour
{
    // Properti statis untuk menyimpan satu-satunya instance
    public static EventSystemSingleton Instance;

    private void Awake()
    {
        // Jika belum ada instance utama...
        if (Instance == null)
        {
            // ...maka jadikan GameObject ini sebagai instance utama.
            Instance = this;
            // Dan jangan hancurkan saat pindah scene.
            DontDestroyOnLoad(gameObject);
        }
        // Jika sudah ada instance utama dan itu bukan GameObject ini...
        else if (Instance != this)
        {
            // ...maka hancurkan GameObject ini karena ia adalah duplikat.
            Debug.LogWarning("Duplicate EventSystem found and destroyed.");
            Destroy(gameObject);
        }
    }
}