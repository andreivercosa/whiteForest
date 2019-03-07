using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlayer : MonoBehaviour
{
    public float timeDestroy;

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    void Update()
    {
        
    }
}
