using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private const float CHEESE_PER_SURVIVOR = 0.5f;
    private const float CHEESE_SPAWNED_FACTOR = 1.2f;

    public static int GetCheeseAmountNeeded()
    {
        int survivors = References.GetSurvivors();
        float cheeseAmountNeeded = survivors * CHEESE_PER_SURVIVOR;
        float cheeseAmountToSpawn = cheeseAmountNeeded * CHEESE_SPAWNED_FACTOR;
        int cheesePiecesToSpawn = Mathf.CeilToInt(cheeseAmountToSpawn / Cheese.AVG_CHEESE_AMOUNT);
        return cheesePiecesToSpawn;
    }

    public static int GetAmountsCatsToSpawn()
    {
        //add a cat every fifth day
        return 1+References.GetDayCounter() / 5;
    }

    public static int GetAmountsTrapsToSpawn()
    {
        //add a trap every day
        return 1+References.GetDayCounter() / 5;
    }

    public static int GetAmountsMushroomsToSpawn()
    {
        return 3;
    }

    public static void FinishLevel(float cheeseCarried)
    {
        EstimateSurvivors(cheeseCarried);
        References.AddDay();
        HUDManager.Instance.ShowDayEndsScreenOverlay(References.GetDayCounter(), References.GetSurvivors(), References.GetRelativeLosses(), ProcessNight);
    }

    public static void ProcessNight()
    {
        LevelManager.Instance.SetAmountCheeseToSpawn(GetCheeseAmountNeeded());
        LevelManager.Instance.SetAmountCatsToSpawn(GetAmountsCatsToSpawn());
        LevelManager.Instance.SetAmountTrapsToSpawn(GetAmountsTrapsToSpawn());
        LevelManager.Instance.SetAmountMushroomsToSpawn(GetAmountsMushroomsToSpawn());
        LevelManager.Instance.CleanAndRespawnAll();
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
