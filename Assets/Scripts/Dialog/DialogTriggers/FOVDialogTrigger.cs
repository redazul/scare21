using UnityEngine;
using Scare.AI.Components;

namespace Scare.Dialog.Triggers {

    /// <summary>
    /// The FOV Dialog Trigger will trigger dialog when an Item Of Interest is 
    /// returned through a FOV Components event.
    /// </summary>
    public class FOVDialogTrigger : MonoBehaviour
    {

        [Header("AI Components")]
        [SerializeField]
        private FOVComponent fovComponent;

        [Header("Dialog Components")]
        [SerializeField]
        private DialogController controller;
        
        //These will probably be arrays with a few different dialogs for each Item of Interest.
        [SerializeField]
        private Dialog cheeseDialog;
        [SerializeField]
        private Dialog catDialog;

        private void OnItemFound(Collider collider)
        {
            //TODO depending on what the item is use the best dialog for the situation
            
            controller.StartDialog(cheeseDialog);

        }

        private void OnEnable()
        {
            fovComponent.foundItemCallBack += OnItemFound;
        }

        private void OnDisable()
        {
            fovComponent.foundItemCallBack -= OnItemFound;
        }

    }
}