using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of an interactable Rubik's Cube
    // https://www.youtube.com/playlist?list=PLuq_iMEtykn-ZOJyx2cY_k9WkixAhv11n
    public class RotateBigCube : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject target;

        [SerializeField] private float rotateSpeed;

        [SerializeField] private float rotationReduction;

        #endregion

        #region Private Fields

        private Vector2 firstPressPosition;

        private Vector2 secondPressPosition;

        private Vector2 currentSwipe;

        private Vector3 previousMousePosition;

        private Vector3 mouseDelta;

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            Swipe();
            Drag();
        }

        #endregion

        #region "Getter's" & "Setter's"

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private void Drag()
        {
            if (Input.GetMouseButton(1))
            {
                // While the mouse is held down, the cube can be moved around its central
                // axis to provide visual feedback
                mouseDelta = Input.mousePosition - previousMousePosition;
                mouseDelta *= rotationReduction; // Reduction of rotation speed
                transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
            }
            else
            {
                // Automatically move to the target position
                if (transform.rotation != target.transform.rotation)
                {
                    var step = rotateSpeed * Time.deltaTime;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
                }
            }

            previousMousePosition = Input.mousePosition;
        }

        private void Swipe()
        {
            if (Input.GetMouseButtonDown(1))
            {
                // Get the 2D position of the first mouse click
                firstPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            if (Input.GetMouseButtonUp(1))
            {
                // Get the 2D position of the second mouse click
                secondPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                // Create a vector from the first and second click positions
                currentSwipe = new Vector2(secondPressPosition.x - firstPressPosition.x, secondPressPosition.y - firstPressPosition.y);

                // Normalize the 2D vector
                currentSwipe.Normalize();

                if (LeftSwipe())
                {
                    target.transform.Rotate(0, 90, 0, Space.World);
                }
                else if (RightSwipe())
                {
                    target.transform.Rotate(0, -90, 0, Space.World);
                }
                else if (UpLeftSwipe())
                {
                    target.transform.Rotate(90, 0, 0, Space.World);
                }
                else if (UpRightSwipe())
                {
                    target.transform.Rotate(0, 0, -90, Space.World);
                }
                else if (DownLeftSwipe())
                {
                    target.transform.Rotate(0, 0, 90, Space.World);
                }
                else if (DownRightSwipe())
                {
                    target.transform.Rotate(-90, 0, 0, Space.World);
                }
            }
        }

        private bool LeftSwipe()
        {
            return currentSwipe.x < 0f && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
        }

        private bool RightSwipe()
        {
            return currentSwipe.x > 0f && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
        }

        private bool UpLeftSwipe()
        {
            return currentSwipe.y > 0f && currentSwipe.x < 0f;
        }

        private bool UpRightSwipe()
        {
            return currentSwipe.y > 0f && currentSwipe.x > 0f;
        }

        private bool DownLeftSwipe()
        {
            return currentSwipe.y < 0f && currentSwipe.x < 0f;
        }

        private bool DownRightSwipe()
        {
            return currentSwipe.y < 0f && currentSwipe.x > 0f;
        }

        #endregion
    }
}
