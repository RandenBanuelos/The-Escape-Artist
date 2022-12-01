using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    [RequireComponent(typeof(Collider))]
    public class TooltipTrigger : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private string textToDisplay;

        [SerializeField] private float tooltipDuration = 5f;

        #endregion

        #region Private Fields

        private Collider trigger;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            trigger = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            trigger.enabled = false;
            HideRevealManager.Instance.AddHideRevealChange(this.gameObject.name, false);
            Debug.Log($"TooltipTrigger.OnTriggerEnter(): {textToDisplay}");
            Tooltip.Instance.AddTooltipToQueue(textToDisplay, tooltipDuration);
        }

        #endregion
    }
}
