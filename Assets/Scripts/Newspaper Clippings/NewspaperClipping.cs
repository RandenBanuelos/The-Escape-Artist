using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    [CreateAssetMenu(fileName = "NewNewspaperClipping", menuName = "Newspaper Clippings/Create New Newspaper Clipping")]
    public class NewspaperClipping : ScriptableObject
    {
        #region Public Fields

        public string Headline;

        public string Date;

        [TextArea(15, 20)]
        public string Content;

        #endregion
    }
}
