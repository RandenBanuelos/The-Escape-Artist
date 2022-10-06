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

        #endregion

        #region Singleton Caches

        private InventoryManager inventoryCache;

        private MenuManager menuCache;

        private ItemInspector inspectorCache;

        private StereoscopeView stereoscopeCache;

        private InteractionController interactionCache;

        private PocketWatch pocketWatchCache;

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

            if (!menuCache.IsInMenu)
            {
                if (!inspectorCache.IsInspecting)
                {
                    if (inventoryCache.IsInInventory(stereoscope) && !inventoryCache.IsInInventory(pocketWatch))
                    {
                        if (Input.GetKeyDown(KeyCode.V))
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
                        if (Input.GetKeyDown(KeyCode.V))
                        {
                            pocketWatchCache.TimeShift();
                        }
                    }
                }
            }

            if (stereoscopeCache.IsViewing)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    StartCoroutine(stereoscopeCache.NextSlide());
                }
            }

            if (!inspectorCache.IsInspecting && !stereoscopeCache.IsViewing)
            {
                /*if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (menuCache.IsInMenu)
                        menuCache.ClosePauseMenu();
                    else
                        menuCache.OpenPauseMenu();
                }*/
            }
        }

        #endregion

        #region Private Methods

        private void CollectCaches()
        {
            if (!inventoryCache)
                inventoryCache = InventoryManager.Instance;

            if (!menuCache)
                menuCache = MenuManager.Instance;

            if (!inspectorCache)
                inspectorCache = ItemInspector.Instance;

            if (!stereoscopeCache)
                stereoscopeCache = StereoscopeView.Instance;

            if (!interactionCache)
                interactionCache = InteractionController.Instance;

            if (!pocketWatchCache)
                pocketWatchCache = PocketWatch.Instance;

            if (inventoryCache && menuCache && inspectorCache && stereoscopeCache && interactionCache && pocketWatchCache)
                collectedAllCaches = true;
        }

        #endregion
    }
}
