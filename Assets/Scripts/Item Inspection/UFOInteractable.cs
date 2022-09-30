using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class UFOInteractable : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private Animator ufoAnimator;

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