using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doors : MonoBehaviour
{
    public bool open_right;
    public bool open_left;
    public float triggerdist;
    GameObject player;
    GameObject message_screen;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        message_screen = GameObject.Find("message_screen");
    }


    void Update()
    {
        if((player.transform.position-transform.position).magnitude<=triggerdist){
            message_screen.GetComponent<switchmessages>().messages.Add("press E to open door");
        }
        else{
            int ind = message_screen.GetComponent<switchmessages>().messages.IndexOf("press E to open door");
            if(ind>=0){
                message_screen.GetComponent<switchmessages>().messages.RemoveAt(ind);
            }
        }
    }
}
