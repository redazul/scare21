using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHealth : MonoBehaviour
{
    [SerializeField]
    int _maxHealth = 3;

    [SerializeField]
    Color _threeBarColor, _twoBarColor, _oneBarColor;

    [SerializeField]
    Image _imageHeart, _bar1, _bar2, _bar3;


    int health = 3;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnHealthChanged(++health);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnHealthChanged(--health);
        }
    }



    public void OnHealthChanged(int newHealth)
    {
        newHealth = (int)Mathf.Clamp(newHealth, 0f, _maxHealth);

        if (newHealth == 3)
        {
            _bar3.color = _threeBarColor;
            _bar2.color = _threeBarColor;
            _bar1.color = _threeBarColor;
        }
        else if (newHealth == 2)
        {
            _bar2.color = _twoBarColor;
            _bar1.color = _twoBarColor;
        }
        else if (newHealth == 1)
        {
            _bar1.color = _oneBarColor;
        }

        HandleHealthBarVisibility(newHealth);
    }


    void HandleHealthBarVisibility(int newHealth)
    {
        _bar3.enabled = newHealth >= 3;
        _bar2.enabled = newHealth >= 2;
        _bar1.enabled = newHealth >= 1;
    }
}
