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
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            message_screen.GetComponent<TMPro.TextMeshProUGUI>().text = "press enter to pick up item";
            if(Input.GetKeyDown(KeyCode.Return)){
                int i;
                for(i=0; i<player.GetComponent<player_control>().player_items.inv.Count; i++){
                    Debug.Log(player.GetComponent<player_control>().player_items.inv[i].Item1);
                    if(player.GetComponent<player_control>().player_items.inv[i].Item1==gameObject.name){
                        player.GetComponent<player_control>().player_items.inv[i] = Tuple.Create(gameObject.name, player.GetComponent<player_control>().player_items.inv[i].Item2+1);
                        Debug.Log(player.GetComponent<player_control>().player_items.inv[i].Item2.ToString());
                        player.GetComponent<player_control>().Update_quickslot();
                        break;
                    }
                }
                Destroy(gameObject);
            }
        }
        if((player.transform.position-transform.position).magnitude>=trigger_dist){
            if(message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to pick up item"){
                message_screen.GetComponent<TMPro.TextMeshProUGUI>().text="\n";
            }
        }
    }
}
