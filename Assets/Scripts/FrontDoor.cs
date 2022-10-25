using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class FrontDoor : MonoBehaviour
    {
        private Animator frontDoor;

        private VoiceActingManager vaManager;

        private AudioSource doorCreak;

        private void Start()
        {
            frontDoor = GetComponent<Animator>();
            doorCreak = GetComponent<AudioSource>();
            vaManager = VoiceActingManager.Instance;
        }

        public void OpenDoor()
        {
            StartCoroutine(OpenDoorAfterVoiceOver());
        }

        private IEnumerator OpenDoorAfterVoiceOver()
        {
            yield return new WaitUntil(() => vaManager.IsSpeaking == false);
            doorCreak.Play();
            frontDoor.SetTrigger("OpenDoor");
            Invoke(nameof(ClearTrigger), 1f);
        }

        private void ClearTrigger()
        {
            frontDoor.ResetTrigger("OpenDoor");
        }
    }
}
