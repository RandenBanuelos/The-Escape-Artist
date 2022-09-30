using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    [CreateAssetMenu(fileName = "InteractionInputData", menuName = "Interaction System/Create New Input Data")]
    public class InteractionInputData : ScriptableObject
    {
        #region Private Fields

        private bool interactClicked;

        private bool interactReleased;

        #endregion

        #region "Getter's" & "Setters"

        public bool InteractClicked
        {
            get => interactClicked;
            set => interactClicked = value;
        }

        public bool InteractReleased
        {
            get => interactReleased;
            set => interactReleased = value;
        }

        #endregion

        #region Public Methods

        public void ResetInput()
        {
            interactClicked = false;
            interactReleased = false;
        }

        #endregion
    }
}
