using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInteractableObject : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Coin Setting")]
    [SerializeField] private int value = 1;

    private const string ON_COIN_COLLECT = nameof(OnInteract);
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
    public void OnInteract(PlayerInteraction interaction)
    {
        ///Give points
        interaction.AddCoins(value);
        DisableCoin();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
            PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
            if (interaction != null)
            {
                if (interaction.View.IsMine)
                    OnInteract(interaction);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(_isActive);
            stream.SendNext(transform.position);
        }
        else
        {
            gameObject.SetActive((bool)stream.ReceiveNext());
            transform.position= (Vector3)stream.ReceiveNext();
        }
    }
}
