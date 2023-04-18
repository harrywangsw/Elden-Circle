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

    void Start()
    {
        
    }

    void Update()
    {
        new_input = *p_newinput;
        thrust_vel = range/thrust_period;
        if (new_input&&!attacking) StartCoroutine(thrust());
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

        transform.localPosition = init_loc;
        yield return new WaitForSeconds(0.1f);
        attacking = false;
    }
}
