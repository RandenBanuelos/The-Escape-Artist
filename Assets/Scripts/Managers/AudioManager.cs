using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TheEscapeArtist
{
    // Based on GamesPlusJames's implementation of an audio menu
    // https://www.youtube.com/watch?v=tepGwzd-zRg
    public class AudioManager : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private AudioMixer mainMixer;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            if (PlayerPrefs.HasKey("MasterVolume"))
                mainMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));

            if (PlayerPrefs.HasKey("MusicVolume"))
                mainMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));

            if (PlayerPrefs.HasKey("SFXVolume"))
                mainMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }

        #endregion
    }
}
