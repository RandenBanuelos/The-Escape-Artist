using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TheEscapeArtist
{
    public class SequencedTransformInspectable : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private float forwardOffset = 0f;

        [SerializeField] private Vector3 constantOffsetPosition = Vector3.zero;

        [SerializeField] private Vector3 constantOffsetRotation = Vector3.zero;

        [SerializeField] private float timeToAdjust = 1f;

        [SerializeField] private List<Transform> objectsToAdjust = new List<Transform>();

        #endregion

        #region Private Field

        private bool adjust = true;

        private bool isAdjusting = false;

        private Transform currentAdjustObject;

        private Vector3 currentOriginalPosition = Vector3.zero;

        private Vector3 currentOriginalRotation = Vector3.zero;

        private Vector3 currentAdjustedPosition = Vector3.zero;

        private Vector3 currentAdjustedRotation = Vector3.zero;

        private float timer;

        #endregion

        private void Start()
        {
            adjust = true;
        }

        private void Update()
        {
            if (currentAdjustObject)
            {
                if (forwardOffset > 0f)
                {
                    if (adjust)
                    {
                        currentAdjustObject.DOMove(currentAdjustedPosition, timeToAdjust);
                    }
                    else
                    {
                        currentAdjustObject.DOMove(currentOriginalPosition, timeToAdjust);
                    }
                }

                if (constantOffsetRotation != Vector3.zero)
                {
                    if (adjust)
                    {
                        currentAdjustObject.DOLocalRotate(currentAdjustedRotation, timeToAdjust);
                    }
                    else
                    {
                        currentAdjustObject.DOLocalRotate(currentOriginalRotation, timeToAdjust);
                    }
                }

                timer += Time.deltaTime;

                if (timer > timeToAdjust)
                {
                    timer = 0f;
                    currentAdjustObject = null;
                }
            }
        }

        public override void OnInteract()
        {
            if (!isAdjusting)
            {
                base.OnInteract();
                StartCoroutine(nameof(AdjustObjects));
            }
        }

        private IEnumerator AdjustObjects()
        {
            isAdjusting = true;

            foreach (Transform adjustObject in objectsToAdjust)
            {
                currentAdjustObject = adjustObject;

                currentOriginalPosition = adjustObject.position;
                currentOriginalRotation = adjustObject.localEulerAngles;

                currentAdjustedPosition = adjustObject.position + (-adjustObject.right * forwardOffset);
                currentAdjustedRotation = currentOriginalRotation + constantOffsetRotation;

                yield return new WaitUntil(() => currentAdjustObject == null);
            }

            adjust = !adjust;
            isAdjusting = false;
        }

        private void OnDrawGizmos()
        {
            foreach (Transform obj in objectsToAdjust)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(obj.position, 0.1f);

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(obj.position + (-obj.right * forwardOffset), 0.1f);
            }
        }
    }
}
