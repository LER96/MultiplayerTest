using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System;
public  enum BoxStatus
    {
        Idle,
        PickedUp
    }

public abstract  class BoxBase : MonoBehaviourPunCallbacks ,IPunObservable,IInteractable
{
    
    public BoxStatus Status { get; protected set; } = BoxStatus.Idle;
    
    public abstract void Interact(PlayerInteraction interactor);


    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }
}
