using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    #region Private Serializable Fields

    [SerializeField] private Transform player;

    [SerializeField] private CharacterController charController;

    [SerializeField] private Transform receiver;

    #endregion

    #region Private Fields

    private bool playerIsOverlapping = false;

    #endregion

    #region MonoBehaviour Callbacks

    private void FixedUpdate()
    {
        if (playerIsOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // True when player has moved across the portal
            if (dotProduct < 0f)
            {
                Debug.Log($"Teleporting from {transform.name} to {receiver.name}...");
                float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
                rotationDiff += 180f;

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;

                charController.enabled = false;
                player.Rotate(Vector3.up, rotationDiff);
                player.position = receiver.position + positionOffset;
                charController.enabled = true;

                playerIsOverlapping = false;
            }
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }

    #endregion
}
