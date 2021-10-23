using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    static GameMenu gameMenu;

    static bool paused;

    public const int GAME_OVER = 0;
    public const int PAUSE = 1;
    public const int OPTIONS = 2;

    static CursorLockMode cursorStatePrev;

    static int survivors = 10;
    static int relativeLosses = 0;
    static int dayCounter = 0;

    public static void SetPaused(bool _paused)
    {
        paused = _paused;

        if (paused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public static bool GetPaused()
    {
        return paused;
    }

    public static void SetGameMenu(GameMenu menu)
    {
        gameMenu = menu;
    }
    public static GameMenu GetGameMenu()
    {
        return gameMenu;
    }

    public static int GetSurvivors()
    {
        return survivors;
    }

    public static void SetSurvivors(int _survivors)
    {
        survivors = _survivors;
        ScoreManager.OnSurvivorChange();
    }
    
    public static int GetRelativeLosses()
    {
        return relativeLosses;
    }

    public static void SetRelativeLosses(int losses)
    {
        relativeLosses = losses;
    }

    public static int GetDayCounter()
    {
        return dayCounter;
    }

    public static void AddDay()
    {
        dayCounter++;
    }
}
