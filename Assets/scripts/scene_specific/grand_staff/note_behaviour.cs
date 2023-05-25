using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class note_behaviour : MonoBehaviour
{
    GameObject player;
    public int space;
    public bool controled;
    public float trigger_dist;
    public float spacing;
    Collider2D[] c;
    player_control player_c;
    reactive_messages r;
    void Start()
    {
        player = GameObject.Find("player");
        player_c = player.GetComponent<player_control>();
        c = GetComponents<Collider2D>();
        transform.GetChild(0).gameObject.GetComponent<teleporter_behaviour>().destination_loc = transform.position;
        r = GameObject.Find("temporary_messages").GetComponent<reactive_messages>();
    }

    void Update()
    {
        if((player.transform.position-transform.position).magnitude>trigger_dist) foreach(Collider2D col in c) col.enabled = true;
        if(player.transform.position==transform.position&&!player_c.stop){
            player_c.stop = true;
            controled = true;
            StartCoroutine(r.show_message("w and s to move up and down one space. enter to regain control"));
        }
        if(controled){
            move();
        }
        if(Input.GetButton("confirm")&&controled){
            controled = false;
            player_c.stop = false;
            foreach(Collider2D col in c) col.enabled = false;
        }
    }

    void move(){
        if(Input.GetAxisRaw("Vertical")>0f&&space<4){
            transform.position+=new Vector3(0f, spacing, 0f);
            player.transform.position = transform.position;
            space+=1;
        }
        else if(Input.GetAxisRaw("Vertical")<0f&&space>0){
            transform.position-=new Vector3(0f, spacing, 0f);
            player.transform.position = transform.position;
            space-=1;
        }
    }
}
