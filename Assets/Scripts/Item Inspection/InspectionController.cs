using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheEscapeArtist
{
    public class InspectionController : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private Camera inspectionCamera;

        [SerializeField] private InteractionData interactionData;

        [SerializeField] private LayerMask inspectionMask;

        [SerializeField] private RectTransform inspectPromptUI;

        [SerializeField] private RectTransform mainCanvas;

        [SerializeField] private Image cursorDetection;

        #endregion

        #region Private Fields

        private Vector2 canvasDimensions;

        private bool inspecting;

        #endregion

        private void Start()
        {
            canvasDimensions = new Vector2(mainCanvas.rect.width, mainCanvas.rect.height);
        }

        private void Update()
        {
            CheckForInspectable();
            CheckForInspectableInput();
        }

        private void CheckForInspectable()
        {
            Vector3 rawMouseInput = Input.mousePosition;
            Vector3 mousePos = new Vector3((rawMouseInput.x / Screen.width) * 1920, 
                                            (rawMouseInput.y / Screen.height) * 1080, 0f);

            cursorDetection.transform.position = rawMouseInput;

            Ray ray = inspectionCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            bool hitSomething = Physics.Raycast(ray, out hit, 100f, inspectionMask);
            if (hitSomething)
            {
                cursorDetection.color = Color.green;
                inspectPromptUI.gameObject.SetActive(true);
                inspectPromptUI.localPosition = new Vector3(-canvasDimensions.x + mousePos.x, -canvasDimensions.y + mousePos.y, 0f);

                InteractableBase interactable = hit.transform.GetComponent<InteractableBase>();

                if (interactable)
                {
                    if (interactionData.IsEmpty())
                    {
                        interactionData.Interactable = interactable;
                    }
                    else
                    {
                        if (!interactionData.IsSameInteractable(interactable))
                            interactionData.Interactable = interactable;
                    }
                }
            }
            else
            {
                cursorDetection.color = Color.red;
                inspectPromptUI.gameObject.SetActive(false);
                interactionData.ResetData();
            }

            Debug.DrawRay(ray.origin, ray.direction * 100f, hitSomething ? Color.green : Color.red);
        }

        public void CheckForInspectableInput()
        {
            if (interactionData.IsEmpty())
                return;

            if (Input.GetMouseButtonDown(0))
            {
                interactionData.Interact();
            }
        }
    }
}
