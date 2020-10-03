using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttacking : MonoBehaviour
{
    public Rigidbody2D rbd;
    private Camera cam;

    public Transform attackPoint;
    public GameObject swordSweep;

    public float attackRadius = 1f;

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
        Vector2 lookDir = (mousePos - rbd.position).normalized;
        attackPoint.position = rbd.position + lookDir * attackRadius;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        attackPoint.rotation = rotation;
    }
}
