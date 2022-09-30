using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftCabinet : MonoBehaviour
{
    private class Cabinet
    {
        public Transform cabinetObj;
        public float extendAmount;
        public float timeToExtend;
        public float timer;
        public bool extendBackwards;
    }

    #region Private Serializable Fields

    [SerializeField] private List<Transform> cabinets = new List<Transform>();

    [SerializeField] private float originalExtension = -0.1113708f;

    [SerializeField] private float maxExtendAmount = -0.332f;

    [SerializeField] private float extendMinTime = 0.1f;

    [SerializeField] private float extendMaxTime = 1f;

    [SerializeField] [Range(0, 18)] private int minNumberOfCabinetsToOpen = 5;

    [SerializeField] [Range(0, 18)] private int maxNumberOfCabinetsToOpen = 18;

    #endregion

    #region Private Fields

    private Transform mainCabinet;

    private List<Cabinet> cabinetsToOpen = new List<Cabinet>();

    #endregion

    #region MonoBehaviourCallbacks

    private void Start()
    {
        if (minNumberOfCabinetsToOpen > maxNumberOfCabinetsToOpen)
            Debug.LogError("ShiftCabinet.Start: The minimum number of cabinets to open is larger than the maximum!");

        int numberOfCabinetsToOpen = Random.Range(minNumberOfCabinetsToOpen, maxNumberOfCabinetsToOpen + 1);

        List<Transform> cabinetsCopy = cabinets;

        for (int i = 0; i < numberOfCabinetsToOpen; i++)
        {
            Cabinet newCabinet = new Cabinet();
            Transform cabinetObjToAdd = cabinetsCopy[Random.Range(0, cabinetsCopy.Count)];
            newCabinet.cabinetObj = cabinetObjToAdd;
            newCabinet.timeToExtend = Random.Range(extendMinTime, extendMaxTime);
            newCabinet.extendAmount = Mathf.Lerp(originalExtension, maxExtendAmount, newCabinet.timeToExtend);
            newCabinet.timer = 0f;
            newCabinet.extendBackwards = false;

            cabinetsToOpen.Add(newCabinet);
            cabinetsCopy.Remove(cabinetObjToAdd);
        }
    }

    private void Update()
    {
        foreach (Cabinet cabinet in cabinetsToOpen)
        {
            float newPositionX;

            if (!cabinet.extendBackwards)
                newPositionX = Mathf.Lerp(originalExtension, cabinet.extendAmount, cabinet.timer);
            else
                newPositionX = Mathf.Lerp(cabinet.extendAmount, originalExtension, cabinet.timer);

            cabinet.cabinetObj.localPosition = new Vector3(newPositionX, cabinet.cabinetObj.localPosition.y, cabinet.cabinetObj.localPosition.z);

            cabinet.timer += cabinet.timeToExtend * Time.deltaTime;

            if (cabinet.timer > 1.0f)
            {
                cabinet.timer = 0f;
                cabinet.extendBackwards = !cabinet.extendBackwards;
            }
        }
    }

    #endregion
}
