using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickuppable : MonoBehaviour
{
    public int coinValue;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UpgradesManager.Instance.AddMoney(coinValue);
            Destroy(gameObject);
        }
    }
}
