/*Benton Justice
 *10/15/2021
 * Box Trigger For Dialogs
 */
using UnityEngine;

namespace Scare.Dialog.Triggers {
    /// <summary>
    /// The Box Trigger is for dialogs to be 
    /// started when the player steps in bounds of the box.
    /// </summary>
    public class BoxTrigger : MonoBehaviour
    {
        [Header("Dialog Components")]
        [SerializeField]
        private DialogController dialogController;
        [SerializeField]
        private Dialog dialogRoot;

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.CompareTag("Player"))
                dialogController.StartDialog(dialogRoot);
        }

    }
}