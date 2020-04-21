using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutManager : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Image fadeImage;
    
    public void FadeOut(float time)
    {
        StartCoroutine(Fade(time));
    }

    IEnumerator Fade(float time)
    {
        if(fadeImage.enabled == false)
        {
            fadeImage.enabled = true;
        }
        for(float t = 0f; t < 1f; t += Time.deltaTime/time)
        {
            fadeImage.color = Color.Lerp(Color.clear, Color.black, t);
            yield return null;
        }
    }
}
