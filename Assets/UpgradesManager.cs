using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public int money;
    public Transform ownedUpgradesIconsParent;

    public List<Upgrade> allUpgrades = new List<Upgrade>();

    public List<Upgrade> ownedUpgrades = new List<Upgrade>();
    private List<GameObject> upgradesIcons = new List<GameObject>();

    private static UpgradesManager _instance;

    public TextMeshProUGUI moneyText;

    public Upgrade warmCoffeeSO;
    public bool warmCoffee;
    public Upgrade expiredMedicineSO;
    public bool expiredMedicine;
    public Upgrade balloonsSO;
    public bool balloons;
    public Upgrade flamingMuffinSO;
    public bool flamingMuffin;
    public Upgrade cosmicDonutSO;
    public bool cosmicDonut;
    public Upgrade angelPancakeSO;
    public bool angelPancake;

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
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (ownedUpgradesIconsParent == null)
            ownedUpgradesIconsParent = GameObject.Find("UpgradesGrid").transform;

        for (int i = 0; i < ownedUpgradesIconsParent.childCount; i++)
        {
            GameObject go = ownedUpgradesIconsParent.GetChild(i).gameObject;
            go.SetActive(false);
            upgradesIcons.Add(go);
        }
    }

    private void Update()
    {
        CheckUpgrades();
        UpdateOwnedUpgradesIcons();
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

    public void UpdateOwnedUpgradesIcons()
    {
        for (int i = 0; i < ownedUpgrades.Count; i++)
        {
            if (!upgradesIcons[i].activeSelf)
            {
                var img = upgradesIcons[i].GetComponent<Image>();
                img.sprite = ownedUpgrades[i].icon;
                upgradesIcons[i].SetActive(true);
            }
        }
    }

    public void CheckUpgrades()
    {

        warmCoffee = ownedUpgrades.Contains(warmCoffeeSO);

        expiredMedicine = ownedUpgrades.Contains(expiredMedicineSO);

        balloons = ownedUpgrades.Contains(balloonsSO);

        flamingMuffin = ownedUpgrades.Contains(flamingMuffinSO);

        cosmicDonut = ownedUpgrades.Contains(cosmicDonutSO);

        angelPancake = ownedUpgrades.Contains(angelPancakeSO);

    }
}
