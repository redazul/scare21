using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject options;
    private void Awake()
    {
        References.SetGameMenu(this);
    }
    public void SetMenuActive(int menuIndex, bool active)
    {
        DeactivateAllMenus();

        GameObject menu = gameObject;

        switch (menuIndex)
        {
            case References.GAME_OVER: menu = gameOver; break;
            case References.PAUSE: menu = pause; break;
            case References.OPTIONS: menu = options; break;
        }

        if (menu != gameObject)
        {
            menu.SetActive(active);
        }
    }

    public void SetMenu(int menuIndex)
    {
        DeactivateAllMenus();

        GameObject menu = gameObject;

        switch (menuIndex)
        {
            case References.GAME_OVER: menu = gameOver; break;
            case References.PAUSE: menu = pause; break;
            case References.OPTIONS: menu = options; break;
        }

        if (menu != gameObject)
        {
            menu.SetActive(true);
        }
    }

    public bool GetPauseState()
    {
        return pause.activeInHierarchy;
    }

    public void DeactivateAllMenus()
    {
        gameOver.SetActive(false);
        pause.SetActive(false);
        options.SetActive(false);
    }
}





