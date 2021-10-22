using UnityEngine;
using UnityEngine.UI;

public class HUDMushLight : MonoBehaviour
{
    private const float batteryBarScale = 300;

    [SerializeField]
    private Color colorFull;

    [SerializeField]
    private Color colorEmpty;

    [SerializeField]
    Image batteryBar;

    private RectTransform batteryBarRectTransform;

    void Awake()
    {
        GetComponentInChildren<CanvasGroup>().alpha = 0;
        batteryBarRectTransform = batteryBar.rectTransform;
    }

    public void SetRelativeBatteryLife(float relativeBattery)
    {
        batteryBar.color = Color.Lerp(colorEmpty, colorFull, relativeBattery);
        batteryBarRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, relativeBattery * batteryBarScale);
    }

    public void SetBatteryBarVisible(bool isVisible)
    {
        GetComponentInChildren<CanvasGroup>().alpha = isVisible ? 1 : 0;
    }
}
