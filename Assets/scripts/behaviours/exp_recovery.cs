using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exp_recovery : MonoBehaviour
{
    public float trigger_dist;
    switchmessages swi;
    player_control player;
    GameObject message_screen;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        message_screen = GameObject.Find("message_screen");
        swi = message_screen.GetComponent<switchmessages>();
        player = GameObject.Find("player").GetComponent<player_control>();
        int ind = swi.messages.IndexOf("recover lost exp");
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            if(ind<0) {
                swi.messages.Add("recover lost exp");
                swi.current = swi.messages.Count-1;
            }

        }
        else if(ind>=0) swi.messages.Remove("recover lost exp");

        if(Input.GetButtonDown("confirm")&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="recover lost exp"){
            player.unbuffed_player_stat.exp+=player.unbuffed_player_stat.exp_lost;
            player.unbuffed_player_stat.exp_lost = 0;
            player.unbuffed_player_stat.exp_pos_x = float.PositiveInfinity;
            player.unbuffed_player_stat.exp_pos_y = float.PositiveInfinity;
            if(swi.messages.IndexOf("recover lost exp")>=0) swi.messages.RemoveAt(ind);
            Destroy(gameObject);
            return;
        }
    }
}
