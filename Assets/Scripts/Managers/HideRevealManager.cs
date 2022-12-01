using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class HideRevealManager : MonoBehaviour
    {
        #region Private Serializable Fields

        #endregion

        #region Private Fields

        private Dictionary<string, bool> hideRevealDict = new Dictionary<string, bool>();

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static HideRevealManager _instance;

        public static HideRevealManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one HideRevealManager Singleton!");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            if (ES3.KeyExists("hideRevealDict"))
            {
                hideRevealDict = ES3.Load<Dictionary<string, bool>>("hideRevealDict");
                foreach (string objectToCheck in hideRevealDict.Keys)
                {
                    bool state = hideRevealDict[objectToCheck];
                    GameObject foundObject = GameObject.Find(objectToCheck);

                    if (foundObject)
                    {
                        foundObject.SetActive(state);
                    }
                }
            }
        }

        #endregion

        #region "Getter's" & "Setter's"

        #endregion

        #region Public Methods

        public void AddHideRevealChange(string objectToSave, bool state)
        {
            hideRevealDict[objectToSave] = state;
        }

        public void SaveHideRevealChanges()
        {
            ES3.Save("hideRevealDict", hideRevealDict);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
