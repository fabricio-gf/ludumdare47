using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttacking : MonoBehaviour
{
    public Rigidbody2D rbd;
    public Camera cam;

    public Transform attackPoint;
    public GameObject swordSweep;

    public float attackCooldown = 1f;
    public float swordSweepLifetime = 0.5f;

    private float _currentTime;
    [HideInInspector] public bool _isOnCooldown;
    private bool _isSwordSweepActive;

    [Header("Hand - Change Color")] 
    public GameObject hand;
    public Color normalHandColor;
    public Color onCooldownHandColor;
    
    //public float attackRadius = 1f;

    private Vector2 mousePos;

    private Character2DController _controller;

    private void Awake()
    {
        if (rbd == null)
            rbd = GetComponent<Rigidbody2D>();

        if (cam == null)
            cam = Camera.main;
    }

    private void Start()
    {
        _controller = GetComponent<Character2DController>();
    }

    private void Update()
    {
        if (GameManager.Instance.isGamePaused || GameManager.Instance.gameState != GameManager.GameState.Playing) return;
        
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire1") && !_isOnCooldown && !_controller.isStunned)
        {
            _isOnCooldown = true;
            _isSwordSweepActive = true;
            
            swordSweep.SetActive(true);

            hand.GetComponent<SpriteRenderer>().color = onCooldownHandColor;

            _currentTime = 0f;
        }

        _currentTime += Time.deltaTime;

        if (_isSwordSweepActive && _currentTime >= swordSweepLifetime)
        {
            _isSwordSweepActive = false;
            swordSweep.SetActive(false);
        }

        if (_isOnCooldown && _currentTime >= attackCooldown)
        {
            _isOnCooldown = false;
            hand.GetComponent<SpriteRenderer>().color = normalHandColor;
            
            //redundancy to avoid errors
            _isSwordSweepActive = false;
            swordSweep.SetActive(false);
        }


        
        //else if (Input.GetButtonUp("Fire1"))
        //    swordSweep.SetActive(false);
    }

    private void FixedUpdate()
    {
        HandleAttackPos();
    }

    void HandleAttackPos()
    {
        if (_isOnCooldown) return;

        Vector2 lookDir = (mousePos - (Vector2)attackPoint.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        attackPoint.rotation = rotation;
    }
}
