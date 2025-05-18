using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinSpawnManager :MonoBehaviourPunCallbacks
{
    [Header("References")]
    [SerializeField] private List<CoinInteractableObject> interactableCoins;

    [Header("Spawn settings")]
    [Tooltip("The amount of active coin objects allowed to be active in the scene")]
    [SerializeField] private int activeAmountAllowed;
    [SerializeField] private float respawnTimer;
    [SerializeField] private int minAmountToSpawn = 1;
    [SerializeField] private int maxAmountToSpawn = 1;


    private float _spawnCounter;
    ///Rename?, 
    private bool _isFull => GetAmountOfActiveInteractableCoins() >= activeAmountAllowed;

    private void Awake()
    {
        Init();
    }


    private void Update()
    {
        if (_isFull)
            return;

        _spawnCounter -= Time.deltaTime;
        if (_spawnCounter <= 0)
        {
            SpawnCoins();
            ResetCounter();
        }
    }

    private void Init()
    {
        ResetCounter();
        DisableAllCoins();
    }
    private void DisableAllCoins()
    {
        foreach (var coin in interactableCoins)
        {
            coin.DisableCoin();
        }
    }

    private void ResetCounter()
    {
        _spawnCounter = respawnTimer;
    }
    private void SpawnCoins()
    {
        List<CoinInteractableObject> coinsToSpawn = GetRandomUnactiveCoinObjects();

        foreach (var coin in coinsToSpawn)
        {
            coin.SpawnCoin();
        }
    }




    private List<CoinInteractableObject> GetRandomUnactiveCoinObjects()
    {
        int amountToSpawn = GetAmountToSpawn();

        List<CoinInteractableObject> coinObjects = new List<CoinInteractableObject>();

        for (int i = 0; i < amountToSpawn; i++)
        {
            coinObjects.Add( GetRandomUnactiveCoinObject());
        }

        return coinObjects;
    }
    private CoinInteractableObject GetRandomUnactiveCoinObject()
    {
        List<CoinInteractableObject> unactiveCoins = interactableCoins.ToList();
        unactiveCoins.RemoveAll(coin => coin.IsActive);

        int listLength = unactiveCoins.Count;

        CoinInteractableObject randomizedCoin = unactiveCoins[Random.Range(0,listLength)];

        return randomizedCoin;
    }

    private int GetAmountToSpawn()
    {
        int randomAmount = Random.Range(minAmountToSpawn, maxAmountToSpawn);
        int leftover = (GetAmountOfActiveInteractableCoins() + randomAmount) - activeAmountAllowed;
        Debug.Log($"Random amount: {randomAmount} the leftover {leftover}");
        if (leftover > 0)
        {
            randomAmount -= leftover;
        }

        return randomAmount;
    }
    private int GetAmountOfActiveInteractableCoins()
    {
        int amount = 0;

        foreach (var coin in interactableCoins)
        {
            if (coin.IsActive)
            {
                amount++;
            }
        }

        return amount;
    }

}
