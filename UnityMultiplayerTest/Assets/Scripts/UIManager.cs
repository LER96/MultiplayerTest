using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject boxMenuPanel;

    [Header("GUI")]
    [SerializeField] private Button pickupButton;
    [SerializeField] private Button dropButton;
    [SerializeField] private Button giveButton;

    [SerializeField] private TMP_Text scoreText;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        var myPlayerInteraction = GameManager.Instance.MyPlayerInteraction;
        pickupButton.onClick.AddListener(myPlayerInteraction.TryPickup);
        dropButton.onClick.AddListener(myPlayerInteraction.TryDrop);
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
        ShowDropButton(give);
        ShowPickupButton(!give);
        ShowGiveButton(!give);
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