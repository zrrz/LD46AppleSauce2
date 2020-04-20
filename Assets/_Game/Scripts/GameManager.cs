using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public PlayerHealth playerHealth;
        public PlayerMovement playerMovement;
        public PlayerInventory playerInventory;
        public PlayerShootingHandler shootingHandler;
        public PlayerInput playerInput;
    }

    public PlayerData playerData;

    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject uiCanvas;

    private void Awake()
    {
        Instance = this;
        SetMenuState(false);
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetMenuState(!uiCanvas.gameObject.activeSelf);
            //if(uiCanvas.gameObject.activeSelf)
            //{
            //    uiCanvas.gameObject.SetActive(false);
            //    LockCursor();
            //    Time.timeScale = 1f;
            //}
            //else
            //{
            //    uiCanvas.gameObject.SetActive(true);
            //    UnlockCursor();
            //    Time.timeScale = 0f;
            //}
        }
    }

    void SetMenuState(bool on)
    {
        if (on)
        {
            uiCanvas.gameObject.SetActive(true);
            UnlockCursor();
            Time.timeScale = 0f;
        }
        else
        {
            uiCanvas.gameObject.SetActive(false);
            LockCursor();
            Time.timeScale = 1f;
        }
    }

}
