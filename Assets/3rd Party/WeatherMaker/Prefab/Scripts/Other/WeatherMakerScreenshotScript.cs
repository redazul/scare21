using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.WeatherMaker
{
    /// <summary>
    /// Simple screenshot script for game view. Press R to take screenshots, file is saved to desktop. There is a short delay from pressing R and the screenshot actually saving.
    /// </summary>
    public class WeatherMakerScreenshotScript : MonoBehaviour
    {
        private int needsScreenshot;

        private void Update()
        {
            if (needsScreenshot == 0)
            {
                needsScreenshot = Input.GetKeyDown(KeyCode.R) ? 1 : 0;
            }
            else if (++needsScreenshot == 64)
            {
                ScreenCapture.CaptureScreenshot(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/screenshot.png");
                needsScreenshot = 0;
            }
        }
    }
}
