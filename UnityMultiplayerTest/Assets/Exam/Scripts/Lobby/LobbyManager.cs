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
    
    [Header("Log In")]
    [SerializeField] GameObject _logInFather;
    [SerializeField] Button _logInButton;

    [Header("Input Name")]
    [SerializeField] GameObject _inputNameFather;
    [SerializeField] TMP_InputField _nicknameInputField;
    [SerializeField] Button _enterRoomButton;

    [Header("Create/Join Options")]
    [SerializeField] GameObject _creatOrJoinFather;
    
    
    [Header("Tabs")]
    [SerializeField] private CreateTab _createTab;
    [SerializeField] private JoinTab _joinTab;
    [SerializeField] private FilterTab _filterTab;
    

    [Header("Filter ")]
    [SerializeField] GameObject _filterFather;
    [SerializeField] GameObject _roomsActiveFather;
    [SerializeField] TMP_Dropdown _dropDownRandomJoinList;
    [SerializeField] TMP_Dropdown _dropDownMaxPlayers;
    [SerializeField] Button _filterJoinTabButton;
    int _randomMaxPlayers;

    [Header("Info")]
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _leaveRoomButton;
    [SerializeField] TextMeshProUGUI _roomPlayersText;
    [SerializeField] TextMeshProUGUI _playerListText;

    List<RoomInfo> _roomsInfo = new List<RoomInfo>();
    List<string> _roomNames = new List<string>();
    List<string> _namesInGame = new List<string>();
    string _startInput;
    
    public CreateTab CreateTabWindow=> _createTab;
    public JoinTab JoinTabWindow => _joinTab;
    public FilterTab FilterTabWindow => _filterTab;
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

        //this.rounds = _numberOfRounds;
        _startInput = _nicknameInputField.text;
        _startGameButton.interactable = false;
        _leaveRoomButton.interactable = false;
        

        //Random DropDowns
        _dropDownMaxPlayers.onValueChanged.AddListener(delegate { SetRandomMaxPlayers(_dropDownMaxPlayers); });
        _dropDownRandomJoinList.onValueChanged.AddListener(delegate { SetRandomInput(_joinTab.DropdownJoinList); });

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        //If something is written in the input field
        if (_nicknameInputField.text != "" || _nicknameInputField.text != _startInput)
        {
            _enterRoomButton.interactable = true;
        }
        
        if (_createTab.CanCreateRoom)
        {
            _createTab.CreateRoomButton.interactable = true;
        }
        else
        {
            _createTab.CreateRoomButton.interactable = true;
        }
    }

    #region LogIn
    public void LoginToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();

        //Disable login and activate input
        _logInFather.SetActive(false);
        _inputNameFather.SetActive(true);
    }

    //On log in press, add to list of names, log in to photon
    public void TryConnect()
    {
        PhotonNetwork.NickName = _nicknameInputField.text;
        if (SameName(_nicknameInputField.text) == false)
        {
            _namesInGame.Add(_nicknameInputField.text);

            _inputNameFather.SetActive(false);
            _creatOrJoinFather.SetActive(true);
            CreateTabMenu();
        }
        else
        {
            _nicknameInputField.text = "";
            _enterRoomButton.interactable = false;
        }
    }

    //Check if there is otherPlayers with the same name
    bool SameName(string currentName)
    {
        foreach (Player playerName in PhotonNetwork.PlayerList)
        {
            _namesInGame.Clear();
            _namesInGame.Add(playerName.NickName);
        }

        if (_namesInGame.Count > 0)
        {
            foreach (string name in _namesInGame)
            {
                if (currentName.Equals(name))
                {
                    return true;
                }
            }
        }
        else
            return false;

        return false;
    }
    #endregion
    
    void StartUI()
    {
        _logInFather.SetActive(true);
        _inputNameFather.SetActive(false);
        _creatOrJoinFather.SetActive(false);
    }

    public void CreateTabMenu()
    {
        _filterJoinTabButton.interactable = true;
    }

    public void JoinTabMenu()
    {
        _filterFather.SetActive(false);
        _roomsActiveFather.SetActive(true);
        
        _filterJoinTabButton.interactable = true;
    }

    public void FilterTabMenu()
    {
        _filterFather.SetActive(true);
        _roomsActiveFather.SetActive(false);
        
        _filterJoinTabButton.interactable = false;
    }

    #region DropDown
    
    public void SetRandomInput(TMP_Dropdown dropdown)
    {
        RefreshUI();
        int i = dropdown.value;
        //_joinRoomNameInputField.text = dropdown.options[i].text;

        RefreshUI();
    }

    public void SetRandomMaxPlayers(TMP_Dropdown dropdown)
    {
        int i = dropdown.value;
        _randomMaxPlayers = int.Parse(dropdown.options[i].text);
        RefreshUI();
    }
    #endregion

    #region Join
    
    #endregion

    #region Condition Rooms

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.JoinLobby();
        //_joinTabRoomButton.interactable = true;
        _leaveRoomButton.interactable = false;
        RefreshUI();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        RefreshUI();

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == _createTab. MaxPlayers)
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
        _joinTab.DropdownJoinList.options.Clear();
        _dropDownRandomJoinList.options.Clear();

        _dropDownRandomJoinList.options.Add(new TMP_Dropdown.OptionData() { text = "None" });
        _joinTab.DropdownJoinList.options.Add(new TMP_Dropdown.OptionData() { text = "None" });
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
                    _joinTab.DropdownJoinList.options.Add(new TMP_Dropdown.OptionData() { text = room.Name });
                    
                    if(room.MaxPlayers== _randomMaxPlayers)
                    {
                        _dropDownRandomJoinList.options.Add(new TMP_Dropdown.OptionData() { text = room.Name });
                    }
                    
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

    //Replace masterClient
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }
}
