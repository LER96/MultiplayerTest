using UnityEngine;
using UnityEngine.InputSystem;
using static Controller;
    //[CreateAssetMenu(fileName = "PlayerController", menuName = "NaughtyCharacter/PlayerController")]
    public class PlayerController : Controller
    {
        public float ControlRotationSensitivity = 1.0f;

        public PlayerInputComponent playerInput;
        public PlayerCamera playerCamera;

    public PlayerController()
    {

    }

    public bool HasComponents()
    {
        return playerInput != null && playerCamera!=null;
    }

    public override void Init()
    {
        //_playerInput = FindObjectOfType<PlayerInputComponent>();
        //_playerCamera = FindObjectOfType<PlayerCamera>();
    }

    public override void OnCharacterUpdate()
    {
        UpdateControlRotation();
        Character.SetMovementInput(GetMovementInput());
        Character.SetJumpInput(playerInput.JumpInput);
    }

    public override void OnCharacterFixedUpdate()
    {
        playerCamera.SetPosition(Character.transform.position);
        playerCamera.SetControlRotation(Character.GetControlRotation());
    }

    private void UpdateControlRotation()
    {
        Vector2 camInput = playerInput.CameraInput;
        Vector2 controlRotation = Character.GetControlRotation();

        // Adjust the pitch angle (X Rotation)
        float pitchAngle = controlRotation.x;
        pitchAngle -= camInput.y * ControlRotationSensitivity;

        // Adjust the yaw angle (Y Rotation)
        float yawAngle = controlRotation.y;
        yawAngle += camInput.x * ControlRotationSensitivity;

        controlRotation = new Vector2(pitchAngle, yawAngle);
        Character.SetControlRotation(controlRotation);
    }

    private Vector3 GetMovementInput()
    {
        // Calculate the move direction relative to the character's yaw rotation
        Quaternion yawRotation = Quaternion.Euler(0.0f, Character.GetControlRotation().y, 0.0f);
        Vector3 forward = yawRotation * Vector3.forward;
        Vector3 right = yawRotation * Vector3.right;
        Vector3 movementInput = (forward * playerInput.MoveInput.y + right * playerInput.MoveInput.x);

        if (movementInput.sqrMagnitude > 1f)
        {
            movementInput.Normalize();
        }

        return movementInput;
    }
    
}