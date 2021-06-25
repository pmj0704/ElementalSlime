using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDistroy : MonoBehaviour
{
    void Awake()
    {
        var obj = FindObjectsOfType<AudioListener>();
        if (obj.Length == 1) {
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
    }
}
}
