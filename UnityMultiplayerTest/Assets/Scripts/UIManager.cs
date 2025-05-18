using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourPunCallbacks
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject boxMenuPanel; 

    [Header("GUI")]
    [SerializeField] private Button pickupButton;
    [SerializeField] private Button dropButton;
    [SerializeField] private Button giveButton;

    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text scoreText;


    [Header("Start Game")]
    [SerializeField] Button _startBTN;
    [SerializeField] GameObject _startWindow;

    [Header("Lead Score")]
    [SerializeField] GameObject _scoreBoard;
    [SerializeField] List<PlayerScoreUI> _playersUI = new List<PlayerScoreUI>();
    //[SerializeField] 

    //private const string ON_TIMER_END = nameof(TimerUP);
    [SerializeField]  private float _roomtimer, _currentTime;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _roomtimer = (int)PhotonNetwork.CurrentRoom.CustomProperties["Time"];
        Instance = this;
    }

    private void Start()
    {
        var myPlayerInteraction = GameManager.Instance.MyPlayerInteraction;
        //pickupButton.onClick.AddListener(myPlayerInteraction.TryPickup);
        //dropButton.onClick.AddListener(myPlayerInteraction.TryDrop);
    }

    private void Update()
    {
       if(SpawnManager.Instance.hasGameStarted)
        {
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        if (_currentTime < _roomtimer)
        {
            _currentTime += Time.deltaTime;
            _timerText.text = $"{Mathf.Ceil(_roomtimer - _currentTime)}";
        }
        else
        {
            //_currentTime = 0;
            //_timerText.text = $"{0}";
            SpawnManager.Instance.GameOver();
            ShowGameBoardScore();
        }
    }

    void ShowGameBoardScore()
    {
        _scoreBoard.SetActive(true);
        for (int i = 0; i < PhotonNetwork.CurrentRoom.Players.Count; i++)
        {
            _playersUI[i].gameObject.SetActive(true);
            Player player = PhotonNetwork.CurrentRoom.Players.Values.ElementAt(i);
            _playersUI[i].SetScore(player);
        }
    }

    public void SetStartGame(bool startGame)
    {
        _startWindow.SetActive(startGame);
        _startBTN.interactable = startGame;
    }

    private void ShowPickupButton(bool show)
    {
        pickupButton.gameObject.SetActive(show);
    }

    private void ShowDropButton(bool show)
    {
        dropButton.gameObject.SetActive(show);
    }

    private void ShowGiveButton(bool show)
    {
        giveButton.gameObject.SetActive(show);
    }
    
    public void ShowBoxMenu(bool show)
    {
        boxMenuPanel.SetActive(show);
    }


    public void HideAllBoxButtons()
    {
        ShowPickupButton(false);
        ShowDropButton(false);
        ShowGiveButton(false);
    }

    public void ShowButtonsForState(bool isHoldingBox)
    {
        ShowPickupButton(!isHoldingBox);
        ShowDropButton(isHoldingBox);
    }

    public void ShowGive(bool give)
    {
        ShowDropButton(!give);
        ShowPickupButton(!give);
        ShowGiveButton(give);
    }

    public void HideEverything()
    {
      //  HideAllBoxButtons();
        ShowBoxMenu(false);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}