using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigationManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip selectClip;

    [SerializeField]
    private AudioClip startClip;

    [SerializeField]
    private AudioClip musicClip;

    [SerializeField]
    GameObject mainMenu;
    MenuManager mainMenuManager;

    [SerializeField]
    GameObject optionsMenu;
    MenuManager optionsMenuManager;

    private AudioSource soundAudioSource;
    private AudioSource musicAudioSource;

    private MenuProgressControl menuProgressControl;

    private MenuManager activeMenu;
    public enum MenuNavigationTarget 
    {
        //careful: reordering might mess up unity editor values (adding is fine)
        mainMenu, optionsMenu, startGame, quitGame, showCredits, resetProgress
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
        menuProgressControl = GetComponent<MenuProgressControl>();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        soundAudioSource = audioSources[0];
        soundAudioSource.loop = false;

        musicAudioSource = audioSources[1];
        musicAudioSource.loop = true;

    }

    void Start()
    {
        activeMenu = mainMenuManager;
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
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



    public void SetButtonSelected(MenuButton menuButton)
    {
        activeMenu.SetButtonSelected(menuButton);
        soundAudioSource.clip = selectClip;
        soundAudioSource.Play();
    }

    public void UseCurrentButton()
    {
        activeMenu.UseButton();
        soundAudioSource.clip = startClip;
        soundAudioSource.Play();
    }

    public void SelectNextButton()
    {
        activeMenu.SelectNextButton();
        soundAudioSource.clip = selectClip;
        soundAudioSource.Play();
    }

    public void SelectPreviousButton()
    {
        activeMenu.SelectPreviousButton();
        soundAudioSource.clip = selectClip;
        soundAudioSource.Play();
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
            case MenuNavigationTarget.resetProgress:
                {
                    menuProgressControl.ResetProgress(); 
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
