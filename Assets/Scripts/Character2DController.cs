﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float startRollSpeed = 40f;
    private float rollSpeed;
    public float startRollCooldown = 1f;
    private float rollCooldown;

    public Rigidbody2D rbd;

    private Vector2 movement;
    private Vector2 rollDir;

    private bool dodgeRoll;
    private bool canRoll = true;

    [SerializeField]
    private CharacterState charState;
    private enum CharacterState
    {
        Normal,
        DodgeRolling,
    }

    private void Awake()
    {
        if (rbd == null)
            rbd = GetComponent<Rigidbody2D>();

        Init();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        switch (charState)
        {
            case CharacterState.Normal:
                HandleMovement();
                break;
            case CharacterState.DodgeRolling:
                HandleDodgeRoll();
                break;
        }

        HandleDodgeCooldown();
    }

    void GetInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Jump"))
            dodgeRoll = true;
        else if (Input.GetButtonUp("Jump"))
            dodgeRoll = false;
    }

    void HandleMovement()
    {
        rbd.MovePosition(rbd.position + movement * moveSpeed * Time.fixedDeltaTime);

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
                rollDir = (Input.mousePosition - Camera.main.WorldToScreenPoint(rbd.position)).normalized;
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

    void ChangeChararacterStateTo(CharacterState characterState)
    {
        charState = characterState;
    }

    void Init()
    {
        ChangeChararacterStateTo(CharacterState.Normal);
        rollCooldown = startRollCooldown;
    }
}
