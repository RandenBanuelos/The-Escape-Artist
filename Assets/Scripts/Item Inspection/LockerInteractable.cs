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

        [SerializeField] private Transform lockerDoor;

        [SerializeField] private float openAngle = -80f;

        [SerializeField] private float openCloseDoorTime = 2f;

        private bool isSwapping = false;


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
            InventoryManager.Instance.RemoveFromInventory(stereoscopeItem);

            lockerDoor.DOLocalRotate(Vector3.zero, openCloseDoorTime);
            yield return new WaitForSeconds(openCloseDoorTime);

            stereoscope.SetActive(false);
            pocketWatch.SetActive(true);

            regularDoor.SetActive(true);
            lockedDoor.SetActive(false);

            mainRoom.SetActive(false);
            pastVilla.SetActive(true);
            presentVilla.SetActive(true);

            lockerDoor.DOLocalRotate(new Vector3(0f, openAngle, 0f), openCloseDoorTime);
            yield return new WaitForSeconds(openCloseDoorTime);

            shelf.SetActive(true);
            this.gameObject.SetActive(false);
        }

        #endregion
    }
}
