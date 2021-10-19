using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static void FinishLevel(float cheeseCarried)
    {
        EstimateSurvivors(cheeseCarried);
        References.AddDay();
        HUDManager.Instance.ShowDayEndsScreenOverlay(References.GetDayCounter(), References.GetSurvivors(), References.GetRelativeLosses());
    }

    public static void OnSurvivorChange()
    {
        UpdateSurvivorUI();
    }

    private static void UpdateSurvivorUI()
    {
        HUDManager.Instance.UpdateSurvivorsAmount(References.GetSurvivors());
    }

    private static void EstimateSurvivors(float cheeseCarried)
    {        
        //Placeholder

        int previousSurvivors = References.GetSurvivors();
        int currentSurvivors = Mathf.Min(previousSurvivors, (int) (cheeseCarried / 0.5f));
        int losses = previousSurvivors - currentSurvivors;

        References.SetSurvivors(currentSurvivors);
        References.SetRelativeLosses(losses);

        OnSurvivorChange();
    }
}
