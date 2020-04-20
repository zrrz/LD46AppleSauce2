using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public void Destroy(float time)
    {
        Destroy(gameObject, time);
    }
}
