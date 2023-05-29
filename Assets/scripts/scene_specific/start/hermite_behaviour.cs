using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hermite_behaviour : MonoBehaviour
{
    public GameObject boss_stopper, teleporter, message_screen;
    npc_control n;
    rope r;
    player_control player;
    public float health;
    public stats stat;

    void Start()
    {
        player = GameObject.Find("player").GetComponent<player_control>();
        n = GetComponent<npc_control>();
        r = GetComponent<rope>();
        r.enabled = false;
        message_screen = GameObject.Find("message_screen");
    }

    void Update()
    {
        if(player.current_world.hermite_dead){
            Destroy(gameObject);
        }
        if(!n.enabled) return;
        //if the boss has finished the monologue
        if(n.index==1) {
            message_screen.GetComponent<switchmessages>().messages.Remove("press enter/B to talk");
            GetComponent<AudioSource>().Play();
            boss_stopper.SetActive(true);
            r.enabled = true;
            r.init();
            n.enabled = false;
        }
    }

    IEnumerator diag(){
        StartCoroutine(lower_volume());
        boss_stopper.SetActive(false);
        GameObject dialogue_screen = GameObject.Find("dialogue_screen");
        TMPro.TextMeshProUGUI dialogue_text_bar = dialogue_screen.GetComponent<TMPro.TextMeshProUGUI>();
        dialogue_screen.transform.parent.localScale = Vector3.one;
        dialogue_text_bar.text = "You don't understand";
        float time = 0f;
        while(time<3f){
            if(Input.GetButtonDown("confirm")){
                break;
            }
            time+=Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        dialogue_text_bar.text = "You will never understand";
        time = 0f;
        while(time<3f){
            if(Input.GetButtonDown("confirm")){
                break;
            }
            time+=Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        dialogue_screen.transform.parent.localScale = Vector3.zero;
        StartCoroutine(GameObject.Find("temporary_messages").GetComponent<reactive_messages>().show_message("walk into the teleporter"));
        teleporter.SetActive(true);
        teleporter.transform.position = transform.position;
        player.current_world.hermite_dead = true;
        save_load.SavePlayer(player.player_stat);
        save_load.Saveworld(player.current_world, player.player_stat.name);
    }

    IEnumerator lower_volume(){
        float time = 0f;
        while(time<6f){
            GetComponent<AudioSource>().volume-=Time.deltaTime/6f;
            time+=Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D c){
        damage_manager d = c.collider.gameObject.GetComponent<damage_manager>();
        if(!d) return; 
        StartCoroutine(statics.animate_hurt(GetComponent<SpriteRenderer>()));
        health-=statics.calc_damage(stat, d);
        if(health<=0f){
            StartCoroutine(diag());
            GetComponent<rope>().dead = true;
        }
    }
}
