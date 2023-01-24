using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class RunawayDoor : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private Transform movingDoor;  

        [SerializeField] private Transform playerTransform;

        [SerializeField] private float lerpSpeed = 1f;

        [SerializeField] private float adjustmentDistance = 2f;

        #endregion

        #region Private Fields

        private float timer = 0f;

        private bool isLerpingToNewX = false;

        private float originalX;

        private float xToLerpTo;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            timer = 0f;
            isLerpingToNewX = false;
        }

        private void FixedUpdate()
        {
            if (isLerpingToNewX)
            {
                if (timer < lerpSpeed)
                {
                    float newX = Mathf.Lerp(originalX, xToLerpTo, timer / lerpSpeed);
                    movingDoor.position = new Vector3(newX, movingDoor.position.y, movingDoor.position.z);
                    timer += Time.deltaTime;
                }
                else
                {
                    movingDoor.position = new Vector3(xToLerpTo, movingDoor.position.y, movingDoor.position.z);
                    timer = 0f;
                    isLerpingToNewX = false;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isLerpingToNewX && other.tag == "Player")
            {
                isLerpingToNewX = true;
                originalX = movingDoor.position.x;
                xToLerpTo = originalX + adjustmentDistance;
            }
        }

        #endregion
    }
}
