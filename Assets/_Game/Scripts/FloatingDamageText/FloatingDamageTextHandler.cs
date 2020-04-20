using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageTextHandler : MonoBehaviour
{
    public enum DamageType
    {
        Normal, Critical
    }

    [System.Serializable]
    public class FloatingTextData
    {
        public DamageType damageType;
        public float fontSize;
        public Color fontColor;
    }

    private Dictionary<DamageType, FloatingTextData> floatingTextDataMap;

    [SerializeField]
    FloatingDamageText floatingDamageTextPrefab;

    [SerializeField]
    private FloatingTextData[] floatingTextData;

    static FloatingDamageTextHandler instance;

    void Start()
    {
        instance = this;

        floatingTextDataMap = new Dictionary<DamageType, FloatingTextData>();
        for(int i = 0; i < floatingTextData.Length; i++)
        {
            floatingTextDataMap.Add(floatingTextData[i].damageType, floatingTextData[i]);
        }
    }

    public static void CreateFloatingText(DamageType damageType, float damageAmount, Vector3 position)
    {
        damageAmount *= 12f;
        if (instance == null)
        {
            Debug.LogError($"No instance of {nameof(FloatingDamageTextHandler)} in scene");
            return;
        }

        instance.CreateFloatingText_Impl(damageType, damageAmount, position);
    }

    private void CreateFloatingText_Impl(DamageType damageType, float damageAmount, Vector3 position)
    {
        var floatingDamageText = Instantiate(floatingDamageTextPrefab, position, Quaternion.identity);
        floatingDamageText.Initialize(floatingTextDataMap[damageType], position, damageAmount.ToString());
    }
}
