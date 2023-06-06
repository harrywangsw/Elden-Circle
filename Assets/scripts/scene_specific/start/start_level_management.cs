using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_level_management : MonoBehaviour
{
    player_control player;
    public GameObject hari;
    void Start()
    {

    }

    public void late_start(world_details world){
        if(world.npc_index["hari"]==1){
            Debug.Log("wtf");
            Destroy(hari);
        }
        else if(world.npc_index["hari"]==0){
            hari.SetActive(true);
            StartCoroutine(GameObject.Find("temporary_messages").GetComponent<reactive_messages>().show_message("Press enter to continue"));
        }
    }

    void Update()
    {
    }
}
