/* Benton Justice
 * 10/16/2021
 * Cheese Implementation
 */

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cheese : MonoBehaviour, IInteractable, ISpawnable
{
    public const float MIN_CHEESE_AMOUNT = 0.2f;
    public const float MAX_CHEESE_AMOUNT = 0.45f;
    public const float AVG_CHEESE_AMOUNT = (MIN_CHEESE_AMOUNT + MAX_CHEESE_AMOUNT) / 2;

    private bool wasSpawned = false;

    Vector3 avgScale;

    float amount;

    void Awake()
    {
        avgScale = transform.localScale;
        SetAmount(GetRandomCheeseAmount());
    }

    private void OnTriggerEnter(Collider other)
    {
        Interact(other.transform);
    }

    public void Interact(Transform other)
    {
        if (other.CompareTag(PlayerController.PLAYER_TAG))
        {
            other.GetComponent<PlayerController>().AddCheese(amount);
            if (wasSpawned)
            {
               LevelManager.Instance.DespawnSingleCheese(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetAmount(float amount)
    {
        this.amount = amount;

        float resizeFactor = amount / AVG_CHEESE_AMOUNT;
        transform.localScale = avgScale * resizeFactor;
    }

    public float GetAmount()
    {
        return amount;
    }

    public void SetSpawned(bool wasSpawned)
    {
        this.wasSpawned = wasSpawned;
    }

    public static float GetRandomCheeseAmount()
    {
        return (float)System.Math.Round(Random.Range(MIN_CHEESE_AMOUNT, MAX_CHEESE_AMOUNT), 2); ;
    }
}
