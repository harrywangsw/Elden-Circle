using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_control : MonoBehaviour
{
    public float trigger_dist = 0f;
    public bool in_conversation;
    GameObject message_screen, switch_message, player;
    switchmessages message_controller;
    void Start()
    {
        message_screen = GameObject.Find("message_screen");
        message_controller = message_controller;
        switch_message = GameObject.Find("switch message");
        player = GameObject.Find("player");
    }

    void Update()
    {
        if(in_conversation){

        }
        int ind = message_controller.messages.IndexOf("press enter to talk");
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            if(ind<0){
                message_controller.messages.Add("press enter to talk");
                message_controller.current = message_controller.messages.Count-1;
            }
            if(Input.GetKeyDown(KeyCode.Return)&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to talk"){
                message_screen.transform.localScale = Vector3.zero;
                switch_message.transform.localScale = Vector3.zero;
                in_conversation = true;
            }
        }
        else{
            if(ind>=0){
                message_controller.messages.RemoveAt(ind);
            }
        }
    }
}
