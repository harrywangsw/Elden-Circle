using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class dagger_fan : MonoBehaviour
{
    GameObject dagger, user;
    public int num;
    public float period, range, stamina_cost;
    bool all_ready = false;
    public bool new_input, attacking = false, clone = false;
    public bool* p_newinput;
    public Vector3 init_loc = new Vector3();
    
    void Start()
    {
        dagger = Resources.Load<GameObject>("weapons/dagger");
        if(transform.parent!=null) user = transform.parent.gameObject;
    }

    
    void Update()
    {
        //Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity);
        if(clone) return;
        new_input = *p_newinput;
        if(!attacking&&new_input){
            attacking = true;
            StartCoroutine(spawn_dagger());
        }
    }

    IEnumerator spawn_dagger(){
        float angle = 0f;
        for(int i=0; i<num; i++){
            Vector3 pos = transform.position;
            angle = i*360f/num;
            if(angle>180) angle-=360f;
            //to flip the dagger sprite
            angle-=180f;
            pos=Quaternion.Euler(0f, 0f, i*360f/num)*pos;
            GameObject n_dagger = GameObject.Instantiate(dagger, user.transform.position+pos, Quaternion.Euler(0f, 0f, i*360f/num));
            n_dagger.GetComponent<dagger_fan>().clone = true;
            n_dagger.transform.localScale = new Vector3(0f, 1f, 1f);
            StartCoroutine(shoot(n_dagger));
            yield return new WaitForSeconds(period/num);
        }
        all_ready = true;
        yield return new WaitForSeconds(2f*period);
        attacking = false;
    }

    IEnumerator shoot(GameObject dag){
        float expand_rate = 1f/period;
        while(dag.transform.localScale.x<1f){
            dag.transform.localScale+=new Vector3(expand_rate*Time.deltaTime, 0f, 0f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        dag.transform.localScale = Vector3.one;
        while(!all_ready){
            yield return new WaitForSeconds(Time.deltaTime);
        }
        dag.GetComponent<Rigidbody2D>().velocity = range/(2f*period)*(-dag.transform.up);
        Debug.Log(dag.GetComponent<Rigidbody2D>().velocity);
        yield return new WaitForSeconds(2f*period);
        Destroy(dag);
    }
}
