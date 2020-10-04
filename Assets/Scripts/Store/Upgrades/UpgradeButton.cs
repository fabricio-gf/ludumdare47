using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI nameText;
    public Image icon;
    public TextMeshProUGUI costText;

    private string descriptionText;

    private StoreBehavior StoreBehavior;

    public RectTransform descriptionPanelPos;

    public void AddButtonValues(string _name, Sprite _icon, string _description, int _cost, StoreBehavior storeBehavior)
    {
        nameText.text = _name;
        icon.sprite = _icon;
        descriptionText = _description;
        costText.text = _cost.ToString();

        StoreBehavior = storeBehavior;
    }

    void ShowUpgradeDescription(Vector2 pos)
    {
        TMP_Text[] texts = StoreBehavior.upgradeDescriptionPanel.GetComponentsInChildren<TMP_Text>();
        texts[0].text = nameText.text;
        texts[1].text = descriptionText;
        RectTransform transform = StoreBehavior.upgradeDescriptionPanel.GetComponent<RectTransform>();

        transform.position = descriptionPanelPos.position;
        StoreBehavior.upgradeDescriptionPanel.SetActive(true);
    }

    void CloseUpgradeDescription()
    {
        StoreBehavior.upgradeDescriptionPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowUpgradeDescription(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CloseUpgradeDescription();   
    }
}
