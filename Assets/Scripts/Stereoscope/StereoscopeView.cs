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
        [SerializeField] private RectTransform leftViewAnchor;
        [SerializeField] private RectTransform leftCoverAnchor;

        [SerializeField] private RectTransform rightViewAnchor;
        [SerializeField] private RectTransform rightCoverAnchor;

        [SerializeField] private TMP_Text viewText;
        [SerializeField] private RectTransform viewTextCover;

        public bool IsViewing { get; private set; }

        #endregion

        #region Private Fields

        private int lastIndex = 0;

        private float timer = 0f;

        private bool isChangingToNextSlide = false;

        private bool isDoneRotatingView;

        private Vector3 leftAnchorStartingRotation;
        private Vector3 leftAnchorEndRotation;
        private Vector3 leftCoverAnchorEndRotation;

        private Vector3 rightAnchorStartingRotation;
        private Vector3 rightAnchorEndRotation;
        private Vector3 rightCoverAnchorEndRotation;

        private Vector3 viewTextStartingPosition;
        private Vector3 viewTextEndPosition;

        private Vector3 viewCoverStartingPosition;
        private Vector3 viewCoverEndPosition;

        private bool rotateOut = false;

        private RawImage leftView;

        private RawImage rightView;

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
            leftView = leftViewAnchor.GetComponentInChildren<RawImage>();
            rightView = rightViewAnchor.GetComponentInChildren<RawImage>();

            leftAnchorStartingRotation = leftViewAnchor.localEulerAngles;
            leftAnchorEndRotation = new Vector3(leftAnchorStartingRotation.x, leftAnchorStartingRotation.y, leftAnchorStartingRotation.z + viewRotationAmount);
            leftCoverAnchorEndRotation = new Vector3(leftAnchorStartingRotation.x, leftAnchorStartingRotation.y, leftAnchorStartingRotation.z - viewRotationAmount);

            rightAnchorStartingRotation = rightViewAnchor.localEulerAngles;
            rightAnchorEndRotation = new Vector3(rightAnchorStartingRotation.x, rightAnchorStartingRotation.y, rightAnchorStartingRotation.z + viewRotationAmount);
            rightCoverAnchorEndRotation = new Vector3(rightAnchorStartingRotation.x, rightAnchorStartingRotation.y, rightAnchorStartingRotation.z - viewRotationAmount);

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
                    leftViewAnchor.localEulerAngles = Vector3.Lerp(leftAnchorStartingRotation, leftAnchorEndRotation, timer / (viewRotationOutTime * viewRotationModifier));
                    rightViewAnchor.localEulerAngles = Vector3.Lerp(rightAnchorStartingRotation, rightAnchorEndRotation, timer / (viewRotationOutTime * viewRotationModifier));
                    viewText.transform.position = Vector3.Lerp(viewTextStartingPosition, viewTextEndPosition, timer / viewRotationOutTime);
                    viewTextCover.transform.position = Vector3.Lerp(viewCoverStartingPosition, viewCoverEndPosition, timer / viewRotationOutTime);

                    timer += Time.deltaTime;
                    if (timer >= viewRotationOutTime)
                    {
                        leftViewAnchor.localEulerAngles = leftAnchorEndRotation;
                        rightViewAnchor.localEulerAngles = rightAnchorEndRotation;
                        viewText.transform.position = viewTextEndPosition;
                        viewTextCover.transform.position = viewCoverEndPosition;

                        timer = 0f;
                        isDoneRotatingView = true;
                    }
                }
                else
                {
                    leftCoverAnchor.localEulerAngles = Vector3.Lerp(leftAnchorStartingRotation, leftCoverAnchorEndRotation, timer / (viewRotationInTime * viewRotationModifier));
                    rightCoverAnchor.localEulerAngles = Vector3.Lerp(rightAnchorStartingRotation, rightCoverAnchorEndRotation, timer / (viewRotationInTime * viewRotationModifier));
                    viewTextCover.transform.position = Vector3.Lerp(viewCoverEndPosition, viewCoverStartingPosition, timer / viewRotationInTime);

                    timer += Time.deltaTime;
                    if (timer >= viewRotationInTime)
                    {
                        leftCoverAnchor.localEulerAngles = leftCoverAnchorEndRotation;
                        rightCoverAnchor.localEulerAngles = rightCoverAnchorEndRotation;
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

                leftCoverAnchor.localEulerAngles = leftAnchorStartingRotation;
                rightCoverAnchor.localEulerAngles = rightAnchorStartingRotation;

                leftViewAnchor.localEulerAngles = leftAnchorStartingRotation;
                rightViewAnchor.localEulerAngles = rightAnchorStartingRotation;

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
            leftView.texture = newTexture;
            rightView.texture = newTexture;
        }

        private void ResetView()
        {
            stereoscopeUI.SetActive(false);
        }

        #endregion
    }
}
