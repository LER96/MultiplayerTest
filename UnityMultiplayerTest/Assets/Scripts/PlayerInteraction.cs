using Photon.Pun;
using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviourPunCallbacks
{
    public Transform holdPoint;
    public float interactRange = 2f;
    private PickupBox heldBox;
    private PickupBox nearbyBox;
    private int currentScore = 0;
    private PhotonView _view;
    
    public void HoldBox(PickupBox box) => heldBox = box;
    public void DropBox() => heldBox = null;
    public bool IsHolding(PickupBox box) => heldBox == box;
    public bool Holding => heldBox != null;
    public PhotonView View => _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        if (_view.IsMine)
            GameManager.Instance.RegisterPlayer(this);
    }

    public void OnEnterPickupZone(PickupBox box)
    {
        if (heldBox is null)
        {
            nearbyBox = box;
            UIManager.Instance.ShowBoxMenu(true);
        }
    }

    public void OnEnterDeliverZone(PickupBox box)
    {
        UIManager.Instance.ShowBoxMenu(true);
    }

    public void OnExitPickupZone(PickupBox box)
    {
        if (IsHolding(box)) return;
        nearbyBox = null;
        UIManager.Instance.HideEverything();
    }

    public void TryPickup()
    {
        if (nearbyBox != null)
        {
            if (nearbyBox.taken == false)
            {
                nearbyBox.Interact(this);
                UIManager.Instance.ShowButtonsForState(true);
            }
            else
            {
                nearbyBox.Interact(this);
                UIManager.Instance.ShowGive(true);
            }
        }
    }

    public void TryDrop()
    {
        if (heldBox != null)
        {
            heldBox.Interact(this);
            UIManager.Instance.ShowButtonsForState(false);
            UIManager.Instance.HideEverything();
        }
    }
    
    public void AddCoins(int amount)
    {
        currentScore += amount;
        UIManager.Instance.UpdateScore(currentScore);
    }
}