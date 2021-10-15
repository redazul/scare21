using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler
{
    private readonly Color selectedBackgroundColor = new Color(0.15f, 0.15f, 0.15f, 0.25f);
    private readonly Color unselectedBackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    private readonly Color selectedTextColor = new Color(1f, 1f, 1f);
    private readonly Color unselectedTextColor = new Color(0.8f, 0.8f, 0.8f);

    private Text buttonText;
    private Image backgroundImage;

    [SerializeField]
    MenuNavigationManager.MenuNavigationTarget target;

    void Awake()
    {
        this.buttonText = GetComponentInChildren<Text>();
        this.backgroundImage = GetComponent<Image>();
    }

    public void Use()
    {
        MenuNavigationManager.Instance.NavigateTo(target);
    }

    public void SetSelected(bool isSelected)
    {
        this.buttonText.color = isSelected ? selectedTextColor : unselectedTextColor;
        this.backgroundImage.color = isSelected ? selectedBackgroundColor : unselectedBackgroundColor;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuNavigationManager.Instance.SetButtonSelected(this);
    }
}
