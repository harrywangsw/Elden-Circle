using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(damage_manager))]
[RequireComponent(typeof(BoxCollider2D))]
public unsafe class spear_attack : MonoBehaviour
{
    public bool init_attack, new_input, attacking;
    public float thrust_vel, thrust_period, range, parriable_window;
    public bool* p_newinput;
    public Vector3 init_loc;
    SpriteRenderer sprite;
    Collider2D c;
    Rigidbody2D body;

    void Start()
    {
        c = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        c.enabled = false;
        sprite.enabled = false;
        GameObject user = transform.parent.gameObject;
        Physics2D.IgnoreCollision(user.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
    }

    void Update()
    {
        new_input = *p_newinput;
        thrust_vel = range/thrust_period;
        if (new_input&&!attacking) StartCoroutine(thrust());
    }

    IEnumerator thrust(){
        attacking = true;
        c.enabled = true;
        sprite.enabled = true;
        //Debug.Log("wtf"+body.velocity.y.ToString());
        //body.velocity = thrust_vel*Vector2.up;
        float time  = 0f;
        while(time<thrust_period/2f){
            transform.localPosition+=new Vector3(0f, thrust_vel*Time.deltaTime, 0f);
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        yield return new WaitForSeconds(thrust_period/2f);
        //body.velocity = -thrust_vel*Vector2.up;
        while(time>thrust_period/2f&&time<thrust_period){
            transform.localPosition-=new Vector3(0f, thrust_vel*Time.deltaTime, 0f);
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }

        transform.localPosition = init_loc;
        yield return new WaitForSeconds(0.1f);
        attacking = false;
        c.enabled = false;
        sprite.enabled = false;
    }
}
