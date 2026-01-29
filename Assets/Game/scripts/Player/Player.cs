using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool movingAvailable = true;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpForce = 2.0f;
    [SerializeField] private float dropForce = 3f;

    [Header("Camera Settings")]
    // mouse properties
    [SerializeField] private float mouseSensitivity = 25.0f;

    // gamepad properties
    [SerializeField] private bool gamepadMode = false;
    [SerializeField] private float gamepadSensitivity = 250.0f;
    [SerializeField] private float gamepadDeadZone = 0.001f;
    public Transform cameraTransform;

    private CharacterController characterController => GetComponent<CharacterController>();
    private Vector3 velocity;
    private float xRotation = 0f;
    
    private InputAction moveAction => InputSystem.actions.FindAction("Move");
    private InputAction jumpAction => InputSystem.actions.FindAction("Jump");
    private InputAction lookAction => InputSystem.actions.FindAction("Look");
    // if you need interaction or attack
    private InputAction attackAction => InputSystem.actions.FindAction("Attack");
    private InputAction interactAction => InputSystem.actions.FindAction("Interact");
    private Interaction interactionComponent => GetComponent<Interaction>();
    private Inventory inventoryComponent => GetComponent<Inventory>();

    private void Start()
    {
        if (tag == "Untagged") tag = "Player";
    }
    
    private void Update()
    { 
        HandleCameraRotation();
        HandleMovement();
         // if you need interaction or attack
        if (interactAction.WasPressedThisFrame()) interactionComponent.TryInteract();
        // if (attackAction.WasPressedThisFrame() && interactionComponent.carriedObject != null) interactionComponent.CheckAction(dropForce);
        if (attackAction.WasPressedThisFrame())
        {
            ChangeCursoreMode();
            inventoryComponent.UseItem();
        }
    }

    private void ChangeCursoreMode()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    #region Input

    private void OnEnable() => InputSystem.onActionChange += OnActionChange;
    private void OnDisable() => InputSystem.onActionChange -= OnActionChange;

    private void OnActionChange(object obj, InputActionChange change)
    {
        var action = obj as InputAction;

        if (change != InputActionChange.ActionPerformed) return;
        if (action == null || action.activeControl == null) return;

        gamepadMode = action.activeControl.device is Gamepad;
    }

    #endregion

    #region Player Movement

    private void HandleMovement()
    {
        if(!movingAvailable) return;
        
        Vector3 move = transform.right * moveAction.ReadValue<Vector2>().x + transform.forward * moveAction.ReadValue<Vector2>().y;

        if(characterController.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (jumpAction.WasPressedThisFrame() && characterController.isGrounded && jumpForce != 0)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }
        else if(!characterController.isGrounded)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }

        Vector3 motion = move.normalized * moveSpeed;
        motion.y = velocity.y;

        CollisionFlags flags = characterController.Move(motion * Time.deltaTime);

        if ((flags & CollisionFlags.Above) != 0 && velocity.y > 0)
        {
            velocity.y = -1f;
        }
    }

    #endregion

    #region Handle camera rotations

    private void HandleCameraRotation()
    {
        if(Cursor.lockState != CursorLockMode.Locked) return;
        Vector2 look = lookAction.ReadValue<Vector2>();

        if (gamepadMode) HandleGamepadCamera(look);
        else HandleMouseCamera(look);
    }

    private void HandleMouseCamera(Vector2 look)
    {
        float mouseX = look.x * mouseSensitivity * Time.deltaTime;
        float mouseY = look.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleGamepadCamera(Vector2 look)
    {

        if (look.sqrMagnitude < gamepadDeadZone) return;

        float stickX = look.x * gamepadSensitivity * Time.deltaTime;
        float stickY = look.y * gamepadSensitivity * Time.deltaTime;

        xRotation -= stickY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        transform.Rotate(Vector3.up * stickX);
    }
    
    #endregion

}