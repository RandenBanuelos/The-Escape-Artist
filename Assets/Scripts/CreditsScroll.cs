using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheEscapeArtist
{
    public class CreditsScroll : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private MainMenu menu;

        [SerializeField] private RectTransform scrollRect;

        [SerializeField] private GameObject godRays;

        [SerializeField] private AudioSource backgroundMusic;

        [SerializeField] private AudioSource creditsMusic;

        [SerializeField] private float scrollSpeed;

        #endregion

        #region Private Fields

        private bool canScroll = false;

        #endregion

        #region MonoBehaviour Callbacks

        private void FixedUpdate()
        {
            if (canScroll)
            {
                Vector2 newPosition = new Vector2(0f, scrollRect.anchoredPosition.y + scrollSpeed);
                scrollRect.anchoredPosition = newPosition;
            }
        }

        #endregion

        #region Public Methods

        public void Scroll()
        {
            ResetScroll();
            godRays.SetActive(false);
            backgroundMusic.Pause();
            creditsMusic.Play();
            canScroll = true;

            Invoke(nameof(BackToCredits), creditsMusic.clip.length);
        }

        public void StopScroll()
        {
            canScroll = false;
            godRays.SetActive(true);
            creditsMusic.Stop();
            backgroundMusic.Play();
        }

        #endregion

        #region Private Methods

        private void ResetScroll()
        {
            scrollRect.anchoredPosition = new Vector2(0f, -Screen.height);
        }

        private void BackToCredits()
        {
            menu.CloseCredits();
        }

        #endregion
    }
}
