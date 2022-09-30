using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class InventoryItemPickup : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private InventoryItem itemToAdd;

        #endregion

        public override void OnInteract()
        {
            base.OnInteract();
            InventoryManager.Instance.AddToInventory(itemToAdd);
            InteractionUIPanel.Instance.SetPickup($"Picked Up {itemToAdd.ItemName}");
            this.gameObject.SetActive(false);
        }
    }
}
