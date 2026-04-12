using UnityEngine;
using UnityEngine.InputSystem;

public class RobotMovement : MonoBehaviour
{
    CharacterController robotController;
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] Transform cameraTransform;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform fireTransform;
    
    InputActionMap actionMap;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction attakAction;
    float currentVelocity; // For smooth rotation pass to Mathf.SmoothDampAngle

    public Vector2 moveInput;

    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSmoothTime = 0.1f;
    [SerializeField] float jumpHeight = 0.5f;
    bool isGrounded;
    float verticalVelocity = -2.0f;
    float gravity = -9.81f;

    void Awake() {
        actionMap = inputActions.FindActionMap("Player");
        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");
        attakAction = actionMap.FindAction("Attack");
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

        if (attakAction.WasPressedThisFrame()) {
            Instantiate(bullet, fireTransform.position, fireTransform.rotation);           
        }


        isGrounded = robotController.isGrounded;
        if(isGrounded && verticalVelocity < 0) {
            verticalVelocity = -2f; // Small negative to keep grounded
        }

        if(jumpAction.IsPressed() && isGrounded) {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        verticalVelocity += gravity * Time.deltaTime;

        moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = Vector3.zero;
        if (moveInput.magnitude > 0.01f) { // Check if there's significant input
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);
            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        }
        Vector3 velocity = moveDir * speed + Vector3.up * verticalVelocity * gravity * -1;
        robotController.Move(velocity * Time.deltaTime);
    }
}
