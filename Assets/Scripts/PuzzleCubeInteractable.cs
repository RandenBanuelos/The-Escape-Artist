using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class PuzzleCubeInteractable : InteractableBase
    {
        public List<string> movesToSolve = new List<string>();

        public override void OnInteract()
        {
            base.OnInteract();
            InteractionUIPanel.Instance.ResetUI();
            InteractionController.Instance.ResetOutline();
            PuzzleCubeManager.Instance.OpenPuzzleCube();

            if (movesToSolve.Count > 0)
                PuzzleCubeManager.Instance.SendMoves(movesToSolve);
        }
    }
}
