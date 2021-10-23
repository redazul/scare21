using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject pause;

    public GameObject background;
    private void Awake()
    {
        References.SetGameMenu(this);
    }

    public void SetPaused(bool paused)
    {
        if (!paused)
        {
            DeactivateAllMenus();
        }

        References.SetPaused(paused);
    }

    public void ReloadScene()
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader)
        {
            sceneLoader.ReloadScene();
        }
    }

    public void GoToMainMenu()
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader)
        {
            sceneLoader.ReturnToMainMenu();
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

        return -1;
    }

    public void DeactivateAllMenus()
    {
        gameOver.SetActive(false);
        pause.SetActive(false);
    }
}





