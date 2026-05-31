using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string rotateObject = "RotateObject";
    [SerializeField] private string fire = "Fire";
    [SerializeField] private string reload = "Reload";


    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction rotateObjectAction;
    private InputAction fireAction;
    private InputAction reloadAction;



    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool RotateObjectTriggered { get; private set; }
    public bool FireTriggered { get; private set; }
    public bool ReloadTriggered { get; private set; }


    private void Awake()
    {
        if (playerControls == null)
        {
            Debug.LogError("[PLAYER INPUT] PlayerControls Asset is missing! Please assign it in the Inspector.");
            return;
        }

        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);
        if (mapReference == null)
        {
            Debug.LogError($"[PLAYER INPUT] Action Map '{actionMapName}' not found!");
            return;
        }

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        rotateObjectAction = mapReference.FindAction(rotateObject);
        fireAction = mapReference.FindAction(fire);
        reloadAction = mapReference.FindAction(reload);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        if (movementAction != null) {
            movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
            movementAction.canceled += inputInfo => MovementInput = Vector2.zero;
        }

        if (rotationAction != null) {
            rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
            rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;
        }

        if (jumpAction != null) {
            jumpAction.performed += inputInfo => { JumpTriggered = true; Debug.Log("[INPUT] Jump Pressed"); };
            jumpAction.canceled += inputInfo => JumpTriggered = false;
        }

        if (sprintAction != null) {
            sprintAction.performed += inputInfo => SprintTriggered = true;
            sprintAction.canceled += inputInfo => SprintTriggered = false;
        }

        if (rotateObjectAction != null) {
            rotateObjectAction.performed += inputInfo => RotateObjectTriggered = true;
            rotateObjectAction.canceled += inputInfo => RotateObjectTriggered = false;
        }

        if (fireAction != null) {
            fireAction.performed += inputInfo => { FireTriggered = true; Debug.Log("[INPUT] Fire Pressed"); };
            fireAction.canceled += inputInfo => FireTriggered = false;
        }

        if (reloadAction != null) {
            reloadAction.performed += inputInfo => { ReloadTriggered = true; Debug.Log("[INPUT] Reload Pressed"); };
            reloadAction.canceled += inputInfo => ReloadTriggered = false;
        }
    }

    private void OnEnable()
    {
        if (playerControls != null)
            playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        if (playerControls != null)
            playerControls.FindActionMap(actionMapName).Disable();
    }
}
