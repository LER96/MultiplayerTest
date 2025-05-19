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
    
    [SerializeField] private int _numberOfPlayers;
    [SerializeField] private int _timerForRound = 1;
    private bool _numberOfPlayersCheck;

    public Button CreateRoomButton => _createRoom;
    public bool CanCreateRoom => _numberOfPlayersCheck;
    public int MaxPlayers => _numberOfPlayers;
    
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("We are in a room!");
        LobbyManager.Instance.roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Time", _timerForRound } });
        LobbyManager.Instance.CheckRoom();
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
                sameName = true;
            }
        }

        //If there isn't a room with the same name, then cre
        if (sameName == false && _numberOfPlayers >= 1 && _timerForRound > 1) 
        {
            PhotonNetwork.CreateRoom(_inputField.text, new RoomOptions() { MaxPlayers = _numberOfPlayers, EmptyRoomTtl = 3000, IsVisible = true, IsOpen = true, CleanupCacheOnLeave = false},null);
            LobbyManager.Instance.JoinTabWindow.JoinList=_inputField.text;
            
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
        if (i==0)
        {
            _timerForRound = 1;
        }
        else
        {
            _timerForRound = int.Parse(dropdown.options[i].text);
        }
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        _createRoom.interactable = true;
    }
    
}
