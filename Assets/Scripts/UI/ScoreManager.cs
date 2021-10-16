using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cheeseText;
    [SerializeField] TextMeshProUGUI liveStockText;

    //Placeholder. I don't know where the cheese value is stored yet.
    [SerializeField] float cheese;

    int liveStock = 10;
    private void Start()
    {
        UpdateText();
    }

    void UpdateText()
    {
        cheeseText.text = cheese.ToString();
        liveStockText.text = liveStock.ToString();
    }

    private void Update()
    {
        EstimateLiveStock(cheese);
    }

    private void EstimateLiveStock(float _cheese)
    {
        //Placeholder
        if (_cheese < 1) liveStock = 0;
        else if (_cheese < 5) liveStock = 5;
        else if (_cheese < 10) liveStock = 8;
        else if (_cheese >= 10) liveStock = 10;
    }
}
