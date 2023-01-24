using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

namespace TheEscapeArtist
{
    // Based on GamesPlusJames's implementation of an options screen
    // https://www.youtube.com/watch?v=yeaELkoxD9w
    public class OptionsScreen : MonoBehaviour
    {
        #region Private Serializable Fields

        [Header("References")]
        [SerializeField] private PostProcessVolume ppVolume;

        [SerializeField] private GameObject graphicsObject, audioObject;

        [SerializeField] private TMP_Text switchGraphicsAudioText;


        [Header("Graphics")]
        [SerializeField] private TMP_Text resolutionLabel;

        [SerializeField] private Toggle fullscreenToggle, vsyncToggle;

        [SerializeField] private Toggle postProcessingToggle, motionBlurToggle;

        [SerializeField] private List<ResolutionItem> resolutions = new List<ResolutionItem>();


        [Header("Audio")]
        [SerializeField] private AudioMixer mainMixer;

        [SerializeField] private TMP_Text masterLabel, musicLabel, sfxLabel;

        [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;

        #endregion

        #region Private Fields

        private int selectedResolution;

        private MotionBlur motionBlur;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            ppVolume.profile.TryGetSettings(out motionBlur);

            postProcessingToggle.isOn = ppVolume.weight == 1;

            motionBlurToggle.isOn = motionBlur.active;

            fullscreenToggle.isOn = Screen.fullScreen;

            vsyncToggle.isOn = QualitySettings.vSyncCount == 0 ? false : true;

            bool foundResolution = false;
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
                {
                    foundResolution = true;
                    selectedResolution = i;
                    UpdateResolutionLabel();
                }
            }

            if (!foundResolution)
            {
                ResolutionItem newResolution = new ResolutionItem();
                newResolution.horizontal = Screen.width;
                newResolution.vertical = Screen.height;

                resolutions.Add(newResolution);
                selectedResolution = resolutions.Count - 1;

                UpdateResolutionLabel();
            }

            float volume = 0f;
            mainMixer.GetFloat("MasterVolume", out volume);
            masterSlider.value = volume;

            mainMixer.GetFloat("MusicVolume", out volume);
            musicSlider.value = volume;

            mainMixer.GetFloat("SFXVolume", out volume);
            sfxSlider.value = volume;

            masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
            musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
            sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        }

        #endregion

        #region Public Methods

        public void ResolutionLeft()
        {
            selectedResolution--;

            if (selectedResolution < 0)
            {
                selectedResolution = 0;
            }

            UpdateResolutionLabel();
        }

        public void ResolutionRight()
        {
            selectedResolution++;

            if (selectedResolution > resolutions.Count - 1)
            {
                selectedResolution = resolutions.Count - 1;
            }

            UpdateResolutionLabel();
        }

        public void ApplyGraphics()
        {
            QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;

            Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenToggle.isOn);

            ppVolume.weight = postProcessingToggle.isOn ? 1 : 0;

            motionBlur.active = motionBlurToggle.isOn ? true : false;
        }

        public void SetMasterVolume()
        {
            masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
            mainMixer.SetFloat("MasterVolume", masterSlider.value);
            PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        }

        public void SetMusicVolume()
        {
            musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
            mainMixer.SetFloat("MusicVolume", musicSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        }

        public void SetSFXVolume()
        {
            sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
            mainMixer.SetFloat("SFXVolume", sfxSlider.value);
            PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        }

        public void SwitchGraphicsAudio()
        {
            if (graphicsObject.activeSelf == true)
            {
                graphicsObject.SetActive(false);
                audioObject.SetActive(true);
                switchGraphicsAudioText.text = "<< Graphics";
            }
            else
            {
                graphicsObject.SetActive(true);
                audioObject.SetActive(false);
                switchGraphicsAudioText.text = "Audio >>";
            }
        }

        #endregion

        #region Private Methods

        private void UpdateResolutionLabel()
        {
            resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
        }

        #endregion
    }
}

[System.Serializable]
public class ResolutionItem
{
    public int horizontal;

    public int vertical;
}
