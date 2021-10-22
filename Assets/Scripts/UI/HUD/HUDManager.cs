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
    private HUDScreenOverlay hudScreenOverlay;
    private HUDMushLight hudMushLight;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one HUDManager in this scene.");
        }
        Instance = this;

        hudHealth = GetComponentInChildren<HUDHealth>();
        hudCheese = GetComponentInChildren<HUDCheese>();
        hudSurvivors = GetComponentInChildren<HUDSurvivors>();
        hudScreenOverlay = GetComponentInChildren<HUDScreenOverlay>();
        hudMushLight = GetComponentInChildren<HUDMushLight>();
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

    public void DisableHUD()
    {
        hudCheese.gameObject.SetActive(false);
        hudSurvivors.gameObject.SetActive(false);
        hudCheese.gameObject.SetActive(false);
    }

    public void ShowDayEndsScreenOverlay(int nextDay, int survivors, int losses, System.Action processNightCallback = null, System.Action onStartNewDayCallback = null)
    {
        hudScreenOverlay.ShowOverlayDailyTexts(nextDay, survivors, losses);
        hudScreenOverlay.DisplayOverlay(processNightCallback, onStartNewDayCallback);
    }

    public void UpdateMushroomBattery(float relativeBattery)
    {
        hudMushLight.SetRelativeBatteryLife(relativeBattery);
    }

    public void SetMushLightHudActive(bool mushLightHudActive)
    {
        hudMushLight.SetBatteryBarVisible(mushLightHudActive);
    }

}
