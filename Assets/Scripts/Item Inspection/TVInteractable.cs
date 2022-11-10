using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace TheEscapeArtist
{
    public class TVInteractable : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private MeshRenderer screen;

        [SerializeField] private VideoPlayer tv;

        [SerializeField] private InventoryItem remote;

        [SerializeField] private Material tvBaseMaterial, tvPlayingMaterial;

        #endregion

        #region Private Fields

        private bool isPlaying = false;

        #endregion

        public override void OnInteract()
        {
            if (InventoryManager.Instance.IsInInventory(remote))
            {
                base.OnInteract();

                if (isPlaying)
                {
                    tv.Pause();
                    screen.material = tvBaseMaterial;
                    isPlaying = false;
                }
                else
                {
                    isPlaying = true;
                    screen.material = tvPlayingMaterial;
                    tv.Play();
                }
            }
            else
            {
                InteractionUIPanel.Instance.SetRequirement($"Requires {remote.ItemName}");
            }
        }
    }
}
