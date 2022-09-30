using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideReveal : MonoBehaviour
{
    [SerializeField] private bool checkToReveal = false;

    [SerializeField] private bool hideSelf = true;

    [SerializeField] private List<Transform> thingsToHide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!checkToReveal)
            {
                foreach (Transform item in thingsToHide)
                    item.gameObject.SetActive(false);

                if (hideSelf)
                    this.gameObject.SetActive(false);
            }
            else
            {
                foreach (Transform item in thingsToHide)
                    item.gameObject.SetActive(true);
            }
        }
    }
}
