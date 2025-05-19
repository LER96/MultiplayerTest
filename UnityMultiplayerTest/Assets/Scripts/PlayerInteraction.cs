using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;


public class PlayerInteraction : MonoBehaviourPunCallbacks
{
    public Transform holdPoint;
    public float interactRange = 2f;
    [SerializeField] private PickupBox heldBox;
    [SerializeField] private PickupBox nearbyBox;
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
        GameManager.Instance.View.RPC("RegisterPlayer", RpcTarget.AllBuffered, _view.ViewID);
        _view.Owner.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Score", 0 } });
    }

    public void OnEnterPickupZone(PickupBox box)
    {
        if (_view.IsMine)
        {
            if (heldBox == null)
            {
                nearbyBox = box;
                UIManager.Instance.ShowBoxMenu(true);
            }
        }
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
                //nearbyBox.Interact(this);
                if (_view.IsMine)
                {
                    nearbyBox.View.RPC("Interact", RpcTarget.AllViaServer, _view.ViewID);
                    UIManager.Instance.ShowButtonsForState(true);
                }
            }
        }
    }

    public void TryPass()
    {
        if (heldBox.Interactables.Count > 1)
        {
            heldBox.Pass(heldBox.CurrentHolder, heldBox.Interactables[1]);
        }
        else
        {
            TryDrop();
        }
    }

    public void TryDrop()
    {
        if (heldBox != null)
        {
            //heldBox.Interact(this);
            heldBox .View.RPC("Interact", RpcTarget.AllViaServer, _view.ViewID);
            UIManager.Instance.ShowButtonsForState(false);
            UIManager.Instance.HideEverything();
        }
    }
    
    public void AddCoins(int amount)
    {
        currentScore += amount;
        _view.Owner.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Score", currentScore } });
        UIManager.Instance.UpdateScore(currentScore);
    }
}