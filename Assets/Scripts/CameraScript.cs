using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour {
    public InputActionAsset inputActions;
    private InputActionMap playerMap;
    private InputAction look;

    float yaw, pitch;
    [SerializeField] Transform player;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] Vector2 pitchMinMax = new Vector2(-40, 80); // Min and max vertical angles
    [SerializeField] float cameraDistance = 4f; // Distance from player
    [SerializeField] float cameraHeight = 2f; 

    void Awake() {
        playerMap = inputActions.FindActionMap("Player");
        look = playerMap.FindAction("Look");
    }

    private void OnEnable() => playerMap.Enable();
    private void OnDisable() => playerMap.Disable();

    void Start() {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        Vector2 lookInput = look.ReadValue<Vector2>();
        yaw += lookInput.x;
        pitch -= lookInput.y; // Invert Y so moving mouse up = camera up
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        // Calculate desired rotation and position
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 offset = targetRotation * new Vector3(0f, 0f, -cameraDistance);
        Vector3 desiredPosition = player.position + Vector3.up * cameraHeight + offset;

        // Smooth the position and rotation
        transform.position = Vector3.Lerp(transform.position, desiredPosition, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
