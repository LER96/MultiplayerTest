using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInteractableObject : MonoBehaviour
{
[   Header("Coin Setting")]
    [SerializeField] private int value = 1;

    private bool _isActive;

    public bool IsActive => _isActive;


    public void SpawnCoin()
    {
        _isActive = true;
        gameObject.SetActive(true);
    }

    public void DisableCoin()
    {
        _isActive = false;
        gameObject.SetActive(false);
    }
    public void OnInteract(PlayerInteraction interactor)
    {
        ///Give points
        interactor.AddCoins(value);
        DisableCoin();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
            if (interaction != null)
            {
                OnInteract(interaction);
            }
        }
    }
}
