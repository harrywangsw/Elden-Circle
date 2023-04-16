using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(damage_manager))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public unsafe class spear_attack : MonoBehaviour
{
    public bool init_attack, new_input, attacking;
    public float thrust_vel, thrust_period, range;
    public bool* p_newinput;
    Rigidbody2D body;
    public Vector3 init_loc;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        new_input = *p_newinput;
        thrust_vel = range/thrust_period;
        if (get_new_input()) init_attack = new_input;
        if (init_attack&&!attacking) StartCoroutine(thrust());
    }

    IEnumerator thrust(){
        attacking = true;
        //Debug.Log("wtf"+body.velocity.y.ToString());
        float time  = 0f;
        while(time<thrust_period/2f){
            transform.localPosition+=new Vector3(0f, thrust_vel*Time.deltaTime, 0f);
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        yield return new WaitForSeconds(0.1f);
        while(time>thrust_period/2f&&time<thrust_period){
            transform.localPosition-=new Vector3(0f, thrust_vel*Time.deltaTime, 0f);
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }

        body.position = init_loc;
        yield return new WaitForSeconds(0.1f);
        attacking = false;
    }

    bool get_new_input(){
        return body.velocity.y<=0f;
    }
}
