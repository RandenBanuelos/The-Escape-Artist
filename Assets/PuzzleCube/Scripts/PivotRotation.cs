using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class PivotRotation : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private float sensitivity;

        [SerializeField] private float speed = 300f;

        #endregion

        #region Private Fields

        private List<GameObject> activeSide;

        private ReadCube readCube;

        private CubeState cubeState;

        private Quaternion targetQuaternion;

        private Vector3 localForward;

        private Vector3 mouseReference;

        private Vector3 rotation;

        private bool dragging = false;

        private bool autoRotating = false;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            readCube = FindObjectOfType<ReadCube>();
            cubeState = FindObjectOfType<CubeState>();
        }

        private void Update()
        {
            if (dragging)
            {
                SpinSide(activeSide);
                if (Input.GetMouseButtonUp(0))
                {
                    dragging = false;
                    RotateToRightAngle();
                }
            }

            if (autoRotating)
            {
                AutoRotate();
            }
        }

        #endregion

        #region "Getter's" & "Setter's"

        #endregion

        #region Public Methods

        public void Rotate(List<GameObject> side)
        {
            activeSide = side;
            mouseReference = Input.mousePosition;
            dragging = true;

            // Create a vector to rotate around
            localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        }

        public void RotateToRightAngle()
        {
            Vector3 vec = transform.localEulerAngles;

            // Round vec to nearest 90 degrees
            vec.x = Mathf.Round(vec.x / 90) * 90;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = Mathf.Round(vec.z / 90) * 90;

            targetQuaternion.eulerAngles = vec;
            autoRotating = true;
        }

        #endregion

        #region Private Methods

        private void SpinSide(List<GameObject> side)
        {
            // Reset the rotation
            rotation = Vector3.zero;

            // Current mouse position minus the last mouse position
            Vector3 mouseOffset = (Input.mousePosition - mouseReference);

            if (side == cubeState.up)
            {
                rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
            }

            if (side == cubeState.down)
            {
                rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
            }

            if (side == cubeState.left)
            {
                rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
            }

            if (side == cubeState.right)
            {
                rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
            }

            if (side == cubeState.front)
            {
                rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
            }

            if (side == cubeState.back)
            {
                rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
            }

            // Rotate
            transform.Rotate(rotation, Space.Self);

            // Store mouse
            mouseReference = Input.mousePosition;
        }

        private void AutoRotate()
        {
            dragging = false;
            var step = speed * Time.deltaTime;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

            // If within one degree, set angle to target angle and end the rotation
            if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
            {
                transform.localRotation = targetQuaternion;

                // Unparent the little cubes
                cubeState.PutDown(activeSide, transform.parent);
                readCube.ReadState();
                autoRotating = false;
                dragging = false;
            }
        }

        #endregion
    }
}
