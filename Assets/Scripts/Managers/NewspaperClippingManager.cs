using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TheEscapeArtist
{
    public class NewspaperClippingManager : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private ScreenBlur screenBlur;

        [SerializeField] private GameObject newspaperPanel;

        [SerializeField] private TMP_Text headline;

        [SerializeField] private TMP_Text date;

        [SerializeField] private TMP_Text content;

        #endregion

        #region Private Fields

        private bool newspaperIsActive = false;

        private RectTransform contentRect;

        private Outline outline;

        #endregion

        #region Singleton
        // Singleton pattern from user PearsonArtPhoto on StackExchange
        // https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        private static NewspaperClippingManager _instance;

        public static NewspaperClippingManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Cannot have more than one NewspaperClippingManager Singleton!");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        #endregion

        #region "Getter's"

        public bool NewspaperIsActive => newspaperIsActive;

        #endregion

        #region Public Methods

        public void ShowNewspaper(NewspaperClipping clipping)
        {
            newspaperIsActive = true;
            screenBlur.ToggleBlur(true);
            newspaperPanel.SetActive(true);
            headline.text = clipping.Headline;
            date.text = clipping.Date;
            content.text = clipping.Content;

            if (!contentRect)
                contentRect = content.GetComponentInParent<RectTransform>();
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, 0f);

            Cursor.lockState = CursorLockMode.Confined;
        }

        public void HideNewspaper()
        {
            Cursor.lockState = CursorLockMode.Locked;
            headline.text = "";
            date.text = "";
            content.text = "";
            newspaperPanel.SetActive(false);
            screenBlur.ToggleBlur(false);
            newspaperIsActive = false;
        }

        #endregion
    }
}
