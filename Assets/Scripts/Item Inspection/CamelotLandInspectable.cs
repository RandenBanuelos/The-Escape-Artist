using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    public class CamelotLandInspectable : InteractableBase
    {
        #region Private Serializable Fields

        [SerializeField] private CharacterController playerController;

        [SerializeField] private Transform resetPosition;

        [SerializeField] private Animator leverAnim;

        [SerializeField] private GameObject blackout;

        [SerializeField] private float cooldown = 1f;

        [SerializeField] private float lightsOffTimer = 1f;

        [SerializeField] private List<GameObject> rides = new List<GameObject>();

        #endregion

        #region Private Fields

        private int currentRideIndex = 0;

        private GameObject currentRide;

        private bool onCooldown = false;

        private bool lightsOff = false;

        private float cooldownTimer = 0f;

        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            if (rides.Count > 0)
            {
                currentRideIndex = 0;
                currentRide = rides[currentRideIndex];
                DisableAllRides();
                currentRide.SetActive(true);
            }
        }

        private void Update()
        {
            if (onCooldown && !lightsOff)
            {
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer > cooldown)
                {
                    cooldownTimer = 0f;
                    onCooldown = false;
                }
            }
        }

        #endregion

        public override void OnInteract()
        {
            if (!onCooldown)
            {
                base.OnInteract();

                if (rides.Count == 0)
                {
                    Debug.LogError("CamelotLandInspectable.OnInteract(): No rides to enable / disable!");
                    return;
                }

                onCooldown = true;
                leverAnim.SetTrigger("Flip");
                leverAnim.ResetTrigger("Flip");

                blackout.SetActive(true);

                playerController.enabled = false;
                playerController.GetComponentInParent<Transform>().position = resetPosition.position;

                currentRideIndex = (currentRideIndex + 1) % rides.Count;
                currentRide = rides[currentRideIndex];
                DisableAllRides();
                currentRide.SetActive(true);

                Invoke(nameof(LightsOn), lightsOffTimer);
            }
        }

        private void LightsOn()
        {
            playerController.enabled = true;
            blackout.SetActive(false);
        }

        private void DisableAllRides()
        {
            foreach (GameObject ride in rides)
                ride.SetActive(false);
        }
    }
}
