using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class TVScreenInteractable : InteractableBase
    {
        #region Private Fields

        private TVChannel currentChannel;

        private int channelIndex = 0;

        private MeshRenderer screen;

        private List<Material> channelMaterials = new List<Material>();

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            screen = this.gameObject.GetComponent<MeshRenderer>();
            channelMaterials = TVRoomManager.Instance.Channels;
            channelIndex = 0;
            currentChannel = (TVChannel)channelIndex;
        }

        #endregion

        #region Public Override Fields

        public override void OnInteract()
        {
            base.OnInteract();
            channelIndex = channelIndex + 1 == channelMaterials.Count ? 0 : channelIndex + 1;
            currentChannel = (TVChannel)channelIndex;

            screen.sharedMaterial = channelMaterials[channelIndex];

            Debug.Log($"{this.name}'s New Channel: {currentChannel}, {screen.sharedMaterial.name}");
            TVRoomManager.Instance.ChangeChannel(this.gameObject, currentChannel);
        }

        #endregion
    }
}
