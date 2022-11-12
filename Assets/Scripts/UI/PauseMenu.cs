using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


namespace TheEscapeArtist
{
    // Based on GamesPlusJames's implementation of a pause menu and loading screen
    // https://www.udemy.com/course/unitymenus/learn/lecture/15477106#overview
    // https://www.udemy.com/course/unitymenus/learn/lecture/15477110#overview
    public class PauseMenu : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private ScreenBlur screenBlur;

        [SerializeField] private CharacterController playerController;

        [SerializeField] private string mainMenuSceneName;

        [SerializeField] private GameObject optionsScreen, pauseScreen;

        [SerializeField] private GameObject pauseMenuReminder;

        [SerializeField] private GameObject loadingScreen, loadingIcon;

        [SerializeField] private TMP_Text loadingText;

        #endregion

        #region Private Fields

        public bool IsPaused { get; private set; }

        // private bool isInOptions = false;

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static PauseMenu _instance;

        public static PauseMenu Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one PauseMenu Singleton!");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        #endregion

        #region Public Methods

        public void PauseUnpause()
        {
            if (!IsPaused)
            {
                IsPaused = true;

                playerController.enabled = false;
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.Confined;
                pauseMenuReminder.SetActive(false);
                screenBlur.ToggleBlur(true);

                pauseScreen.SetActive(true);
            }
            else
            {
                pauseScreen.SetActive(false);

                screenBlur.ToggleBlur(false);
                pauseMenuReminder.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                playerController.enabled = true;

                IsPaused = false;
            }
        }

        public void OpenOptions()
        {
            // isInOptions = true;
            optionsScreen.SetActive(true);
        }

        public void CloseOptions()
        {
            optionsScreen.SetActive(false);
            // isInOptions = false;
        }

        public void QuitToMainMenu()
        {
            StartCoroutine(LoadMain());
        }

        #endregion

        #region Private Methods

        private IEnumerator LoadMain()
        {
            loadingScreen.SetActive(true);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainMenuSceneName);

            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= .9f)
                {
                    Time.timeScale = 1f;
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        #endregion
    }
}