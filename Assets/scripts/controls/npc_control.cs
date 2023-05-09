using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class npc_control : MonoBehaviour
{
    public float trigger_dist = 0f;
    public bool in_conversation;
    public string npc_name;
    GameObject message_screen, switch_message, player, dialogue_screen;
    switchmessages message_controller;
    player_control p;
    List<string> dialogues;
    TMPro.TextMeshProUGUI dialogue_text_bar;
    string[] lines;
    //-1 cause the same button press that triggers the conversation also changes current_line by one
    public int current_line = -1, index = 0;
    void Start()
    {
        dialogue_screen = GameObject.Find("dialogue_screen");
        message_screen = GameObject.Find("message_screen");
        message_controller = message_screen.GetComponent<switchmessages>();
        switch_message = GameObject.Find("switch message");
        player = GameObject.Find("player");
        p = player.GetComponent<player_control>();
        dialogues = statics.npc_lines[npc_name];
        dialogue_text_bar = dialogue_screen.GetComponent<TMPro.TextMeshProUGUI>();
        dialogue_text_bar.color = Color.white;

        start_new_lines();
    }

    void Update()
    {
        int ind = message_controller.messages.IndexOf("press enter to talk");
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            if(p.near_by_npcs.IndexOf(gameObject)<0) p.near_by_npcs.Add(gameObject);
            if(p.locked_npc==null) p.locked_npc = gameObject;
            if(ind<0){
                message_controller.messages.Add("press enter to talk");
                message_controller.current = message_controller.messages.Count-1;
            }
            if(Input.GetKeyDown(KeyCode.Return)&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to talk"&&!in_conversation&&p.locked_npc==gameObject){
                message_screen.transform.localScale = Vector3.zero;
                switch_message.transform.localScale = Vector3.zero;
                dialogue_screen.transform.parent.localScale = Vector3.one;
                p.in_conversation = true;
                in_conversation = true;
                start_new_lines();
            }
        }
        else{
            if(gameObject==p.locked_npc) Destroy(p.npc_marker);
            if(ind>=0){
                message_controller.messages.RemoveAt(ind);
            }
            int a = p.near_by_npcs.IndexOf(gameObject);
            if(a<0) return;
            p.near_by_npcs.RemoveAt(a);
        }
        if(in_conversation){
            converse();
        }
    }

    void converse(){
        if(Input.GetKeyDown(KeyCode.Return)) current_line+=1;
        if(current_line==lines.Length){
            message_screen.transform.localScale = Vector3.one;
            switch_message.transform.localScale = Vector3.one;
            dialogue_screen.transform.parent.localScale = Vector3.zero;
            p.in_conversation = false;
            in_conversation = false;
            //switch to the buffer dialogue if we're not on the buffer dialogue right now
            if(index%2==0) p.current_world.npc_index[npc_name]+=1;
            return;
        }
        dialogue_text_bar.text = lines[current_line];
    }

    void start_new_lines(){
        current_line = -1;
        //get the current list of strings that the npc should say, which is stored in world details.
        index = p.current_world.npc_index[npc_name];
        lines = statics.npc_lines[npc_name][index].Split("\n");
    }
}
