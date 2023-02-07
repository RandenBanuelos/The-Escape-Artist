using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class FireManager : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private int numberOfFires = 1;

        [SerializeField] private Animator anim;

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static FireManager _instance;

        public static FireManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one FireManager Singleton!");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        #endregion

        #region Public Methods

        public void ExtinguishFire()
        {
            numberOfFires--;
            if (numberOfFires == 0)
            {
                anim.SetTrigger("OpenDoor");
                Invoke(nameof(ClearTrigger), .1f);
            }
        }

        #endregion

        #region Private Methods

        private void ClearTrigger()
        {
            anim.ResetTrigger("OpenDoor");
        }

        #endregion
    }
}
