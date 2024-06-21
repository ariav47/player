using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerEnemy : MonoBehaviour
{
    private EnemyController enemyController;
    public Slider healthBar;
    public float animationDuration = 0.5f; // Durasi animasi tween
    public Transform enemy; // Referensi ke objek musuh
    public Vector3 offset; // Offset untuk posisi health bar di atas musuh
    

    private bool isHealthBarVisible = false;

    void Start()
    {
        enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            healthBar.maxValue = enemyController.maxHealth;
            healthBar.value = enemyController.currentHealth;
        }
        healthBar.gameObject.SetActive(false); // Menyembunyikan health bar saat start
    }

    void Update()
    {
        if (enemy != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(enemy.position + offset);
            healthBar.transform.position = screenPosition;
        }

        if (isHealthBarVisible && healthBar.value != enemyController.currentHealth)
        {
            StartCoroutine(AnimateHealthBar(enemyController.currentHealth));
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

    public void ShowHealthBar()
    {
        healthBar.gameObject.SetActive(true);
        isHealthBarVisible = true;
    }

    public void HideHealthBar()
    {
        healthBar.gameObject.SetActive(false);
        isHealthBarVisible = false;
    }
}
