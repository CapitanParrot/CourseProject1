using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float Time;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Time);
    }
    void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
