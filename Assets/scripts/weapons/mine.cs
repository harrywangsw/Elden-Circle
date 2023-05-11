using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class mine : MonoBehaviour
{
    public bool new_input, attacking, attack_interrupted;
    public float range, stamina_cost;
    public bool* p_newinput;
    public Vector3 init_loc;
    
    public float period;
    enemy_control enemy;
    player_control player;
    public GameObject mines, user;
    void Start()
    {
        user = transform.parent.gameObject;
        if(user.GetComponent<player_control>()!=null){
            player = user.GetComponent<player_control>();
        }
        else{
            enemy = user.GetComponent<enemy_control>();
        }
    }

    void Update()
    {
        if(player!=null) attack_interrupted = player.attack_interrupted;
        new_input = *p_newinput;
        if(new_input&&!attacking){
            StartCoroutine(spawn_mine());
        }
    }

    IEnumerator spawn_mine(){
        attacking = true;
        GameObject m = GameObject.Instantiate(mines, user.transform.position+init_loc, Quaternion.identity);
        statics.apply_stats(GetComponent<damage_manager>(), m.GetComponent<damage_manager>(), new stats());
        Collider2D c = m.GetComponent<Collider2D>();
        c.enabled = false;
        m.transform.localScale = Vector3.zero;
        float time = 0f;
        while(time<period&&!attack_interrupted){
            m.transform.localScale+=Vector3.one*Time.deltaTime/period;
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        attacking = false;
        if(attack_interrupted){
            Destroy(m);
            yield break;
        }
        c.enabled = true;
    }
}
