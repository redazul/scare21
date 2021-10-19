using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    static int cheese;
    static int survivors = 10;

    static GameMenu gameMenu;

    public const int GAME_OVER = 0;

    public static void SetGameMenu(GameMenu menu)
    {
        gameMenu = menu;
    }
    public static GameMenu GetGameMenu()
    {
        return gameMenu;
    }

    public static int GetCheese()
    {
        return cheese;
    }

    public static void SetCheese(int _cheese)
    {
        cheese = _cheese;
        ScoreManager.OnCheeseChange();
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
}

