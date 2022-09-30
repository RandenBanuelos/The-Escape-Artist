using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheEscapeArtist
{
    // Based on GamesPlusJames's implementation of a main menu
    // https://www.youtube.com/watch?v=76WOa6IU_s8
    public class MainMenu : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private string firstLevelName;

        [SerializeField] private GameObject optionsScreen;

        #endregion

        #region Public Methods

        public void StartGame()
        {
            SceneManager.LoadScene(firstLevelName);
        }

        public void OpenOptions()
        {
            optionsScreen.SetActive(true);
        }

        public void CloseOptions()
        {
            optionsScreen.SetActive(false);
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Quitting...");
        }

        #endregion
    }
}
