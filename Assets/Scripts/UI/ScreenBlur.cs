using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

namespace TheEscapeArtist
{
    public class ScreenBlur : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private PostProcessVolume volume;

        [SerializeField] private Image darken;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            // ToggleBlur(false);
        }

        #endregion

        #region Public Methods

        public void ToggleBlur(bool isActivated)
        {
            float volumeAlpha = isActivated ? 1f : 0f;
            float darkenAlpha = isActivated ? 0.5f : 0f;

            volume.weight = volumeAlpha;
            var imageColor = darken.color;
            imageColor.a = darkenAlpha;
            darken.color = imageColor;
        }

        #endregion
    }
}
