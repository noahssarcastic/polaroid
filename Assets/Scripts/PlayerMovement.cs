using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed = 8f;
    [SerializeField] private float smoothing = 0.3f;
    [SerializeField] private float gravity = -1f;

    private PlayerInput actions;

    private CharacterController controller;

    // The following members are used for input smoothing.
    private Vector2 previousDirection = Vector2.zero;
    private Vector2 horizontalVelocity = Vector2.zero;

    private float verticalVelocity = 0f;

    public void Awake()
    {
        controller = GetComponent<CharacterController>();
        actions = new PlayerInput();
        actions.Player.Enable();
    }

    public void Update()
    {
        Move(actions.Player.Move.ReadValue<Vector2>());
    }

    public void FixedUpdate()
    {
        verticalVelocity += gravity * Time.deltaTime;
        if (controller.isGrounded)
        {
            verticalVelocity = 0;
        }
        Debug.Log(verticalVelocity);
    }

    public void Move(Vector2 targetDirection)
    {
        // Have to normalize for SmoothDamp.
        targetDirection.Normalize();
        Vector2 direction = Vector2.SmoothDamp(
            previousDirection, targetDirection, ref horizontalVelocity, smoothing);
        // Update previousDirection with current direction for next iteration.
        previousDirection = direction;

        // Map 2D input axis to 3D world movement.
        Vector3 x = direction.x * transform.right * speed * Time.deltaTime;
        Vector3 y = Vector3.up * verticalVelocity;
        Vector3 z = direction.y * transform.forward * speed * Time.deltaTime;

        controller.Move(x + y + z);
    }
}
