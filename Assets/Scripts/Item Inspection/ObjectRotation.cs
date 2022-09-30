using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheEscapeArtist
{
    // Most all of this code is derived from user Mattias-Wargren's
    // implementation of 3-D object rotation based on mouse drag
    // https://answers.unity.com/questions/177391/drag-to-rotate-gameobject.html
    public class ObjectRotation : MonoBehaviour
    {
        [SerializeField] private float zoomMin = 2f;

        [SerializeField] private float zoomMax = 2f;

        private Transform itemObject = null;
        private float _sensitivity;
        private Vector3 _mouseReference;
        private Vector3 _mouseOffset;
        private Vector3 _rotation;
        private bool _isRotating;
        private float originalDistanceFromCamera = 0f;

        void Start()
        {
            _sensitivity = 0.4f;
            _rotation = Vector3.zero;
        }

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            if (itemObject)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    // rotating flag
                    _isRotating = true;

                    // store mouse
                    _mouseReference = Input.mousePosition; 
                }

                if (Input.GetMouseButtonUp(1))
                {
                    // rotating flag
                    _isRotating = false;
                }

                if (_isRotating)
                {
                    // offset
                    _mouseOffset = (Input.mousePosition - _mouseReference);

                    // apply rotation
                    _rotation.y = -(_mouseOffset.x) * _sensitivity;
                    _rotation.x = _mouseOffset.y * _sensitivity;

                    // rotate
                    itemObject.Rotate(_rotation, Space.World);


                    // store mouse
                    _mouseReference = Input.mousePosition;
                }

                float zoom = Input.mouseScrollDelta.y;

                if (zoom < 0)
                {
                    if (!(itemObject.position.z + zoom < originalDistanceFromCamera - zoomMax))
                    {
                        itemObject.Translate(0, 0, zoom, Space.World);
                    }
                } else if (zoom > 0)
                {
                    if (!(itemObject.position.z + zoom > originalDistanceFromCamera + zoomMin))
                    {
                        itemObject.Translate(0, 0, zoom, Space.World);
                    }
                }
            }
        }

        public void SetObject(Transform newObject)
        {
            itemObject = newObject;
            originalDistanceFromCamera = itemObject.position.z;
        }

        public void ResetObject()
        {
            itemObject = null;
        }
    }
}
