using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //[SerializeField]
    //private float mouseSensitivity = 100f;

    [SerializeField]
    private Transform playerBody;

    private float xRotation = 0f;

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = PlayerInput.Instance;
    }

    void Update()
    {
        if(playerInput == null)
        {
            playerInput = PlayerInput.Instance;
        }

        float mouseSensitivity = GameSettingsManager.GetSettingFloat(GameSettingsManager.SettingType.MouseSensitivity);
        float mouseSensitivyScaled = Mathf.Lerp(5f, 80f, mouseSensitivity);

        mouseSensitivyScaled *= playerInput.aim.IsPressed ? 0.5f : 1f;

        float mouseX = playerInput.lookHorizontal.Value * mouseSensitivyScaled * Time.deltaTime;
        float mouseY = playerInput.lookVertical.Value * mouseSensitivyScaled * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
