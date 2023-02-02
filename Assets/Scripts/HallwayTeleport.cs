using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class HallwayTeleport : MonoBehaviour
    {
        private VoiceActingManager vaManager;

        [SerializeField] private Transform player;

        // private AudioSource lightbulb;

        [SerializeField] private GameObject blackout;

        [SerializeField] private VoiceClip vaClip;

        [SerializeField] private Transform teleportSpawn;

        [SerializeField] private GameObject pocketWatch;

        private void Start()
        {
            // lightbulb = GetComponent<AudioSource>();
            vaManager = VoiceActingManager.Instance;
            StartCoroutine(Teleport());
        }

        private IEnumerator Teleport()
        {
            yield return new WaitForSeconds(5f);
            yield return new WaitUntil(() => vaManager.NotVoiceActing() == true);

            blackout.SetActive(true);
            CharacterController ca = player.GetComponent<CharacterController>();
            ca.enabled = false;
            pocketWatch.SetActive(false);
            player.position = teleportSpawn.position;

            vaManager.Say(vaClip);
            yield return new WaitUntil(() => vaManager.NotVoiceActing() == true);

            blackout.SetActive(false);
            ca.enabled = true;
        }
    }
}
