using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtDiamonds;
    [SerializeField] private GameObject winCondition;
    [SerializeField] private TextMeshProUGUI txtWinCondition;
    [SerializeField] private Slider healthBar;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Image diamondImage;
    [SerializeField] private Sprite[] diamondSprites;

    private HealthManager healthMan;
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
        Debug.Log("Scene loaded: " + scene.name);
        ResetHealthBar();
        UpdateDiamondImage(scene.name);
        HideWinCondition(); // Pastikan win condition tersembunyi setiap kali scene dimuat ulang
    }

    void Start()
    {
        healthMan = FindObjectOfType<HealthManager>();
        if (healthMan != null)
        {
            healthBar.maxValue = healthMan.maxHealth;
            healthBar.value = healthMan.currentHealth;
        }
        UpdateDiamondImage(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (healthMan != null && healthBar.value != healthMan.currentHealth)
        {
            StartCoroutine(AnimateHealthBar(healthMan.currentHealth));
        }
    }

    private IEnumerator AnimateHealthBar(float targetHealth)
    {
        float startHealth = healthBar.value;
        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            healthBar.value = Mathf.Lerp(startHealth, targetHealth, timer / animationDuration);
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
        txtWinCondition.text = "You need " + (_winCondition - _diamonds) + " more items";
    }

    public void HideWinCondition()
    {
        winCondition.SetActive(false);
    }

    private void UpdateDiamondImage(string sceneName)
    {
        switch (sceneName)
        {
            case "Char":
                if (diamondSprites.Length > 0)
                    diamondImage.sprite = diamondSprites[0];
                break;
            case "Level 2":
                if (diamondSprites.Length > 1)
                    diamondImage.sprite = diamondSprites[1];
                break;
            case "Level 3":
                if (diamondSprites.Length > 2)
                    diamondImage.sprite = diamondSprites[2];
                break;
            default:
                if (diamondSprites.Length > 0)
                    diamondImage.sprite = diamondSprites[0];
                break;
        }
        Debug.Log("Diamond image updated to: " + diamondImage.sprite.name);
    }
}
