using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGravityController : MonoBehaviour
{
    public float gravity = -9.81f;
    public float groundedOffset = 0.1f;

    private CharacterController controller;
    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundedOffset + 0.01f);
        
        if (isGrounded)
        {
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 move = new Vector3(0, verticalVelocity, 0);
        controller.Move(move * Time.deltaTime);
    }
}
