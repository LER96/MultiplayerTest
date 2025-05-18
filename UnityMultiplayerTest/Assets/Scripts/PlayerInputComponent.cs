using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputComponent : MonoBehaviourPunCallbacks
{
    [SerializeField] Vector2 _mouseInput;
    public Vector2 MoveInput { get; private set; }
    public Vector2 LastMoveInput { get; private set; }
    public Vector2 CameraInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool HasMoveInput { get; private set; }

    public bool InteractInput { get; private set; }
    public PhotonView View {  get=> _view; set=> _view=value; }

    private bool isPressed;
    private PhotonView _view;


    public void OnMoveEvent(InputAction.CallbackContext context)
    {

        Vector2 moveInput = context.ReadValue<Vector2>();

        bool hasMoveInput = moveInput.sqrMagnitude > 0.0f;
        if (HasMoveInput && !hasMoveInput)
        {
            LastMoveInput = MoveInput;
        }

        MoveInput = moveInput;
        HasMoveInput = hasMoveInput;
    }

    public void OnLookEvent(InputAction.CallbackContext context)
    {
        CameraInput = context.ReadValue<Vector2>();
        _mouseInput = CameraInput;
    }

    public void OnJumpEvent(InputAction.CallbackContext context)
    {

        if (context.started || context.performed)
        {
            JumpInput = true;
        }
        else if (context.canceled)
        {
            JumpInput = false;
        }

    }

    public void OnInteractEvent(InputAction.CallbackContext context)
    {
        isPressed = context.performed;
        var myPlayerInteraction = GameManager.Instance.MyPlayerInteraction;
        if (isPressed)
        {
            if (myPlayerInteraction.Holding)
            {
                myPlayerInteraction.TryPass();
            }
            else
            {
                myPlayerInteraction.TryPickup();
            }
        }
    }
}

