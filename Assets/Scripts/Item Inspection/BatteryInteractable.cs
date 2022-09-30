using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class BatteryInteractable : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private List<InventoryItem> requiredItems = new List<InventoryItem>();

        [SerializeField] private GameObject batteryObject;

        [SerializeField] private UFOInteractable ufo;

        #endregion

        #region Private Fields

        private InventoryManager inventoryCache = null;

        private bool isPowered = false;

        #endregion

        public override void OnInteract()
        {
            if (inventoryCache == null)
                inventoryCache = InventoryManager.Instance;

            bool canInteract = true;
            string firstMissingItem = "";

            foreach (InventoryItem item in requiredItems)
            {
                if (!inventoryCache.IsInInventory(item))
                {
                    canInteract = false;
                    firstMissingItem = item.ItemName;
                    break;
                }
            }

            if (canInteract)
            {
                batteryObject.SetActive(true);
                ufo.EnablePower();
                base.OnInteract();
                this.gameObject.SetActive(false);
            }
            else
            {
                InteractionUIPanel.Instance.SetRequirement($"Needs {firstMissingItem}");
            }
        }

        #region "Getter's"

        public bool IsPowered => isPowered;

        #endregion
    }
}
