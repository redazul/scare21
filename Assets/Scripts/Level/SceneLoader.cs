using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum AvailableScene
    {
        //careful: reordering might mess up unity editor values (adding is fine)
        mainMenu, level1, credits, sandbox, sandbox2, playground
    }

    public static SceneLoader Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one SceneLoader in this scene.");
        }
        Instance = this;
    }

    [Tooltip("The name of the initial scene that is started from the menu.")]
    [SerializeField]
    AvailableScene initialScene = AvailableScene.level1;

    /// <summary>
    /// Starts the game by switching to the initialScene.
    /// </summary>
    public void StartGame()
    {
        SwitchToScene(initialScene);
    }

    public void SwitchToScene(AvailableScene targetScene)
    {
        SceneManager.LoadScene(ToSceneName(targetScene));
    }

    public void ReturnToMainMenu()
    {
        //load the scene
        SwitchToScene(AvailableScene.mainMenu);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Returns the scene name according to the given enum. This may be needed for scenes with names different from AvailableScenes ToString() result. 
    /// </summary>
    private string ToSceneName(AvailableScene availableScene)
    {
        switch (availableScene)
        {
            case AvailableScene.mainMenu:
                {
                    return "MainMenu";
                }
            case AvailableScene.level1:
                {
                    return "SallysHome Takuto's Edit";
                }
            case AvailableScene.credits:
                {
                    return "Credits";
                }
        }

        return availableScene.ToString();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
