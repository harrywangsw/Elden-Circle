using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class save_point_behaviour : MonoBehaviour
{
    GameObject player;
    GameObject message_screen;
    public float trigger_dist;
    player_control p;
    void Start()
    {
        player = GameObject.Find("player");
        p = player.GetComponent<player_control>();
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
            if(Input.GetKeyDown(KeyCode.Return)&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to save"&&!p.dashing){
                save_load.SavePlayer(p.unbuffed_player_stat);
                p.current_world.player_pos_x = player.transform.position.x;
                p.current_world.player_pos_y = player.transform.position.y;
                save_load.Saveworld(p.current_world, p.player_stat.name);
            }
        }
        else{
            if(ind>=0){
                message_screen.GetComponent<switchmessages>().messages.RemoveAt(ind);
            }
        }
    }
}
