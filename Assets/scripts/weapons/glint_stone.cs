using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class glint_stone : MonoBehaviour
{
    public bool new_input, attacking;
    public float range;
    public bool* p_newinput;
    public Vector3 init_loc;

    Rigidbody2D body;
    void Start()
    {
        
    }

    void Update()
    {
        new_input = *p_newinput;

    }
}
