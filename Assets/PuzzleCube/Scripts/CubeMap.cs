using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of an interactable Rubik's Cube
    // https://www.youtube.com/playlist?list=PLuq_iMEtykn-ZOJyx2cY_k9WkixAhv11n
    public class CubeMap : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private Transform up;

        [SerializeField] private Transform down;

        [SerializeField] private Transform left;

        [SerializeField] private Transform right;

        [SerializeField] private Transform front;

        [SerializeField] private Transform back;

        #endregion

        #region Private Fields

        private CubeState cubeState;

        #endregion

        #region MonoBehaviour Callbacks

        #endregion

        #region "Getter's" & "Setter's"

        #endregion

        #region Public Methods

        public void Set()
        {
            cubeState = FindObjectOfType<CubeState>();

            UpdateMap(cubeState.up, up);
            UpdateMap(cubeState.down, down);
            UpdateMap(cubeState.left, left);
            UpdateMap(cubeState.right, right);
            UpdateMap(cubeState.front, front);
            UpdateMap(cubeState.back, back);
        }

        #endregion

        #region Private Methods

        private void UpdateMap(List<GameObject> face, Transform side)
        {
            int i = 0;
            foreach (Transform map in side)
            {
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

                if (face[i].name[0] == 'F')
                {
                    map.GetComponent<Image>().color = new Color(1f, 0.5f, 0f, 1f);
                }

                if (face[i].name[0] == 'B')
                {
                    map.GetComponent<Image>().color = Color.red;
                }

                i++;
            }
        }

        #endregion
    }
}
