using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigationManager : MonoBehaviour
{
    public enum MenuNavigationTarget 
    {
        //careful: reordering might mess up unity editor values (adding is fine)
        mainMenu, optionsMenu, startGame, quitGame, showCredits
    }

    public static MenuNavigationManager Instance = null;
    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("There is more than one MenuNavigationManager in this scene.");
        }
        Instance = this;

        mainMenuManager = mainMenu.GetComponentInChildren<MenuManager>();
        optionsMenuManager = optionsMenu.GetComponentInChildren<MenuManager>();
    }

    void Start()
    {
        activeMenu = mainMenuManager;
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


    [SerializeField]
    GameObject mainMenu;
    MenuManager mainMenuManager;

    [SerializeField]
    GameObject optionsMenu;
    MenuManager optionsMenuManager;

    private MenuManager activeMenu;

    public void SetButtonSelected(MenuButton menuButton)
    {
        activeMenu.SetButtonSelected(menuButton);
    }

    public void UseCurrentButton()
    {
        activeMenu.UseButton();
    }

    public void SelectNextButton()
    {
        activeMenu.SelectNextButton();
    }

    public void SelectPreviousButton()
    {
        activeMenu.SelectPreviousButton();
    }

    public void NavigateTo(MenuNavigationTarget target)
    {
        switch (target)
        {
            case MenuNavigationTarget.mainMenu:
                {
                    activeMenu = mainMenuManager;
                    mainMenu.SetActive(true);
                    optionsMenu.SetActive(false);
                    break;
                }
            case MenuNavigationTarget.optionsMenu:
                {
                    activeMenu = optionsMenuManager;
                    mainMenu.gameObject.SetActive(false);
                    optionsMenu.gameObject.SetActive(true);
                    break;
                }
            case MenuNavigationTarget.startGame:
                {
                    SceneLoader.Instance.StartGame();
                    break;
                }
            case MenuNavigationTarget.showCredits:
                {
                    SceneLoader.Instance.SwitchToScene(SceneLoader.AvailableScene.credits);
                    break;
                }
            case MenuNavigationTarget.quitGame:
                {
                    SceneLoader.Instance.QuitGame();
                    break;
                }
        }
    }
}
