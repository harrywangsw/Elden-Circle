using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hermite_behaviour : MonoBehaviour
{
    public GameObject boss_stopper;
    npc_control n;
    rope r;
    player_control player;
    void Start()
    {
        player = GameObject.Find("player").GetComponent<player_control>();
        n = GetComponent<npc_control>();
        r = GetComponent<rope>();
        r.enabled = false;
    }

    void Update()
    {
        if(player.current_world.hermite_dead){
            Destroy(gameObject);
        }
        if(r.dead){
            player.current_world.hermite_dead = true;
            save_load.SavePlayer(player.player_stat);
            save_load.Saveworld(player.current_world, player.player_stat.name);
            StartCoroutine(diag());
        }
        if(!n.enabled) return;
        if(n.index==1) {
            boss_stopper.SetActive(true);
            r.enabled = true;
            r.init();
            n.enabled = false;
        }
    }

    IEnumerator diag(){
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
    }
}
