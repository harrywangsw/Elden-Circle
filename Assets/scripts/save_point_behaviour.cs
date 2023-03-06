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
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            message_screen.GetComponent<TMPro.TextMeshProUGUI>().text = "press enter to save";
            if(Input.GetKeyDown(KeyCode.Return)){
                save_load.SavePlayer(player.GetComponent<player_control>().player_stat);
                save_load.SavePlayerItem(player.GetComponent<player_control>().player_items, player.GetComponent<player_control>().player_name);
                player.GetComponent<player_control>().current_world.player_pos_x = player.transform.position.x;
                player.GetComponent<player_control>().current_world.player_pos_y = player.transform.position.y;
                save_load.Saveworld(player.GetComponent<player_control>().current_world, player.GetComponent<player_control>().player_name);
            }
        }
        if((player.transform.position-transform.position).magnitude>=trigger_dist){
            if(message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to save"){
                message_screen.GetComponent<TMPro.TextMeshProUGUI>().text="\n";
            }
        }
    }
}
