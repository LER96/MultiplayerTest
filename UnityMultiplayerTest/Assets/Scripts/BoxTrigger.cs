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
            
            if (interaction != null && !pickupBox.taken)
            {
                interaction.OnEnterPickupZone(pickupBox);
            }
            else if (interaction != null && pickupBox.taken)
            {
                UIManager.Instance.ShowGive(true);
                pickupBox.AddInteractable(interaction);
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
                pickupBox.RemoveInteractable(interaction);
                interaction.OnExitPickupZone(pickupBox);
            }
        }
    }
}

