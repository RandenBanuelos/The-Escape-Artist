using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of a Rubik's Cube in Unity
    // https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/
    public class CubeState : MonoBehaviour
    {
        #region Public Static Fields

        public static bool autoRotating = false;
        public static bool started = false;

        #endregion

        #region Public Fields

        // Sides
        public List<GameObject> front = new List<GameObject>();
        public List<GameObject> back = new List<GameObject>();
        public List<GameObject> up = new List<GameObject>();
        public List<GameObject> down = new List<GameObject>();
        public List<GameObject> left = new List<GameObject>();
        public List<GameObject> right = new List<GameObject>();

        #endregion

        #region Public Methods

        public void PickUp(List<GameObject> cubeSide)
        {
            foreach (GameObject face in cubeSide)
            {
                // Attach the parent of each face (the little cube)
                // to the parent of the 4th index (the little cube in the middle) 
                // Unless it is already the 4th index
                if (face != cubeSide[4])
                {
                    face.transform.parent.transform.parent = cubeSide[4].transform.parent;
                }
            }
        }

        public void PutDown(List<GameObject> littleCubes, Transform pivot)
        {
            foreach (GameObject littleCube in littleCubes)
            {
                if (littleCube != littleCubes[4])
                {
                    littleCube.transform.parent.transform.parent = pivot;
                }
            }
        }

        public string GetStateString()
        {
            string stateString = "";
            stateString += GetSideString(up);
            stateString += GetSideString(right);
            stateString += GetSideString(front);
            stateString += GetSideString(down);
            stateString += GetSideString(left);
            stateString += GetSideString(back);
            return stateString;
        }

        #endregion

        #region Private Methods

        private string GetSideString(List<GameObject> side)
        {
            string sideString = "";
            foreach (GameObject face in side)
            {
                sideString += face.name[0].ToString();
            }
            return sideString;
        }

        #endregion
    }
}
