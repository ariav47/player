using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int _currentHealth;
    public int CurrentHealth { get { return _currentHealth; } }

    [Header("Invincibility & Flash")]
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float flashDelay = 0.1f;
    private SpriteRenderer playerSprite;
    private bool isInvincible = false;

    [Header("Death & Respawn")]
    [SerializeField] private float respawnDelay = 1f;
    private Vector2 startPos;
    
    [Header("Audio")]
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip hurtSound;
    private AudioSource audioSource;

    private Animator animator;

    private void Awake()
    {
        // Semua inisialisasi komponen di satu tempat
        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null) Debug.LogError("Animator component not found!");
        if (audioSource == null) Debug.LogError("AudioSource component not found!");
    }

    private void Start()
    {
        startPos = transform.position;
        _currentHealth = maxHealth;
        // Panggil UI Manager untuk set health bar di awal (pastikan UIManager ada)
        if (UIManager.MyInstance != null)
            UIManager.MyInstance.SetHealthBarValue(_currentHealth, maxHealth);
    }

    public void HurtPlayer(int damageToGive)
    {
        if (isInvincible) return; // Abaikan damage jika sedang kebal

        _currentHealth -= damageToGive;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);

        if (UIManager.MyInstance != null)
            UIManager.MyInstance.SetHealthBarValue(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    public void Heal(int healAmount)
    {
        _currentHealth += healAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

        if (audioSource != null && healSound != null)
            audioSource.PlayOneShot(healSound);
        
        if (UIManager.MyInstance != null)
            UIManager.MyInstance.SetHealthBarValue(_currentHealth, maxHealth);
    }

    private void Die()
    {
        isInvincible = true;
        animator.SetTrigger("death");
        // Proses selanjutnya (GameOver/Respawn) akan dipanggil oleh Animation Event
    }
    
    // PENTING: Method ini harus Anda panggil dari Animation Event di frame terakhir animasi kematian
    public void OnDeathAnimationFinished()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        
        if (GameManager.MyInstance != null)
            GameManager.MyInstance.GameOver();
    }
    
    public void RespawnPlayer()
    {
        transform.position = startPos;
        _currentHealth = maxHealth;
        isInvincible = false;
        animator.Play("player_idle"); // Paksa kembali ke animasi idle
        if (UIManager.MyInstance != null)
            UIManager.MyInstance.SetHealthBarValue(_currentHealth, maxHealth);
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;
        // Logika kedip yang jauh lebih simpel
        for (float i = 0; i < invincibilityDuration; i += flashDelay * 2)
        {
            playerSprite.color = new Color(1f, 1f, 1f, 0.5f); // Set transparan
            yield return new WaitForSeconds(flashDelay);
            playerSprite.color = Color.white; // Kembali normal
            yield return new WaitForSeconds(flashDelay);
        }
        isInvincible = false;
    }
}