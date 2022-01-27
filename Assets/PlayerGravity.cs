using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerGravity : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] private float atmospheres = 1;

    private CharacterController controller;
    private Vector3 verticalVelocity;
    private float gravity;

    public void Awake()
    {
        controller = GetComponent<CharacterController>();
        gravity = atmospheres * -9.81f;
    }
    public void Start()
    {
        verticalVelocity = Vector3.zero;
    }

    public void Update()
    {
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0f;
        }
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

    }
}
