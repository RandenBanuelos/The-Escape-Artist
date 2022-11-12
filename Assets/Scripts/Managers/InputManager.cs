using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheEscapeArtist
{
    public class InputManager : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private InventoryItem stereoscope;

        [SerializeField] private InventoryItem pocketWatch;

        [SerializeField] private GameObject pauseReminder;

        #endregion

        #region Singleton Caches

        private InventoryManager inventoryCache;

        private PauseMenu pauseMenuCache;

        private ItemInspector inspectorCache;

        private StereoscopeView stereoscopeCache;

        private InteractionController interactionCache;

        private PocketWatch pocketWatchCache;

        private PuzzleCubeManager puzzleCubeManagerCache;

        private bool collectedAllCaches = false;

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            if (!collectedAllCaches)
            {
                CollectCaches();
                return;
            }

            if (puzzleCubeManagerCache.PuzzleCubeIsOpen && !pauseMenuCache.IsPaused && !inspectorCache.IsInspecting && !stereoscopeCache.IsViewing)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Debug.Log("InputManager.Update(): Closing puzzle cube...");
                    puzzleCubeManagerCache.ClosePuzzleCube();
                }
            }

            else if (!puzzleCubeManagerCache.PuzzleCubeIsOpen && !inspectorCache.IsInspecting && !stereoscopeCache.IsViewing)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Debug.Log("InputManager.Update(): Opening / closing pause menu...");
                    pauseMenuCache.PauseUnpause();
                }
            }

            if (!pauseMenuCache.IsPaused)
            {
                if (!inspectorCache.IsInspecting)
                {
                    if (inventoryCache.IsInInventory(stereoscope) && !inventoryCache.IsInInventory(pocketWatch))
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            if (!stereoscopeCache.IsViewing)
                            {
                                stereoscopeCache.OpenView();
                            }
                            else
                            {
                                StartCoroutine(stereoscopeCache.CloseView());
                            }
                        }
                    }
                    else if (inventoryCache.IsInInventory(pocketWatch) && !inventoryCache.IsInInventory(stereoscope))
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            pocketWatchCache.TimeShift();
                        }
                    }
                }
            }

            if (stereoscopeCache.IsViewing)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    StartCoroutine(stereoscopeCache.NextSlide());
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("InputManager.Update(): Toggling pause reminder...");
                pauseReminder.SetActive(!pauseReminder.activeSelf);
            }
        }

        #endregion

        #region Private Methods

        private void CollectCaches()
        {
            if (!inventoryCache)
                inventoryCache = InventoryManager.Instance;

            if (!pauseMenuCache)
                pauseMenuCache = PauseMenu.Instance;

            if (!inspectorCache)
                inspectorCache = ItemInspector.Instance;

            if (!stereoscopeCache)
                stereoscopeCache = StereoscopeView.Instance;

            if (!interactionCache)
                interactionCache = InteractionController.Instance;

            if (!pocketWatchCache)
                pocketWatchCache = PocketWatch.Instance;

            if (!puzzleCubeManagerCache)
                puzzleCubeManagerCache = PuzzleCubeManager.Instance;

            if (inventoryCache && pauseMenuCache && inspectorCache && stereoscopeCache && interactionCache && pocketWatchCache && puzzleCubeManagerCache)
                collectedAllCaches = true;
        }

        #endregion
    }
}
