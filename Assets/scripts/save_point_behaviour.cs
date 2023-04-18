using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class save_point_behaviour : MonoBehaviour
{
    GameObject player;
    GameObject message_screen;
    public float trigger_dist;
    void Start()
    {
        player = GameObject.Find("player");
        message_screen = GameObject.Find("message_screen");
    }


    void Update()
    {
        int ind = message_screen.GetComponent<switchmessages>().messages.IndexOf("press enter to save");
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            if(ind<0){
                message_screen.GetComponent<switchmessages>().messages.Add("press enter to save");
                message_screen.GetComponent<switchmessages>().current = message_screen.GetComponent<switchmessages>().messages.Count-1;
            }
            if(Input.GetKeyDown(KeyCode.Return)&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to save"){
                save_load.SavePlayer(player.GetComponent<player_control>().player_stat);
                save_load.SavePlayerItem(player.GetComponent<player_control>().player_items, player.GetComponent<player_control>().player_name);
                player.GetComponent<player_control>().current_world.player_pos_x = player.transform.position.x;
                player.GetComponent<player_control>().current_world.player_pos_y = player.transform.position.y;
                save_load.Saveworld(player.GetComponent<player_control>().current_world, player.GetComponent<player_control>().player_name);
            }
        }
        else{
            if(ind>=0){
                message_screen.GetComponent<switchmessages>().messages.RemoveAt(ind);
            }
        }
    }
}
