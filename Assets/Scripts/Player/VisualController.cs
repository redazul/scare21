using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualController : MonoBehaviour
{
    [SerializeField]
    private float zoomDuration = 1.0f;

    [SerializeField]
    private float reverseZoomDuration = 0.8f;

    [SerializeField]
    private float zoomedInFov = 70f;

    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera playerCamera;

    private bool reverse = false;

    private Timer zoomTimer;

    public static VisualController Instance = null;

    private float zoomedOutFov = 10f;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one VisualController in this scene.");
        }
        Instance = this;

        zoomTimer = gameObject.AddComponent<Timer>();

        zoomedOutFov = playerCamera.m_Lens.FieldOfView;
    }

    void Update()
    {
        if (zoomTimer.IsRunning())
        {
            float value = reverse ? 1 - zoomTimer.GetRelativeProgress() : zoomTimer.GetRelativeProgress();
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(zoomedOutFov, zoomedInFov, value);
        }
    }

    public void StartDollyZoomIn()
    {
        reverse = false;
        if (zoomTimer.IsRunning())
        {
            float progress = 1 - zoomTimer.GetRelativeProgress();
            zoomTimer.Init(zoomDuration);
            zoomTimer.SetTimePassed(zoomDuration * progress);
        } else
        {
            zoomTimer.Init(zoomDuration);
        }

    }

    public void StartDollyZoomOut()
    {
        reverse = true;
        if (zoomTimer.IsRunning())
        {
            float progress = 1 - zoomTimer.GetRelativeProgress();
            zoomTimer.Init(reverseZoomDuration);
            zoomTimer.SetTimePassed(reverseZoomDuration * progress);
        }
        else
        {
            zoomTimer.Init(reverseZoomDuration);
        }
    }
}