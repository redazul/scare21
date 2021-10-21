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

    public int GetMenu()
    {
        if (gameOver.activeInHierarchy) return References.GAME_OVER;
        if (pause.activeInHierarchy) return References.PAUSE;
        if (options.activeInHierarchy) return References.OPTIONS;

        return -1;
    }

    public void DeactivateAllMenus()
    {
        gameOver.SetActive(false);
        pause.SetActive(false);
        options.SetActive(false);
    }

    public void PauseGame(bool paused)
    {
        References.SetPaused(paused);
    }
}





