using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmazingAssets.DynamicRadialMasks;

namespace TheEscapeArtist
{
    public class PocketWatch : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private DRMGameObject watchObject;

        [SerializeField] private Animator pocketWatchAnim;

        [SerializeField] private SphereCollider nearbyColliders;

        [SerializeField] private float watchRange = 5f;

        [SerializeField] private float watchGrowShrinkTimer = 1f;

        #endregion

        #region Private Fields

        private bool isInPast = false;

        private bool isGrowingShrinking = false;

        private bool isActive = false;

        private float timer = 0f;

        #endregion

        #region Singleton

        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static PocketWatch _instance;

        public static PocketWatch Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(this.gameObject);
            else
                _instance = this;
        }

        #endregion

        private void Start()
        {
            watchObject.radius = -2f;
            nearbyColliders.radius = 0f;
            nearbyColliders.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (isGrowingShrinking)
            {
                if (isInPast)
                {
                    if (!nearbyColliders.gameObject.activeSelf)
                        nearbyColliders.gameObject.SetActive(true);

                    watchObject.radius = Mathf.Lerp(watchObject.radius, watchRange, timer);
                    nearbyColliders.radius = watchObject.radius;
                    timer += Time.deltaTime;
                    if (timer > watchGrowShrinkTimer)
                    {
                        watchObject.radius = watchRange;
                        nearbyColliders.radius = watchRange;
                        timer = 0f;
                        isGrowingShrinking = false;
                    }
                }
                else
                {
                    watchObject.radius = Mathf.Lerp(watchObject.radius, -2f, timer);
                    nearbyColliders.radius = watchObject.radius + 2f;
                    timer += Time.deltaTime;
                    if (timer > watchGrowShrinkTimer)
                    {
                        watchObject.radius = -2f;
                        nearbyColliders.radius = 0f;
                        nearbyColliders.gameObject.SetActive(false);
                        timer = 0f;
                        isGrowingShrinking = false;
                    }
                }
            }
        }

        #region Public Methods

        public void TimeShift()
        {
            if (!isGrowingShrinking)
            {
                isInPast = !isInPast;
                isActive = !isActive;
                isGrowingShrinking = true;
                timer = 0f;

                pocketWatchAnim.SetBool("Toggle", isInPast);
            }
        }

        public void SetWatchRange(float newRange)
        {
            watchRange = newRange;
        }

        public bool IsActive => isActive;

        #endregion
    }
}
