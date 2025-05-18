using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    private bool _gameStarted;

    public static GameManager Instance { get; private set; }
    public bool GameStarted => _gameStarted;

    public PlayerInteraction MyPlayerInteraction { get; private set; }

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void RegisterPlayer(PlayerInteraction player)
    {
        MyPlayerInteraction = player;
    }
}
