namespace _Scripts
{
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        private PlayerControls controls;
        private Vector2 moveInput;

        private CharacterController controller;

        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float turnSpeed = 120f; // degrees per second

        [Header("Jump & Gravity")]
        public float jumpForce = 8f;
        public float gravity = -20f;
        private Vector3 velocity;
        private bool isGrounded;

        private void Awake()
        {
            controls = new PlayerControls();
            controller = GetComponent<CharacterController>();

            // Movement input
            controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

            // Jump input
            controls.Player.Jump.performed += ctx => Jump();
        }

        private void OnEnable()  => controls.Enable();
        private void OnDisable() => controls.Disable();

        private void Update()
        {
            HandleMovement();
            ApplyGravity();
        }

        private void HandleMovement()
        {
            // Ground check
            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            // Rotate left/right with A and D
            float turn = moveInput.x * turnSpeed * Time.deltaTime;
            transform.Rotate(0, turn, 0);

            // Move forward/back with W and S
            Vector3 move = transform.forward * moveInput.y * moveSpeed * Time.deltaTime;
            controller.Move(move);
        }

        private void ApplyGravity()
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        private void Jump()
        {
            if (isGrounded)
                velocity.y = jumpForce;
        }
    }

}