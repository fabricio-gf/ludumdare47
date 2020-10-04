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
    List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();

    public Transform buttonGrid;

    public GameObject buttonPrefab;
    public GameObject upgradeDescriptionPanel;

    public TextMeshProUGUI moneyText;

    private void Start()
    {
        FillStore();
        SetMoney(UpgradesManager.Instance.money);
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
                        var upButton = p.GetComponentInChildren<UpgradeButton>();
                        upButton.AddButtonValues(newUpgrades[index].upgradeName, newUpgrades[index].icon, newUpgrades[index].description, newUpgrades[index].cost, this);
                        upgradeButtons.Add(upButton);

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
                var upButton = p.GetComponentInChildren<UpgradeButton>();
                upButton.AddButtonValues(upgrade.upgradeName, upgrade.icon, upgrade.description, upgrade.cost, this);
                upgradeButtons.Add(upButton);

                var localIndex = i;
                var upgradeButton = p.GetComponentInChildren<Button>();
                upgradeButton.onClick.AddListener(() => BuyUpgrade(localIndex, upgradeButton));

                i++;

                storeUpgrades.Add(upgrade);
            }
        }
    }

    public void ResumeGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BuyUpgrade(int index, Button button)
    {
        print("Pressing button " + index);
        if (UpgradesManager.Instance.money >= storeUpgrades[index].cost)
        {
            UpgradesManager.Instance.AddUpgrade(storeUpgrades[index]);
            upgradeButtons[index].bought = true;
            UpdateMoney(-storeUpgrades[index].cost);
        }
    }

    public void UpdateMoney(int change)
    {
        UpgradesManager.Instance.money += change;
        SetMoney(UpgradesManager.Instance.money);
    }

    public void SetMoney(int value)
    {
        moneyText.text = value.ToString();
    }
}
