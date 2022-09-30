using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TheEscapeArtist
{
    [RequireComponent(typeof(ScreenBlur))]
    [RequireComponent(typeof(ObjectRotation))]
    public class ItemInspector : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private Transform inspectLocation;

        [SerializeField] private RectTransform cursorDetection;

        #endregion

        #region Private Fields

        private Transform itemPrefab;

        private ScreenBlur screenBlur;

        private ObjectRotation rotator;

        private bool isInspecting = false;

        private bool isMoving = false;

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static ItemInspector _instance;

        public static ItemInspector Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one ItemInspector Singleton!");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            screenBlur = GetComponent<ScreenBlur>();
            rotator = GetComponent<ObjectRotation>();
            rotator.enabled = false;
        }

        private void Update()
        {
            if (isInspecting && Input.GetKeyDown(KeyCode.X))
            {
                CloseInspect();
            }
        }

        #endregion

        #region "Getter's" & "Setter's"

        public bool IsInspecting
        {
            get => isInspecting;
            set => isInspecting = value;
        }

        public bool IsMoving
        {
            get => isMoving;
            set => isMoving = value;
        }

        #endregion

        #region Public Methods

        public void InspectNewItem(Transform itemToInspect)
        {
            rotator.enabled = true;

            if (itemPrefab != null)
            {
                Destroy(itemPrefab.gameObject);
                rotator.ResetObject();
            }

            screenBlur.ToggleBlur(true);
            cursorDetection.gameObject.SetActive(true);
            itemPrefab = Instantiate(itemToInspect, inspectLocation.position, Quaternion.identity);
            rotator.SetObject(itemPrefab);
            isInspecting = true;
        }

        public void CloseInspect()
        {
            rotator.ResetObject();
            rotator.enabled = false;
            Destroy(itemPrefab.gameObject);
            cursorDetection.gameObject.SetActive(false);
            screenBlur.ToggleBlur(false);
            isInspecting = false;
        }

        #endregion
    }
}
