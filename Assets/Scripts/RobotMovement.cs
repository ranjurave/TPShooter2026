using UnityEngine;
using UnityEngine.InputSystem;

public class RobotMovement : MonoBehaviour
{
    CharacterController robotController;
    [SerializeField] InputActionAsset inputActions;
    InputActionMap actionMap;
    InputAction moveAction;
    float currentVelocity; // For smooth rotation pass to Mathf.SmoothDampAngle

    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSmoothTime = 0.1f;

    void Awake() {
        actionMap = inputActions.FindActionMap("Player");
        moveAction = actionMap.FindAction("Move");
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
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        robotController.Move(moveDirection * Time.deltaTime * speed);
    }
}
