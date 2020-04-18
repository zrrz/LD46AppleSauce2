using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private float walkSpeed = 3.7f;

    [SerializeField]
    private float sprintSpeed = 7.1f;

    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float jumpHeight = 3f;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    PlayerInput playerInput;

    void Update()
    {
        if (playerInput == null)
        {
            playerInput = PlayerInput.Instance;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //TODO check if diagonal movement is faster
        float x = playerInput.moveHorizontal.Value;
        float z = playerInput.moveVertical.Value;

        Vector3 move = transform.right * x + transform.forward * z;

        bool sprinting = playerInput.sprint.IsPressed;

        controller.Move(move * (sprinting ? sprintSpeed : walkSpeed) * Time.deltaTime);

        if(playerInput.jump.IsPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
