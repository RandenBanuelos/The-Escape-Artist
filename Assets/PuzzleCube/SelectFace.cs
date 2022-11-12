using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of a Rubik's Cube in Unity
    // https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/
    public class SelectFace : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private LayerMask layerMask;

        [SerializeField] private Camera screenPointCamera;

        #endregion

        #region Private Fields

        private CubeState cubeState;

        private ReadCube readCube;

        private SolveTwoPhase solver;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            readCube = FindObjectOfType<ReadCube>();
            cubeState = FindObjectOfType<CubeState>();
            solver = FindObjectOfType<SolveTwoPhase>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating)
            {
                // read the current state of the cube            
                readCube.ReadState();

                // raycast from the mouse towards the cube to see if a face is hit  
                RaycastHit hit;
                Ray ray = screenPointCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
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
                            //Pick it up
                            cubeState.PickUp(cubeSide);

                            // Adjust user rotation speed
                            solver.ChangePivotSpeed(300f);

                            //start the side rotation logic
                            cubeSide[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
