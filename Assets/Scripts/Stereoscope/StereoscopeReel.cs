using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class StereoscopeReel : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private StereoscopeSlide[] slides = new StereoscopeSlide[7];

        #endregion

        #region "Getter's"

        public StereoscopeSlide[] Slides => slides;

        #endregion
    }
}
