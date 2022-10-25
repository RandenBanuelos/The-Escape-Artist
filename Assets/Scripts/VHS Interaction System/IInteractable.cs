using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public interface IInteractable
    {
        #region Fields

        Transform InteractablePrefab { get; }

        string TooltipMessage { get; }

        string NotInteractableTooltipMessage { get; }

        float HoldDuration { get; }

        bool HoldInteract { get; }

        bool MultipleUse { get; }

        bool IsInteractable { get; }

        Outline InteractOutline { get; }

        VoiceClip VoiceClip { get; }

        #endregion

        #region Methods

        void OnInteract();

        #endregion
    }
}
