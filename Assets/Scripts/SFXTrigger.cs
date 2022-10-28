using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public class SFXTrigger : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private AudioClip sfxClip;

        [SerializeField] private float sfxDelay = 0f;

        #endregion

        #region Private Fields

        private AudioSource audioPlayer;

        private Collider trigger;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            audioPlayer = GetComponent<AudioSource>();
            trigger = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            trigger.enabled = false;
            Invoke(nameof(PlaySFX), sfxDelay);
        }

        #endregion

        #region Private Fields

        private void PlaySFX()
        {
            audioPlayer.PlayOneShot(sfxClip);
        }

        #endregion
    }
}
