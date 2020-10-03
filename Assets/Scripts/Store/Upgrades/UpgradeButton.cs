using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image icon;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;

    public void AddButtonValues(string _name, Sprite _icon, string _description, int _cost)
    {
        nameText.text = _name;
        icon.sprite = _icon;
        descriptionText.text = _description;
        costText.text = _cost.ToString();
    }
}
