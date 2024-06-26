using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtDiamonds, txtWinCondition;
    [SerializeField] GameObject winCondition;

    private HealthManager healthMan;
    public Slider healthBar;
    public float animationDuration = 0.5f; // Durasi animasi tween

    [SerializeField] private Image diamondImage; // Tambahkan referensi ke Image untuk diamond
    [SerializeField] private Sprite[] diamondSprites; // Tambahkan array untuk menyimpan gambar diamond untuk setiap scene

    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name); // Debug line
        ResetHealthBar();
        UpdateDiamondImage(scene.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        healthMan = FindObjectOfType<HealthManager>();
        healthBar.maxValue = healthMan.maxHealth;
        healthBar.value = healthMan.currentHealth;
        UpdateDiamondImage(SceneManager.GetActiveScene().name); // Update diamond image at start
    }

    // Update is called once per frame
    void Update()
    {
        // Hanya lakukan tweening jika nilai health berubah
        if (healthBar.value != healthMan.currentHealth)
        {
            StartCoroutine(AnimateHealthBar(healthMan.currentHealth));
        }
    }

    private IEnumerator AnimateHealthBar(float targetHealth)
    {
        float startHealth = healthBar.value;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            healthBar.value = Mathf.Lerp(startHealth, targetHealth, elapsed / animationDuration);
            yield return null;
        }

        healthBar.value = targetHealth;
    }

    public void SetHealthBarValue(float value)
    {
        healthBar.value = value;
    }

    public void ResetHealthBar()
    {
        healthMan = FindObjectOfType<HealthManager>(); // Reset reference to HealthManager
        if (healthMan != null)
        {
            healthBar.maxValue = healthMan.maxHealth;
            healthBar.value = healthMan.currentHealth;
        }
    }

    public void UpdateDiamondUI(int _diamonds, int _winCondition)
    {
        txtDiamonds.text = _diamonds + " / " + _winCondition;
    }

    public void ShowWinCondition(int _diamonds, int _winCondition)
    {
        winCondition.SetActive(true);
        txtWinCondition.text = "You need " + (_winCondition - _diamonds) + " more items"; // Update this line
    }

    public void HideWinCondition()
    {
        winCondition.SetActive(false);
    }

    private void UpdateDiamondImage(string sceneName)
    {
        Debug.Log("Updating diamond image for scene: " + sceneName); // Debug line
        switch (sceneName)
        {
            case "Char":
                diamondImage.sprite = diamondSprites[0];
                break;
            case "Level 2":
                diamondImage.sprite = diamondSprites[1];
                break;
            case "Level 3":
                diamondImage.sprite = diamondSprites[2];
                break;
            default:
                diamondImage.sprite = diamondSprites[0];
                break;
        }
        Debug.Log("Diamond image updated to: " + diamondImage.sprite.name); // Debug line
    }
}
