/* Benton Justice
 * 10/16/2021
 * Interaction Implementation
 */

using UnityEngine;

public interface IInteractable
{

    /// <summary>
    /// Method to be called to interact with object.
    /// </summary>
    public void Interact(Transform other);

}
