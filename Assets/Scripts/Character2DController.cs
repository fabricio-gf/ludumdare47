using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rollSpeed = 20f;
    private float _rollSpeed;

    public Rigidbody2D rbd;

    private Vector2 movement;
    private bool dodgeRoll;

    private Vector2 rollDir;

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

        charState = CharacterState.Normal;
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
    }

    void GetInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Fire1"))
            dodgeRoll = true;
    }

    void HandleMovement()
    {
        if (dodgeRoll)
        {
            dodgeRoll = false;

            charState = CharacterState.DodgeRolling;
            rollDir = (Input.mousePosition - Camera.main.WorldToScreenPoint(rbd.position)).normalized;
            _rollSpeed = rollSpeed;
            Debug.Log(rollDir);
        }

        rbd.MovePosition(rbd.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleDodgeRoll()
    {
        rbd.MovePosition(rbd.position + rollDir * _rollSpeed * Time.fixedDeltaTime);
        //transform.position += rollDir * _rollSpeed * Time.deltaTime;

        _rollSpeed -= rollSpeed * 10f * Time.fixedDeltaTime;
        if (_rollSpeed < 5f)
        {
            charState = CharacterState.Normal;
        }
    }
}
