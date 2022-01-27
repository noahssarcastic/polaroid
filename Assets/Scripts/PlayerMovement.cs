using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed = 8f;
    [SerializeField] private float smoothing = 0.3f;

    // Allows player input.
    private PlayerInput actions;

    private CharacterController controller;

    // The following members are used for input smoothing.
    private Vector2 previousDirection = Vector2.zero;
    private Vector2 horizontalVelocity = Vector2.zero;

    public void Awake()
    {
        controller = GetComponent<CharacterController>();

        actions = new PlayerInput();
        actions.Player.Enable();
    }

    public void Update()
    {
        // This is a normalized vector with magnitude between 0 and 1.
        // In the case of a keyboard this value is always 1 when a key is pressed.
        Vector2 movementInput = actions.Player.Move.ReadValue<Vector2>();
        Move(movementInput);
        Debug.Log(controller.velocity.magnitude);
    }

    public void Move(Vector2 targetDirection)
    {
        Vector2 direction = Vector2.SmoothDamp(
            previousDirection, targetDirection, ref horizontalVelocity, smoothing);

        // Update previousDirection with current direction for next iteration.
        previousDirection = direction;

        // Map 2D input axis to 3D world movement.
        Vector3 x = direction.x * transform.right;
        Vector3 z = direction.y * transform.forward;
        Vector3 targetVelocity = (x + z) * speed;
        controller.Move(targetVelocity * Time.deltaTime);
    }
}
