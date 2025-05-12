using UnityEngine;
    public  enum BoxStatus
    {
        Idle,
        PickedUp
    }

public abstract  class BoxBase : MonoBehaviour,IInteractable
{
    
    public BoxStatus Status { get; protected set; } = BoxStatus.Idle;
    
    public abstract void Interact(PlayerInteraction interactor);
    
    
}
