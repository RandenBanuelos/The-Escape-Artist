﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of a Rubik's Cube in Unity
    // https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/
    public class PivotRotation : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private float sensitivity = 0.4f;

        public float speed = 300f;

        #endregion

        #region Private Fields

        private List<GameObject> activeSide;
        private Vector3 localForward;
        private Vector3 mouseRef;
        private bool dragging = false;

        private bool autoRotating = false;
        private bool instantRotating = false;
        private Vector3 rotation;

        private Quaternion targetQuaternion;

        private ReadCube readCube;
        private CubeState cubeState;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            readCube = FindObjectOfType<ReadCube>();
            cubeState = FindObjectOfType<CubeState>();
        }

        private void LateUpdate()
        {
            if (dragging && !autoRotating && !instantRotating)
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
                if (instantRotating)
                    InstantRotate();
                else
                    AutoRotate();
            }

        }

        #endregion

        #region Public Methods

        public void Rotate(List<GameObject> side)
        {
            activeSide = side;
            mouseRef = Input.mousePosition;
            dragging = true;
            // Create a vector to rotate around
            localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        }

        public void StartAutoRotate(List<GameObject> side, float angle, bool instantRotate)
        {
            cubeState.PickUp(side);
            Vector3 localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
            targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
            activeSide = side;
            autoRotating = true;
            instantRotating = instantRotate;
        }

        public void RotateToRightAngle()
        {
            Vector3 vec = transform.localEulerAngles;
            // round vec to nearest 90 degrees
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
            // reset the rotation
            rotation = Vector3.zero;

            // current mouse position minus the last mouse position
            Vector3 mouseOffset = (Input.mousePosition - mouseRef);


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
            // rotate
            transform.Rotate(rotation, Space.Self);

            // store mouse
            mouseRef = Input.mousePosition;
        }

        private void AutoRotate()
        {
            dragging = false;
            var step = speed * Time.deltaTime;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

            // if within one degree, set angle to target angle and end the rotation
            if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
            {
                InstantRotate();
            }
        }

        private void InstantRotate()
        {
            transform.localRotation = targetQuaternion;
            // unparent the little cubes
            cubeState.PutDown(activeSide, transform.parent);
            readCube.ReadState();
            CubeState.autoRotating = false;
            autoRotating = false;
            instantRotating = false;
            dragging = false;
        }

        #endregion
    }
}
