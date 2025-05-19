using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public ExitGames.Client.Photon.Hashtable roomProperties;
    
    public static LobbyManager Instance;
    
    [Header("Handlers")]
    [SerializeField] private WindowHandler _windowHandler;
    
    [Header("Tabs")]
    [SerializeField] private CreateTab _createTab;
    [SerializeField] private JoinTab _joinTab;
    
    [Header("Info")]
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _leaveRoomButton;
    [SerializeField] TextMeshProUGUI _roomPlayersText;
    [SerializeField] TextMeshProUGUI _playerListText;

    List<RoomInfo> _roomsInfo = new List<RoomInfo>();
    List<string> _roomNames = new List<string>();
    string _startInput;

    public WindowHandler WindowHandler => _windowHandler;
    public JoinTab JoinTabWindow => _joinTab;
    public List<string> RoomNames => _roomNames;
    public Button LeaveRoomButton => _leaveRoomButton;
    

    //make start game button not interactable at the start of the game.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        StartUI();
        _startGameButton.interactable = false;
        _leaveRoomButton.interactable = false;
        
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        
        if (_createTab.CanCreateRoom)
        {
            _createTab.CreateRoomButton.interactable = true;
        }
        else
        {
            _createTab.CreateRoomButton.interactable = true;
        }
    }
    
    void StartUI()
    {
        _windowHandler.SetWindow(0);
        _createTab.OnTabSelected();
    }

    public void SetTabs(LobbyTab tab)
    {
        if (tab==_createTab)
        {
            _joinTab.SetTab(false);
        }
        else
        {
            _createTab.SetTab(false);
        }
    }
    


    #region Condition Rooms

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.JoinLobby();
        _leaveRoomButton.interactable = false;
        RefreshUI();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        RefreshUI();

        CheckRoom();
    }

    public void CheckRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == _createTab.MaxPlayers)
            {
                _startGameButton.interactable = true;
            }
        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        _startGameButton.interactable = false;
        RefreshUI();
    }
    #endregion

    #region On Rooms Update
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _roomsInfo.Clear();
        _roomsInfo = roomList;
        base.OnRoomListUpdate(roomList);
        ManageRooms(roomList);
    }

    //Clear the rooms list and orginize available rooms
    void ManageRooms(List<RoomInfo> roomList)
    {
        ResetDrop();
        UpdateAvailableRooms(roomList);
    }

    //Reset rooms list
    void ResetDrop()
    {
        _roomNames.Clear();
        _joinTab.Dropdown.options.Clear();
        _joinTab.JoinList="None";
    }

    //Orginize the room list drop down, checking if the current room isn't full
    void UpdateAvailableRooms(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (!room.RemovedFromList)
            {
                if (room.PlayerCount < room.MaxPlayers)
                {
                    _roomNames.Add(room.Name);
                    _joinTab.JoinList=room.Name;
                    
                }
            }
            else
            {
                if (_roomNames.Contains(room.Name))
                {
                    _roomNames.Remove(room.Name);
                }
            }
        }
    }
    #endregion

    public void RefreshUI()
    {
        ManageRooms(_roomsInfo);
        _playerListText.text = "";
        if (PhotonNetwork.InRoom)
        {
            foreach (Player photonPlayer in PhotonNetwork.PlayerList)
            {
                _playerListText.text += $"{photonPlayer.NickName} In the Room" + Environment.NewLine;
            }
        }
    }
    
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause == DisconnectCause.Exception || cause == DisconnectCause.ClientTimeout)
        {
            StartCoroutine(TryRejoin());
        }
    }

    private IEnumerator TryRejoin()
    {
        while (!PhotonNetwork.IsConnectedAndReady)
            yield return null;


        string roomName = (string)PhotonNetwork.LocalPlayer.CustomProperties["RoomName"];
        PhotonNetwork.RejoinRoom(roomName);
    }
}
