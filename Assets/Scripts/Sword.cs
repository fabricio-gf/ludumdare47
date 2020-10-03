using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Rigidbody2D playerRbd;

    public float pushForce = 10f;

    private void Awake()
    {
        if (playerRbd == null)
            playerRbd = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D colRbd = collision.gameObject.GetComponent<Rigidbody2D>();
        if (colRbd != null)
        {
            Vector2 dir = (colRbd.position - playerRbd.position).normalized;
            colRbd.AddForce(dir * pushForce);
        }
    }
}
