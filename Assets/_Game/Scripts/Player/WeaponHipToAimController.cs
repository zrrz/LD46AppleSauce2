using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHipToAimController : MonoBehaviour
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

    [SerializeField]
    private Animator animator;

    private void Start()
    {
        weapon.parent = hipFireState.targetTransform;
    }

    void Update()
    {
        if(playerInput == null)
        {
            playerInput = PlayerInput.Instance;
        }

        animator.SetFloat("MoveSpeed", playerInput.MoveDir.magnitude);

        if (currentAimState == null)
        {
            currentAimState = hipFireState;
        }

        if(playerInput.sprint.IsPressed && GameManager.Instance.playerData.playerMovement.isGrounded)
        {
            animator.SetBool("Sprinting", true);
            if (playerInput.sprint.WasPressed)
            {
                transitionAmount = 0f;
                animator.SetBool("Sprinting", true);
                weapon.parent = sprintState.targetTransform;
            }
            currentAimState = sprintState;
        }
        else
        {
            animator.SetBool("Sprinting", false);
            if (playerInput.sprint.Released || Input.GetButtonUp("Fire2"))
            {
                currentAimState = hipFireState;
                transitionAmount = 0f;
                animator.SetBool("Sprinting", false);
                weapon.parent = hipFireState.targetTransform;
            }
            if (Input.GetButtonDown("Fire2"))
            {
                currentAimState = aimSightsState;
                transitionAmount = 0f;
                weapon.parent = aimSightsState.targetTransform;
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
