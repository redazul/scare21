using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSurvivors : MonoBehaviour
{
    [SerializeField]
    Text _textSurvivorsCount;

    [SerializeField]
    string _prefixSurvivorsCount = "x ";

    [SerializeField]
    int _maxSurvivors = 10;

    int _survivorsCountTest;

    private void Start()
    {
        _textSurvivorsCount.text = _prefixSurvivorsCount + _maxSurvivors;

        _survivorsCountTest = _maxSurvivors;
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnSurvivorsCountChanged(--_survivorsCountTest);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnSurvivorsCountChanged(++_survivorsCountTest);
        }*/
    }

    public void OnSurvivorsCountChanged(int survivors)
    {
        survivors = (int)Mathf.Clamp(survivors, 0f, _maxSurvivors);
        _textSurvivorsCount.text = _prefixSurvivorsCount + survivors;
    }
}
