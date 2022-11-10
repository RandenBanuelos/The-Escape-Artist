using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

namespace TheEscapeArtist
{
    public class AnimationInspectable : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private Animator anim;

        [SerializeField] private AudioSource sfxSource;

        [SerializeField] private string animTriggerName;

        [SerializeField] private float reuseDelay = 0f;

        [SerializeField] private List<InventoryItem> requiredItems = new List<InventoryItem>();

        #endregion

        #region Private Fields

        private Outline outline;

        #endregion

        private void Start()
        {
            outline = GetComponent<Outline>();
        }

        public override void OnInteract()
        {
            if (requiredItems.Count > 0)
            {
                foreach (InventoryItem item in requiredItems)
                {
                    if (!InventoryManager.Instance.IsInInventory(item))
                    {
                        InteractionUIPanel.Instance.SetRequirement($"Requires {item.ItemName}");
                        return;
                    }
                }
            }

            base.OnInteract();
            gameObject.layer = LayerMask.NameToLayer("Default");
            outline.OutlineWidth = 0f;

            if (anim && animTriggerName != "")
                anim.SetTrigger(animTriggerName);

            if (sfxSource)
                sfxSource.Play();

            if (VoiceClip && VoiceActingManager.Instance)
            {
                VoiceActingManager.Instance.Say(VoiceClip);
            }

            if (ShakeCamera)
            {
                CameraShaker.Instance.ShakeOnce(ShakeMagnitude, 4f, .1f, ShakeDuration);
            }

            if (anim && animTriggerName != "")
                Invoke(nameof(ClearTrigger), .1f);

            if (reuseDelay > 0)
                Invoke(nameof(ResetInspectable), reuseDelay);
        }

        private void ClearTrigger()
        {
            anim.ResetTrigger(animTriggerName);
        }

        private void ResetInspectable()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
    }
}
