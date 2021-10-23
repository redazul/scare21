using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance = null;

    private Dictionary<int, CheckPointArea> checkpointDict;

    [Tooltip("Provide the checkpoints that should be managed by this manager. If no checkpoints are provided, the manager will use children")]
    [SerializeField]
    private CheckPointArea[] checkPointsToManage;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one CheckPointManager in this scene.");
        }
        Instance = this;

        CollectCheckpoints();
    }

    private void CollectCheckpoints()
    {
        CheckPointArea[] checkpoints;
        if (checkPointsToManage.Length > 0)
        {
            checkpoints = checkPointsToManage;
        }
        else
        {
            checkpoints = GetComponentsInChildren<CheckPointArea>();
        }

        int idCounter = 0;
        checkpointDict = new Dictionary<int, CheckPointArea>();
        foreach(CheckPointArea cpa in checkpoints)
        {
            cpa.SetId(idCounter);
            checkpointDict.Add(idCounter, cpa);
            idCounter++;
        }
    }

    public CheckPointArea GetCurrentCheckpoint()
    {
        //no key was saved yet -> no checkpoint available
        if (!PlayerPrefs.HasKey(CheckPointArea.PP_KEY_CHECKPOINT_ID))
        {
            return null;
        }
        int checkpointId = PlayerPrefs.GetInt(CheckPointArea.PP_KEY_CHECKPOINT_ID);
        if(checkpointId > 0)
        {
            return checkpointDict[checkpointId];
        }
        else
        {
            return null;
        }
    }

    public static bool HasCheckPoint()
    {
        if (!PlayerPrefs.HasKey(CheckPointArea.PP_KEY_CHECKPOINT_ID))
        {
            return false;
        }
        int checkpointId = PlayerPrefs.GetInt(CheckPointArea.PP_KEY_CHECKPOINT_ID);
        return checkpointId >= 0;
    }

    public static void ResetCheckPoint()
    {
        PlayerPrefs.SetInt(CheckPointArea.PP_KEY_CHECKPOINT_ID, -1);
    }

}