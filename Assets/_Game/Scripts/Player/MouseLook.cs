using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 100f;

    [SerializeField]
    private Transform playerBody;

    private float xRotation = 0f;

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = PlayerInput.Instance;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(playerInput == null)
        {
            playerInput = PlayerInput.Instance;
        }

        float mouseX = playerInput.lookHorizontal.Value * mouseSensitivity * Time.deltaTime;
        float mouseY = playerInput.lookVertical.Value * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
