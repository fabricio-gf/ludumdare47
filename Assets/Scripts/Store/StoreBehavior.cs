using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreBehavior : MonoBehaviour
{
    public List<Upgrade> upgradeList;
    [SerializeField]
    List<Upgrade> storeUpgrades = new List<Upgrade>();

    public Transform buttonGrid;

    public GameObject buttonPrefab;
    public GameObject upgradeDescriptionPanel;

    public TextMeshProUGUI moneyText;

    //temp
    public int money = 100;

    private void Start()
    {
        FillStore();
        SetMoney(money);
    }

    public void FillStore()
    {
        List<Upgrade> newUpgrades = new List<Upgrade>();
        foreach (Upgrade upgrade1 in upgradeList)
        {
            if (!UpgradesManager.Instance.ownedUpgrades.Contains(upgrade1))
                newUpgrades.Add(upgrade1);
        }

        if (newUpgrades.Count > 3)
        {
            for (int i = 0; i < 3; i++)
            {
                while (true)
                {
                    int index = UnityEngine.Random.Range(0, newUpgrades.Count);
                    if (!storeUpgrades.Contains(newUpgrades[index]))
                    {
                        storeUpgrades.Add(newUpgrades[index]);

                        var p = Instantiate(buttonPrefab, buttonGrid);
                        p.GetComponentInChildren<UpgradeButton>().AddButtonValues(newUpgrades[index].upgradeName, newUpgrades[index].icon, newUpgrades[index].description, newUpgrades[index].cost, this);

                        var localIndex = i;
                        var upgradeButton = p.GetComponentInChildren<Button>();
                        upgradeButton.onClick.AddListener(() => BuyUpgrade(localIndex, upgradeButton));
                        break;
                    }
                }              
            }
        }
        else
        {
            var i = 0;
            foreach (var upgrade in newUpgrades)
            {
                var p = Instantiate(buttonPrefab, buttonGrid);
                p.GetComponentInChildren<UpgradeButton>().AddButtonValues(upgrade.upgradeName, upgrade.icon, upgrade.description, upgrade.cost, this);

                var localIndex = i;
                var upgradeButton = p.GetComponentInChildren<Button>();
                upgradeButton.onClick.AddListener(() => BuyUpgrade(localIndex, upgradeButton));

                i++;

                storeUpgrades.Add(upgrade);
            }
        }

        //var i = 0;
        //foreach (var upgrade in upgradeList)
        //{
        //    var p = Instantiate(buttonPrefab, buttonGrid);
        //    p.GetComponentInChildren<UpgradeButton>().AddButtonValues(upgrade.upgradeName, upgrade.icon, upgrade.description, upgrade.cost, this);

        //    var localIndex = i;
        //    p.GetComponentInChildren<Button>().onClick.AddListener(() => BuyUpgrade(localIndex));

        //    i++;
        //}
    }
    
    public void ResumeGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BuyUpgrade(int index, Button button)
    {
        print("Pressing button " + index);
        if (money >= storeUpgrades[index].cost)
        {
            UpgradesManager.Instance.AddUpgrade(storeUpgrades[index]);
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Bought";
            button.interactable = false;
            UpdateMoney(-storeUpgrades[index].cost);
        }
    }

    public void UpdateMoney(int change)
    {
        money += change;
        SetMoney(money);
    }

    public void SetMoney(int value)
    {
        moneyText.text = value.ToString();
    }
}
