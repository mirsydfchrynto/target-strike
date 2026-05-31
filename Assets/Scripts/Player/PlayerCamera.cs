using UnityEngine;

namespace TargetStrike.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Sensitivity Settings")]
        public float mouseSensitivity = 0.15f; // New Input System Delta values are large
        [SerializeField] private float xRotationLimit = 90f;

        [Header("Smoothing")]
        [SerializeField] private bool smoothRotation = true;
        [SerializeField] private float smoothTime = 5f;

        private PlayerInputHandler inputHandler;
        private Transform playerBody;
        
        private float xRotation = 0f;
        private float yRotation = 0f;

        private float targetX;
        private float targetY;

        private void Awake()
        {
            inputHandler = GetComponentInParent<PlayerInputHandler>();
            playerBody = transform.parent; // Assuming parent is the Player root
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate() // Use LateUpdate for Camera to follow movement perfectly
        {
            HandleRotation();
        }

        private void HandleRotation()
        {
            if (inputHandler == null) return;

            // BUG FIX: Mouse Delta in New Input System should NOT be multiplied by Time.deltaTime
            // if you want frame-independent sensitivity.
            Vector2 lookInput = inputHandler.LookInput * mouseSensitivity;

            targetX -= lookInput.y;
            targetY += lookInput.x;

            targetX = Mathf.Clamp(targetX, -xRotationLimit, xRotationLimit);

            if (smoothRotation)
            {
                xRotation = Mathf.Lerp(xRotation, targetX, Time.deltaTime * smoothTime * 10f);
                yRotation = Mathf.Lerp(yRotation, targetY, Time.deltaTime * smoothTime * 10f);
            }
            else
            {
                xRotation = targetX;
                yRotation = targetY;
            }

            // Apply rotations
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }
        
        public void SetBody(Transform body) => playerBody = body;
    }
}
