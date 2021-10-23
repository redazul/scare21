using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuButton : MonoBehaviour, IPointerEnterHandler
{
    private readonly Color selectedBackgroundColor = new Color(0.15f, 0.15f, 0.15f, 0.25f);
    private readonly Color unselectedBackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    private readonly Color selectedTextColor = new Color(1f, 1f, 1f);
    private readonly Color unselectedTextColor = new Color(0.8f, 0.8f, 0.8f);

    private Text buttonText;
    private TextMeshProUGUI buttonTextTMP;
    private Image backgroundImage;

    [SerializeField]
    MenuNavigationManager.MenuNavigationTarget target;

    void OnEnable()
    {
        this.buttonText = GetComponentInChildren<Text>();
        buttonTextTMP = GetComponentInChildren<TextMeshProUGUI>();

        this.backgroundImage = GetComponent<Image>();

        backgroundImage.color = unselectedBackgroundColor;

        if (buttonText)
        {
            buttonText.color = unselectedTextColor;
        }
        else if (buttonTextTMP)
        {
            buttonTextTMP.color = unselectedTextColor;
        }
    }

    public void Use()
    {
        MenuNavigationManager.Instance.NavigateTo(target);
    }

    public void SetSelected(bool isSelected)
    {
        if (buttonText)
        {
            buttonText.color = isSelected ? selectedTextColor : unselectedTextColor;
        }
        else if (buttonTextTMP)
        {
            buttonTextTMP.color = isSelected ? selectedTextColor : unselectedTextColor;
        }
        this.backgroundImage.color = isSelected ? selectedBackgroundColor : unselectedBackgroundColor;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuNavigationManager.Instance.SetButtonSelected(this);
    }
}
