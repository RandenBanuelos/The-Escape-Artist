using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TheEscapeArtist
{
    // Based on Comp-3 Interactive's implementation of a dynamic subtitles system
    // https://www.youtube.com/watch?v=Wu46UAVlFL4
    public class VoiceActingManager : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private AudioSource source;

        [SerializeField] private TMP_Text subtitles;

        [Header("Walkie Talkie SFX")]
        [SerializeField] private AudioClip walkieTalkieOn;

        [SerializeField] private AudioClip walkieTalkieOff;

        #endregion

        #region Private Fields

        private bool isSpeaking = false;

        private Queue<VoiceClip> voiceClipQueue = new Queue<VoiceClip>();

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static VoiceActingManager _instance;

        public static VoiceActingManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one VoiceActingManager Singleton!");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                ClearSubtitles();
            }
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            if (!isSpeaking && voiceClipQueue.Count > 0)
            {
                VoiceClip nextVoiceClip = voiceClipQueue.Dequeue();

                StartCoroutine(PlayWalkieTalkie(nextVoiceClip));
            }
        }

        #endregion

        #region Public Methods

        public void Say(VoiceClip voiceClip)
        {
            voiceClipQueue.Enqueue(voiceClip);
        }

        public void PauseUnpauseVA(bool toggle)
        {
            if (toggle)
                source.Pause();
            else
                source.Play();
        }

        public bool IsSpeaking => isSpeaking;

        public bool NotVoiceActing()
        {
            return !isSpeaking && voiceClipQueue.Count == 0;
        }

        #endregion

        #region Private Methods

        private IEnumerator PlayWalkieTalkie(VoiceClip voiceClip)
        {
            isSpeaking = true;

            source.clip = walkieTalkieOn;
            source.Play();
            yield return new WaitForSeconds(walkieTalkieOn.length);

            source.clip = voiceClip.Clip;
            source.Play();
            StartCoroutine(PlaySubtitles(voiceClip.Subtitles));
            yield return new WaitForSeconds(voiceClip.Clip.length);

            source.clip = walkieTalkieOff;
            source.Play();
            yield return new WaitForSeconds(walkieTalkieOff.length);

            isSpeaking = false;
        }

        private IEnumerator PlaySubtitles(List<Subtitle> subtitles)
        {
            foreach (Subtitle subtitle in subtitles)
            {
                SetSubtitles(subtitle.subtitleText);
                yield return new WaitForSeconds(subtitle.displayTime);
                ClearSubtitles();
            }
        }

        private void SetSubtitles(string subtitle)
        {
            subtitles.text = subtitle;
        }

        private void ClearSubtitles()
        {
            subtitles.text = "";
        }

        #endregion
    }
}
