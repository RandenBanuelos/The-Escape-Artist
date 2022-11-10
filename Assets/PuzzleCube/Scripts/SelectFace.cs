using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class SelectFace : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private LayerMask facesLayerMask;

        #endregion

        #region Private Fields

        private CubeState cubeState;

        private ReadCube readCube;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            readCube = FindObjectOfType<ReadCube>();
            cubeState = FindObjectOfType<CubeState>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Read the current state of the cube
                readCube.ReadState();

                // Raycast from the mouse towards the cube to see if a face is hit
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100f, facesLayerMask))
                {
                    GameObject face = hit.collider.gameObject;

                    // Make a list of all the sides (lists of face GameObjects)
                    List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                    {
                        cubeState.up,
                        cubeState.down,
                        cubeState.left,
                        cubeState.right,
                        cubeState.front,
                        cubeState.back
                    };

                    // If the face hit exists within a side
                    foreach (List<GameObject> cubeSide in cubeSides)
                    {
                        if (cubeSide.Contains(face))
                        {
                            // Pick it up
                            cubeState.PickUp(cubeSide);
                        }
                    }
                }
            }
        }

        #endregion

        #region "Getter's" & "Setter's"

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
