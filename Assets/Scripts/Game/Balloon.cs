using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{

    public bool canDropMoney;

    public int pointValue = 10;
    
    public void PopBalloon()
    {
        if (!UpgradesManager.Instance.balloons) return;
        
        GetComponent<DropLoot>().DropMoneyOnChance();
        gameObject.SetActive(false);
        GameManager.Instance.AddScore(pointValue);
    }
}
