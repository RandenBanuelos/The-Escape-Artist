using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class PuzzleCubeInteractable : InteractableBase
    {
        public override void OnInteract()
        {
            base.OnInteract();
            InteractionUIPanel.Instance.ResetUI();
            InteractionController.Instance.ResetOutline();
            PuzzleCubeManager.Instance.OpenPuzzleCube();
        }
    }
}
