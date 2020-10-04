using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreBehavior : MonoBehaviour
{
    public List<Upgrade> upgradeList;

    public Transform buttonGrid;

    public GameObject buttonPrefab;

    public GameObject upgradeDescriptionPanel;

    private void Start()
    {
        FillStore();
    }

    public void FillStore()
    {
        var i = 0;
        foreach (var upgrade in upgradeList)
        {
            var p = Instantiate(buttonPrefab, buttonGrid);
            p.GetComponentInChildren<UpgradeButton>().AddButtonValues(upgrade.upgradeName, upgrade.icon, upgrade.description, upgrade.cost, this);
           
            var localIndex = i;
            p.GetComponentInChildren<Button>().onClick.AddListener(() => BuyUpgrade(localIndex));
            
            i++;
        }
    }
    
    public void ResumeGame()
    {
        
    }

    public void BuyUpgrade(int index)
    {
        print("Pressing button " + index);
    }

    public void UpdateMoney(int change)
    {
        
    }

    public void SetMoney(int value)
    {
        
    }
}
