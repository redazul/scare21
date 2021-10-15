/* Benton Justice
 * 10/14/2021
 * Dialog Controller
 */
using TMPro;
using UnityEngine;
using System.Collections;

namespace Scare.Dialog
{
    /// <summary>
    /// The Dialog controller controls the flow of the dialog scene. It handles 
    /// updating text and text color as well as determining when to end and clear the 
    /// dialog. If I have time I would like to create a captions stand-alone class.
    /// </summary>
    public class DialogController : MonoBehaviour
    {

        //Components
        [SerializeField]
        private AudioSource[] audioSources;

        [SerializeField]
        private TextMeshProUGUI captionsText;

        private Dialog dialogRoot;

        /// <summary>
        /// This method should be called to begin a dialog chain.
        /// </summary>
        /// <param name="dialog">The root that contains all of the dialog for a scenario.</param>
        public void StartDialog(Dialog dialog)
        {
            dialogRoot = dialog;

            UpdateCaption();
            StartCoroutine(PlayAudioClip(dialogRoot.DialogAudio));
        }

        /// <summary>
        /// On AudioClip finish go to the next dialog object.
        /// </summary>
        private void OnClipFinished()
        {
            if (dialogRoot.Next != null)
            {
                dialogRoot = dialogRoot.Next;
                UpdateCaption();
                StartCoroutine(PlayAudioClip(dialogRoot.DialogAudio));
            }
            else
            {
                ClearDialog();
            }
        }

        /// <summary>
        /// Updates captionat bottom of the screen.
        /// </summary>
        private void UpdateCaption()
        {
            captionsText.color = dialogRoot.DialogColor;
            captionsText.text = dialogRoot.DialogText;
        }

        private void ClearDialog()
        {
            captionsText.text = "";
        }

        /// <summary>
        /// A simple callback to traverse the dialog chain.
        /// </summary>
        /// <param name="clip">The AudioClip to be played.</param>
        private IEnumerator PlayAudioClip(AudioClip clip)
        {

            if(dialogRoot.SpeakerIndex >= audioSources.Length || dialogRoot.SpeakerIndex < 0)
            {
                Debug.LogError(name + ": dialogRoot.SpeakerIndex out of bounds. Check the Dialog Scriptable Objects.");
            }

            audioSources[dialogRoot.SpeakerIndex].clip = clip;
            audioSources[dialogRoot.SpeakerIndex].PlayDelayed(dialogRoot.Delay);

            yield return new WaitForSeconds(clip.length + dialogRoot.Delay);

            OnClipFinished();

        }


    }
}
