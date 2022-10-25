using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on Comp-3 Interactive's implementation of a dynamic subtitles system
    // https://www.youtube.com/watch?v=Wu46UAVlFL4
    [CreateAssetMenu(fileName = "NewVoiceClip", menuName = "Voice Acting/Create New Voice Clip")]
    public class VoiceClip : ScriptableObject
    {
        #region Private Serializable Fields

        public AudioClip Clip;

        public List<Subtitle> Subtitles = new List<Subtitle>();

        #endregion
    }
}

[Serializable]
public class Subtitle
{
    public string subtitleText = "";

    public float displayTime = 0f;
}
