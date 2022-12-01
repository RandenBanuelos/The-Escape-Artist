using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class HideReveal : MonoBehaviour
    {
        [SerializeField] private bool checkToReveal = false;

        [SerializeField] private bool hideSelf = true;

        [SerializeField] private List<Transform> thingsToHide;

        private HideRevealManager hrManager;

        private void Start()
        {
            hrManager = HideRevealManager.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                foreach (Transform item in thingsToHide)
                {
                    hrManager.AddHideRevealChange(item.gameObject.name, checkToReveal);
                    item.gameObject.SetActive(checkToReveal);
                }

                if (hideSelf)
                {
                    hrManager.AddHideRevealChange(this.gameObject.name, false);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
