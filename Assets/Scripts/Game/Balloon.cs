using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{

    public bool canDropMoney;
    
    public void PopBalloon()
    {
        if (!canDropMoney) return;
        
        GetComponent<DropLoot>().DropMoneyOnChance();
        gameObject.SetActive(false);
    }
}
