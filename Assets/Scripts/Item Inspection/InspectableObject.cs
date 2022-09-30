using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class InspectableObject : InteractableBase
    {
        public override void OnInteract()
        {
            base.OnInteract();
            InteractionUIPanel.Instance.ResetUI();
            InteractionController.Instance.ResetOutline();
            ItemInspector.Instance.InspectNewItem(this.InteractablePrefab);
        }
    }
}
