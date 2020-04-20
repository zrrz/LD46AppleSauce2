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

    private void Awake()
    {
        Instance = this;
    }
}
