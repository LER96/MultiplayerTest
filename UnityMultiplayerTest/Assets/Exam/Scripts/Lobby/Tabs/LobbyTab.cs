using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyTab : MonoBehaviourPunCallbacks
{
    [SerializeField] protected GameObject _tabPanel;
    [SerializeField] protected Button tabBTN;
    [SerializeField] protected TMP_InputField _inputField;
    
    protected virtual void Start()
    {
        SetDropDown();
    }
    
    protected virtual void SetDropDown()
    {
        
    }
}
