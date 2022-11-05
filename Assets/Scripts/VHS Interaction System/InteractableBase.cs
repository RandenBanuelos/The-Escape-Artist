using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{

    public class InteractableBase : MonoBehaviour, IInteractable
    {
        #region Private Serializable Fields

            [Header("Interactable Settings")]
            [SerializeField] private Transform interactablePrefab = null;

            [SerializeField] private string tooltipMessage = "Interact";

            [SerializeField] private string notInteractableTooltipMessage = "Cannot Interact";

            [SerializeField] private float holdDuration = 0f;

            [Space]
            [SerializeField] private bool holdInteract = false;

            [SerializeField] private bool multipleUse = true;

            [SerializeField] private bool isInteractable = true;

            [SerializeField] private bool shakeCamera = false;

            [SerializeField] private float shakeDuration = 1f;

            [SerializeField] private float shakeMagnitude = 4f;

            [SerializeField] private VoiceClip voiceClip;

        #endregion

        #region Private Fields

            private Outline interactOutline;

        #endregion


        #region "Getter's"
            public Transform InteractablePrefab => interactablePrefab;

            public string TooltipMessage => tooltipMessage;

            public string NotInteractableTooltipMessage => notInteractableTooltipMessage;

            public float HoldDuration => holdDuration;

            public bool HoldInteract => holdInteract;

            public bool MultipleUse => multipleUse;

            public bool IsInteractable => isInteractable;

            public bool ShakeCamera => shakeCamera;

            public float ShakeDuration => shakeDuration;

            public float ShakeMagnitude => shakeMagnitude;

            public Outline InteractOutline => interactOutline;

            public VoiceClip VoiceClip => voiceClip;

        #endregion

        #region Public Methods

            private void Awake()
            {
                interactOutline = GetComponent<Outline>();
            }

            public virtual void OnInteract()
            {
                Debug.Log($"InteractableBase.OnInteract(): Interacted with {gameObject.name}!");
        }

            public void ToggleInteractable(bool toggle)
            {
                isInteractable = toggle;
            }

        #endregion
    }
}
