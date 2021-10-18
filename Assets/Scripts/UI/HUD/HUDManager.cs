using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public static HUDManager Instance = null;
    private HUDSurvivors hudSurvivors;
    private HUDHealth hudHealth;
    private HUDCheese hudCheese;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one MenuManager in this scene.");
        }
        Instance = this;

        hudHealth = GetComponentInChildren<HUDHealth>();
        hudCheese = GetComponentInChildren<HUDCheese>();
        hudSurvivors = GetComponentInChildren<HUDSurvivors>();
    }

    public void UpdateCheeseAmount(float newCheeseAmount)
    {
        hudCheese.OnCheeseAmountChanged(newCheeseAmount);
    }

    public void UpdateSurvivorsAmount(int newSurvivorsAmount)
    {
        hudSurvivors.OnSurvivorsCountChanged(newSurvivorsAmount);
    }

    public void UpdateHealthAmount(int newHealthAmount)
    {
        hudHealth.OnHealthChanged(newHealthAmount);
    }

}
