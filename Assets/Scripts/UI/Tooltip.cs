using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TheEscapeArtist
{
    public class Tooltip : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private float slideInOutTime = 1f;

        [SerializeField] private TMP_Text tooltipText;

        #endregion

        #region Private Fields

        private float timer = 0f;

        private bool isSliding = false;
        
        private bool isSlidingIn = true;

        private Vector3 originalPos;

        private RectTransform rect;

        private Queue<TooltipQueueItem> tooltipQueue = new Queue<TooltipQueueItem>();

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static Tooltip _instance;

        public static Tooltip Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one Tooltip Singleton!");
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
            rect = GetComponent<RectTransform>();
            originalPos = rect.anchoredPosition;
            SetTooltipText("");
        }

        private void Update()
        {
            if (isSliding)
            {
                if (isSlidingIn)
                {
                    Vector3 newPos = new Vector3(Mathf.Lerp(rect.anchoredPosition.x, -originalPos.x, Time.deltaTime * slideInOutTime), originalPos.y, originalPos.z);
                    rect.anchoredPosition = newPos;

                    timer += Time.deltaTime;
                    if (timer > slideInOutTime)
                    {
                        rect.anchoredPosition = new Vector3(-originalPos.x, originalPos.y, originalPos.z);
                        isSliding = false;
                    }
                }
                else
                {
                    Vector3 newPos = new Vector3(Mathf.Lerp(rect.anchoredPosition.x, originalPos.x, Time.deltaTime * slideInOutTime), originalPos.y, originalPos.z);
                    rect.anchoredPosition = newPos;

                    timer += Time.deltaTime;
                    if (timer > slideInOutTime)
                    {
                        rect.anchoredPosition = new Vector3(originalPos.x, originalPos.y, originalPos.z);
                        isSliding = false;
                    }
                }
            }
            else
            {
                if (tooltipQueue.Count > 0)
                {
                    TooltipQueueItem nextString = tooltipQueue.Dequeue();
                    SetTooltipText(nextString.queueText);
                    SlideIn();
                    Invoke(nameof(SlideOut), nextString.textDuration);
                }
            }
        }

        #endregion

        #region Public Methods

        public void AddTooltipToQueue(string addedText, float duration)
        {
            TooltipQueueItem newQueueItem = new TooltipQueueItem();
            newQueueItem.queueText = addedText;
            newQueueItem.textDuration = duration;
            tooltipQueue.Enqueue(newQueueItem);
        }

        #endregion

        #region Private Methods

        private void SlideIn()
        {
            if (!isSliding)
            {
                isSliding = true;
                isSlidingIn = true;
                timer = 0f;
            }
        }

        private void SlideOut()
        {
            if (!isSliding)
            {
                isSliding = true;
                isSlidingIn = false;
                timer = 0f;
            }
        }

        private void SetTooltipText(string newText)
        {
            tooltipText.text = newText;
        }

        #endregion
    }
}

class TooltipQueueItem
{
    public string queueText;
    public float textDuration;
}
