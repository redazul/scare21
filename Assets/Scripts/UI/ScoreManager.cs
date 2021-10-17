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
        UpdateCheeseUI();
        EstimateSurvivors();
    }

    public static void OnSurvivorChange()
    {
        UpdateSurvivorUI();
    }

    private static void UpdateCheeseUI()
    {
        var cheeseUI = FindObjectOfType<HUDCheeseTest>();
        if (cheeseUI != null)
        {
            cheeseUI.OnCheeseAmountChanged(References.cheese);
        }
    }

    private static void UpdateSurvivorUI()
    {
        var survivorsUI = FindObjectOfType<HUDSurvivorsTest>();
        if (survivorsUI != null)
        {
            survivorsUI.OnSurvivorsCountChanged(References.survivors);
        }
    }

    public static void EstimateSurvivors()
    {
        //Placeholder
        float survivors = References.cheese / 10;
        References.survivors = (int)survivors;

        OnSurvivorChange();
    }
}
