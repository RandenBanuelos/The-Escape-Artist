using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class CustomInputs : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private InteractionInputData interactionInputData = null;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            interactionInputData.ResetInput();
        }

        private void Update()
        {
            GetInteractionInputData();
        }

        #endregion

        #region Private Methods

        private void GetInteractionInputData()
        {
            interactionInputData.InteractClicked = Input.GetMouseButtonDown(0);

            interactionInputData.InteractReleased = Input.GetMouseButtonUp(0);
        }

        #endregion
    }
}
