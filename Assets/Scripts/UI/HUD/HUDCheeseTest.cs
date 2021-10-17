using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCheeseTest : MonoBehaviour
{
    [SerializeField]
    float _cheeseCapacity = 100f;

    [SerializeField]
    Image _imageCheeseCapacity;

    [SerializeField]
    Text _textFull;

    [SerializeField]
    float _cheeseBarMaxWidth = 300;

    [SerializeField]
    Color _colorLow, _colorMed, _colorHigh, _colorMax;

    [SerializeField]
    [Header("decimal between 0 and 1 indicating when bar changes color")]
    float _cutOffMed, _cutOffHigh;

    RectTransform _rtCheeseCapacity;
    float _cheeseBarScale;

    float _cheeseCountTest;


    private void Start()
    {
        _rtCheeseCapacity = _imageCheeseCapacity.rectTransform;
        _cheeseBarScale = _cheeseBarMaxWidth / _cheeseCapacity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            References.cheese = (int)Mathf.Clamp(References.cheese + 5, 0, _cheeseCapacity);
            OnCheeseAmountChanged(References.cheese);
            ScoreManager.OnCheeseChange();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            References.cheese = (int)Mathf.Clamp(References.cheese - 5, 0, _cheeseCapacity);
            OnCheeseAmountChanged(References.cheese);
            ScoreManager.OnCheeseChange();
        }
    }


    public void OnCheeseAmountChanged(float cheese)
    {
        _textFull.enabled = (cheese >= _cheeseCapacity);
        cheese = Mathf.Clamp(cheese, 0f, _cheeseCapacity);

        print(cheese);


        Color newColor = _colorMax;
        if (cheese < _cutOffMed) newColor = Color.Lerp(_colorLow, _colorMed, cheese / _cutOffMed);
        else if (cheese >= _cutOffMed && cheese < _cutOffHigh) newColor = Color.Lerp(_colorMed, _colorHigh, (cheese - _cutOffMed) / (_cutOffHigh - _cutOffMed));
        else if (cheese >= _cutOffHigh && cheese < _cheeseCapacity) newColor = Color.Lerp(_colorHigh, _colorMax, (cheese - _cutOffHigh) / (_cheeseCapacity - _cutOffHigh));

        _imageCheeseCapacity.color = newColor;


        _rtCheeseCapacity.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cheese * _cheeseBarScale);
    }
}
