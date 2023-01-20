using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class PuzzleCubeManager : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private List<GameObject> cubes = new List<GameObject>();

        [SerializeField] private List<InventoryItem> algorithms = new List<InventoryItem>();

        [SerializeField] private GameObject solveButton;

        [SerializeField] private GameObject puzzleCubeHolder;

        [SerializeField] private GameObject puzzleCubeCamera;

        [SerializeField] private GameObject puzzleCubeCanvas;

        [SerializeField] private GameObject puzzleCubeImage;

        [SerializeField] private ScreenBlur screenBlur;

        #endregion

        #region Private Fields

        public bool PuzzleCubeIsOpen { get; private set; }

        private GameObject currentCube;

        private int currentCubeIndex = 0;

        private Automate automate;

        private bool foundAlgorithm = false;

        private InventoryManager inventory;

        private InventoryItem currentAlgorithm;

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static PuzzleCubeManager _instance;

        public static PuzzleCubeManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one PuzzleCubeManager Singleton!");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            currentCubeIndex = 0;
            currentCube = cubes[currentCubeIndex];
            automate = puzzleCubeHolder.GetComponentInChildren<Automate>();
            solveButton.SetActive(false);
            foundAlgorithm = false;
            inventory = InventoryManager.Instance;
            currentAlgorithm = algorithms[currentCubeIndex];
        }

        private void Update()
        {
            if (!foundAlgorithm)
            {
                if (inventory.IsInInventory(currentAlgorithm))
                {
                    solveButton.SetActive(true);
                    foundAlgorithm = true;
                }
            }
        }

        #endregion

        #region Public Methods

        public void OpenPuzzleCube()
        {
            PuzzleCubeIsOpen = true;
            screenBlur.ToggleBlur(true);
            puzzleCubeHolder.SetActive(true);
            puzzleCubeCamera.SetActive(true);
            puzzleCubeCanvas.SetActive(true);
            puzzleCubeImage.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void SendMoves(List<string> movesToSend)
        {
            automate.SetUnlockMoves(movesToSend);
        }

        public void DisableAndClose()
        {
            currentCube.layer = LayerMask.NameToLayer("Default");
            currentCubeIndex = Mathf.Clamp(currentCubeIndex += 1, 0, cubes.Count - 1);
            currentCube = cubes[currentCubeIndex];
            currentAlgorithm = algorithms[currentCubeIndex];
            foundAlgorithm = false;
            solveButton.SetActive(false);
            ClosePuzzleCube();
        }

        public void ClosePuzzleCube()
        {
            Cursor.lockState = CursorLockMode.Locked;
            puzzleCubeHolder.SetActive(false);
            puzzleCubeCamera.SetActive(false);
            puzzleCubeCanvas.SetActive(false);
            puzzleCubeImage.SetActive(false);
            screenBlur.ToggleBlur(false);
            PuzzleCubeIsOpen = false;
        }

        #endregion
    }
}
