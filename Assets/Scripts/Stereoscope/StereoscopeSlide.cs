using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class StereoscopeSlide : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private string slideDescription = "Example Slide";

        [SerializeField] private GameObject scenery;

        #endregion

        #region Private Fields

        private Transform spawnPosition;

        private Vector3 cameraForward;

        private RenderTexture cameraTexture;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            Camera renderCamera = GetComponent<Camera>();

            if (renderCamera.targetTexture != null)
            {
                renderCamera.targetTexture.Release();
            }

            renderCamera.targetTexture = new RenderTexture(800, 800, 24);
            cameraTexture = renderCamera.targetTexture;

            spawnPosition = renderCamera.transform;
            cameraForward = renderCamera.transform.forward;
        }

        #endregion

        #region "Getter's"

        public string SlideDescription => slideDescription;

        public Transform SpawnPosition => spawnPosition;

        public Vector3 CameraForward => cameraForward;

        public RenderTexture CameraTexture => cameraTexture;

        public GameObject Scenery => scenery;

        #endregion
    }
}
