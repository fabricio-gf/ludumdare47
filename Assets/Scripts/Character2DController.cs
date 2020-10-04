using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;

public class Character2DController : MonoBehaviour
{
    public float startLife = 6f;
    private float currentLife;

    public float damageCooldown = 0.5f;
    private float lastDamageTime;

    public float startStunnedTime = 3f;
    private float stunnedTime;

    public float moveSpeed = 5f;
    public float startRollSpeed = 40f;
    private float rollSpeed;
    public float startRollCooldown = 1f;
    private float rollCooldown;
    public float handRadius = 0.5f;

    public Camera cam;
    public Rigidbody2D rbd;
    public Transform playerHand;
    public Transform handPivot;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private SpriteRenderer handSpriteRenderer;
    
    private Vector2 movement;
    private Vector2 rollDir;
    private Vector2 mousePos;
    private Vector3 mousePosToScreen;
    private Vector3 handPivotPosToScreen;
    private float handDist;

    private bool dodgeRoll;
    private bool canRoll = true;
    private bool faceRight = true;

    [SerializeField]
    private CharacterState charState;
    private enum CharacterState
    {
        Normal,
        DodgeRolling,
        Stunned
    }

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;

        if (rbd == null)
            rbd = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (handSpriteRenderer == null)
            handSpriteRenderer = playerHand.GetComponent<SpriteRenderer>();

        handDist = Vector3.Distance(handPivot.position, playerHand.position);

        Init();
    }

    private void Update()
    {
        if (GameManager.Instance.isGamePaused || GameManager.Instance.gameState != GameManager.GameState.Playing) return;
        
        GetInput();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F))
            TakeDamage(2f);
#endif
    }

    private void FixedUpdate()
    {
        switch (charState)
        {
            case CharacterState.Normal:
                HandleMovement();
                HandleAnimation();
                break;
            case CharacterState.DodgeRolling:
                HandleDodgeRoll();
                break;
            case CharacterState.Stunned:
                HandleStunned();
                break;
        }

        HandleHandPos();

        HandleDodgeCooldown();
        
        animator.SetLayerWeight(1, GameManager.Instance.remainingTimePercent <= .3f ? 1 : 0);
    }

    void GetInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        if (Input.GetButtonDown("Jump"))
            dodgeRoll = true;
        else if (Input.GetButtonUp("Jump"))
            dodgeRoll = false;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosToScreen = cam.ScreenToViewportPoint(Input.mousePosition);
        handPivotPosToScreen = cam.WorldToViewportPoint(handPivot.position);
    }

    void HandleAnimation()
    {
        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (mousePosToScreen.x > handPivotPosToScreen.x)
        {
            handSpriteRenderer.flipX = false;
        }
        else
        {
            handSpriteRenderer.flipX = true;
        }

        if (movement != Vector2.zero)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    void HandleMovement()
    {
        rbd.MovePosition(rbd.position + movement.normalized * (moveSpeed * Time.fixedDeltaTime));

        if (dodgeRoll && canRoll)
        {
            canRoll = false;

            rollSpeed = startRollSpeed;

            if (movement != Vector2.zero)
            {
                rollDir = movement.normalized;
            }
            else
            {
                rollDir = (mousePos - rbd.position).normalized;
            }

            ChangeChararacterStateTo(CharacterState.DodgeRolling);
        }
    }

    void HandleDodgeRoll()
    {
        rbd.MovePosition(rbd.position + rollDir * rollSpeed * Time.fixedDeltaTime);

        rollSpeed -= rollSpeed * 10f * Time.fixedDeltaTime;
        if (rollSpeed < 5f)
        {
            ChangeChararacterStateTo(CharacterState.Normal);
        }
    }

    void HandleDodgeCooldown()
    {
        if (!canRoll)
        {
            rollCooldown -= Time.fixedDeltaTime;
            if (rollCooldown <= 0)
            {
                rollCooldown = startRollCooldown;
                canRoll = true;
            }
        }
    }

    void HandleStunned()
    {
        if (stunnedTime >= 0)
        {
            stunnedTime -= Time.deltaTime;
        }
        else
        {
            currentLife = startLife;
            spriteRenderer.color = Color.white;
            ChangeChararacterStateTo(CharacterState.Normal);
        }
    }

    void ChangeChararacterStateTo(CharacterState characterState)
    {
        charState = characterState;
    }

    void Init()
    {
        ChangeChararacterStateTo(CharacterState.Normal);
        rollCooldown = startRollCooldown;
        currentLife = startLife;
        stunnedTime = startStunnedTime;
    }

    void HandleHandPos()
    {
        Vector3 lookDir = (mousePos - (Vector2)handPivot.position).normalized;
        playerHand.position = handPivot.position + lookDir * handDist;
        //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //playerHand.rotation = rotation;
    }

    public void TakeDamage(float damage)
    {
        if (Time.unscaledTime < lastDamageTime + damageCooldown || charState != CharacterState.Normal) return;

        if (currentLife - damage > 0)
        {
            lastDamageTime = Time.unscaledTime;
            currentLife -= damage;
            spriteRenderer.DOColor(Color.red, 0.15f).SetLoops(2, LoopType.Yoyo);   
        }
        else
        {
            stunnedTime = startStunnedTime;
            spriteRenderer.color = Color.yellow;
            ChangeChararacterStateTo(CharacterState.Stunned);
        }
    }
    
    public void ResetVelocity()
    {
        rbd.velocity = Vector2.zero;
    }
}
