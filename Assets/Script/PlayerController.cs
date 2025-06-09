using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

 public struct AttackResult
    {
        public int damage;
        public bool isCritical; 
    }
    
public class PlayerController : MonoBehaviour
{
    // --- Letakkan ini di bagian atas class PlayerController ---
    [Header("Combat Stats")]
    public float baseDamage = 10f;
    [SerializeField] private float bonusDamage = 0f;
    public float TotalDamage { get { return baseDamage + bonusDamage; } }

    [Header("Critical Hit Stats")]
    [Range(0f, 1f)] // Membuat slider di Inspector dari 0 sampai 1
    public float criticalChance = 0.2f; // Peluang 20% untuk kritikal
    public float criticalMultiplier = 2f; // Damage akan menjadi 2x lipat saat kritikal

    public float runSpeed = 5f;
    Vector2 moveInput;
    private bool _isAttacking = false;
    private int attackCombo = 0;
    private float comboTimer = 0f;
    public float comboDelay = 0f;

    public AudioClip attackSound;
    private AudioSource audioSource;

    [Header("Slide Settings")]
    public float slideSpeed = 10f;
    public float slideDuration = 0.5f;

    public bool CanMove { get { return animator.GetBool(AnimationStrings.canMove); } }

    private bool _isMoving = false;
    public bool IsMoving { 
        get { return _isMoving; } 
        private set {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    private bool _isSliding = false; 
    public bool IsSliding {
        get { return _isSliding; }
        private set {
            _isSliding = value;
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight {
        get { return _isFacingRight; } 
        private set {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
    
    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); 
    }

    // #################### PERBAIKAN DI SINI ####################
    private void FixedUpdate()
    {
        // Jika sedang menyerang, paksa berhenti total.
        if (_isAttacking)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // Jika sedang sliding, jangan lakukan apa-apa di FixedUpdate.
        // Biarkan coroutine yang mengatur kecepatan.
        if (_isSliding)
        {
            return;
        }

        // Jika tidak sedang menyerang ataupun sliding, jalankan pergerakan normal.
        rb.velocity = new Vector2(moveInput.x * runSpeed, moveInput.y * runSpeed);

        // Logika timer combo tetap di sini
        if (attackCombo > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboDelay)
            {
                ResetCombo();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!_isAttacking && !_isSliding)
        {
            moveInput = context.ReadValue<Vector2>();
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            moveInput = Vector2.zero; 
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !_isAttacking && !_isSliding && attackCombo == 0) 
        {
            _isAttacking = true;
            animator.SetBool(AnimationStrings.canMove, false);

            attackCombo++;
            comboTimer = 0f;
            animator.SetTrigger(AnimationStrings.attackTrigger);
            Debug.Log("Attack started, combo: " + attackCombo);

            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.started && !_isAttacking && !_isSliding && IsMoving)
        {
            StartCoroutine(PerformSlide());
        }
    }

    private IEnumerator PerformSlide()
    {
        _isSliding = true;
        animator.SetBool(AnimationStrings.isSliding, true);
        animator.SetBool(AnimationStrings.canMove, false); 

        float slideDirection = IsFacingRight ? 1f : -1f;
        // Kita hanya terapkan kecepatan di sumbu X untuk slide
        rb.velocity = new Vector2(slideDirection * slideSpeed, 0f);

        yield return new WaitForSeconds(slideDuration);

        _isSliding = false;
        animator.SetBool(AnimationStrings.isSliding, false);
        animator.SetBool(AnimationStrings.canMove, true); 
    }

    public void OnAttackAnimationEnd()
    {
        Debug.Log("Attack animation ended");
        ResetCombo();
        _isAttacking = false;
        animator.SetBool(AnimationStrings.canMove, true); 
    }

    private void ResetCombo()
    {
        attackCombo = 0;
        Debug.Log("Combo reset");
    }

    public static class AnimationStrings
    {
        public static string isMoving = "isMoving";
        public static string isSliding = "isSliding";
        public static string canMove = "canMove";
        public static string attackTrigger = "Attack";
    }

    // --- Letakkan method ini di dalam class PlayerController ---
    public void ApplyDamageBuff(float damageAmount, float buffDuration)
    {
        StartCoroutine(DamageBuffCoroutine(damageAmount, buffDuration));
    }

    private IEnumerator DamageBuffCoroutine(float damageToAdd, float duration)
    {
        bonusDamage += damageToAdd;
        Debug.Log("DAMAGE UP! Total Damage: " + TotalDamage + " selama " + duration + " detik.");

        yield return new WaitForSeconds(duration);

        bonusDamage -= damageToAdd;
        Debug.Log("Buff selesai. Total Damage kembali ke normal: " + TotalDamage);
    }

    public AttackResult CalculateAttackDamage()
    {
        float damage = this.TotalDamage;
        bool isCriticalHit = false;

        // "Lempar dadu" dengan angka acak antara 0 dan 1
        if (Random.value <= criticalChance)
        {
            // Jika angka acak lebih kecil atau sama dengan peluang kita, itu KRITIKAL!
            isCriticalHit = true;
            damage *= criticalMultiplier;
            Debug.Log("CRITICAL HIT!");
        }

        // Siapkan hasilnya
        AttackResult result = new AttackResult
        {
            damage = Mathf.RoundToInt(damage),
            isCritical = isCriticalHit
        };

        return result;
    }
}