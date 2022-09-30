using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TheEscapeArtist
{
    public class MenuManager : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private FPSController fpsController;
        [SerializeField] private CharacterController playerController;
        [SerializeField] private Camera playerCam;

        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject pauseMenuReminder;

        [Header("Camera")]
        [SerializeField] private Slider fovSlider;
        [SerializeField] private TMP_Text fovText;

        [SerializeField] private Slider mouseSensitivitySlider;
        [SerializeField] private TMP_Text mouseSensitivityText;

        [SerializeField] private Slider rotationSmoothTimeSlider;
        [SerializeField] private TMP_Text rotationSmoothTimeText;

        [Header("Movement")]
        [SerializeField] private Slider rotationSmoothMoveSlider;
        [SerializeField] private TMP_Text rotationSmoothMoveText;

        #endregion

        #region Private Fields

        private bool isInMenu = false;

        private float defaultFOV = 60f;

        private float defaultMouseSensitivity = 8f;

        private float defaultSmoothTime = .05f;

        private float defaultSmoothMove = .15f;

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static MenuManager _instance;

        public static MenuManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one MenuManager Singleton!");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            fovSlider.onValueChanged.AddListener((v) =>
            {
                playerCam.fieldOfView = v;
                fovText.text = v.ToString();
            });

            mouseSensitivitySlider.onValueChanged.AddListener((v) =>
            {
                fpsController.MouseSensitivityAdjust(v);
                mouseSensitivityText.text = v.ToString("0.00");
            });

            rotationSmoothTimeSlider.onValueChanged.AddListener((v) =>
            {
                fpsController.RotationSmoothAdjust(v);
                rotationSmoothTimeText.text = v.ToString("0.00");
            });

            rotationSmoothMoveSlider.onValueChanged.AddListener((v) =>
            {
                fpsController.SmoothMoveAdjust(v);
                rotationSmoothMoveText.text = v.ToString("0.00");
            });

            ResetAllCameraValues();
        }

        #endregion

        public bool IsInMenu
        {
            get => isInMenu;
            set => isInMenu = value;
        }

        #region Public Methods

        public void OpenPauseMenu()
        {
            isInMenu = true;
            playerController.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            pauseMenuReminder.SetActive(false);
            pauseMenu.SetActive(true);
        }

        public void ClosePauseMenu()
        {
            pauseMenu.SetActive(false);
            pauseMenuReminder.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            playerController.enabled = true;
            isInMenu = false;
        }

        public void ResetAllCameraValues()
        {
            ResetFOV();
            ResetMouseSensitivity();
            ResetSmoothTime();
            ResetSmoothMove();
        }

        public void ResetFOV()
        {
            fovSlider.value = defaultFOV;
        }

        public void ResetMouseSensitivity()
        {
            mouseSensitivitySlider.value = defaultMouseSensitivity;
        }

        public void ResetSmoothTime()
        {
            rotationSmoothTimeSlider.value = defaultSmoothTime;
        }

        public void ResetSmoothMove()
        {
            rotationSmoothMoveSlider.value = defaultSmoothMove;
        }

        #endregion
    }
}