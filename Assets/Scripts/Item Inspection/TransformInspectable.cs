using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TheEscapeArtist
{
    public class TransformInspectable : InteractableBase
    {
        [Serializable]
        public struct TransformInspectableSettings
        {
            public Transform itemToAdjust;

            public Vector3 positionAdjustment;

            public float positionTime;
            public Vector3 rotationAdjustment;

            public float rotationTime;
            public Vector3 scaleAdjustment;
            public float scaleTime;
        }

        #region Private Serializable Fields

        [SerializeField] private List<TransformInspectableSettings> objectsToAdjust = new List<TransformInspectableSettings>();

        #endregion

        #region Private Field

        private List<TransformInspectableSettings> originalAdjustments = new List<TransformInspectableSettings>();

        #endregion

        private void Start()
        {
            foreach (TransformInspectableSettings adjustment in objectsToAdjust)
            {
                TransformInspectableSettings newSettings = new TransformInspectableSettings();
                Transform transformCache = adjustment.itemToAdjust;

                newSettings.itemToAdjust = transformCache;

                newSettings.positionAdjustment = transformCache.localPosition;
                newSettings.positionTime = adjustment.positionTime;

                newSettings.rotationAdjustment = transformCache.localEulerAngles;
                newSettings.rotationTime = adjustment.rotationTime;

                newSettings.scaleAdjustment = transformCache.localScale;
                newSettings.scaleTime = adjustment.scaleTime;

                originalAdjustments.Add(newSettings);
            }
        }

        public override void OnInteract()
        {
            base.OnInteract();
            
            for (int i = 0; i < objectsToAdjust.Count; i++)
            {
                TransformInspectableSettings adjustment = objectsToAdjust[i];
                TransformInspectableSettings original = originalAdjustments[i];

                Transform adjustCache = adjustment.itemToAdjust;

                if (adjustCache.localPosition - adjustment.positionAdjustment != Vector3.zero) // Adjust in secondary position
                {
                    adjustment.itemToAdjust.DOLocalMove(adjustment.positionAdjustment, adjustment.positionTime);
                }
                else
                {
                    adjustment.itemToAdjust.DOLocalMove(original.positionAdjustment, adjustment.positionTime);
                }

                if (adjustCache.localEulerAngles - adjustment.rotationAdjustment != Vector3.zero) // Adjust in secondary rotation
                {
                    adjustment.itemToAdjust.DOLocalRotate(adjustment.rotationAdjustment, adjustment.rotationTime);
                }
                else
                {
                    adjustment.itemToAdjust.DOLocalRotate(original.rotationAdjustment, adjustment.rotationTime);
                }

                if (adjustCache.localScale - adjustment.scaleAdjustment != Vector3.zero) // Adjust in secondary scale
                {
                    adjustment.itemToAdjust.DOScale(adjustment.scaleAdjustment, adjustment.scaleTime);
                }
                else
                {
                    adjustment.itemToAdjust.DOScale(original.scaleAdjustment, adjustment.scaleTime);
                }
            }
        }
    }
}
