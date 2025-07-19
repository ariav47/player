using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // ==========================================================
    // BAGIAN 1: SINGLETON PATTERN (INI PERBAIKANNYA)
    // ==========================================================
    public static LevelManager MyInstance; // Menggunakan 'MyInstance' agar konsisten dengan panggilan Anda

    private void Awake()
    {
        // Setup Singleton agar hanya ada satu LevelManager di seluruh game
        if (MyInstance == null)
        {
            MyInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Jika sudah ada, hancurkan duplikatnya
            Destroy(gameObject);
        }
    }
    // ==========================================================

    // BAGIAN 2: LOGIKA LEVEL LOCKING
    private const string LevelUnlockedKey = "HighestLevelUnlocked";

    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex <= PlayerPrefs.GetInt(LevelUnlockedKey, 1);
    }

    public void UnlockNextLevel(int completedLevelIndex)
    {
        int nextLevelIndex = completedLevelIndex + 1;
        if (nextLevelIndex > PlayerPrefs.GetInt(LevelUnlockedKey, 1))
        {
            PlayerPrefs.SetInt(LevelUnlockedKey, nextLevelIndex);
            PlayerPrefs.Save();
            Debug.Log("LEVEL " + nextLevelIndex + " UNLOCKED!");
        }
    }

   public void LoadToScene(string sceneName)
   {
       // --- PERBAIKAN DI SINI ---
       // Cari SceneFader yang aktif di scene saat ini
       SceneFader fader = FindObjectOfType<SceneFader>();
       if (fader != null)
       {
           // Jika ditemukan, gunakan untuk berpindah scene
           fader.FadeOutAndLoadScene(sceneName);
       }
       else
       {
           // Fallback jika tidak ada fader
           Debug.LogWarning("SceneFader not found in scene. Loading scene directly.");
           UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
       }
       // --- AKHIR PERBAIKAN ---
   }
    
    // (Opsional) Method untuk mereset progres untuk keperluan testing
    [ContextMenu("Reset Level Progress")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(LevelUnlockedKey);
        Debug.Log("Level progress has been reset. Only Level 1 is unlocked.");
    }
}