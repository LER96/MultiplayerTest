using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxTrigger : MonoBehaviourPunCallbacks
{
    [SerializeField]PickupBox pickupBox;
    [SerializeField] PlayerInteraction interaction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interaction = other.GetComponent<PlayerInteraction>();
            if (interaction != null)
            {
                interaction.OnEnterPickupZone(pickupBox);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interaction = other.GetComponent<PlayerInteraction>();
            if (interaction != null)
            {
                interaction.OnExitPickupZone(pickupBox);
            }
        }
    }
}

