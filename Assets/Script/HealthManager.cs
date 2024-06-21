using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    Vector2 startPos;

    public int currentHealth;
    public int maxHealth;

    private bool flashActive;
    [SerializeField]
    private float healthAmount;
    private float flashLength = 0f;
    private float flashCounter = 0f;
    private SpriteRenderer playerSprite;

    private Animator animator;

    [SerializeField]
    private float respawnDelay = 1f; // Waktu delay tambahan setelah animasi kematian selesai

    void Start()
    {
        startPos = transform.position;

        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the game object.");
        }
    }

    void Update()
    {
        if (flashActive)
        {
            HandleFlash();
        }
    }

    private void HandleFlash()
    {
        if (flashCounter > flashLength * .99f)
        {
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
        }
        else if (flashCounter > flashLength * .82f)
        {
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
        }
        else if (flashCounter > flashLength * .66f)
        {
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
        }
        else if (flashCounter > flashLength * .49f)
        {
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
        }
        else if (flashCounter > flashLength * .33f)
        {
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
        }
        else if (flashCounter > flashLength * .16f)
        {
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
        }
        else if (flashCounter > 0f)
        {
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
        }
        else
        {
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            flashActive = false;
        }
        flashCounter -= Time.deltaTime;
    }

    public void HurtPlayer(int damageToGive)
    {
        currentHealth -= damageToGive;
        flashActive = true;
        flashCounter = flashLength;

        Debug.Log("HurtPlayer called, currentHealth: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Triggering player_death animation");
            flashActive = false;  // Stop flashing
            animator.SetTrigger("death");
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Get the current animation state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait until the death animation is finished
        yield return new WaitForSeconds(stateInfo.length);

        // Add additional delay
        yield return new WaitForSeconds(respawnDelay);
        // Respawn the player
        Respawn();
    }

    void Respawn()
    {
        transform.position = startPos;
        currentHealth = maxHealth;
        Debug.Log("Player respawned.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D called with " + collision.gameObject.name); // Debug line
        if (collision.CompareTag("Health"))
        {
            Debug.Log("Collision with Health item detected"); // Debug line
            Heal(healthAmount);
            collision.gameObject.SetActive(false);
        }
    }

    private void Heal(float amount)
    {
        Debug.Log("Healing player by " + amount); // Debug line
        int previousHealth = currentHealth; // Debug line
        currentHealth += Mathf.RoundToInt(amount);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Health changed from " + previousHealth + " to " + currentHealth); // Debug line
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // Implement your health bar update logic here
    }
}
