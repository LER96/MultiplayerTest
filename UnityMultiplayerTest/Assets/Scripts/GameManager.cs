using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerInteraction _playerInteraction;
    private PhotonView _view;

    public static GameManager Instance { get; private set; }
    public PhotonView View => _view;
    public PlayerInteraction MyPlayerInteraction {get=> _playerInteraction; set=> _playerInteraction = value; }

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _view= GetComponent<PhotonView>();
        Instance = this;
    }
    
    [PunRPC]
    public void RegisterPlayer(int id)
    {
        PhotonView view = PhotonView.Find(id);
        if (view.IsMine)
        {
            PlayerInteraction player = view.GetComponent<PlayerInteraction>();
            _playerInteraction = player;
        }
    }
}
