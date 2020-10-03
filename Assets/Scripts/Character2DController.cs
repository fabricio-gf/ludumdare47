using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rbd;

    private Vector2 movement;

    private CharacterState charState;
    private enum CharacterState
    {
        Normal,
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
        }
    }

    void GetInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void HandleMovement()
    {
        rbd.MovePosition(rbd.position + movement * moveSpeed * Time.deltaTime);
    }
}
