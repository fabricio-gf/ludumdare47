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

    public RectTransform descriptionPanelPos;

    public Button button;

    public bool bought = false;

    private StoreBehavior storeBehavior;

    private void Update()
    {
        UpdateButtonState();
    }

    void UpdateButtonState()
    {
        if (bought)
        {
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Bought";
            button.interactable = false;
        }
        else
        {
            if (UpgradesManager.Instance.money < int.Parse(costText.text) && button.interactable == true)
                button.interactable = false;
            else if (UpgradesManager.Instance.money >= int.Parse(costText.text) && button.interactable == false)
                button.interactable = true;
        }
    }

    public void AddButtonValues(string _name, Sprite _icon, string _description, int _cost, StoreBehavior _storeBehavior)
    {
        nameText.text = _name;
        icon.sprite = _icon;
        descriptionText = _description;
        costText.text = _cost.ToString();
        storeBehavior = _storeBehavior;
    }

    void ShowUpgradeDescription(Vector2 pos)
    {
        TMP_Text[] texts = storeBehavior.upgradeDescriptionPanel.GetComponentsInChildren<TMP_Text>();
        texts[0].text = nameText.text;
        texts[1].text = descriptionText;
        RectTransform transform = storeBehavior.upgradeDescriptionPanel.GetComponent<RectTransform>();

        transform.position = descriptionPanelPos.position;
        storeBehavior.upgradeDescriptionPanel.SetActive(true);
    }

    void CloseUpgradeDescription()
    {
        storeBehavior.upgradeDescriptionPanel.SetActive(false);
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
