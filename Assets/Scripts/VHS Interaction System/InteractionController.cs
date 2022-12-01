using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class InteractionController : MonoBehaviour
    {
        #region Private Serializable Fields

            [Header("Data")]
            [SerializeField] private InteractionInputData interactionInputData;

            [SerializeField] private InteractionData interactionData;

            [Space]
            [Header("UI")]
            [SerializeField] private float highlightedOutlineWidth = 2f;

            [SerializeField] private Color highlightedColor;

            [SerializeField] private float defaultOutlineWidth = 4f;


            [Space]
            [Header("Ray Settings")]
            [SerializeField] private Camera cam;

            [SerializeField] private float rayDistance;

            [SerializeField] private float raySphereRadius;

            [SerializeField] private LayerMask interactableLayer;

        #endregion

        #region Private Fields

            private InteractionUIPanel uiPanel;

            private bool interacting;

            private float holdTimer = 0f;

            private Vector3 debugHitPoint = Vector3.zero;

            private ItemInspector inspectorCache;

        #endregion

        #region Singleton

            // Singleton pattern from user PearsonArtPhoto on StackExchange
            // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
            private static InteractionController _instance;

            public static InteractionController Instance { get { return _instance; } }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
            {
                if (_instance != null && _instance != this)
                    Destroy(this.gameObject);
                else
                    _instance = this;
            }

            private void Update()
            {
                if (inspectorCache == null)
                    inspectorCache = ItemInspector.Instance;

                if (!inspectorCache.IsInspecting && 
                    !PuzzleCubeManager.Instance.PuzzleCubeIsOpen && 
                    !PauseMenu.Instance.IsPaused && 
                    !NewspaperClippingManager.Instance.NewspaperIsActive)
                {
                    CheckForInteractable();
                    CheckForInteractableInput();
                }  
            }

        #endregion

        #region Public Methods

        public void CheckForInteractable()
        {
            if (uiPanel == null)
                uiPanel = InteractionUIPanel.Instance;

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            bool hitSomething = Physics.SphereCast(ray, raySphereRadius, out hit, rayDistance, interactableLayer);

            if (hitSomething)
            {
                InteractableBase interactable = hit.transform.GetComponent<InteractableBase>();

                if (interactable)
                {
                    if (interactionData.IsEmpty())
                    {
                        interactionData.Interactable = interactable;
                        uiPanel.SetTooltip(interactionData.Interactable.TooltipMessage);
                        interactionData.Interactable.InteractOutline.OutlineWidth = highlightedOutlineWidth;
                        interactionData.Interactable.InteractOutline.OutlineColor = highlightedColor;
                    }
                    else
                    {
                        if (!interactionData.IsSameInteractable(interactable))
                            interactionData.Interactable = interactable;
                            uiPanel.SetTooltip(interactionData.Interactable.TooltipMessage);
                            interactionData.Interactable.InteractOutline.OutlineWidth = highlightedOutlineWidth;
                            interactionData.Interactable.InteractOutline.OutlineColor = highlightedColor;
                    }
                }
            }
            else
            {
                if (!interactionData.IsEmpty())
                {
                    interactionData.Interactable.InteractOutline.OutlineWidth = defaultOutlineWidth;
                    interactionData.Interactable.InteractOutline.OutlineColor = Color.white;
                }

                uiPanel.ResetUI();
                interactionData.ResetData();
            }

            Debug.DrawRay(ray.origin, ray.direction * rayDistance, hitSomething ? Color.green : Color.red);
            debugHitPoint = ray.origin + (ray.direction * rayDistance);
        }

        public void CheckForInteractableInput()
        {
            if (interactionData.IsEmpty())
                return;

            if (interactionInputData.InteractClicked)
            {
                interacting = true;
                holdTimer = 0f;
            }

            if (interactionInputData.InteractReleased)
            {
                interacting = false;
                holdTimer = 0f;
                uiPanel.UpdateProgressBar(0f);
            }

            if (interacting)
            {
                if (!interactionData.Interactable.IsInteractable)
                {
                    uiPanel.SetTooltip(interactionData.Interactable.NotInteractableTooltipMessage);
                    return;
                }

                if (interactionData.Interactable.HoldInteract)
                {
                    holdTimer += Time.deltaTime;

                    float heldPercent = holdTimer / interactionData.Interactable.HoldDuration;
                    uiPanel.UpdateProgressBar(heldPercent);

                    if (heldPercent >= 1f)
                    {
                        interacting = false;
                        uiPanel.UpdateProgressBar(0f);
                        interactionData.Interact();
                    }
                }
                else
                {
                    interactionData.Interact();
                    interacting = false;
                }
            }
        }

        public void ResetOutline()
        {
            if (!interactionData.IsEmpty())
                interactionData.Interactable.InteractOutline.OutlineWidth = 0f;
        }

        #endregion

        private void OnDrawGizmos()
        {
            if (debugHitPoint != Vector3.zero)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(debugHitPoint, raySphereRadius);
            }
        }
    }
}
