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
    
    [SerializeField] List<PlayerInteraction> _interactables = new List<PlayerInteraction>();
    public bool taken;

    private PhotonView _view;
    private Vector3 _position;

    public PhotonView View=> _view;
    public PlayerInteraction CurrentHolder=> _currentInteractor;
    public List<PlayerInteraction> Interactables => _interactables;
    
    
    private void Awake()
    {
        originalParent = transform.parent;
    }

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }
    
    public void AddInteractable(PlayerInteraction interactor)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!_interactables.Contains(interactor))
            {
                _interactables.Add(interactor);
            }
        }
    }

    public void RemoveInteractable(PlayerInteraction interactor)
    {
        if (_interactables.Contains(interactor))
        {
            _interactables.Remove(interactor);
        }
    }

    public void Pass(PlayerInteraction holder, PlayerInteraction interactor)
    {
       Drop(holder);
       PickUp(interactor);
    }
    

    //private void Update()
    //{
    //    if (!_view.IsMine)
    //        transform.position = Vector3.Lerp(transform.position, _position, .1f);

        
    //}

    [PunRPC]
    public override void Interact(int id)
    {
        PhotonView playerView= PhotonView.Find(id);
        PlayerInteraction interactor = playerView.GetComponent<PlayerInteraction>();
        if (Status == BoxStatus.Idle && taken==false)
        {
            PickUp(interactor);
        }
        else if (Status == BoxStatus.PickedUp && interactor.IsHolding(this))
        {
            Drop(interactor);
        }
    }

    public void PickUp(PlayerInteraction interactor)
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

    public void Drop(PlayerInteraction interactor)
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
            //_position = (Vector3)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();
            originalRigidBody.isKinematic= (bool)stream.ReceiveNext();
        }
    }

}