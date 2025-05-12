using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

public class CreateTab : LobbyTab
{
    [SerializeField] TMP_Dropdown _dropDownPlayersNumberList;
    [SerializeField] TMP_Dropdown _dropDownTimerForRound;
    [SerializeField] Button _createRoom;
    
    private int _numberOfPlayers;
    private int _timerForRound = 1;
    private bool _numberOfPlayersCheck;
    private TMP_Dropdown _joinDropdown;

    public Button CreateRoomButton => _createRoom;
    public bool CanCreateRoom => _numberOfPlayersCheck;
    public int MaxPlayers => _numberOfPlayers;
    
    private void Start()
    {
        SetDropDown();
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("We are in a room!");
        LobbyManager.Instance.roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Time", _timerForRound } });
        LobbyManager.Instance.RefreshUI();
    }
    
    public void CreateRoom()
    {
        bool sameName = false;
        foreach (string roomName in LobbyManager.Instance.RoomNames)
        {
            if (roomName == _inputField.text)
            {
                _inputField.text = "";
            }
        }

        //If there isn't a room with the same name, then cre
        if (sameName == false)
        {
            PhotonNetwork.CreateRoom(_inputField.text, new RoomOptions() { MaxPlayers = _numberOfPlayers, EmptyRoomTtl = 2000 },
                null);
            _joinDropdown = LobbyManager.Instance.JoinTabWindow.DropdownJoinList;
            _joinDropdown.options.Add(new TMP_Dropdown.OptionData() { text = _inputField.text });
        }
        else
        {
            _inputField.text = "";
        }

    }

    protected override void SetDropDown()
    {
        _dropDownPlayersNumberList.onValueChanged.AddListener(delegate { SetCreateInput(_dropDownPlayersNumberList); });// set the number of players in a room
        _dropDownTimerForRound.onValueChanged.AddListener(delegate { SetRoundsDropDown(_dropDownTimerForRound); });
    }
    
    void SetCreateInput(TMP_Dropdown dropdown)
    {
        int i = dropdown.value;
        if (dropdown.options[i].text != "None")
        {
            _numberOfPlayers = int.Parse(dropdown.options[i].text);
            _numberOfPlayersCheck = true;
        }
        else
        {
            _numberOfPlayersCheck = false;
        }
    }
    
    void SetRoundsDropDown(TMP_Dropdown dropdown)
    {
        int i = dropdown.value;
        _timerForRound = int.Parse(dropdown.options[i].text);
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        _createRoom.interactable = true;
    }
    
}
