using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of a Rubik's Cube in Unity
    // https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/
    public class ReadCube : MonoBehaviour
    {
        #region Public Fields

        public GameObject emptyGO;

        public LayerMask layerMask; // This layerMask is for the faces of the cube only

        public Transform tUp;
        public Transform tDown;
        public Transform tLeft;
        public Transform tRight;
        public Transform tFront;
        public Transform tBack;

        #endregion

        #region Private Fields

        private List<GameObject> frontRays = new List<GameObject>();
        private List<GameObject> backRays = new List<GameObject>();
        private List<GameObject> upRays = new List<GameObject>();
        private List<GameObject> downRays = new List<GameObject>();
        private List<GameObject> leftRays = new List<GameObject>();
        private List<GameObject> rightRays = new List<GameObject>();

        private CubeState cubeState;
        private CubeMap cubeMap;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            SetRayTransforms();

            cubeState = FindObjectOfType<CubeState>();
            cubeMap = FindObjectOfType<CubeMap>();
            ReadState();
            CubeState.started = true;
        }

        #endregion

        #region Public Methods

        public void ReadState()
        {
            cubeState = FindObjectOfType<CubeState>();
            cubeMap = FindObjectOfType<CubeMap>();

            // set the state of each position in the list of sides so we know
            // what color is in what position
            cubeState.up = ReadFace(upRays, tUp);
            cubeState.down = ReadFace(downRays, tDown);
            cubeState.left = ReadFace(leftRays, tLeft);
            cubeState.right = ReadFace(rightRays, tRight);
            cubeState.front = ReadFace(frontRays, tFront);
            cubeState.back = ReadFace(backRays, tBack);

            // update the map with the found positions
            cubeMap.Set();

        }

        public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
        {
            List<GameObject> facesHit = new List<GameObject>();

            foreach (GameObject rayStart in rayStarts)
            {
                Vector3 ray = rayStart.transform.position;
                RaycastHit hit;

                // Does the ray intersect any objects in the layerMask?
                if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                    facesHit.Add(hit.collider.gameObject);
                    //print(hit.collider.gameObject.name);
                }
                else
                {
                    Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
                }
            }
            return facesHit;
        }

        #endregion

        #region Private Methods

        private void SetRayTransforms()
        {
            // populate the ray lists with raycasts eminating from the transform, angled towards the cube.
            upRays = BuildRays(tUp, new Vector3(90, 90, 0));
            downRays = BuildRays(tDown, new Vector3(270, 90, 0));
            leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
            rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
            frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
            backRays = BuildRays(tBack, new Vector3(0, 270, 0));
        }

        private List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
        {
            // The ray count is used to name the rays so we can be sure they are in the right order.
            int rayCount = 0;
            List<GameObject> rays = new List<GameObject>();
            // This creates 9 rays in the shape of the side of the cube with
            // Ray 0 at the top left and Ray 8 at the bottom right:
            //  |0|1|2|
            //  |3|4|5|
            //  |6|7|8|

            for (int y = 1; y > -2; y--)
            {
                for (int x = -1; x < 2; x++)
                {
                    Vector3 startPos = new Vector3(rayTransform.localPosition.x + x,
                                                    rayTransform.localPosition.y + y,
                                                    rayTransform.localPosition.z);
                    GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                    rayStart.name = rayCount.ToString();
                    rays.Add(rayStart);
                    rayCount++;
                }
            }
            rayTransform.localRotation = Quaternion.Euler(direction);
            return rays;
        }

        #endregion
    }
}
