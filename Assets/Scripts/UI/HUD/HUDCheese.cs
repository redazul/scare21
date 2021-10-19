using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCheese : MonoBehaviour
{
    //[SerializeField]
    //this can be set in the playercontroller settings
    private float _cheeseCapacity = 10f;

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
    float _cutOffMed = 0.5f;

    [SerializeField]
    [Header("decimal between 0 and 1 indicating when bar changes color")]
    float _cutOffHigh = 0.75f;


    [SerializeField]
    bool cheeseAmountTextVisible = true;

    [SerializeField]
    Text cheeseAmountText;

    RectTransform _rtCheeseCapacity;
    float _cheeseBarScale;

    private void Start()
    {
        _cheeseCapacity = PlayerController.GetCheeseCapacity();

        _rtCheeseCapacity = _imageCheeseCapacity.rectTransform;
        _cheeseBarScale = _cheeseBarMaxWidth / _cheeseCapacity;

        if (!cheeseAmountTextVisible && cheeseAmountText)
        {
            cheeseAmountText.gameObject.SetActive(false);
        }
        SetCheeseText(0);
    }

    public void OnCheeseAmountChanged(float cheese)
    {
        SetCheeseBar(cheese);
        SetCheeseText(cheese);
    }

    private void SetCheeseBar(float cheese)
    {
        _textFull.enabled = (cheese >= _cheeseCapacity);
        cheese = Mathf.Clamp(cheese, 0f, _cheeseCapacity);

        float relativeCheese = cheese / _cheeseCapacity;

        Color newColor = _colorMax;
        if (relativeCheese < _cutOffMed)
        {
            newColor = Color.Lerp(_colorLow, _colorMed, relativeCheese / _cutOffMed);
        }
        else if (relativeCheese >= _cutOffMed && relativeCheese < _cutOffHigh)
        {
            newColor = Color.Lerp(_colorMed, _colorHigh, (relativeCheese - _cutOffMed) / (_cutOffHigh - _cutOffMed));
        }
        else if (relativeCheese >= _cutOffHigh && relativeCheese < 1)
        {
            newColor = Color.Lerp(_colorHigh, _colorMax, (relativeCheese - _cutOffHigh) / (1 - _cutOffHigh));
        }

        _imageCheeseCapacity.color = newColor;
        _rtCheeseCapacity.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cheese * _cheeseBarScale);
    }

    private void SetCheeseText(float cheeseAmount)
    {
        if(!cheeseAmountText || !cheeseAmountTextVisible)
        {
            return;
        }
        cheeseAmountText.text = string.Format("{0:0.00}g", cheeseAmount);
    }
}
