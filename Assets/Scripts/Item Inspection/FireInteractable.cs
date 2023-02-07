using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class FireInteractable : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject fire;

        #endregion

        #region Private Fields

        private FireManager fireCache;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            fireCache = FireManager.Instance;
        }

        #endregion

        #region Public Methods

        public override void OnInteract()
        {
            base.OnInteract();
            fireCache.ExtinguishFire();
            gameObject.layer = LayerMask.NameToLayer("Default");
            GetComponent<Outline>().OutlineWidth = 0f;
            fire.SetActive(false);
        }

        #endregion
    }
}
