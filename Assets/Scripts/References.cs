using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    static int cheese;
    static int survivors = 10;

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
