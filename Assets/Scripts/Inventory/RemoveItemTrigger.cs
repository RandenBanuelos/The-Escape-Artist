using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class RemoveItemTrigger : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private List<InventoryItem> itemsToRemove = new List<InventoryItem>();

        #endregion

        #region Private Fields

        bool activated = false;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnTriggerEnter(Collider other)
        {
            if (!activated)
            {
                activated = true;
                InventoryManager manager = InventoryManager.Instance;
                foreach (InventoryItem item in itemsToRemove)
                {
                    if (manager.IsInInventory(item))
                        manager.RemoveFromInventory(item);
                }
            }
        }

        #endregion
    }
}
