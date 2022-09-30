using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    #region Private Serializable Fields

    [SerializeField] private Camera cameraA;

    [SerializeField] private Material cameraMaterialA;

    [SerializeField] private Camera cameraB;

    [SerializeField] private Material cameraMaterialB;

    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        if (cameraA.targetTexture != null)
        {
            cameraA.targetTexture.Release();
        }
        cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMaterialA.mainTexture = cameraA.targetTexture;

        if (cameraB.targetTexture != null)
        {
            cameraB.targetTexture.Release();
        }
        cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMaterialB.mainTexture = cameraB.targetTexture;
    }

    #endregion
}
