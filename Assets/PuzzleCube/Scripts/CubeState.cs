using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of an interactable Rubik's Cube
    // https://www.youtube.com/playlist?list=PLuq_iMEtykn-ZOJyx2cY_k9WkixAhv11n
    public class CubeState : MonoBehaviour
    {
        #region Public Fields

        [Header("Sides")]
        public List<GameObject> up = new List<GameObject>();

        public List<GameObject> down = new List<GameObject>();

        public List<GameObject> left = new List<GameObject>();

        public List<GameObject> right = new List<GameObject>();

        public List<GameObject> front = new List<GameObject>();

        public List<GameObject> back = new List<GameObject>();

        #endregion

        #region Private Fields

        #endregion

        #region MonoBehaviour Callbacks

        #endregion

        #region "Getter's" & "Setter's"

        #endregion

        #region Public Methods

        public void PickUp(List<GameObject> cubeSide)
        {
            foreach (GameObject face in cubeSide)
            {
                // Attach the parent of each face (the little cube)
                // to the parent of the 4th index (the little cube in the middle),
                // unless it is already the 4th index
                if (face != cubeSide[4])
                {
                    face.transform.parent.transform.parent = cubeSide[4].transform.parent;
                }
            }

            // Start the side rotation logic
            cubeSide[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);
        }

        public void PutDown(List<GameObject> littleCubes, Transform pivot)
        {
            foreach(GameObject littleCube in littleCubes)
            {
                if (littleCube != littleCubes[4])
                {
                    littleCube.transform.parent.transform.parent = pivot;
                }
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
