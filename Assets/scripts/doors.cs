using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doors : MonoBehaviour
{
    public bool open_right;
    public bool open_left;
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


    void Update()
    {
        if(player.position)
    }
}
