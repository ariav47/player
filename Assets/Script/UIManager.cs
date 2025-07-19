using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // --- Singleton Instance (Non-Persisten) ---
    public static UIManager MyInstance;

    // --- Referensi ke Elemen UI (Diatur di Inspector per scene) ---
    [Header("UI Elements")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI txtDiamonds;
    [SerializeField] private GameObject winConditionPanel;
    [SerializeField] private TextMeshProUGUI txtWinCondition;
    [SerializeField] private Image diamondImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject buffDurationPanel;
    [SerializeField] private TextMeshProUGUI buffDurationText;
    
    [Header("Assets")]
    [SerializeField] private Sprite[] diamondSprites;
    
    [Header("Settings")]
    [SerializeField] private float healthAnimationDuration = 0.5f;

    private HealthManager healthMan;
    private Coroutine healthAnimationCoroutine;

    private void Awake()
    {
        Debug.Log("STATUS: Scene 'Level' SUDAH dimuat, method Awake() dari UIManager berjalan.");
        // Menjadikan dirinya sebagai instance UTAMA untuk scene ini.
        MyInstance = this;
    }

    // Method ini dipanggil oleh GameManager setiap kali scene dimuat
    public void UpdateUIOnSceneLoad()
    {
        // Cari referensi ke HealthManager di scene ini
        healthMan = FindObjectOfType<HealthManager>();
        
        // Nonaktifkan panel-panel yang tidak seharusnya muncul di awal
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winConditionPanel != null) winConditionPanel.SetActive(false);
        if (buffDurationPanel != null) buffDurationPanel.SetActive(false);
        
        // Reset Health Bar
        ResetHealthBar();

        // Update gambar diamond sesuai nama scene
        if (GameManager.MyInstance != null)
        {
            UpdateDiamondImage(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
    
    // --- Method untuk Health Bar ---
    public void SetHealthBarValue(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            if (healthAnimationCoroutine != null)
            {
                StopCoroutine(healthAnimationCoroutine);
            }
            healthAnimationCoroutine = StartCoroutine(AnimateHealthBar(currentHealth));
        }
    }
    
    public void ResetHealthBar()
    {
        if (healthMan == null) healthMan = FindObjectOfType<HealthManager>();

        if (healthMan != null && healthBar != null)
        {
            healthBar.maxValue = healthMan.maxHealth;
            healthBar.value = healthMan.CurrentHealth;
        }
    }

    private System.Collections.IEnumerator AnimateHealthBar(float targetHealth)
    {
        float startHealth = healthBar.value;
        float timer = 0f;
        while (timer < healthAnimationDuration)
        {
            timer += Time.deltaTime;
            healthBar.value = Mathf.Lerp(startHealth, targetHealth, timer / healthAnimationDuration);
            yield return null;
        }
        healthBar.value = targetHealth;
        healthAnimationCoroutine = null;
    }
    
    // --- Method untuk UI Lainnya ---

    public void UpdateDiamondUI(int diamonds, int winCondition)
    {
        if (txtDiamonds != null)
        {
            txtDiamonds.text = diamonds + " / " + winCondition;
        }
    }

    public void ShowWinCondition(int diamonds, int winCondition)
    {
        if (winConditionPanel != null && txtWinCondition != null)
        {
            winConditionPanel.SetActive(true);
            txtWinCondition.text = "You need " + (winCondition - diamonds) + " more items";
        }
    }

    public void HideWinCondition()
    {
        if (winConditionPanel != null)
        {
            winConditionPanel.SetActive(false);
        }
    }

    public void ShowGameOverUI()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    private void UpdateDiamondImage(string sceneName)
    {
        if (diamondImage == null || diamondSprites == null || diamondSprites.Length == 0) return;

        switch (sceneName)
        {
            case "Char":
                diamondImage.sprite = diamondSprites[0];
                break;
            case "Level 2":
                if (diamondSprites.Length > 1) diamondImage.sprite = diamondSprites[1];
                break;
            case "Level 3":
                if (diamondSprites.Length > 2) diamondImage.sprite = diamondSprites[2];
                break;
            default:
                diamondImage.sprite = diamondSprites[0];
                break;
        }
    }

    // --- Method untuk Buff Timer ---
    
    public void ShowBuffTimer(bool status)
    {
        if (buffDurationPanel != null)
        {
            buffDurationPanel.SetActive(status);
        }
    }

    public void UpdateBuffTimer(float timeRemaining)
    {
        if (buffDurationPanel != null && buffDurationPanel.activeSelf)
        {
            buffDurationText.text = Mathf.Ceil(timeRemaining).ToString();
        }
    }
}