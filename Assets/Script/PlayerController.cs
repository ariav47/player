using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    Vector2 moveInput;
    private int attackCombo = 0;
    private float comboTimer = 0f;
    public float comboDelay = 0f; // Time delay to reset combo

    public AudioClip attackSound; // Audio clip for the attack sound
    private AudioSource audioSource; // Audio source component

    public float CurrentMovSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving)
                {
                    return runSpeed;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                // Movement lock
                return 0;
            }
        }
    }

    private bool _isMoving = false;
    public bool IsMoving { 
        get { return _isMoving; } 
        private set {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
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
    
    public bool CanMove {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * CurrentMovSpeed;

        // Combo timer logic
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
        moveInput = context.ReadValue<Vector2>();

        if (CanMove)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
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
        if (context.started && CanMove && attackCombo == 0) // Attack only if currently able to move and not in a combo
        {
            attackCombo++;
            comboTimer = 0f; // Reset combo timer
            animator.SetBool(AnimationStrings.canMove, false); // Disable movement
            animator.SetInteger("attackCombo", attackCombo); // Set combo parameter
            animator.SetTrigger(AnimationStrings.attackTrigger);
            Debug.Log("Attack started, combo: " + attackCombo);

            // Play attack sound
            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
    }


    // This method should be called at the end of each attack animation
    public void OnAttackAnimationEnd()
    {
        Debug.Log("Attack animation ended, combo: " + attackCombo);
        if (attackCombo >= 2)
        {
            ResetCombo();
        }
        else if (attackCombo > 0)
        {
            // Keep the combo going
            comboTimer = 0f;
        }
        else
        {
            animator.SetBool(AnimationStrings.canMove, true); // Enable movement after combo
        }
    }

    private void ResetCombo()
    {
        attackCombo = 0;
        animator.SetInteger("attackCombo", attackCombo);
        animator.SetBool(AnimationStrings.canMove, true); // Enable movement after combo
        Debug.Log("Combo reset");
    }
}
