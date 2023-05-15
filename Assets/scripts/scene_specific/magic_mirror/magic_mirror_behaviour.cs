using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic_mirror_behaviour : MonoBehaviour
{
    npc_control con;
    bool torque_added = false;
    GameObject mom;
    void Start()
    {
        con = gameObject.GetComponent<npc_control>();
        mom = GameObject.Find("happy_mother");
    }

    void Update()
    {
        if(con.dialogue_text_bar.text=="You may enter."&&!torque_added){
            torque_added = true;
            GetComponent<Rigidbody2D>().AddTorque(88f);
        }
        if(con.player_input.IndexOf("yuan")>=0||con.player_input.IndexOf("Yuan")>=0){
            Debug.Log("sd");
            con.player_input_box.transform.localScale = Vector3.zero;
            mom.transform.localScale = Vector3.one;
            con.wait_for_input = false;
            con.in_conversation = true;
        }
    }
}
