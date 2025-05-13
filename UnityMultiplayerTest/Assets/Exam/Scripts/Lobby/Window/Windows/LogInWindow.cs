using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LogInWindow : LobbyWindow
{
    [SerializeField] TMP_InputField _nicknameInputField;
    public override void ActivateWindow()
    {
        if (SameName(_nicknameInputField.text) == false)
        {
            PhotonNetwork.NickName = _nicknameInputField.text;
            LobbyManager.Instance.WindowHandler.SwitchWindow(_nextWindow);
        }
        else
        {
            _nicknameInputField.text = "";
        }
    }
    
    bool SameName(string currentName)
    {
        foreach (Player playerName in PhotonNetwork.PlayerList)
        {
            if (currentName == playerName.NickName)
                return true;
        }

        return false;
    }
}
