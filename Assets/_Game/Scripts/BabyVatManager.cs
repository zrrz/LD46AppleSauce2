using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyVatManager : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 200f;

    private float currentHealth;

    private bool playerNearby = false;

    private bool depositing = false;
    [SerializeField]
    private float energyDepositTime = 1f;
    private float currentDepositTime = 0f;
    [SerializeField]
    private float energyDrainTime = 1.5f;
    private float currentDrainTime = 0f;

    [SerializeField]
    private Transform depositStartPosition;
    [SerializeField]
    private Transform depositEndPosition;
    [SerializeField]
    private GameObject depositVisualObject;

    [SerializeField]
    private TMPro.TextMeshPro insertEnergyPrompt;
    private Vector3 textPromptStartPos;

    //TODO baby transform energy values

    [System.Serializable]
    private class BabyTransformState
    {
        public int energyRequirement = 1;
        public Texture babyTexture;
        //Sound
        //Particles
    }

    [SerializeField]
    private BabyTransformState[] babyTransformStates = new BabyTransformState[6];

    private int currentBabyTransformState = 0;

    [SerializeField]
    private MeshRenderer babyRenderer;

    private int energyLevel = 0;

    private Animator animator;

    [SerializeField]
    private MeshRenderer liquidColorRenderer;

    [SerializeField]
    private Gradient liquidColorGradient;
    [SerializeField]
    private Gradient liquidColorGradientEmission;

    void Start()
    {
        currentHealth = maxHealth;
        depositVisualObject.SetActive(false);
        textPromptStartPos = insertEnergyPrompt.transform.position;
        insertEnergyPrompt.gameObject.SetActive(false);
        currentBabyTransformState = 0;
        babyRenderer.material.mainTexture = babyTransformStates[currentBabyTransformState].babyTexture;
        energyLevel = 0;
        animator = GetComponentInChildren<Animator>();

        animator.SetFloat("IdleSpeed", (1f));
        babyRenderer.material.mainTexture = babyTransformStates[0].babyTexture;
        liquidColorRenderer.material.color = liquidColorGradient.Evaluate(0f);
        liquidColorRenderer.material.SetColor("_EmissionColor", liquidColorGradientEmission.Evaluate(0f));
    }

    void Update()
    {
        if(insertEnergyPrompt.gameObject.activeSelf)
        {
            insertEnergyPrompt.transform.position = textPromptStartPos + Vector3.up * Mathf.Sign(Time.time * 2.2f) / 4f;
        }
        if(!depositing)
        {
            if(playerNearby && GameManager.Instance.playerData.playerInventory.HasItem(PlayerInventory.ItemType.Energy))
            {
                if(!insertEnergyPrompt.gameObject.activeSelf)
                {
                    insertEnergyPrompt.gameObject.SetActive(true);
                }
                //TODO (Zack.Rock) move input somewhere else. I'm so sorry for this code
                if (GameManager.Instance.playerData.playerInput.interact.WasPressed)
                {
                    StartEnergyDeposit();
                }
            }
        }
        else
        {
            if(currentDepositTime < 1f)
            {
                currentDepositTime += Time.deltaTime / energyDepositTime;
                depositVisualObject.transform.position = Vector3.Lerp(depositStartPosition.position, depositEndPosition.position, currentDepositTime);
                depositVisualObject.transform.rotation = Quaternion.Lerp(depositStartPosition.rotation, depositEndPosition.rotation, currentDepositTime);
                if(currentDepositTime >= 1f)
                {
                    currentDrainTime = 0f;
                    //Play particle effect
                }
            }
            else
            {
                if(currentDrainTime < 1f)
                {
                    currentDrainTime += Time.deltaTime / energyDrainTime;
                    depositVisualObject.transform.Rotate(0f, 0f, 180f * Time.deltaTime);
                    Color emissiveColor = Color.Lerp(Color.white * 0.7f, Color.black, currentDrainTime);
                    depositVisualObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", emissiveColor);
                    if (currentDrainTime >= 1f)
                    {
                        depositVisualObject.SetActive(false);
                        depositing = false;
                        energyLevel++;
                        //TODO Play sound or text prompt
                        UpdateBabyTransformState();
                    }
                }
            }
        }
    }

    private void UpdateBabyTransformState()
    {
        if(energyLevel >= babyTransformStates[currentBabyTransformState+1].energyRequirement)
        {
            currentBabyTransformState++;
            animator.SetFloat("IdleSpeed", (currentBabyTransformState * 2f));
            babyRenderer.material.mainTexture = babyTransformStates[currentBabyTransformState].babyTexture;
            float transformValue = (float)currentBabyTransformState / ((float)babyTransformStates.Length - 1f);
            liquidColorRenderer.material.color = liquidColorGradient.Evaluate(transformValue);
            liquidColorRenderer.material.SetColor("_EmissionColor", liquidColorGradientEmission.Evaluate(transformValue));
            if (currentBabyTransformState == babyTransformStates.Length-1)
            {
                babyRenderer.enabled = false;
                GameManager.Instance.DisablePlayerInput();
                GetComponentInChildren<AudioSource>().Play();
                FindObjectOfType<MusicManager>().GetComponentInChildren<AudioSource>().Stop();
                this.enabled = false;
                
                Invoke("StartFade", 3f);
            }
        }
    }

    void StartFade()
    {
        FindObjectOfType<FadeOutManager>().FadeOut(3f);
        Invoke("EndGame", 3f);
    }

    void EndGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void StartEnergyDeposit()
    {
        depositing = true;
        currentDepositTime = 0f;
        insertEnergyPrompt.gameObject.SetActive(false);
        depositVisualObject.SetActive(true);
        depositVisualObject.transform.position = depositStartPosition.position;
        depositVisualObject.transform.rotation = depositStartPosition.rotation;
        depositVisualObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", Color.white * 0.7f);
        GameManager.Instance.playerData.playerInventory.RemoveItem(PlayerInventory.ItemType.Energy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerInventory>())
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInventory>())
        {
            playerNearby = false;
            insertEnergyPrompt.gameObject.SetActive(false);
        }
    }
}
