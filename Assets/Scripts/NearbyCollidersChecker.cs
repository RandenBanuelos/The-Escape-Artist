using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class NearbyCollidersChecker : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private List<Collider> pastColliders = new List<Collider>();

        [SerializeField] private List<Collider> presentColliders = new List<Collider>();

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            // The object that entered the sphere is from the past
            if (pastColliders.Contains(other))
            {
                other.isTrigger = false;
            }

            // The object that entered the sphere is from the present
            else if (presentColliders.Contains(other))
            {
                other.isTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // The object that left the sphere is from the past
            if (pastColliders.Contains(other))
            {
                other.isTrigger = true;
            }

            // The object that left the sphere is from the present
            else if (presentColliders.Contains(other))
            {
                other.isTrigger = false;
            }
        }
    }
}
