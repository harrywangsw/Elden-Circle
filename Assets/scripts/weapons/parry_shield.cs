using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class parry_shield : MonoBehaviour
{
    public bool* p_newinput;
    public bool attacking, new_input;
    public float period, range;
    public Vector3 init_loc;
    CircleCollider2D col;
    SpriteRenderer sprite;

    void Start()
    {
        col = gameObject.GetComponent<CircleCollider2D>();
        col.enabled = false;
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        new_input = *p_newinput;
        if(new_input&&!attacking){
            StartCoroutine(shield_up());
        }
    }

    IEnumerator shield_up(){
        attacking = true;
        float time = 0f;
        while(time<period){
            time+=Time.deltaTime;
            transform.localScale+= new Vector3(Time.deltaTime/period, Time.deltaTime/period, 0f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        col.enabled = true;
        gameObject.tag = "poise_breaker";
        yield return new WaitForSeconds(period);
        col.enabled = false;
        gameObject.tag = "Untagged";
        time = 0f;
        while(time<period){
            time+=Time.deltaTime;
            sprite.color = sprite.color-new Color(0f, 0f, 0f, Time.deltaTime/period);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        sprite.color = Color.black;
        transform.localScale = Vector3.zero;
        attacking = false;
    }
}
