using UnityEngine;

namespace TargetStrike.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float sprintSpeed = 10f;
        [SerializeField] private float gravity = -25f;
        [SerializeField] private float jumpHeight = 2f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.3f;
        [SerializeField] private LayerMask groundMask;

        [Header("Auto Movement (Level 3)")]
        [SerializeField] private bool enableAutoMovement = false;
        [SerializeField] private float autoMoveSpeed = 3f;
        [SerializeField] private float autoMoveDistance = 6f;

        public void ConfigureAutoMovement(bool enable, float speed = 3f, float distance = 6f)
        {
            enableAutoMovement = enable;
            autoMoveSpeed = speed;
            autoMoveDistance = distance;
            startPosition = transform.position; // Reset start position to current
        }

        private CharacterController controller;
        private PlayerInputHandler inputHandler;
        
        private Vector3 velocity;
        private bool isGrounded;
        private Vector3 startPosition;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            inputHandler = GetComponent<PlayerInputHandler>();
            startPosition = transform.position;
        }

        private void Update()
        {
            HandleGroundCheck();
            
            if (enableAutoMovement) HandleAutoMovement();
            else HandleMovement();

            HandleJumping();
        }

        private void HandleGroundCheck()
        {
            if (groundCheck == null)
            {
                isGrounded = controller.isGrounded;
                return;
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Keep the player grounded
            }
        }

        private void HandleMovement()
        {
            if (inputHandler == null) return;

            Vector2 input = inputHandler.MoveInput;
            float speed = inputHandler.SprintValue ? sprintSpeed : walkSpeed;

            // Interpolated movement for smoothness
            Vector3 move = transform.right * input.x + transform.forward * input.y;
            controller.Move(move * speed * Time.deltaTime);
        }

        private void HandleAutoMovement()
        {
            float offset = Mathf.PingPong(Time.time * autoMoveSpeed, autoMoveDistance * 2) - autoMoveDistance;
            Vector3 targetPosition = startPosition + transform.right * offset;
            
            Vector3 moveDelta = targetPosition - transform.position;
            controller.Move(new Vector3(moveDelta.x, 0, moveDelta.z));
        }

        private void HandleJumping()
        {
            if (inputHandler != null && inputHandler.JumpTriggered && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
