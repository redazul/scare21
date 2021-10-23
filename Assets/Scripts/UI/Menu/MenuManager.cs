using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private int selectedButtonIndex = 0;
    private List<MenuButton> childrenButtons = new List<MenuButton>();

    void OnEnable()
    {
        childrenButtons = new List<MenuButton>(GetComponentsInChildren<MenuButton>());

        if (CompareTag("PauseMenu")) selectedButtonIndex = childrenButtons.Count - 1;
    }

    void Start()
    {
        childrenButtons[selectedButtonIndex].SetSelected(true);
    }

    private void Update()
    {
        childrenButtons[selectedButtonIndex].SetSelected(true);
    }

    public void SetButtonSelected(MenuButton menuButton)   {
        if (childrenButtons.Count == 0)
        {
            return;

        }
        for(int i = 0; i < childrenButtons.Count; i++)
        {
            if (menuButton == childrenButtons[i])
            {
                selectedButtonIndex = i;
                childrenButtons[i].SetSelected(true);
            }
            else
            {
                childrenButtons[i].SetSelected(false);
            }
        }
    }

    public void UseButton()
    {
        childrenButtons[selectedButtonIndex].Use();
    }

    public void SelectNextButton()
    {
        selectedButtonIndex = Mathf.Clamp(selectedButtonIndex + 1, 0, childrenButtons.Count - 1);
        UpdateButtonSelections();
    }

    public void SelectPreviousButton()
    {
        selectedButtonIndex = Mathf.Clamp(selectedButtonIndex - 1, 0, childrenButtons.Count - 1);
        UpdateButtonSelections();
    }

    public void UpdateButtonSelections()
    {
        for (int i = 0; i < childrenButtons.Count; i++)
        {
            if (i==selectedButtonIndex)
            {
                childrenButtons[i].SetSelected(true);
            }
            else
            {
                childrenButtons[i].SetSelected(false);
            }
        }
    }

}
