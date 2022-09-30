using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    [CreateAssetMenu(fileName = "InteractionData", menuName = "Interaction System/Create New Interaction Data")]
    public class InteractionData : ScriptableObject
    {
        #region Private Fields

        private InteractableBase interactable;

        #endregion

        #region "Getter's" & "Setters"

        public InteractableBase Interactable
        {
            get => interactable;
            set => interactable = value;
        }

        #endregion

        #region Public Methods

        public void Interact()
        {
            interactable.OnInteract();
            ResetData();
        }

        public bool IsSameInteractable(InteractableBase newInteractable) => interactable == newInteractable;

        public bool IsEmpty() => interactable == null;

        public bool ResetData() => interactable = null;

        #endregion
    }
}
