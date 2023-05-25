using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class npc_control : MonoBehaviour
{
    public float trigger_dist = 0f;
    public bool in_conversation, force_talk, wait_for_input, look_at_player = true;
    GameObject message_screen, switch_message, player, dialogue_screen;
    public GameObject player_input_box;
    switchmessages message_controller;
    player_control p;
    List<string> dialogues;
    public TMPro.TextMeshProUGUI dialogue_text_bar, npc_name_bar;
    string[] lines;
    //-1 cause the same button press that triggers the conversation also changes current_line by one
    public int current_line = -1, index = 0;
    public string player_input;
    void Start()
    {
        player_input_box = GameObject.Find("player_input_box");
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
        if(wait_for_input) return;
        if(lines[0].IndexOf("force_player_into_conversation")>=0){
            force_talk = true;
            lines[0] = lines[0].Remove(0, 30);
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
            if(look_at_player) face_player();
            if(p.near_by_npcs.IndexOf(gameObject)<0) p.near_by_npcs.Add(gameObject);
            if(p.locked_npc==null) p.locked_npc = gameObject;
            if(ind<0){
                message_controller.messages.Add("press enter to talk");
                message_controller.current = message_controller.messages.Count-1;
            }
            if(Input.GetButton("confirm")&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to talk"&&!in_conversation&&p.locked_npc==gameObject){
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
        if(Input.GetButton("confirm")) current_line+=1;
    
        //if npc has finished a set of dialogues, stop the conversation and prevent it from starting again in this frame
        if(current_line>=lines.Length){
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
        if(lines[current_line]=="[player_input]"){
            wait_for_input = true;
            in_conversation = false;
            current_line+=1;
            get_input();
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

    void get_input(){
        player_input_box.transform.localScale = Vector3.one;
        player_input_box.GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate{player_input = player_input_box.GetComponent<TMP_InputField>().text;});
    }
}
