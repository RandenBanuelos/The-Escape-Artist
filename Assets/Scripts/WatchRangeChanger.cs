using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class WatchRangeChanger : MonoBehaviour
    {
        [SerializeField] private float newRange = 2.5f;

        [SerializeField] private PocketWatch watch;

        private void OnTriggerEnter(Collider other)
        {
            watch.SetWatchRange(newRange);
        }
    }
}
