using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class save_point_behaviour : MonoBehaviour
{
    GameObject player;
    GameObject message_screen;
    void Start()
    {
        player = GameObject.Find("player");
        message_screen = GameObject.Find("message_screen");
    }


    void Update()
    {
        if((player.transform.position-transform.position).magnitude<=8f){
            message_screen.GetComponent<TMPro.TextMeshProUGUI>().text = "press enter to save";
            if(Input.GetKeyDown(KeyCode.Return)){
                save_load.SavePlayer(player.GetComponent<player_control>().player_stat, player.GetComponent<player_control>().player_name);
                save_load.SavePlayerItem(player.GetComponent<player_control>().player_items, player.GetComponent<player_control>().player_name);
            }
        }
    }
}
