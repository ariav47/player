using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    // ##########################################################


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
}