using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public DetectionZone attackZone;
    Animator animator;
    Rigidbody2D rb;

    private Animator myAnim;
    private Transform target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxRange;
    [SerializeField]
    private float minRange;
    [SerializeField]
    private float roamingRange;
    [SerializeField]
    private float minRoamingDelay = 1f;
    [SerializeField]
    private float maxRoamingDelay = 3f;

    private Vector3 roamingPosition;
    private bool isRoaming = false;

    // Health variables
    [SerializeField]
    public int maxHealth = 50;
    [SerializeField]
    public int currentHealth = 50;

    public GameObject healthBars;

    private UIManagerEnemy uiManagerEnemy;

    // Damage amount to test
    public int damageToTest = 10;

    // Audio variables
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip attackSound;

    void Start()
    {
        myAnim = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;
        currentHealth = maxHealth;
        StartCoroutine(Roaming());

        // Make sure to find the UIManagerEnemy in the correct way
        uiManagerEnemy = healthBars.GetComponent<UIManagerEnemy>();

        if (uiManagerEnemy != null)
        {
            uiManagerEnemy.SetHealth(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogWarning("UIManagerEnemy not found on healthBars.");
        }

        // Subscribe to the attack detection event
        if (attackZone != null)
        {
            attackZone.onPlayerAttackDetected += HandlePlayerAttackDetected;
        }

        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the game object.");
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);

        if (distanceToPlayer <= maxRange && distanceToPlayer >= minRange)
        {
            StopAllCoroutines();
            isRoaming = false;
            FollowPlayer();
        }
        else if (!isRoaming)
        {
            StartCoroutine(Roaming());
        }

        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    public void FollowPlayer()
    {
        myAnim.SetBool("isMoving", true);
        myAnim.SetFloat("moveX", target.position.x - transform.position.x);
        myAnim.SetFloat("moveY", target.position.y - transform.position.y);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    private IEnumerator Roaming()
    {
        isRoaming = true;
        while (true)
        {
            ChooseRoamingPosition();
            float distanceToRoamingPosition = Vector3.Distance(roamingPosition, transform.position);

            float currentSpeed = Random.Range(speed / 2, speed);
            while (distanceToRoamingPosition > 0.1f)
            {
                myAnim.SetBool("isMoving", true);
                myAnim.SetFloat("moveX", roamingPosition.x - transform.position.x);
                myAnim.SetFloat("moveY", roamingPosition.y - transform.position.y);
                transform.position = Vector3.MoveTowards(transform.position, roamingPosition, currentSpeed * Time.deltaTime);
                distanceToRoamingPosition = Vector3.Distance(roamingPosition, transform.position);
                yield return null;
            }

            myAnim.SetBool("isMoving", false);
            float currentRoamingDelay = Random.Range(minRoamingDelay, maxRoamingDelay);
            yield return new WaitForSeconds(currentRoamingDelay);
        }
    }

    private void ChooseRoamingPosition()
    {
        float randomX = Random.Range(transform.position.x - roamingRange, transform.position.x + roamingRange);
        float randomY = Random.Range(transform.position.y - roamingRange, transform.position.y + roamingRange);
        roamingPosition = new Vector3(randomX, randomY, transform.position.z);
    }

    private void HandlePlayerAttackDetected(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            myAnim.SetTrigger("attack");
            Debug.Log("Enemy is attacking!");

            // Play attack sound
            if (audioSource != null && attackSound != null)
            {
                Debug.Log("Playing attack sound"); // Debug line
                audioSource.PlayOneShot(attackSound);
            }
            else
            {
                Debug.LogWarning("AudioSource or AttackSound is missing."); // Debug line
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D called with: " + collision.gameObject.tag);

        if (collision.CompareTag("PlayerAttack"))
        {
            Debug.Log("Enemy hit by PlayerAttack!");

            // Langkah 1: Dapatkan referensi ke skrip PlayerController dari objek yang menyerang kita
            PlayerController player = collision.GetComponentInParent<PlayerController>();

            // Langkah 2: Lakukan pengecekan apakah player ditemukan
            if (player != null)
            {
                AttackResult attackResult = player.CalculateAttackDamage();
                // Kirim hasil serangan ke musuh
                TakeDamage(attackResult);
            }
            else
            {
                // Ini adalah fallback jika PlayerController tidak ditemukan, bisa pakai damage default
                Debug.LogWarning("PlayerAttack hit, but PlayerController component was not found in parents. Using default damage.");
            }
            if (uiManagerEnemy != null)
            {
                uiManagerEnemy.ShowHealthBar(); // Show health bar when attacked
            }
        }
    }

    [ContextMenu("Take Damage")]
    public void TakeDamage()
    {
        AttackResult testAttack = new AttackResult { damage = damageToTest, isCritical = false };
        TakeDamage(testAttack);
    }

    // Ubah tipe input dari 'int damage' menjadi 'AttackResult attackResult'
    public void TakeDamage(AttackResult attackResult)
    {
        // Untuk mendapatkan angkanya, kita akses properti .damage dari struct
        currentHealth -= attackResult.damage;

        // Kita juga bisa menggunakan informasi .isCritical untuk feedback
        Debug.Log("Enemy took " + attackResult.damage + " damage. Was critical: " + attackResult.isCritical);

        // Di sini Anda bisa menambahkan logika feedback jika kritikal
        // Misalnya, memunculkan teks damage berwarna kuning atau memainkan suara khusus
        if (attackResult.isCritical)
        {
            // ... Logika feedback kritikal ...
        }

        if (uiManagerEnemy != null)
        {
            uiManagerEnemy.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Calling Die method");
            Die();
        }
    }

    private void Die()
    {
        myAnim.SetBool("canMove", false);
        healthBars.SetActive(false);
        myAnim.SetBool("hasTarget", false);
        myAnim.SetTrigger("death");
        // Add any additional death handling logic here

        // Optionally, remove the enemy after a delay to allow death animation to play
        Debug.Log("Starting RemoveAfterDelay Coroutine");
        StartCoroutine(RemoveAfterDelay(2f)); // Adjust delay as needed
    }

    private IEnumerator RemoveAfterDelay(float delay)
    {
        Debug.Log("Starting RemoveAfterDelay Coroutine");
        yield return new WaitForSeconds(delay);
        Debug.Log("Removing Enemy GameObject");
        Destroy(gameObject);
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (attackZone != null)
        {
            attackZone.onPlayerAttackDetected -= HandlePlayerAttackDetected;
        }
    }
}
