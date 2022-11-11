using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of a Rubik's Cube in Unity
    // https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/
    public class CubeMap : MonoBehaviour
    {
        #region Public Fields

        public Transform up;

        public Transform down;

        public Transform left;

        public Transform right;

        public Transform front;

        public Transform back;

        #endregion

        #region Private Fields

        private CubeState cubeState;

        #endregion

        #region Public Methods

        public void Set()
        {
            cubeState = FindObjectOfType<CubeState>();

            UpdateMap(cubeState.front, front);
            UpdateMap(cubeState.back, back);
            UpdateMap(cubeState.left, left);
            UpdateMap(cubeState.right, right);
            UpdateMap(cubeState.up, up);
            UpdateMap(cubeState.down, down);
        }

        #endregion

        #region Private Methods

        private void UpdateMap(List<GameObject> face, Transform side)
        {
            int i = 0;
            foreach (Transform map in side)
            {
                if (face[i].name[0] == 'F')
                {
                    map.GetComponent<Image>().color = new Color(1, 0.5f, 0, 1);
                }
                if (face[i].name[0] == 'B')
                {
                    map.GetComponent<Image>().color = Color.red;
                }
                if (face[i].name[0] == 'U')
                {
                    map.GetComponent<Image>().color = Color.yellow;
                }
                if (face[i].name[0] == 'D')
                {
                    map.GetComponent<Image>().color = Color.white;
                }
                if (face[i].name[0] == 'L')
                {
                    map.GetComponent<Image>().color = Color.green;
                }
                if (face[i].name[0] == 'R')
                {
                    map.GetComponent<Image>().color = Color.blue;
                }
                i++;
            }
        }

        #endregion
    }
}
