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

        [Header("Cassette Player SFX")]
        [SerializeField] private AudioClip insertTape;

        [SerializeField] private AudioClip pressPlay;

        [SerializeField] private AudioClip pressStop;

        [SerializeField] private AudioClip ejectTape;

        [Header("Glitch SFX")]
        [SerializeField] private AudioClip glitchEnter;

        [SerializeField] private AudioClip glitchExit;

        #endregion

        #region Private Fields

        private bool isSpeaking = false;

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
                DontDestroyOnLoad(this);
                ClearSubtitles();
            }
        }

        #endregion

        #region Public Methods

        public void Say(VoiceClip voiceClip, bool isActualTape = false)
        {
            if (source.isPlaying)
            {
                source.Stop();
                StopCoroutine(nameof(PlayCassettePlayer));
            }

            StartCoroutine(PlayCassettePlayer(voiceClip, isActualTape));
        }

        public bool IsSpeaking => isSpeaking;

        #endregion

        #region Private Methods

        private IEnumerator PlayCassettePlayer(VoiceClip voiceClip, bool isActualTape)
        {
            isSpeaking = true;

            if (isActualTape)
            {
                source.PlayOneShot(insertTape);
                yield return new WaitForSeconds(insertTape.length);

                yield return new WaitForSecondsRealtime(0.5f);

                source.PlayOneShot(pressPlay);
                yield return new WaitForSeconds(pressPlay.length);
            }
            else
            {
                source.PlayOneShot(glitchEnter);
                yield return new WaitForSeconds(insertTape.length);
            }

            source.PlayOneShot(voiceClip.Clip);
            StartCoroutine(PlaySubtitles(voiceClip.Subtitles));
            yield return new WaitForSeconds(voiceClip.Clip.length);

            if (isActualTape)
            {
                source.PlayOneShot(pressStop);
                yield return new WaitForSeconds(pressStop.length);

                source.PlayOneShot(ejectTape);
                yield return new WaitForSeconds(ejectTape.length);
            }
            else
            {
                source.PlayOneShot(glitchExit);
                yield return new WaitForSeconds(pressStop.length);
            }

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
