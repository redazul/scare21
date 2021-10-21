using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class FadeInOutUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup managedElement;

    private Timer timer;

    private float fadeInDuration = 0.0f;
    private float displayDuration = 0.0f;
    private float fadeOutDuration = 0.0f;

    private System.Action afterFadedInAction = null;
    private System.Action afterFadedOutAction = null;
    private System.Action afterDisplayAction = null;

    private bool fadeInOnly = false;

    private float minAlpha = 0.0f;
    private float maxAlpha = 1.0f;

    private float startValue;
    private float endValue;
    private bool isFading;

    void Awake()
    {
        timer = gameObject.AddComponent<Timer>();
    }

    void Update()
    {
        if (isFading)
        {
            managedElement.alpha = Mathf.Lerp(startValue, endValue, timer.GetRelativeProgress());
        }
    }

    public void SetMaxAlpha(float maxAlpha)
    {
        this.maxAlpha = maxAlpha;
    }

    public void SetMinAlpha(float minAlpha)
    {
        this.minAlpha = minAlpha;
    }

    public void StartFadeInOut(float fadeInDuration, float displayDuration, float fadeOutDuration, System.Action afterFadedInAction = null, System.Action afterFadedOutAction = null, System.Action afterDisplayAction = null)
    {
        fadeInOnly = false;
        if (managedElement == null)
        {
            Debug.LogWarning("ManagedElement was not set!");
        }

        this.fadeInDuration = fadeInDuration;
        this.displayDuration = displayDuration;
        this.fadeOutDuration = fadeOutDuration;
        this.afterFadedInAction = afterFadedInAction;
        this.afterFadedOutAction = afterFadedOutAction;
        this.afterDisplayAction = afterDisplayAction;
        StartFadingIn();
    }

    public void StartFadeIn(float fadeInDuration, System.Action afterFadedInAction = null)
    {
        fadeInOnly = true;
        if (managedElement == null)
        {
            Debug.LogWarning("ManagedElement was not set!");
        }

        this.fadeInDuration = fadeInDuration;
        this.afterFadedInAction = afterFadedInAction;
        StartFadingIn();
    }

    public void StartFadeOut(float fadeOutDuration, System.Action afterFadedOutAction = null)
    {
        if (managedElement == null)
        {
            Debug.LogWarning("ManagedElement was not set!");
        }

        this.fadeOutDuration = fadeOutDuration;
        this.afterFadedOutAction = afterFadedOutAction;
        StartFadingOut();
    }

    private void StartFadingIn()
    {
        Action finalAction = null;
        if (fadeInOnly)
        {
            finalAction = afterFadedInAction;
        }
        else
        {
            finalAction = delegate
            {
                afterFadedInAction?.Invoke();
                DisplayManagedElement();
            };
        }
        StartFadingFrom(minAlpha, maxAlpha, fadeInDuration, finalAction);
    }

    private void DisplayManagedElement()
    {
        managedElement.alpha = maxAlpha;

        if (displayDuration <= 0.0f)
        {
            afterDisplayAction?.Invoke();
            StartFadingOut();
            return;
        }

        timer.Init(displayDuration, delegate
        {
            afterDisplayAction?.Invoke();
            StartFadingOut();
        });
    }

    private void StartFadingOut()
    {
        StartFadingFrom(maxAlpha, minAlpha, fadeOutDuration, afterFadedOutAction);
    }

    private void StartFadingFrom(float startValue, float endValue, float duration, System.Action afterFadeActionCallback = null)
    {
        this.startValue = startValue;
        this.endValue = endValue;

        if (fadeOutDuration <= 0.0f)
        {
            afterFadeActionCallback?.Invoke();
            return;
        }

        isFading = true;
        timer.Init(duration, delegate
        {
            isFading = false;
            afterFadeActionCallback?.Invoke();
        });
    }

}