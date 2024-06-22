using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtDiamonds, txtWinCondition;
    [SerializeField] GameObject winCondition;
    
    private HealthManager healthMan;
    public Slider healthBar;
    public float animationDuration = 0.5f; // Durasi animasi tween

    // Start is called before the first frame update
    void Start()
    {
        healthMan = FindObjectOfType<HealthManager>();
        healthBar.maxValue = healthMan.maxHealth;
        healthBar.value = healthMan.currentHealth;
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

        private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
                instance = new UIManager();

            return instance;
        }
    }

    public void UpdateDiamondUI(int _diamonds, int _winCondition)
    {
        txtDiamonds.text = _diamonds + " / " + _winCondition;
    }

    public void ShowWinCondition(int _diamonds, int _winCondition)
    {
        winCondition.SetActive(true);
        txtWinCondition.text = "You need " + (_winCondition - _diamonds) + " more Diamonds";
    }

    public void HideWinCondition()
    {
        winCondition.SetActive(false);
    }    
}
