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
                pickupBox.AddInteractable(interaction);
                interaction.OnEnterPickupZone(pickupBox);
            }
            else if (interaction != null && pickupBox.taken)
            {
                pickupBox.AddInteractable(interaction);
                UIManager.Instance.ShowGive(true);
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
                UIManager.Instance.ShowGive(false);
                pickupBox.RemoveInteractable(interaction);
                interaction.OnExitPickupZone(pickupBox);
            }
        }
    }
}

