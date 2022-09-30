using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheEscapeArtist
{
    [CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Create New Item")]
    public class InventoryItem : ScriptableObject
    {
        #region Public Fields

        public string ItemName;

        public string Description;

        public Image Icon;

        public Transform ItemPrefab;

        #endregion
    }
}
