/* Benton Justice
 * 10/16/2021
 * Cheese Implementation
 */

using UnityEngine;

public class Cheese : MonoBehaviour, IInteractable
{
    public void Interact(Transform other)
    {
        References.SetCheese(References.GetCheese() + 1);
        Destroy(this.gameObject);
    }

}
