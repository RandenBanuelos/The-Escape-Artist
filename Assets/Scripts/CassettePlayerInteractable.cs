using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class CassettePlayerInteractable : InteractableBase
    {
        [SerializeField] private FrontDoor frontDoor;

        public override void OnInteract()
        {
            base.OnInteract();

            if (VoiceClip && VoiceActingManager.Instance)
                VoiceActingManager.Instance.Say(VoiceClip, true);

            frontDoor.OpenDoor();

            this.gameObject.SetActive(false);
        }
    }
}
