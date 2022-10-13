using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on a lightmap swapping implementation by Birdmask Studio
    // https://www.youtube.com/watch?v=BRapbR6vPII

    [ExecuteInEditMode]
    public class LightmapManager : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private List<LightmapObject> lightmaps = new List<LightmapObject>();

        [SerializeField] private bool saveLightmaps = false;

        #endregion

        private void Update()
        {
            if (saveLightmaps)
            {
                LightmapData[] lightmapsData = new LightmapData[lightmaps.Count];

                for (int i = 0; i < lightmaps.Count; i++)
                {
                    lightmapsData[i] = new LightmapData();
                    lightmapsData[i].lightmapColor = lightmaps[i].color;
                    lightmapsData[i].lightmapDir = lightmaps[i].direction;
                }

                LightmapSettings.lightmaps = lightmapsData;

                saveLightmaps = false;
            }
        }
    }

    [Serializable]
    public class LightmapObject
    {
        public string referenceName;
        public Texture2D color;
        public Texture2D direction;
    }
}
