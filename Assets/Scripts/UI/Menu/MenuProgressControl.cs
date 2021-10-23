using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuProgressControl : MonoBehaviour
{
    [Tooltip("The name of the start button when no progress is available")]
    [SerializeField]
    private string startText = "Start";

    [Tooltip("The name of the start button when progress is available")]
    [SerializeField]
    private string continueText = "Continue";

    [Tooltip("The label that changes depending on the progress")]
    [SerializeField]
    private Text startButtonLabel;

    [Tooltip("The button that resets the progress and will be hidden when no progress is found")]
    [SerializeField]
    private MenuButton resetProgressButton;

    void Start()
    {
        UpdateMenuDependingOnProgress();
    }

    private void UpdateMenuDependingOnProgress()
    {
        bool hasProgress = CheckpointManager.HasCheckPoint();

        if (!hasProgress)
        {
            startButtonLabel.text = startText;
            resetProgressButton.gameObject.SetActive(false);
        }
        else
        {
            startButtonLabel.text = continueText;
            resetProgressButton.gameObject.SetActive(true);
        }
    }

    public void ResetProgress()
    {
        CheckpointManager.ResetCheckPoint();
        UpdateMenuDependingOnProgress();
    }


}
