using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class NewLockerInteractable : InteractableBase
    {
        [SerializeField] private Animator lockerAnimator;

        [SerializeField] private GameObject watchLid;

        [SerializeField] private float watchEnableDelay = 4.5f;

        public override void OnInteract()
        {
            base.OnInteract();
            gameObject.layer = LayerMask.NameToLayer("Default");
            GetComponent<Outline>().OutlineWidth = 0f;

            lockerAnimator.SetTrigger("Swap");
            Invoke(nameof(ClearTrigger), .1f);
            Invoke(nameof(EnableWatchPickup), watchEnableDelay);
        }

        private void ClearTrigger()
        {
            lockerAnimator.ResetTrigger("Swap");
        }

        private void EnableWatchPickup()
        {
            watchLid.layer = LayerMask.NameToLayer("Interactable");
            lockerAnimator.enabled = false;
        }
    }
}
