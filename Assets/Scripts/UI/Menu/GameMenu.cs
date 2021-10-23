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
    void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectPreviousButton();
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectNextButton();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            UseCurrentButton();
        }

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

    public void SetButtonSelected(MenuButton menuButton)
    {
        //menu.SetButtonSelected(menuButton);
    }

    public void UseCurrentButton()
    {
        //menu.UseButton();
    }

    public void SelectNextButton()
    {
        //menu.SelectNextButton();
    }

    public void SelectPreviousButton()
    {
        //menu.SelectPreviousButton();
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
}





