using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    private void Awake()
    {
        References.SetGameMenu(this);
    }
    public void SetMenuActive(int menuIndex, bool active)
    {
        GameObject menu = gameObject;

        switch (menuIndex)
        {
            case References.GAME_OVER: menu = gameOver; break;
        }

        if (menu != gameObject)
        {
            menu.SetActive(active);
        }
    }
}





