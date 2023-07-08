using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time = 3f;
    
    void Start()
    {
        Invoke(nameof(DestroySelf), time);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
