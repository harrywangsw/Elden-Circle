using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class npc_control : MonoBehaviour
{
    public float trigger_dist = 0f;
    public bool in_conversation, force_talk;
    GameObject message_screen, switch_message, player, dialogue_screen;
    switchmessages message_controller;
    player_control p;
    List<string> dialogues;
    TMPro.TextMeshProUGUI dialogue_text_bar, npc_name_bar;
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
        dialogues = statics.npc_lines[gameObject.name];
        dialogue_text_bar = dialogue_screen.GetComponent<TMPro.TextMeshProUGUI>();
        dialogue_text_bar.color = Color.white;
        npc_name_bar = GameObject.Find("npc_name").GetComponent<TMPro.TextMeshProUGUI>();
        start_new_lines();
    }

    void Update()
    {
        if(lines[current_line].IndexOf("force_player_into_conversation")>=0){
            force_talk = true;
            lines[current_line] = lines[current_line].Remove(0, 30);
        }
        //place this in front so that the enter that triggers in_conversation doesn't add one to current_line
        if(in_conversation){
            converse();
            return;
        }
        
        int ind = message_controller.messages.IndexOf("press enter to talk");
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            if(force_talk&&!in_conversation) {
                message_screen.transform.parent.localScale = Vector3.zero;
                switch_message.transform.parent.localScale = Vector3.zero;
                dialogue_screen.transform.parent.localScale = Vector3.one;
                p.stop = true;
                in_conversation = true;
                start_new_lines();
            }
            face_player();
            if(p.near_by_npcs.IndexOf(gameObject)<0) p.near_by_npcs.Add(gameObject);
            if(p.locked_npc==null) p.locked_npc = gameObject;
            if(ind<0){
                message_controller.messages.Add("press enter to talk");
                message_controller.current = message_controller.messages.Count-1;
            }
            if(Input.GetKeyDown(KeyCode.Return)&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to talk"&&!in_conversation&&p.locked_npc==gameObject){
                message_screen.transform.parent.localScale = Vector3.zero;
                switch_message.transform.parent.localScale = Vector3.zero;
                dialogue_screen.transform.parent.localScale = Vector3.one;
                p.stop = true;
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
    }

    void face_player(){
        transform.rotation = Quaternion.Euler(0f, 0f, Vector3.SignedAngle(Vector3.up, player.transform.position-transform.position, Vector3.forward));
    }

    void converse(){
        npc_name_bar.text = gameObject.name+":";
        if(Input.GetKeyDown(KeyCode.Return)) current_line+=1;
        //if npc has finished a set of dialogues, stop the conversation and prevent it from starting again in this frame
        if(current_line==lines.Length){
            message_screen.transform.parent.localScale = Vector3.one;
            switch_message.transform.parent.localScale = Vector3.one;
            dialogue_screen.transform.parent.localScale = Vector3.zero;
            p.stop = false;
            in_conversation = false;
            force_talk = false;
            //switch to the buffer dialogue if we're not on the buffer dialogue right now
            if(index%2==0) p.current_world.npc_index[gameObject.name]+=1;
            start_new_lines();
            return;
        }
        dialogue_text_bar.text = lines[current_line];
    }

    void start_new_lines(){
        current_line = 0;
        //get the current list of strings that the npc should say, which is stored in world details.
        index = p.current_world.npc_index[gameObject.name];
        lines = statics.npc_lines[gameObject.name][index].Split("\n");
    }
}
