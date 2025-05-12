using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    [SerializeField]PickupBox pickupBox;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
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
            PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
            if (interaction != null)
            {
                interaction.OnExitPickupZone(pickupBox);
            }
        }
    }
}

