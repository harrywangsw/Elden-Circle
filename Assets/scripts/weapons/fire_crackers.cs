using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class fire_crackers : MonoBehaviour
{
    public float period, range;
    public int num = 0, final_num;
    public bool* p_newinput;
    public bool attacking, new_input, init_new_attack=true;
    Sprite explosion, cracker;
    public Vector3 init_loc, init_size;
    GameObject user;

    void Start(){
        explosion = Resources.Load<Sprite>("sprites/explosion");
        cracker = Resources.Load<Sprite>("sprites/firecracker");
        if(transform.parent!=null) user = transform.parent.gameObject;
        if(num==final_num){
            Destroy(gameObject);
        }
        if(num!=0){
            StartCoroutine(explode());
        }
    }

    IEnumerator explode(){
        float original_size = transform.localScale.x;
        float expand_rate = 4f*transform.localScale.x/period; 
        while(transform.localScale.x<=2f*original_size){
            transform.localScale+=expand_rate*Time.deltaTime*Vector3.one;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //spawn another one to the left and track how many has been spawned
        Vector3 new_pos = new Vector3(Random.Range(0f, range*2f), Random.Range(-range/2f, range/2f), 0f);
        new_pos=Quaternion.Euler(0f, 0f, transform.eulerAngles.z)*-new_pos;
        GameObject another = GameObject.Instantiate(gameObject, transform.position+new_pos, transform.rotation);
        another.transform.localScale = Vector3.one*original_size;
        another.GetComponent<fire_crackers>().num = num+1;
        //Debug.Log("num:"+num.ToString()+" size: "+original_size.ToString());
        while(transform.localScale.x<=4f*original_size){
            transform.localScale+=expand_rate*Time.deltaTime*Vector3.one;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //gameObject.GetComponent<SpriteRenderer>().sprite = explosion;
        //yield return new WaitForSeconds(period/2f);
        if(num==0){
            //gameObject.GetComponent<SpriteRenderer>().sprite = cracker;
            transform.localScale = Vector3.zero;
            yield return new WaitForSeconds(period*(final_num-1));
            transform.SetParent(user.transform);
            transform.localPosition = init_loc;
            transform.localRotation = Quaternion.identity;
            init_new_attack = true;
            yield break;
        }
        Destroy(gameObject);
    }

    void Update()
    {
        if(num!=0) return;
        else{
            new_input = *p_newinput;
            if(init_new_attack&&new_input) {
                transform.localScale = init_size;
                transform.parent = null;
                init_new_attack = false;
                StartCoroutine(explode());
            }
        }
    }
}
