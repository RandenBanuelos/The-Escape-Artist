using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    [RequireComponent(typeof(Collider))]
    public class VoiceActingTrigger : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private VoiceClip voiceClip;

        [SerializeField] private float voiceClipDelay = 0f;

        #endregion

        #region Private Fields

        private VoiceActingManager vaManager;

        private Collider trigger;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            vaManager = VoiceActingManager.Instance;
            trigger = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            trigger.enabled = false;
            Invoke(nameof(PlayVoiceClip), voiceClipDelay);
        }

        #endregion

        #region Private Fields

        private void PlayVoiceClip()
        {
            if (!vaManager)
                vaManager = VoiceActingManager.Instance;

            vaManager.Say(voiceClip);
        }

        #endregion
    }
}
