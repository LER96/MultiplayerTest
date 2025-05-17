using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System;

public class PickupBox : BoxBase
{
    
    private Transform originalParent;
    
    [SerializeField]private Rigidbody originalRigidBody;
    [SerializeField] private PlayerInteraction _currentInteractor;
    public bool taken;

    private const string ON_DROP_RPC = nameof(Drop);
    private const string ON_PICKUP_RPC= nameof(PickUp);

    private PhotonView _view;
    private Vector3 _position;

    public PlayerInteraction CurrentHolder=> _currentInteractor;
    private void Awake()
    {
        originalParent = transform.parent;
    }

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!_view.IsMine)
            transform.position = Vector3.Lerp(transform.position, _position, Time.deltaTime);
    }

    

    public override void Interact(PlayerInteraction interactor)
    {
        if (Status == BoxStatus.Idle)
        {
            //_view.RPC(ON_PICKUP_RPC, RpcTarget.All, interactor);
            PickUp(interactor);
        }
        else if (Status == BoxStatus.PickedUp && interactor.IsHolding(this))
        {
            //_view.RPC(ON_DROP_RPC, RpcTarget.All, interactor);
            Drop(interactor);
        }
        else if(Status == BoxStatus.PickedUp && interactor.IsHolding(this)==false)
        {
            Drop(_currentInteractor);
            PickUp(interactor);
        }
    }

    private void PickUp(PlayerInteraction interactor)
    {
        Status = BoxStatus.PickedUp;
        transform.SetParent(interactor.holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        originalRigidBody.isKinematic = true;
        _currentInteractor = interactor;
        taken = true;
        interactor.View.RequestOwnership();
        interactor.HoldBox(this);
    }

    private void Drop(PlayerInteraction interactor)
    {
        Status = BoxStatus.Idle;
        transform.SetParent(originalParent);
        originalRigidBody.isKinematic = false;
        interactor.DropBox();
        taken = false;
        _currentInteractor = null;
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(taken);
            stream.SendNext(transform.position);
            stream.SendNext(originalRigidBody.isKinematic);
        }
        else
        {
            taken = (bool)stream.ReceiveNext();
            _position = (Vector3)stream.ReceiveNext();
            originalRigidBody.isKinematic= (bool)stream.ReceiveNext();
        }
    }

}