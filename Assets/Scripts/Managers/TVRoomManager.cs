using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class TVRoomManager : MonoBehaviour
    {
        #region Public Fields

        public List<Material> Channels = new List<Material>();

        #endregion

        #region Private Serializable Fields

        [SerializeField] private Animator cutsceneAnim;

        [SerializeField] private InventoryItem stereoscope;

        [SerializeField] private List<GameObject> screens = new List<GameObject>();

        [SerializeField] private List<ScreenObject> screensToMatch = new List<ScreenObject>();

        #endregion

        #region Private Fields

        private Dictionary<GameObject, TVChannel> correctScreens = new Dictionary<GameObject, TVChannel>();

        private Dictionary<GameObject, TVChannel> playerScreens = new Dictionary<GameObject, TVChannel>();

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static TVRoomManager _instance;

        public static TVRoomManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one TVRoomManager Singleton!");
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
            foreach (GameObject screen in screens)
            {
                correctScreens.Add(screen, TVChannel.BlueScreen);
                playerScreens.Add(screen, TVChannel.BlueScreen);
            }

            foreach (ScreenObject screenObject in screensToMatch)
            {
                Debug.Log($"{screenObject.screen}:{correctScreens[screenObject.screen]}-->{screenObject.channel}");
                correctScreens[screenObject.screen] = screenObject.channel;
            }
        }

        #endregion

        #region Public Methods

        public void ChangeChannel(GameObject screen, TVChannel originalChannel)
        {
            playerScreens[screen] = originalChannel;

            string comparisonString = CompareDicts(playerScreens, correctScreens);
            if (comparisonString != "None")
            {
                Debug.Log(comparisonString);
            }
            else
            {
                Debug.Log("TVRoomManager.ChangeChannel: Triggering cutscene...");
                InventoryManager.Instance.RemoveFromInventory(stereoscope);
                cutsceneAnim.enabled = true;
                cutsceneAnim.SetTrigger("Cutscene");
                DisableScreens();
                Invoke(nameof(ClearTrigger), .25f);
            }
        }

        #endregion

        #region Private Methods

        private void DisableScreens()
        {
            foreach (GameObject screen in screens)
                screen.layer = LayerMask.NameToLayer("Default");
        }

        private void ClearTrigger()
        {
            cutsceneAnim.ResetTrigger("Cutscene");
        }

        private string CompareDicts(Dictionary<GameObject, TVChannel> player, Dictionary<GameObject, TVChannel> correct)
        {
            foreach (var screenChannel in player)
            {
                if (screenChannel.Value != correct[screenChannel.Key])
                    return $"{screenChannel.Key}: {screenChannel.Value} != {correct[screenChannel.Key]}";
            }
            return "None";
        }

        #endregion
    }
}


[Serializable]
public enum TVChannel
{
    BlueScreen,
    Static,
    Standby,
    RollerCoaster,
    News,
    TestCard,
}


[Serializable]
public class ScreenObject
{
    public GameObject screen;
    public TVChannel channel = TVChannel.BlueScreen;
}
