using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

public class JoinTab : LobbyTab
{
    [FormerlySerializedAs("_dropDownJoinList")] [SerializeField] protected TMP_Dropdown dropDown;
    
    public TMP_Dropdown Dropdown => dropDown;
    public string JoinList { set=>AddToJoin(value);}
    
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_inputField.text);
    }
    
    protected override void SetDropDown()
    {
        dropDown.onValueChanged.AddListener(delegate { SetJoinInput(dropDown); });
    }

    void AddToJoin(string inputText)
    {
        dropDown.options.Add(new TMP_Dropdown.OptionData() { text = inputText});
    }
    
    public void SetJoinInput(TMP_Dropdown dropdown)
    {
        int i = dropdown.value;
        _inputField.text = dropdown.options[i].text;
        LobbyManager.Instance.RefreshUI();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        LobbyManager.Instance.RefreshUI();
        _windowTab_BTN.interactable = false;
        LobbyManager.Instance.LeaveRoomButton.interactable = true;
    }
    
}
