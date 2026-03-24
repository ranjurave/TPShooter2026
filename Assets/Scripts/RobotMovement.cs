using UnityEngine;
using UnityEngine.InputSystem;

public class RobotMovement : MonoBehaviour
{
    CharacterController robotController;
    [SerializeField] InputActionAsset inputActions;
    InputActionMap actionMap;
    InputAction moveAction;
    InputAction jumpAction;
    float currentVelocity; // For smooth rotation pass to Mathf.SmoothDampAngle
    [SerializeField] Transform cameraTransform;
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSmoothTime = 0.1f;

    [SerializeField] float jumpHeight = 2;
    bool isGrounded;
    float gravity = -9.8f;
    float verticalVelocity;


    void Awake() {
        actionMap = inputActions.FindActionMap("Player");
        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");
    }

    void OnEnable() {
        actionMap.Enable();
    }

    void OnDisable() {
        actionMap.Disable();
    }

    void Start() {
        robotController = GetComponent<CharacterController>();
    }

    void Update() {
        isGrounded = robotController.isGrounded;
        if (isGrounded && verticalVelocity < 0) {
            verticalVelocity = -2.0f;
        }
        if (jumpAction.IsPressed() && isGrounded) {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        verticalVelocity += gravity * Time.deltaTime;
        Vector3 moveDir = Vector3.zero;
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        if(moveInput.magnitude > 0.01f) {
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);
            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        }

        Vector3 velocity = moveDir * speed + Vector3.up * verticalVelocity;
        robotController.Move(velocity * Time.deltaTime);
    }
}
