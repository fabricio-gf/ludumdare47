using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{

    public GameObject coinPrefab;
    
    public void DropMoney()
    {
        print("Dropping cash");
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }

    public void DropMoneyOnChance()
    {
        if (Random.value < 0.3)
        {
            print("Here comes the money");
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
}
