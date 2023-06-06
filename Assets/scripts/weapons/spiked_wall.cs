using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class spiked_wall : MonoBehaviour
{
    public bool new_input, attacking, attack_interrupted;
    public float range, stamina_cost;
    public bool* p_newinput;
    public Vector3 init_loc;
    public GameObject user;

    enemy_control enemy;
    player_control player;
    public GameObject wall;
    public float period, vel;
    public bool init_new_attacking = true;
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
        new_input = *p_newinput;
        if(new_input&&init_new_attacking){
            StartCoroutine(spawn_wall());
        }
    }

    IEnumerator spawn_wall(){
        init_new_attacking = false;
        attacking = true;
        GameObject w = GameObject.Instantiate(wall, user.transform.position+user.transform.rotation*Vector3.up*(range*user.transform.localScale.x), Quaternion.Euler(user.transform.rotation.eulerAngles.x, user.transform.rotation.eulerAngles.y, user.transform.rotation.eulerAngles.z-180f));
        Physics2D.IgnoreCollision(w.GetComponent<Collider2D>(), GameObject.Find("Grid").GetComponent<Collider2D>(), true);
        if(enemy!=null) Physics2D.IgnoreCollision(w.GetComponent<Collider2D>(), user.GetComponent<Collider2D>(), true);
        statics.apply_stats(GetComponent<damage_manager>(), w.GetComponent<damage_manager>(), new stats());
        w.transform.localScale = Vector3.zero;
        yield return StartCoroutine(statics.expand(w.transform, period, Vector3.one));
        w.GetComponent<Collider2D>().enabled = true;
        w.GetComponent<Rigidbody2D>().velocity = ((Vector2)(w.transform.rotation*(Vector3.up))*vel);
        float time = range/w.GetComponent<Rigidbody2D>().velocity.magnitude;
        attacking = false;
        yield return new WaitForSeconds(time);
        init_new_attacking = true;
    }

}
