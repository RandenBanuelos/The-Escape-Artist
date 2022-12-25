using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class TwistingHallway : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private List<Material> hallwayMaterials = new List<Material>();

        [SerializeField] private float twistAmount = 1f;

        [SerializeField] private float twistTime = 5f;

        #endregion

        #region Private Fields

        private float twistAmountPositive;

        private float twistAmountNegative;

        private float timer = 0f;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            twistAmountPositive = twistAmount;
            twistAmountNegative = -twistAmount;
        }

        private void FixedUpdate()
        {
            foreach (Material mat in hallwayMaterials)
            {
                mat.SetFloat("_Multiplier", Mathf.Lerp(twistAmountNegative, twistAmountPositive, timer));
            }

            timer += (1 / twistTime) * Time.deltaTime;

            if (timer > 1.0f)
            {
                float temp = twistAmountPositive;
                twistAmountPositive = twistAmountNegative;
                twistAmountNegative = temp;
                timer = 0f;
            }
        }

        #endregion
    }
}
