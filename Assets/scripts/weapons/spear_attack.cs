using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(damage_manager))]
public unsafe class spear_attack : MonoBehaviour
{
    public bool init_attack, new_input, attacking;
    public float thrust_vel, thrust_period, range, parriable_window, stamina_cost;
    public bool* p_newinput;
    public Vector3 init_loc;
    SpriteRenderer sprite;
    Collider2D c;
    Rigidbody2D body;
    GameObject target, user;
    enemy_control enemy;
    player_control player;

    void Start()
    {
        c = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        c.enabled = false;
        sprite.enabled = false;
        user = transform.parent.gameObject;
        Physics2D.IgnoreCollision(user.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        if(user.GetComponent<player_control>()!=null){
            player = user.GetComponent<player_control>();
        }
        else{
            enemy = user.GetComponent<enemy_control>();
        }
    }

    void Update()
    {
        if(player!=null) target = player.locked_enemy;
        else if(enemy!=null) target = enemy.player;
        //Debug.DrawRay(transform.position, target.transform.position-transform.position, Color.green);
        new_input = *p_newinput;
        thrust_vel = range/thrust_period;
        if (new_input&&!attacking) StartCoroutine(thrust());
    }

    IEnumerator thrust(){
        transform.localPosition = init_loc;
        transform.parent = null;
        attacking = true;
        c.enabled = true;
        sprite.enabled = true;
        //Debug.Log("wtf"+body.velocity.y.ToString());
        body.velocity = user.transform.rotation*(Vector2.up)*thrust_vel;
        // Vector3 thrust_vector = Vector3.up;
        // float time  = 0f;
        // while(time<thrust_period/2f){
        //     transform.localPosition+=thrust_vector*thrust_vel*Time.deltaTime;
        //     yield return new WaitForSeconds(Time.deltaTime);
        //     time+=Time.deltaTime;
        // }
        yield return new WaitForSeconds(thrust_period/2f);
        body.velocity = user.transform.rotation*(-Vector2.up)*thrust_vel;
        // while(time>thrust_period/2f&&time<thrust_period){
        //     transform.localPosition-=thrust_vector*thrust_vel*Time.deltaTime;
        //     yield return new WaitForSeconds(Time.deltaTime);
        //     time+=Time.deltaTime;
        // }
        yield return new WaitForSeconds(thrust_period);
        body.velocity = Vector3.zero;
        transform.position = user.transform.position;
        c.enabled = false;
        yield return new WaitForSeconds(0.2f);
        attacking = false;
        sprite.enabled = false;
        transform.parent = user.transform;
        transform.localRotation = Quaternion.identity;
    }

    void OnCollisionEnter2D(){
        c.enabled = false;
    }
}
