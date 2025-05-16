using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Xml;
using TMPro.Examples;

public class SpawnPoint : MonoBehaviourPunCallbacks, IPunObservable
{
    public int ID;
    public bool taken = false;
    [SerializeField] PlayerInputComponent _input;
    [SerializeField] PlayerCamera _camera;

    public PlayerInputComponent PlayerInput => _input;
    public PlayerCamera Camera => _camera;


    [ContextMenu("Turn off taken")]
    void ChangeTaken()
    {
        taken = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(taken);
        }
        else if (stream.IsReading)
        {
            taken = (bool)stream.ReceiveNext();
        }
    }

    public void ActiveComponents()
    {
        _input.gameObject.SetActive(true);
        _camera.gameObject.SetActive(true);
    }
}
