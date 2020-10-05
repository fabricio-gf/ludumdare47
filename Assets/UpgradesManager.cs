using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public int money;
    public List<Upgrade> ownedUpgrades = new List<Upgrade>();

    private static UpgradesManager _instance;

    public TextMeshProUGUI moneyText;

    public static UpgradesManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<UpgradesManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddUpgrade(Upgrade upgrade)
    {
        ownedUpgrades.Add(upgrade);
    }

    public void AddMoney(int value)
    {
        money += value;
        UpdateMoneyText();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = money.ToString();
    }
}
