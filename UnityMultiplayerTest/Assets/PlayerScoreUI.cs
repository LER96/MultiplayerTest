using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

public class PlayerScoreUI :MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text _playerName_Text;
    [SerializeField] TMP_Text _playerScore_Text;

    public void SetScore(Player player)
    {
        _playerName_Text.text=$"{player.NickName}";
        _playerScore_Text.text = $"{(int)player.CustomProperties["Score"]}";
    }
}
