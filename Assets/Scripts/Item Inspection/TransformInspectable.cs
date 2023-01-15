using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZCameraShake;

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

        [SerializeField] private AudioSource sfxSource;

        [SerializeField] private float pressDelay = 7f;

        #endregion

        #region Private Fields

        private List<TransformInspectableSettings> originalAdjustments = new List<TransformInspectableSettings>();

        private bool isBeingPressed = false;

        private Animator buttonAnim;

        private Outline outline;

        private bool alreadyPlayed = false;

        private bool flipAnim = false;

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

            // buttonAnim = GetComponentInParent<Animator>();

            outline = GetComponent<Outline>();

            isBeingPressed = false;

            flipAnim = false;
        }

        public override void OnInteract()
        {
            if (!isBeingPressed)
            {
                base.OnInteract();

                isBeingPressed = true;
                gameObject.layer = LayerMask.NameToLayer("Default");
                outline.OutlineWidth = 0f;

                // if (buttonAnim)
                //    buttonAnim.SetTrigger("PressButton");

                if (sfxSource)
                    sfxSource.Play();

                if (!alreadyPlayed && VoiceClip && VoiceActingManager.Instance)
                {
                    alreadyPlayed = true;
                    VoiceActingManager.Instance.Say(VoiceClip);
                }

                if (ShakeCamera)
                {
                    CameraShaker.Instance.ShakeOnce(ShakeMagnitude, 4f, .1f, ShakeDuration);
                }

                /*if (buttonAnim)
                {
                    Invoke("ClearTrigger('PressButton')", 1f);
                }*/

                if (pressDelay > 0)
                {
                    Invoke(nameof(ResetInteractable), pressDelay);
                }

                for (int i = 0; i < objectsToAdjust.Count; i++)
                {
                    TransformInspectableSettings adjustment = objectsToAdjust[i];
                    TransformInspectableSettings original = originalAdjustments[i];

                    if (!flipAnim) // Adjust to secondary position
                    {
                        adjustment.itemToAdjust.DOLocalMove(adjustment.positionAdjustment, adjustment.positionTime);
                        adjustment.itemToAdjust.DOLocalRotate(adjustment.rotationAdjustment, adjustment.rotationTime);
                        adjustment.itemToAdjust.DOScale(adjustment.scaleAdjustment, adjustment.scaleTime);
                    }
                    else
                    {
                        adjustment.itemToAdjust.DOLocalMove(original.positionAdjustment, adjustment.positionTime);
                        adjustment.itemToAdjust.DOLocalRotate(original.rotationAdjustment, adjustment.rotationTime);
                        adjustment.itemToAdjust.DOScale(original.scaleAdjustment, adjustment.scaleTime);
                    }
                }

                flipAnim = !flipAnim;
            }
        }

        private void ClearTrigger(string triggerName)
        {
            buttonAnim.ResetTrigger(triggerName);
        }

        private void ResetInteractable()
        {
            // buttonAnim.SetTrigger("UnpressButton");

            gameObject.layer = LayerMask.NameToLayer("Interactable");

            if (sfxSource)
                sfxSource.Stop();

            isBeingPressed = false;

            // Invoke("ClearTrigger('UnpressButton')", 1f);
        }
    }
}
