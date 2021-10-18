using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static void OnCheeseChange()
    {
        FindObjectOfType<PlayerController>().SetCheese(References.GetCheese());
        UpdateCheeseUI();
        EstimateSurvivors();
    }

    public static void OnSurvivorChange()
    {
        UpdateSurvivorUI();
    }

    private static void UpdateCheeseUI()
    {
        HUDManager.Instance.UpdateCheeseAmount(References.GetCheese());
    }

    private static void UpdateSurvivorUI()
    {
        HUDManager.Instance.UpdateSurvivorsAmount(References.GetSurvivors());
    }

    public static void EstimateSurvivors()
    {
        //Placeholder
        float survivors = References.GetCheese() / 10;
        References.SetSurvivors((int)survivors);

        OnSurvivorChange();
    }
}
