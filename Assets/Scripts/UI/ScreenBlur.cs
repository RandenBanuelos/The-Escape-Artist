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

        #region Private Fields

        private DepthOfField depthOfField;

        private float originalDOF;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            volume.profile.TryGetSettings(out depthOfField);
            originalDOF = depthOfField.focusDistance.value;
            ToggleBlur(false);
        }

        #endregion

        #region Public Methods

        public void ToggleBlur(bool isActivated)
        {
            float blurValue = isActivated ? 0.1f : originalDOF;
            float darkenAlpha = isActivated ? 0.5f : 0f;

            depthOfField.focusDistance.value = blurValue;
            var imageColor = darken.color;
            imageColor.a = darkenAlpha;
            darken.color = imageColor;
        }

        #endregion
    }
}
