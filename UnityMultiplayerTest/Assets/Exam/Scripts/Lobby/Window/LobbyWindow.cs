using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyWindow : MonoBehaviourPunCallbacks
{
    [SerializeField] protected Button _windowBtn;
    [SerializeField] protected LobbyWindow _nextWindow;

    public virtual void ActivateWindow()
    {
        
    }
}
