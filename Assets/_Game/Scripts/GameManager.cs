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
        public PlayerSoundHandler soundHandler;
    }

    public PlayerData playerData;

    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject uiCanvas;

    [SerializeField]
    private UnityEngine.Audio.AudioMixerGroup musicMixerGroup;
    public static UnityEngine.Audio.AudioMixerGroup MusicMixerGroup => Instance.musicMixerGroup;

    [SerializeField]
    private UnityEngine.Audio.AudioMixerGroup sfxMixerGroup;
    public static UnityEngine.Audio.AudioMixerGroup SfxMixerGroup => Instance.sfxMixerGroup;

    private void Awake()
    {
        Instance = this;
        SetMenuState(false);
    }

    private void Start()
    {
        if(playerData.playerHealth == null || playerData.soundHandler == null)
        {
            var playerHealth = FindObjectOfType<PlayerHealth>();
            if(playerHealth)
            {
                playerData.playerHealth = playerHealth;
                playerData.playerMovement = playerHealth.GetComponentInChildren<PlayerMovement>();
                playerData.playerInventory = playerHealth.GetComponentInChildren<PlayerInventory>();
                playerData.shootingHandler = playerHealth.GetComponentInChildren<PlayerShootingHandler>();
                playerData.playerInput = playerHealth.GetComponentInChildren<PlayerInput>();
                playerData.soundHandler = playerHealth.GetComponentInChildren<PlayerSoundHandler>();
            }
        }
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

    public void SetMenuState(bool on)
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
