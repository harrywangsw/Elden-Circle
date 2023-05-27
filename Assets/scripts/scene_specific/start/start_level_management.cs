using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_level_management : MonoBehaviour
{
    player_control player;
    void Start()
    {
        player = GameObject.Find("player").GetComponent<player_control>();
        StartCoroutine(late_start());
    }

    IEnumerator late_start(){
        yield return new WaitForSeconds(0.1f);
        if(player.current_world.npc_index["hari"]==1){
            Destroy(GameObject.Find("hari"));
        }
        else if(player.current_world.npc_index["hari"]==0){
            StartCoroutine(GameObject.Find("temporary_messages").GetComponent<reactive_messages>().show_message("Press enter to continue"));
        }
    }

    void Update()
    {
    }
}
