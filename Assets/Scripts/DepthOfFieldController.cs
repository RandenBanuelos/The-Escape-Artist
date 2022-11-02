using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace TheEscapeArtist
{
    // Based on Game Dev Guide's implementation of a dynamic depth of field controller
    // https://www.youtube.com/watch?v=7od2j4s85ww
    public class DepthOfFieldController : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private float maxFocusDistance = 5f;

        [SerializeField] private float focusSpeed = 8f;

        [SerializeField] private PostProcessVolume volume;

        [SerializeField] private LayerMask ignoreMask;

        #endregion

        #region Private Fields

        private Ray raycast;

        private RaycastHit hit;

        private bool isHit;

        private float hitDistance;

        private DepthOfField depthOfField;

        private Transform lastHitObject;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            volume.profile.TryGetSettings(out depthOfField);
        }

        private void FixedUpdate()
        {
            raycast = new Ray(transform.position, transform.forward * maxFocusDistance);
            isHit = false;

            if (Physics.Raycast(raycast, out hit, maxFocusDistance, ~ignoreMask))
            {
                if (!lastHitObject || lastHitObject != hit.transform)
                {
                    isHit = true;
                    lastHitObject = hit.transform;
                    hitDistance = Vector3.Distance(transform.position, hit.point);
                    Debug.Log($"DepthOfFieldController: Hit {lastHitObject.name}!");
                }
            }
            else
            {
                if (hitDistance < maxFocusDistance)
                {
                    hitDistance++;
                }
            }

            SetFocus();
        }

        private void OnDrawGizmos()
        {
            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(hit.point, 0.1f);
                Debug.DrawRay(transform.position, transform.forward * hitDistance);
            }
            else
            {
                Gizmos.color = Color.red;
                Debug.DrawRay(transform.position, transform.forward * 100f);
            }
        }

        #endregion

        #region "Getter's" & "Setter's"

        #endregion

        #region Public Fields

        #endregion

        #region Private Fields

        private void SetFocus()
        {
            float newFocusValue = Mathf.Lerp(depthOfField.focusDistance.value,
                                                          hitDistance,
                                                          Time.deltaTime * focusSpeed);

            if (depthOfField.focusDistance.value != newFocusValue)
                depthOfField.focusDistance.value = newFocusValue;
        }

        #endregion
    }
}
