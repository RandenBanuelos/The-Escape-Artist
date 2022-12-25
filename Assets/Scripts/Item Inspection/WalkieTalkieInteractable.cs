using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class WalkieTalkieInteractable : InteractableBase
    {
        [SerializeField] private FrontDoor frontDoor;

        [SerializeField] private GameObject walkieTalkie;

        public override void OnInteract()
        {
            base.OnInteract();

            if (VoiceClip && VoiceActingManager.Instance)
                VoiceActingManager.Instance.Say(VoiceClip);

            frontDoor.OpenDoor();

            walkieTalkie.SetActive(true);

            this.gameObject.SetActive(false);
        }
    }
}
