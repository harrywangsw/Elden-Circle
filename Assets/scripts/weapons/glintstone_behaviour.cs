using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glintstone_behaviour : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(){
        Destroy(gameObject);
    }
}
