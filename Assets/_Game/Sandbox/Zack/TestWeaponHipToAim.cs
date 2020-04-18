using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponHipToAim : MonoBehaviour
{
    [System.Serializable]
    class WeaponAimState
    {
        public Transform targetTransform;
        public float FieldOfView;
    }

    [SerializeField]
    private WeaponAimState hipFireState;

    [SerializeField]
    private WeaponAimState aimSightsState;

    [SerializeField]
    private WeaponAimState sprintState;

    private WeaponAimState currentAimState;

    public Transform weapon;

    public float transitionSpeed = 3f;

    public new Camera camera;

    float transitionAmount = 0f;

    PlayerInput playerInput;

    void Update()
    {
        if(playerInput == null)
        {
            playerInput = PlayerInput.Instance;
        }

        if(currentAimState == null)
        {
            currentAimState = hipFireState;
        }

        //Sorry Jesus for this code I am about to write
        if(playerInput.sprint.IsPressed)
        {
            if(playerInput.sprint.WasPressed)
            {
                transitionAmount = 0f;
            }
            currentAimState = sprintState;
        }
        else
        {
            if (playerInput.sprint.Released || Input.GetButtonUp("Fire2"))
            {
                currentAimState = hipFireState;
                transitionAmount = 0f;
            }
            if (Input.GetButtonDown("Fire2"))
            {
                currentAimState = aimSightsState;
                transitionAmount = 0f;
            }
        }

        if (transitionAmount < 1f)
        {
            transitionAmount += transitionSpeed * Time.deltaTime;
            weapon.transform.position = Vector3.Lerp(weapon.position, currentAimState.targetTransform.position, transitionAmount);
            weapon.transform.rotation = Quaternion.Lerp(weapon.rotation, currentAimState.targetTransform.rotation, transitionAmount);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, currentAimState.FieldOfView, transitionAmount);
        }

    }
}
