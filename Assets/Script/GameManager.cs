using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static GameManager MyInstance;

    // --- References ---
    [Tooltip("Referensi ke SceneFader di setiap scene, akan dicari otomatis.")]
    private SceneFader sceneFader;
    [Tooltip("Referensi ke UIManager yang aktif di scene saat ini, akan dicari otomatis.")]
    public UIManager currentUIManager;

    // --- Game State Variables ---
    private int collectedDiamonds;
    private int winCondition;
    
    [Header("Audio")]
    [SerializeField] private AudioClip diamondCollectSound;
    private AudioSource audioSource;

    private void Awake()
    {
        // Setup Singleton yang abadi (persistent)
        if (MyInstance == null)
        {
            MyInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Inisialisasi AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Mendaftarkan method OnSceneLoaded agar berjalan setiap kali scene baru dimuat
    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    // Method ini berjalan setiap kali scene baru selesai dimuat
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cari UIManager dan SceneFader yang baru di scene yang baru dimuat
        currentUIManager = FindObjectOfType<UIManager>();
        sceneFader = FindObjectOfType<SceneFader>();

        // Memberi peringatan jika komponen penting tidak ditemukan
        if (currentUIManager == null) 
            Debug.LogError("FATAL ERROR: UIManager tidak ditemukan di scene: " + scene.name);
        
        // Memperbarui win condition untuk level saat ini
        SetWinConditionForLevel(scene.name);

        // Memberi perintah ke UIManager untuk memperbarui tampilannya
        if(currentUIManager != null) 
            currentUIManager.UpdateUIOnSceneLoad();
    }

    // Method untuk mengatur win condition berdasarkan nama scene
    private void SetWinConditionForLevel(string sceneName)
    {
        collectedDiamonds = 0; // Reset diamond setiap mulai level baru
        switch (sceneName)
        {
            case "Char":
                winCondition = 3;
                break;
            case "Level 2":
                winCondition = 5;
                break;
            case "Level 3":
                winCondition = 3;
                break;
            default:
                winCondition = 0; // Tidak ada win condition untuk scene lain
                break;
        }
    }

    // Method untuk menambah diamond, dipanggil oleh item diamond
    public void AddDiamonds(int amount)
    {
        collectedDiamonds += amount;
        if (currentUIManager != null)
            currentUIManager.UpdateDiamondUI(collectedDiamonds, winCondition);

        if (audioSource != null && diamondCollectSound != null)
            audioSource.PlayOneShot(diamondCollectSound);
    }
    
    // Method untuk memeriksa penyelesaian level, dipanggil oleh FinishPoint
    public void CheckLevelCompletion(int requiredDiamonds, string sceneToLoad)
    {
        if (collectedDiamonds >= requiredDiamonds)
        {
            if (LevelManager.MyInstance != null)
            {
                // Coba unlock level berikutnya
                string currentSceneName = SceneManager.GetActiveScene().name;
                if (currentSceneName == "Char") LevelManager.MyInstance.UnlockNextLevel(1);
                else if (currentSceneName == "Level 2") LevelManager.MyInstance.UnlockNextLevel(2);
                else if (currentSceneName == "Level 3") LevelManager.MyInstance.UnlockNextLevel(3);
            }
            
            if (sceneFader != null)
                sceneFader.FadeOutAndLoadScene(sceneToLoad);
            else
                SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            if (currentUIManager != null)
                currentUIManager.ShowWinCondition(collectedDiamonds, requiredDiamonds);
        }
    }

    // Method untuk menampilkan layar Game Over
    public void GameOver()
    {
        if (currentUIManager != null)
            currentUIManager.ShowGameOverUI();
        Time.timeScale = 0f;
    }

    // --- Getter Methods ---
    public int GetCurrentCollectedDiamonds() { return collectedDiamonds; }
    public int GetCurrentWinCondition() { return winCondition; }
    
    // --- Scene Management ---
    public void RestartGame() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadHomeScene() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home"); // Pastikan nama scene menu utama Anda adalah "Home"
    }
}