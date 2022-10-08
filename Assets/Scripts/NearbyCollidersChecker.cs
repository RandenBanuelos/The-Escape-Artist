using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class NearbyCollidersChecker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // The object that entered the sphere is from the past
            if (other.gameObject.tag == "Past")
            {
                Debug.Log($"NearbyCollidersChecker.OnTriggerEnter: Enabling object from past - {other.gameObject.name}");
                other.isTrigger = false;
            }

            // The object that entered the sphere is from the present
            else if (other.gameObject.tag == "Present")
            {
                Debug.Log($"NearbyCollidersChecker.OnTriggerEnter: Disabling object from present - {other.gameObject.name}");
                other.isTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // The object that left the sphere is from the past
            if (other.gameObject.tag == "Past")
            {
                Debug.Log($"NearbyCollidersChecker.OnTriggerExit: Disabling object from past - {other.gameObject.name}");
                other.isTrigger = true;
            }

            // The object that left the sphere is from the present
            else if (other.gameObject.tag == "Present")
            {
                Debug.Log($"NearbyCollidersChecker.OnTriggerExit: Enabling object from present - {other.gameObject.name}");
                other.isTrigger = false;
            }
        }
    }
}
