using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDScreenOverlay : MonoBehaviour
{
    private const float FADE_IN_DUR = 0.7f;
    private const float FADE_OUT_DUR = 0.7f;
    private const float OVERLAY_DISPLAY_DUR = 2.5f;

    [SerializeField]
    private Text dayText;

    [SerializeField]
    private Text survivorsText;

    [SerializeField]
    private Text lossesText;

    [SerializeField]
    private FadeInOutUI overlayFader;


    public void DisplayOverlay(System.Action onProcessNight, System.Action onStartNextDay)
    {
        overlayFader.StartFadeInOut(FADE_IN_DUR, OVERLAY_DISPLAY_DUR, FADE_OUT_DUR, onProcessNight, onStartNextDay);
    }

    public void ShowOverlayDailyTexts(int day, int survivors, int losses)
    {
        dayText.text = GetDayString(day);
        survivorsText.text = GetSurvivorsString(survivors);

        if (losses > 0)
        {
            lossesText.gameObject.SetActive(true);
            lossesText.text = GetLossesString(losses);
        }
        else
        {
            lossesText.gameObject.SetActive(false);
        }
    }

    private static string GetDayString(int day)
    {
        return "Day " + day;
    }

    private static string GetSurvivorsString(int survivors)
    {
        return survivors + " have survived";
    }

    private static string GetLossesString(int losses)
    {
        return "but " + losses + " didn't make it";
    }

}
