using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hermite_behaviour : MonoBehaviour
{
    public GameObject boss_stopper, teleporter, message_screen, enemy_health_bar;
    npc_control n;
    rope r;
    player_control player;
    public float health, max_health;
    public stats stat;

    void Start()
    {
        health =  max_health;
        player = GameObject.Find("player").GetComponent<player_control>();
        enemy_health_bar = GameObject.Find("enemy_health_bar");
        n = GetComponent<npc_control>();
        r = GetComponent<rope>();
        r.enabled = false;
        message_screen = GameObject.Find("message_screen");
    }

    void Update()
    {
        if(player.current_world.hermite_dead){
            teleporter.SetActive(true);
            Destroy(gameObject);
        }
        //if the boss fight has started
        if(r.enabled) {
            enemy_health_bar.transform.parent.localScale = Vector3.one;
            enemy_health_bar.transform.localScale = new Vector3(health/max_health, 1, 1);
            return;
        }
        //if the boss has finished the monologue
        if(player.current_world.npc_index["Hermite"]==1){
            n.force_talk = false;
        }
        if(player.current_world.npc_index["Hermite"]==1&&n.entered) {
            GameObject.Find("player_health_bar").GetComponent<health_bar>().boss_fight = true;
            GetComponent<AudioSource>().Play();
            message_screen.GetComponent<switchmessages>().messages.Remove("press enter/B to talk");
            boss_stopper.SetActive(true);
            r.enabled = true;
            r.init();
            n.enabled = false;
        }
    }

    IEnumerator diag(){
        boss_stopper.SetActive(false);
        StartCoroutine(lower_volume());
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
        save_load.SavePlayer(player.unbuffed_player_stat);
        save_load.Saveworld(player.current_world, player.unbuffed_player_stat.name);
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

    IEnumerator lower_volume(){
        float time = 0f;
        while(time<4f){
            GetComponent<AudioSource>().volume-=Time.deltaTime/4f;
            time+=Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
