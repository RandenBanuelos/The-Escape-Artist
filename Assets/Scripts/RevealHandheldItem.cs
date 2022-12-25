using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class RevealHandheldItem : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private InventoryItem itemNeeded;

        [SerializeField] private GameObject oldItemToReveal;

        [SerializeField] private GameObject newItemToReveal;

        [SerializeField] private bool disableSelf = true;

        #endregion

        #region Private Fields

        private InventoryManager manager;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            manager = InventoryManager.Instance;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                if (manager.IsInInventory(itemNeeded))
                {
                    if (oldItemToReveal)
                        oldItemToReveal.SetActive(false);

                    if (newItemToReveal)
                        newItemToReveal.SetActive(true);

                    if (disableSelf)
                        this.gameObject.SetActive(false);
                }
            }
        }

        #endregion
    }
}
