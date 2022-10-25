using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TheEscapeArtist
{
    public class InteractionUIPanel : MonoBehaviour
    {
        #region Private Serializable Fields

            [SerializeField] private Image progressBar;

            [SerializeField] private Image tooltipBackground;

            [SerializeField] private TMP_Text tooltipText;

            [SerializeField] private Image requirementBackground;

            [SerializeField] private TMP_Text requirementText;

            [SerializeField] private Image pickupBackground;

            [SerializeField] private TMP_Text pickupText;

            [SerializeField] private float pickupClearTime = 2f;

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static InteractionUIPanel _instance;

        public static InteractionUIPanel Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one InteractionUIPanel Singleton!");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            ResetPickup();
        }

        #endregion

        #region Public Methods

            public void SetTooltip(string tooltip)
            {
                tooltipBackground.enabled = true;
                tooltipText.SetText(tooltip);
            }

            public void SetRequirement(string requirement)
            {
                requirementBackground.enabled = true;
                requirementText.SetText(requirement);
            }

            public void SetPickup(string pickup)
            {
                pickupBackground.enabled = true;
                pickupText.SetText(pickup);
                Invoke(nameof(ResetPickup), pickupClearTime);
            }

            public void UpdateProgressBar(float fillAmount)
            {
                progressBar.fillAmount = fillAmount;
            }

            public void ResetUI()
            {
                progressBar.fillAmount = 0f;
                tooltipBackground.enabled = false;
                tooltipText.SetText("");
                requirementBackground.enabled = false;
                requirementText.SetText("");
            }

            private void ResetPickup()
            {
                pickupBackground.enabled = false;
                pickupText.SetText("");
            }

        #endregion
    }
}
