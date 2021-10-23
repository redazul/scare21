using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{

    [Tooltip("The level that should be started once the director is finished")]
    [SerializeField]
    private SceneLoader.AvailableScene followingLevel;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FinishCutscene();
        }
    }

    public void FinishCutscene()
    {
        SceneLoader.Instance.SwitchToScene(followingLevel);
    }


}
