using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class LockedTransformInspectable : TransformInspectable
    {
        #region Private Serializable Fields

        [SerializeField] private List<InventoryItem> requiredItems = new List<InventoryItem>();

        #endregion

        #region Private Fields

        private InventoryManager inventoryCache = null;

        private bool isAccessible = false;

        #endregion

        public override void OnInteract()
        {
            if (inventoryCache == null)
                inventoryCache = InventoryManager.Instance;

            if (isAccessible)
            {
                base.OnInteract();
            }
            else
            {
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
                    isAccessible = true;
                    base.OnInteract();
                }
                else
                {
                    InteractionUIPanel.Instance.SetRequirement($"Requires {firstMissingItem}");
                }
            }
        }
    }
}
