using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    [RequireComponent(typeof(Collider))]
    public class StereoscopeReelSwitcher : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private StereoscopeView view;

        [SerializeField] private StereoscopeReel newReel;

        [SerializeField] private GameObject newHouse;

        #endregion

        #region MonoBehaviour Callbacks
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                view.SetReel(newReel);
                view.SetHouse(newHouse);
            }
        }

        #endregion
    }
}
