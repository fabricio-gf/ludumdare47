using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;

    public GameObject bullet;

    public Rigidbody2D rbd;

    public float radius = 1f;

    private Vector2 mousePos;

    public float bulletForce = 20f;

    private void Awake()
    {
        if (rbd == null)
            rbd = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = (mousePos - rbd.position).normalized;

        firePoint.position = rbd.position + lookDir * radius;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        firePoint.rotation = rotation;
    }
}
