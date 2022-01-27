using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float smoothing = 0.03f;

    private PlayerInput actions;

    private float cameraPitch = 0f;

    // The following members are used for input smoothing.
    private Vector2 previousMouseDelta = Vector2.zero;
    private Vector2 mouseVelocity = Vector2.zero;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        actions = new PlayerInput();
        actions.Player.Enable();
    }

    public void Update()
    {
        Look(actions.Player.Look.ReadValue<Vector2>());
    }

    public void Look(Vector2 targetDirection)
    {
        // Have to normalize for SmoothDamp.
        targetDirection.Normalize();
        Vector2 mouseDelta = Vector2.SmoothDamp(
            previousMouseDelta, targetDirection, ref mouseVelocity, smoothing);
        // Update previousDirection with current direction for next iteration.
        previousMouseDelta = mouseDelta;

        float yaw = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        transform.parent.Rotate(Vector3.up * yaw);

        cameraPitch -= mouseDelta.y * mouseSensitivity * Time.deltaTime;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        transform.localRotation = Quaternion.Euler(Vector3.right * cameraPitch);
    }
}
