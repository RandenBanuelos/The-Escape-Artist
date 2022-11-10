using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class UFOInteractable : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject record;

        [SerializeField] private Animator ufoAnimator;

        [SerializeField] private Animator buttonAnimator;

        [SerializeField] private string ufoTrigger = "ActivateRide";

        #endregion

        #region Private Fields

        private bool isPowered = false;

        #endregion

        public override void OnInteract()
        {
            if (isPowered)
            {
                base.OnInteract();
                gameObject.layer = LayerMask.NameToLayer("Default");
                buttonAnimator.SetTrigger("PressButton");
                gameObject.GetComponent<Outline>().OutlineWidth = 0;
                record.SetActive(true);
                ufoAnimator.SetTrigger(ufoTrigger);
                ToggleInteractable(false);
                Invoke(nameof(ResetUFOTrigger), 1f);
            }
            else
            {
                InteractionUIPanel.Instance.SetRequirement($"Needs Battery Power");
            }
        }

        public void EnablePower()
        {
            isPowered = true;
        }

        private void ResetUFOTrigger()
        {
            ufoAnimator.ResetTrigger(ufoTrigger);
        }
    }
}