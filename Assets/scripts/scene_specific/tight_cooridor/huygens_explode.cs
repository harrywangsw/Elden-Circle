using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class huygens_explode : MonoBehaviour
{
    // Attatch to huygens prefab


    void OnCollisionEnter2D(Collision2D c){
        //insta kill player
        if(c.collider.gameObject.GetComponent<damage_manager>()!=null){
            player_control p = GameObject.Find("player").GetComponent<player_control>();
            p.death_period = 0f;
            StartCoroutine(p.death());
        }
    }


}
