using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class FireDissolver : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private bool startOff = true;

        [SerializeField] private ParticleSystem firePs;

        [SerializeField] private ParticleSystem embersPs;

        [SerializeField] private Light fireLight;

        #endregion

        #region Private Fields

        private float originalIntensity = 0f;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            originalIntensity = fireLight.intensity;
            if (startOff)
            {
                firePs.Stop();
                firePs.Clear();
                embersPs.Stop();
                embersPs.Clear();
                fireLight.intensity = 0f;
            }
        }

        #endregion

        #region Public Methods

        public void ToggleFire(bool toggle)
        {
            if (toggle)
            {
                firePs.Play();
                embersPs.Play();
                fireLight.intensity = originalIntensity;
            }
            else
            {
                firePs.Stop();
                firePs.Clear();
                embersPs.Stop();
                embersPs.Clear();
                fireLight.intensity = 0f;
            }
        }

        #endregion
    }
}
