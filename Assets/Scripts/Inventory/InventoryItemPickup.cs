using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class InventoryItemPickup : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private InventoryItem itemToAdd;

        [SerializeField] private AudioSource itemGetSFX;

        [SerializeField] private bool playSFX = true;

        #endregion

        public override void OnInteract()
        {
            base.OnInteract();
            InventoryManager.Instance.AddToInventory(itemToAdd);
            InteractionUIPanel.Instance.SetPickup($"Picked Up {itemToAdd.ItemName}");

            if (VoiceClip && VoiceActingManager.Instance)
                VoiceActingManager.Instance.Say(VoiceClip);

            if (playSFX)
                itemGetSFX.Play();

            HideRevealManager.Instance.AddHideRevealChange(this.gameObject.name, false);
            this.gameObject.SetActive(false);
        }
    }
}
