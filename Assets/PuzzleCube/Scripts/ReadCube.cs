using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of an interactable Rubik's Cube
    // https://www.youtube.com/playlist?list=PLuq_iMEtykn-ZOJyx2cY_k9WkixAhv11n
    public class ReadCube : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private LayerMask facesLayerMask;

        [SerializeField] private GameObject emptyGO;

        [Header("Ray Centers")]
        [SerializeField] private Transform tUp;

        [SerializeField] private Transform tDown;

        [SerializeField] private Transform tLeft;

        [SerializeField] private Transform tRight;

        [SerializeField] private Transform tFront;

        [SerializeField] private Transform tBack;

        #endregion

        #region Private Fields

        private CubeState cubeState;

        private CubeMap cubeMap;

        private List<GameObject> upRays = new List<GameObject>();

        private List<GameObject> downRays = new List<GameObject>();

        private List<GameObject> leftRays = new List<GameObject>();

        private List<GameObject> rightRays = new List<GameObject>();

        private List<GameObject> frontRays = new List<GameObject>();

        private List<GameObject> backRays = new List<GameObject>();

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            SetRayTransforms();

            cubeState = FindObjectOfType<CubeState>();
            cubeMap = FindObjectOfType<CubeMap>();
        }

        #endregion

        #region "Getter's" & "Setter's"

        #endregion

        #region Public Methods

        public void ReadState()
        {
            cubeState = FindObjectOfType<CubeState>();
            cubeMap = FindObjectOfType<CubeMap>();

            // Set the state of each position in the list of sides so we know
            // what color is in what position
            cubeState.up = ReadFace(upRays, tUp);
            cubeState.down = ReadFace(downRays, tDown);
            cubeState.left = ReadFace(leftRays, tLeft);
            cubeState.right = ReadFace(rightRays, tRight);
            cubeState.front = ReadFace(frontRays, tFront);
            cubeState.back = ReadFace(backRays, tBack);

            // Update the map with the found positions
            cubeMap.Set();
        }

        public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
        {
            List<GameObject> facesHit = new List<GameObject>();

            foreach(GameObject rayStart in rayStarts)
            {
                Vector3 ray = rayStart.transform.position;
                RaycastHit hit;

                // Does the ray intersect any objects in the facesLayerMask?
                if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, facesLayerMask))
                {
                    Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                    facesHit.Add(hit.collider.gameObject);
                    // print(hit.collider.gameObject.name);
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
            // Populate the ray lists with Raycasts eminating from the transform,
            // angled towards the cube
            upRays = BuildRays(tUp, new Vector3(90, 90, 0));
            downRays = BuildRays(tDown, new Vector3(270, 90, 0));
            leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
            rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
            frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
            backRays = BuildRays(tBack, new Vector3(0, 270, 0));
        }

        private List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
        {
            // The ray count is used to name the rays, so we can be sure they are in the right order
            int rayCount = 0;
            List<GameObject> rays = new List<GameObject>();

            // This creates nine rays in the shape of the side of the cube, with
            // Ray #0 at the top left, and Ray #8 at the bottom right
            //  |0|1|2|
            //  |3|4|5|
            //  |6|7|8|

            for (int y = 1; y > -2; y--)
            {
                for (int x = -1; x < 2; x++)
                {
                    Vector3 startPosition = new Vector3(
                        rayTransform.localPosition.x + x,
                        rayTransform.localPosition.y + y,
                        rayTransform.localPosition.z
                        );
                    GameObject rayStart = Instantiate(emptyGO, startPosition, Quaternion.identity, rayTransform);
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
