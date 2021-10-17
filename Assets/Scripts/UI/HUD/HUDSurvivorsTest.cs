using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSurvivorsTest : MonoBehaviour
{
    [SerializeField]
    Text _textSurvivorsCount;

    [SerializeField]
    string _prefixSurvivorsCount = "x ";

    [SerializeField]
    int _maxSurvivors = 10;


    private void Start()
    {
        _textSurvivorsCount.text = _prefixSurvivorsCount + References.survivors;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnSurvivorsCountChanged(--References.survivors);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnSurvivorsCountChanged(++References.survivors);
        }
    }


    public void OnSurvivorsCountChanged(int survivors)
    {
        survivors = (int)Mathf.Clamp(survivors, 0f, _maxSurvivors);
        _textSurvivorsCount.text = _prefixSurvivorsCount + survivors;
    }
}
