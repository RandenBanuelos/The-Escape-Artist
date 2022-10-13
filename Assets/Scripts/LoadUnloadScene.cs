using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheEscapeArtist
{
    public class LoadUnloadScene : MonoBehaviour
    {
        [SerializeField] private string sceneToLoadUnload = "";

        [SerializeField] private List<GameObject> associatedColliders = new List<GameObject>();

        [SerializeField] private bool checkToUnload = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if (checkToUnload)
                {
                    DontDestroyOnLoad(other.gameObject);
                    SceneManager.UnloadSceneAsync(sceneToLoadUnload);
                }
                else
                {
                    SceneManager.LoadSceneAsync(sceneToLoadUnload, LoadSceneMode.Additive);
                }

                if (associatedColliders.Count > 0)
                {
                    foreach (GameObject c in associatedColliders)
                        c.SetActive(false);
                }

                this.gameObject.SetActive(false);
            }
        }
    }
}
