using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttacking : MonoBehaviour
{
    public Rigidbody2D rbd;
    public Camera cam;

    public Transform attackPoint;
    public GameObject swordSweep;

    //public float attackRadius = 1f;

    private Vector2 mousePos;

    private void Awake()
    {
        if (rbd == null)
            rbd = GetComponent<Rigidbody2D>();

        if (cam == null)
            cam = Camera.main;
    }

    private void Update()
    {
        if (GameManager.Instance.isGamePaused || GameManager.Instance.gameState != GameManager.GameState.Playing) return;
        
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
            swordSweep.SetActive(true);
        else if (Input.GetButtonUp("Fire1"))
            swordSweep.SetActive(false);
    }

    private void FixedUpdate()
    {
        HandleAttackPos();
    }

    void HandleAttackPos()
    {
        Vector2 lookDir = (mousePos - (Vector2)attackPoint.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        attackPoint.rotation = rotation;
    }
}
