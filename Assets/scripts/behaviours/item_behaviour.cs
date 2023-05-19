using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class item_behaviour : MonoBehaviour
{
    GameObject player, message_screen;
    public float trigger_dist, flash_period;
    public string type;
    public bool entered = false;
    SpriteRenderer sprite;
    inventory_manager inv_manager;
    player_control plac;
    switchmessages swi;
    reactive_messages temp_message;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("player");
        message_screen = GameObject.Find("message_screen");
        StartCoroutine(flash());
        inv_manager = GameObject.Find("inventory_content").GetComponent<inventory_manager>();
        plac = player.GetComponent<player_control>();
        swi = message_screen.GetComponent<switchmessages>();
        temp_message = GameObject.Find("temporary_messages").GetComponent<reactive_messages>();
    }

    void Update()
    {
        int ind = swi.messages.IndexOf("press enter to pick up item");
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            // Debug.Log("vector3 angle: "+Vector3.Angle(transform.up, (player.transform.position-transform.position)).ToString());
            // Debug.Log(player.transform.rotation.eulerAngles.z);
            // float ang = Vector3.Angle(transform.position-player.transform.position);
            // float player_pointing = player.transform.rotation.eulerAngles.z;

            if(ind<0&&!entered){
                entered = true;
                swi.messages.Add("press enter to pick up item");
                swi.current = swi.messages.Count-1;
            }
        }
        else{
            if(ind>=0&&entered) {
                swi.messages.RemoveAt(ind);
                swi.current = swi.messages.Count-1;
            }
            entered = false;
        }

        if(Input.GetKeyDown(KeyCode.Return)&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to pick up item"&&entered){
                int i;
                StartCoroutine(temp_message.show_message("picked up a "+gameObject.name.Replace('_', ' ')));
                //if the player already has the item in inventory, add one to the item's count
                for(i=0; i<plac.unbuffed_player_stat.inv.inv.Count; i++){
                    //Debug.Log(plac.unbuffed_player_stat.inv.inv[i].item_name);
                    if(plac.unbuffed_player_stat.inv.inv[i].item_name==gameObject.name){
                        plac.unbuffed_player_stat.inv.inv[i] = new item(gameObject.name, plac.unbuffed_player_stat.inv.inv[i].num_left+1, statics.item_types[gameObject.name]);
                        if(swi.messages.IndexOf("press enter to pick up item")>=0) swi.messages.RemoveAt(swi.messages.IndexOf("press enter to pick up item"));
                        Destroy(gameObject);
                        return;
                    }
                }
                plac.unbuffed_player_stat.inv.inv.Add(new item(gameObject.name, 1, statics.item_types[gameObject.name]));
                inv_manager.add_item(Resources.Load<GameObject>("prefab/UI_items/"+gameObject.name), 1);
                if(swi.messages.IndexOf("press enter to pick up item")>=0) swi.messages.RemoveAt(swi.messages.IndexOf("press enter to pick up item"));
                Destroy(gameObject);
        }
    }
    
    IEnumerator flash(){
        bool decreasing = true;
        while(true){
            if(!decreasing){
                sprite.color+=new Color(0f, 0f, 0f, Time.deltaTime/flash_period);
                if(sprite.color.a>=1f){
                    decreasing = true;
                }
            }
            else{
                sprite.color-=new Color(0f, 0f, 0f, Time.deltaTime/flash_period);
                if(sprite.color.a<=0f) decreasing = false;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
