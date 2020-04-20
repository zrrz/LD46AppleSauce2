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

    private Camera billBoardCamera;

    [SerializeField]
    private AnimationCurve alphaCurve;

    public void Initialize(FloatingDamageTextHandler.FloatingTextData floatingTextData, Vector3 position, string text)
    {
        billBoardCamera = Camera.main;
        textMesh.fontSize = floatingTextData.fontSize;
        textMesh.color = initializedColor = floatingTextData.fontColor;
        textMesh.transform.position = position;
        textMesh.text = text;

        endColor = initializedColor;

        timer = 0f;
    }

    private void Update()
    {
        transform.LookAt(billBoardCamera.transform.position);
        Color textColor = Color.Lerp(initializedColor, endColor, timer);
        textColor.a = alphaCurve.Evaluate(timer);
        textMesh.color = textColor;
        timer += Time.deltaTime / lifeTime;
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        if(timer > 1f)
        {
            Destroy(gameObject);
        }
    }
}
