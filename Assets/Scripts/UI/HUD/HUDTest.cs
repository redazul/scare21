using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HUDTest : MonoBehaviour
{
    private const int minHealth = 0;
    private const int minCheeseCount = 0;
    private const int minSurvivors = 0;

    [SerializeField]
    private int maxhealth = 3;
    [SerializeField]
    private int health = 3;

    [SerializeField]
    private float maxCheeseCount = 100;

    [SerializeField]
    private float cheeseCount = 0;

    [SerializeField]
    private int maxSurvivorsCount = 10;
    [SerializeField]
    private int survivorsCount = 10;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            health = Mathf.Min(health + 1, maxhealth);
            HUDManager.Instance.UpdateHealthAmount(health);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            health = Mathf.Max(health - 1, minHealth);
            HUDManager.Instance.UpdateHealthAmount(health);
        }

        
        if (Input.GetKeyDown(KeyCode.W))
        {
            cheeseCount = Mathf.Min(cheeseCount + 5, maxCheeseCount);
            HUDManager.Instance.UpdateCheeseAmount(cheeseCount);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            cheeseCount = Mathf.Max(cheeseCount - 5, minCheeseCount);
            HUDManager.Instance.UpdateCheeseAmount(cheeseCount);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            survivorsCount = Mathf.Max(survivorsCount - 1, minSurvivors);

            HUDManager.Instance.UpdateSurvivorsAmount(survivorsCount);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            survivorsCount = Mathf.Min(survivorsCount + 1, maxSurvivorsCount);

            HUDManager.Instance.UpdateSurvivorsAmount(survivorsCount);
        }
    }

}
