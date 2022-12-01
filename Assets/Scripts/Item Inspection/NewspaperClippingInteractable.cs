using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class NewspaperClippingInteractable : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private NewspaperClipping clipping;

        #endregion

        public override void OnInteract()
        {
            base.OnInteract();
            NewspaperClippingManager.Instance.ShowNewspaper(clipping);
        }
    }
}
