using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace TheEscapeArtist
{
    public class StereoscopeView : MonoBehaviour
    {
        #region Private Serializable Fields

        [Header("Base Data")]
        [SerializeField] private Transform player;
        [SerializeField] private GameObject stereoscopeUI;
        [SerializeField] private StereoscopeReel defaultReel;
        [SerializeField] private GameObject house;

        [Header("Movement Amounts")]
        [SerializeField] private float viewRotationAmount = -90f;
        [SerializeField] private float viewCoverMoveX = 600f;

        [Header("Timing")]
        [SerializeField] private float viewRotationOutTime = 1f;
        [SerializeField] private float viewRotationInTime = 2f;
        [SerializeField] private float viewRotationModifier = 1.5f;

        [Header("RectTransform's")]
        [SerializeField] private RectTransform viewAnchor;
        [SerializeField] private RectTransform coverAnchor;

        [SerializeField] private TMP_Text viewText;
        [SerializeField] private RectTransform viewTextCover;

        public bool IsViewing { get; private set; }

        #endregion

        #region Private Fields

        private int lastIndex = 0;

        private float timer = 0f;

        private bool isChangingToNextSlide = false;

        private bool isDoneRotatingView;

        private Vector3 anchorStartingRotation;
        private Vector3 anchorEndRotation;
        private Vector3 coverAnchorEndRotation;

        private Vector3 viewTextStartingPosition;
        private Vector3 viewTextEndPosition;

        private Vector3 viewCoverStartingPosition;
        private Vector3 viewCoverEndPosition;

        private bool rotateOut = false;

        private RawImage view;

        private StereoscopeReel currentReel;

        private StereoscopeSlide currentSlide;

        #endregion

        #region Singleton

        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static StereoscopeView _instance;

        public static StereoscopeView Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(this.gameObject);
            else
                _instance = this;
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            view = viewAnchor.GetComponentInChildren<RawImage>();

            anchorStartingRotation = viewAnchor.localEulerAngles;
            anchorEndRotation = new Vector3(anchorStartingRotation.x, anchorStartingRotation.y, anchorStartingRotation.z + viewRotationAmount);
            coverAnchorEndRotation = new Vector3(anchorStartingRotation.x, anchorStartingRotation.y, anchorStartingRotation.z - viewRotationAmount);

            viewTextStartingPosition = viewText.transform.position;
            viewTextEndPosition = new Vector3(viewTextStartingPosition.x + viewCoverMoveX, viewTextStartingPosition.y, viewTextStartingPosition.z);

            viewCoverStartingPosition = viewTextCover.transform.position;
            viewCoverEndPosition = new Vector3(viewCoverStartingPosition.x + viewCoverMoveX, viewCoverStartingPosition.y, viewCoverStartingPosition.z);

            SetReel(defaultReel);
            ResetView();
        }

        private void FixedUpdate()
        {
            if (isChangingToNextSlide)
            {
                if (rotateOut)
                {
                    viewAnchor.localEulerAngles = Vector3.Lerp(anchorStartingRotation, anchorEndRotation, timer / (viewRotationOutTime * viewRotationModifier));
                    viewText.transform.position = Vector3.Lerp(viewTextStartingPosition, viewTextEndPosition, timer / viewRotationOutTime);
                    viewTextCover.transform.position = Vector3.Lerp(viewCoverStartingPosition, viewCoverEndPosition, timer / viewRotationOutTime);

                    timer += Time.deltaTime;
                    if (timer >= viewRotationOutTime)
                    {
                        viewAnchor.localEulerAngles = anchorEndRotation;
                        viewText.transform.position = viewTextEndPosition;
                        viewTextCover.transform.position = viewCoverEndPosition;

                        timer = 0f;
                        isDoneRotatingView = true;
                    }
                }
                else
                {
                    coverAnchor.localEulerAngles = Vector3.Lerp(anchorStartingRotation, coverAnchorEndRotation, timer / (viewRotationInTime * viewRotationModifier));
                    viewTextCover.transform.position = Vector3.Lerp(viewCoverEndPosition, viewCoverStartingPosition, timer / viewRotationInTime);

                    timer += Time.deltaTime;
                    if (timer >= viewRotationInTime)
                    {
                        coverAnchor.localEulerAngles = coverAnchorEndRotation;
                        viewTextCover.transform.position = viewCoverStartingPosition;

                        timer = 0f;
                        isDoneRotatingView = true;
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public void OpenView()
        {
            IsViewing = true;
            currentSlide = currentReel.Slides[lastIndex];

            if (!currentSlide.Scenery.activeSelf)
                currentSlide.Scenery.SetActive(true);

            stereoscopeUI.SetActive(true);
            viewText.text = currentSlide.SlideDescription;
            SetView(currentSlide.CameraTexture);
        }

        public IEnumerator NextSlide()
        {
            if (!isChangingToNextSlide && IsViewing)
            {
                GameObject pastScenery = currentSlide.Scenery;
                
                isChangingToNextSlide = true;
                lastIndex = lastIndex + 1 >= currentReel.Slides.Length ? 0 : lastIndex + 1;
                currentSlide = currentReel.Slides[lastIndex];

                rotateOut = true;
                isDoneRotatingView = false;
                yield return new WaitUntil(() => isDoneRotatingView);

                SetView(currentSlide.CameraTexture);
                viewText.text = currentSlide.SlideDescription;
                viewText.transform.position = viewTextStartingPosition;

                coverAnchor.localEulerAngles = anchorStartingRotation;

                viewAnchor.localEulerAngles = anchorStartingRotation;

                pastScenery.SetActive(false);
                currentSlide.Scenery.SetActive(true);

                rotateOut = false;
                isDoneRotatingView = false;
                yield return new WaitUntil(() => isDoneRotatingView);

                isChangingToNextSlide = false;
            }
        }

        public IEnumerator CloseView()
        {
            if (!isChangingToNextSlide && IsViewing)
            {
                timer = 0f;
                isChangingToNextSlide = false;
                rotateOut = false;

                if (player == null)
                    player = GameObject.FindGameObjectWithTag("Player").transform;

                CharacterController charController = player.GetComponent<CharacterController>();
                charController.enabled = false;
                player.position = currentSlide.SpawnPosition.position;

                FPSController fps = player.GetComponent<FPSController>();
                fps.StereoscopeAdjust(currentSlide.SpawnPosition.eulerAngles);

                if (house.activeSelf)
                    house.SetActive(false);

                IsViewing = false;
                charController.enabled = true;

                yield return new WaitForSeconds(0.01f);
                ResetView();
            }
        }

        public void SetReel(StereoscopeReel newReel)
        {
            if (!IsViewing && !isChangingToNextSlide)
            {
                currentReel = newReel;
            }
        }

        #endregion

        #region Private Methods

        private void SetView(RenderTexture newTexture)
        {
            view.texture = newTexture;
        }

        private void ResetView()
        {
            stereoscopeUI.SetActive(false);
        }

        #endregion
    }
}
