using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on CodeMonkey's implementation of an in-game clock UI
    // https://www.youtube.com/watch?v=pbTysQw-WNs
    public class GrandfatherClock : MonoBehaviour
    {
        #region Private Serializable Fields

        [Header("Transforms")]
        [SerializeField] private Transform hourHand;

        [SerializeField] private Transform minuteHand;

        [SerializeField] private Transform dayNightDial;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            int realTimeHour = System.DateTime.Now.Hour;
            int realTimeMinute = System.DateTime.Now.Minute;
            int totalTime = ((realTimeHour % 12) * 60) + realTimeMinute;

            hourHand.localEulerAngles = new Vector3((-totalTime / 720f) * 360f, 0f, 0f);
            minuteHand.localEulerAngles = new Vector3((-realTimeMinute / 60f) * 360f, 0f, 0f);
            dayNightDial.localEulerAngles = new Vector3(((realTimeHour - 12f) / 24f) * 360f, 0f, 0f);
        }

        #endregion
    }
}
