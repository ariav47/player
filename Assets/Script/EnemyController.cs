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

    void Start()
    {
        myAnim = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;
        currentHealth = maxHealth;
        StartCoroutine(Roaming());

        uiManagerEnemy = FindObjectOfType<UIManagerEnemy>(); // Temukan UIManagerEnemy
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D called with: " + collision.gameObject.tag);

        if (collision.CompareTag("PlayerAttack")) // Assuming the player's attack collider has the tag "PlayerAttack"
        {
            Debug.Log("Enemy hit by PlayerAttack!");
            // Adjust the damage amount as necessary
            TakeDamage(damageToTest);

            if (uiManagerEnemy != null)
            {
                uiManagerEnemy.ShowHealthBar(); // Show health bar when attacked
            }
        }
    }

    [ContextMenu("Take Damage")]
    public void TakeDamage()
    {
        TakeDamage(damageToTest);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage, current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    private void Die()
    {
        myAnim.SetBool("canMove", false);
        healthBars.SetActive(false);
        myAnim.SetBool("hasTarget", false);
        // Add death animation trigger if needed
        myAnim.SetTrigger("death");
        
        // Start the coroutine to handle death
        // StartCoroutine(HandleDeath());
    }

    // private IEnumerator HandleDeath()
    // {
    //     // Wait for death animation to start
    //     yield return new WaitForEndOfFrame();

    //     // Get the length of the death animation
    //     AnimatorStateInfo stateInfo = myAnim.GetCurrentAnimatorStateInfo(0);
    //     float waitTime = stateInfo.length;

    //     // Wait for the animation to finish
    //     yield return new WaitForSeconds(waitTime);

    //     // Disable the enemy game object
    //     gameObject.SetActive(false);
    // }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }
}