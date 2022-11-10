using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TheEscapeArtist
{
    public class LockerInteractable : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject stereoscope;

        [SerializeField] private InventoryItem stereoscopeItem;

        [SerializeField] private GameObject pocketWatch;

        [SerializeField] private GameObject shelf;

        [SerializeField] private GameObject mainRoom;

        [SerializeField] private GameObject lockedDoor, regularDoor;

        [SerializeField] private GameObject pastVilla, presentVilla;

        [SerializeField] private GameObject flashlight;

        [SerializeField] private Animator lockerDoor;

        private bool isSwapping = false;

        #endregion

        #region Methods

        public override void OnInteract()
        {
            if (!isSwapping)
            {
                isSwapping = true;
                base.OnInteract();
                StartCoroutine(StereoscopeWatchSwap());
            }
        }

        private IEnumerator StereoscopeWatchSwap()
        {
            stereoscope.SetActive(true);
            gameObject.layer = LayerMask.NameToLayer("Default");

            InventoryManager.Instance.RemoveFromInventory(stereoscopeItem);

            lockerDoor.SetTrigger("CloseDoor");
            yield return new WaitForSeconds(3f);
            lockerDoor.ResetTrigger("CloseDoor");

            stereoscope.SetActive(false);
            pocketWatch.SetActive(true);

            /*if (flashlight == null)
                flashlight = GameObject.FindGameObjectWithTag("Flashlight");
            flashlight.SetActive(true);*/

            lockerDoor.SetTrigger("OpenDoor");
            yield return new WaitForSeconds(2f);

            lockerDoor.ResetTrigger("OpenDoor");
            shelf.SetActive(true);
            this.gameObject.SetActive(false);
        }

        #endregion
    }
}
