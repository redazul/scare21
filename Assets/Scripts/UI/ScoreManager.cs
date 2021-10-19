using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private const float CHEESE_PER_SURVIVOR = 0.5f;
    private const float CHEESE_SPAWNED_FACTOR = 1.25f;

    public static void FinishLevel(float cheeseCarried)
    {
        EstimateSurvivors(cheeseCarried);
        References.AddDay();
        HUDManager.Instance.ShowDayEndsScreenOverlay(References.GetDayCounter(), References.GetSurvivors(), References.GetRelativeLosses(), ProcessNight);
    }

    private static void ProcessNight()
    {
        int survivors = References.GetSurvivors();
        float cheeseAmountNeeded = survivors * CHEESE_PER_SURVIVOR;
        float cheeseAmountToSpawn = cheeseAmountNeeded * CHEESE_SPAWNED_FACTOR;
        int cheesePiecesSpawned = (int)(cheeseAmountToSpawn / Cheese.AVG_CHEESE_AMOUNT);
        LevelManager.Instance.SetCheeseAmountToSpawn(cheesePiecesSpawned);
        LevelManager.Instance.CleanAndRespawnCheese();
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
        int currentSurvivors = Mathf.Min(previousSurvivors, (int) (cheeseCarried / CHEESE_PER_SURVIVOR));
        int losses = previousSurvivors - currentSurvivors;

        References.SetSurvivors(currentSurvivors);
        References.SetRelativeLosses(losses);

        OnSurvivorChange();
    }
}
