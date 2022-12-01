using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class InventoryManager : MonoBehaviour
    {
        #region Private Fields

        public List<InventoryItem> items = new List<InventoryItem>();

        public List<InventoryItem> allItems = new List<InventoryItem>();

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static InventoryManager _instance;

        public static InventoryManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one InventoryManager Singleton!");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            if (ES3.KeyExists("savedItems"))
            {
                items = ES3.Load<List<InventoryItem>>("savedItems");
            }
        }

        #endregion

        #region Public Methods

        public void AddToInventory(InventoryItem newItem)
        {
            items.Add(newItem);
        }

        public void RemoveFromInventory(InventoryItem removeItem)
        {
            items.Remove(removeItem);
        }

        public bool IsInInventory(InventoryItem item)
        {
            return items.Contains(item);
        }

        public void SaveInventory()
        {
            ES3.Save("savedItems", items);
        }

        #endregion
    }
}
