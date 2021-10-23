using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDMessageDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject messageGameObject;

    private TextMeshProUGUI messageText;

    private Timer displayTimer;

    void Awake()
    {
        messageText = messageGameObject.GetComponentInChildren<TextMeshProUGUI>();

        displayTimer = gameObject.AddComponent<Timer>();

        HideMessage();
    }

    public void CheckpointSaved()
    {
        ShowMessage("Checkpoint saved.");
    }

    public void ShowCheckpointPromptMessage(float cheeseNeeded)
    {
        ShowMessage("You need " + HUDCheese.CheeseAmountToString(cheeseNeeded) + " cheese to pass. Press E to deliver the cheese.");
    }

    private void ShowMessage(string messageToShow)
    {
        messageGameObject.SetActive(true);
        messageText.SetText(messageToShow);
    }

    public void HideMessage()
    {
        messageGameObject.SetActive(false);
        messageText.SetText("");
    }
}
