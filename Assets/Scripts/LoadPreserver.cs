using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class LoadPreserver : MonoBehaviour
    {
        private void Awake()
        {

            DontDestroyOnLoad(this);
        }
    }
}
