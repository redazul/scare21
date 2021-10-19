using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    static int survivors = 10;
    static int relativeLosses = 0;
    static int dayCounter = 0;

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
