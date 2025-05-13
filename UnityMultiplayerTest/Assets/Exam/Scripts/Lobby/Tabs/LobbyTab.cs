using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

public class LobbyTab : MonoBehaviourPunCallbacks
{
    [SerializeField] protected GameObject _tabPanel;
    [SerializeField] protected Button _windowTab_BTN;
    [SerializeField] protected TMP_InputField _inputField;

    public GameObject TabPanel => _tabPanel;
    public Button TabButton => _windowTab_BTN;
    
    protected virtual void Start()
    {
        SetDropDown();
    }

    public virtual void OnTabSelected()
    {
        SetTab(true);
        LobbyManager.Instance.SetTabs(this);
    }

    public void SetTab(bool selected)
    {
        _tabPanel.SetActive(selected);
        _windowTab_BTN.interactable = !selected;
    }
    
    protected virtual void SetDropDown()
    {
        
    }
}
