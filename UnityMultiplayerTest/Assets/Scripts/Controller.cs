using Photon.Pun;
using UnityEngine;

public abstract class Controller //: ScriptableObject
{
    public Character Character { get; set; }
    
    public Controller() { } 
    public abstract void Init();
    public abstract void OnCharacterUpdate();
    public abstract void OnCharacterFixedUpdate();
}

