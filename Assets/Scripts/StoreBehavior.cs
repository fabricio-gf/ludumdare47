using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBehavior : MonoBehaviour
{
    public List<Upgrade> upgradeList;

    public Transform buttonGrid;

    public GameObject buttonPrefab;

    private void Start()
    {
        FillStore();
    }

    public void FillStore()
    {
        foreach (var upgrade in upgradeList)
        {
            var p = Instantiate(buttonPrefab, buttonGrid);
            p.GetComponent<UpgradeButton>().AddButtonValues(upgrade.upgradeName, upgrade.icon, upgrade.description, upgrade.cost);
        }
    }
    
    public void ResumeGame()
    {
        
    }

    public void BuyStuff(int index)
    {
        
    }

    public void UpdateMoney(int change)
    {
        
    }

    public void SetMoney(int value)
    {
        
    }
}
