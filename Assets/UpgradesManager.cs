using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public int money;
    public Transform ownedUpgradesIconsParent;
    public List<Upgrade> ownedUpgrades = new List<Upgrade>();

    private static UpgradesManager _instance;

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

    public void UpdateOwnedUpgradesIcons()
    {
        if (ownedUpgradesIconsParent == null)
            ownedUpgradesIconsParent = GameObject.Find("OwnedUpgradesIcons").transform;

        foreach (Upgrade upgrade in ownedUpgrades)
        {
            var p = Instantiate(new GameObject(), ownedUpgradesIconsParent);
            var img = p.AddComponent<Image>();
            img.sprite = upgrade.icon;
        }
    }
}
