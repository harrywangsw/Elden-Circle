using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class item_behaviour : MonoBehaviour
{
    GameObject player, message_screen;
    public float trigger_dist;
    void Start()
    {
        player = GameObject.Find("player");
        message_screen = GameObject.Find("message_screen");
    }

    void Update()
    {
        int ind = message_screen.GetComponent<switchmessages>().messages.IndexOf("press enter to pick up item");
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            if(ind<0){
                message_screen.GetComponent<switchmessages>().messages.Add("press enter to pick up item");
                message_screen.GetComponent<switchmessages>().current = message_screen.GetComponent<switchmessages>().messages.Count-1;
            }
            if(Input.GetKeyDown(KeyCode.Return)&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to pick up item"){
                int i;
                for(i=0; i<player.GetComponent<player_control>().player_items.inv.Count; i++){
                    Debug.Log(player.GetComponent<player_control>().player_items.inv[i].Item1);
                    if(player.GetComponent<player_control>().player_items.inv[i].Item1==gameObject.name){
                        player.GetComponent<player_control>().player_items.inv[i] = Tuple.Create(gameObject.name, player.GetComponent<player_control>().player_items.inv[i].Item2+1);
                        Debug.Log(player.GetComponent<player_control>().player_items.inv[i].Item2.ToString());
                        player.GetComponent<player_control>().Update_quickslot();
                        Destroy(gameObject);
                        return;
                    }
                }
                player.GetComponent<player_control>().player_items.inv.Add(Tuple.Create(gameObject.name, 1));
            }
        }
        else{
            if(ind>=0){
                message_screen.GetComponent<switchmessages>().messages.RemoveAt(ind);
            }
        }
    }
}
