using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class JoinTab : LobbyTab
{
    [SerializeField] protected TMP_Dropdown _dropDownJoinList;
    
    public TMP_Dropdown DropdownJoinList => _dropDownJoinList;
    
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_inputField.text);
    }
    
    protected override void SetDropDown()
    {
        _dropDownJoinList.onValueChanged.AddListener(delegate { SetJoinInput(_dropDownJoinList); });
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
        tabBTN.interactable = false;
        LobbyManager.Instance.LeaveRoomButton.interactable = true;
    }
    
}
