using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace TheEscapeArtist
{
    // Based on GamesPlusJames's implementation of a main menu and loading screen
    // https://www.youtube.com/watch?v=76WOa6IU_s8
    // https://www.udemy.com/course/unitymenus/learn/lecture/15477110#overview
    public class MainMenu : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private string firstLevelName;

        [SerializeField] private GameObject optionsScreen;

        [SerializeField] private GameObject confirmNewGameScreen;

        [SerializeField] private GameObject loadingScreen, loadingIcon;

        [SerializeField] private GameObject continueButton;

        [SerializeField] private TMP_Text loadingText;

        #endregion

        #region Private Fields

        private bool hasStartedGameBefore = false;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (loadingScreen.activeSelf)
                loadingScreen.SetActive(false);

            hasStartedGameBefore = ES3.Load<bool>("hasStartedGameBefore", false);

            if (hasStartedGameBefore)
                continueButton.SetActive(true);
        }

        #endregion

        #region Public Methods

        public void StartGame()
        {
            if (hasStartedGameBefore)
            {
                OpenConfirmNewGameScreen();
            }
            else
            {
                ES3.Save("hasStartedGameBefore", true);
                ContinueGame();
            }
        }

        public void ContinueGame()
        {
            StartCoroutine(LoadLevel());
        }

        public void NewGame()
        {
            string[] keys = ES3.GetKeys();
            foreach (string key in keys)
            {
                ES3.DeleteKey(key);
            }

            ES3.Save("hasStartedGameBefore", true);
            ContinueGame();
        }

        public void CloseConfirmNewGameScreen()
        {
            confirmNewGameScreen.SetActive(false);
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
            Debug.Log("Quitting...");
            Application.Quit();
        }

        #endregion

        #region Private Methods

        private void OpenConfirmNewGameScreen()
        {
            confirmNewGameScreen.SetActive(true);
        }

        private IEnumerator LoadLevel()
        {
            loadingScreen.SetActive(true);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(firstLevelName);

            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= .9f)
                {
                    loadingText.text = "PRESS ANY KEY TO CONTINUE";
                    loadingIcon.SetActive(false);

                    if (Input.anyKeyDown)
                    {
                        Time.timeScale = 1f;
                        asyncLoad.allowSceneActivation = true;
                    }
                }

                yield return null;
            }
        }

        #endregion
    }
}
