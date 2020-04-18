using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField]
    TextMeshPro textMesh;

    private Color initializedColor;
    private Color endColor;

    [SerializeField]
    private float lifeTime = 2f;

    [SerializeField]
    private float floatSpeed = 1f;

    private float timer = 0f;

    public void Initialize(FloatingDamageTextHandler.FloatingTextData floatingTextData, Vector3 position, string text)
    {
        textMesh.fontSize = floatingTextData.fontSize;
        textMesh.color = initializedColor = floatingTextData.fontColor;
        textMesh.transform.position = position;
        textMesh.text = text;

        endColor = initializedColor;
        endColor.a = 0f;

        timer = 0f;
    }

    private void Update()
    {
        textMesh.color = Color.Lerp(initializedColor, endColor, timer);
        timer += Time.deltaTime / lifeTime;
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        if(timer > 0f)
        {
            Destroy(gameObject);
        }
    }
}
