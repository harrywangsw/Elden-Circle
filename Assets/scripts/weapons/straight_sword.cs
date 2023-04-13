using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(damage_manager))]
public unsafe class straight_sword : MonoBehaviour
{
    public float extend_time, swing_angle, const_swing_angle, swing_period, range, attack_cool_off;
    public float torque;
    public float angul;
    public int swing_dir = 1;
    public int swings = 0;
    public bool init_attack, new_input, attacking, left;
    public bool* p_newinput;
    Rigidbody2D body;
    public GameObject dot;
    damage_manager manager;
    Collider2D c, selfc;
    Animator sword_animator;
    public SpriteRenderer sword_sprite;
    void Start()
    {
        swing_angle = const_swing_angle;
        body = gameObject.GetComponent<Rigidbody2D>();
        sword_sprite = gameObject.GetComponent<SpriteRenderer>();
        //body.rotation = swing_angle;
        manager = gameObject.GetComponent<damage_manager>();
        //make sure the sword won't heart its owner
        c = transform.parent.gameObject.GetComponent<Collider2D>();
        selfc = gameObject.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(selfc, c, true);
        Physics2D.IgnoreCollision(c, selfc, true);

        sword_animator = gameObject.GetComponent<Animator>();

        //switch off the collider when sword is retracted
        //selfc.enabled = false;
    }

    IEnumerator extend()
    {
        attacking = true;
        //selfc.enabled = true;
        body.angularVelocity = angul*swing_dir;
        if (manager.slash > manager.pierce) (manager.slash, manager.pierce) = (manager.pierce, manager.slash);
        manager.pierce *= 1.2f;
        //extend
        while (transform.localScale.x < 1f)
        {
            transform.localScale += new Vector3(Time.deltaTime / extend_time, 0f, 0f);
            if(transform.localScale.x>=1f) {
                transform.localScale=Vector3.one;
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.localScale = Vector3.one;
        manager.pierce /= 1.2f;
        //transition to swing only if not doing thrust
        if (swings != 3)
        {
            yield return swing();
        }
        else yield return retract();
    }
    
    IEnumerator retract()
    {
        attacking = true;
        //retract
        //Debug.Log("retract");
        while (transform.localScale.x > 0f)
        {
            //transition to extend if attack ordered, except for when thrusting
            if (init_attack&&swings!=3) 
            {
                yield return extend();
                yield break;
            }
            transform.localScale -= new Vector3(Time.deltaTime / extend_time, 0f, 0f);
            if (transform.localScale.x <= 0f)
            {
                //after retracted, reset sword
                transform.localScale = new Vector3(0f, 1f, 1f);
                body.rotation = swing_angle;
                body.angularVelocity = 0f;
                swings = 0;
                swing_dir = 1;
                attacking = false;
                init_attack = false;
                new_input = false;
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //selfc.enabled = false;
    }
    ///momentum carry player forward
    IEnumerator swing()
    {
        if(manager.slash<manager.pierce) (manager.slash, manager.pierce) = (manager.pierce, manager.slash);
        attacking = true;
        init_attack = false;
        new_input = false;
        swings++;
        body.angularVelocity = angul*swing_dir;
        yield return new WaitForSeconds(swing_period);

        // if (swings == 3)
        // {
        //     yield return new WaitForSeconds(0.1f);
        //     transform.localScale = new Vector3(0f, 1f, 1f);
        //     body.rotation = 90f;
        //     //Debug.Log("ended with thrust");
        //     yield return extend();
        //     yield break;
        // }
        // //Debug.Log(swing_dir);
        // float swing_time = 0f;
        // bool reverse = false;
        // while(swing_time<swing_period)
        // {
        //     if(swing_time>swing_period/3f&&swing_time<swing_period*(2f/3f)&&!reverse) {
        //         torque/=100f;
        //         Debug.Log("asdasds1");
        //         //so that torque don't keep decreasing
        //         reverse = true;
        //     }
        //     if(swing_time>swing_period*(2f/3f)&&reverse) {
        //         Debug.Log("asdasds");
        //         torque*=-100f;
        //         reverse = false;
        //     }
        //     Debug.Log("wtf: "+torque.ToString());
        //     body.AddTorque(torque * swing_dir);
        //     yield return new WaitForSeconds(0.001f);
        //     swing_time+=0.001f;
        // }
        // torque*=-1;
        // //at the end of swing, check if we need to begin the next swing or retract
        // body.angularVelocity = 0f;
        // //Debug.Log("swing ended");
        // yield return new WaitForSeconds(0.2f);
        if (!init_attack)
        {
            yield return retract();
        }
        else
        {
            //change direction of swing
            swing_dir *= -1;
            yield return swing();
        }
    }

    void Update()
    {
        swing_angle = const_swing_angle - gameObject.transform.rotation.z;
        new_input = *p_newinput;
        //to trigger the first init_attack
        //if (transform.localScale.x == 0f||get_new_input()) init_attack = new_input;
        if (new_input&&!attacking) {
            sword_animator.SetTrigger("new_input");
            //StartCoroutine(attack_animate());
        }
    }

    IEnumerator attack_animate(){
        attacking = true;
        while(transform.localScale.x<1f){
            transform.localScale+=new Vector3(Time.deltaTime/swing_period, Time.deltaTime/swing_period, Time.deltaTime/swing_period);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(swing_period);
        while(sword_sprite.color.a>0f){
            sword_sprite.color = new Color(0f, 0f, 0f, sword_sprite.color.a-Time.deltaTime/(swing_period));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.localScale = Vector3.zero;
        sword_sprite.color = Color.black;
        yield return new WaitForSeconds(attack_cool_off);
        attacking = false;
    }

    void FixedUpdate(){
        //gameObject.transform.position = Vector2.zero;
    }
    void OnCollisionEnter2D(Collision2D c)
    {
        Physics2D.IgnoreCollision(c.collider, c.otherCollider, true);
        Physics2D.IgnoreCollision(c.otherCollider, c.collider, true);
        if (c.gameObject.tag == "blocked")
        {
            body.angularVelocity = 0f;
            StartCoroutine(retract());
        }
        else
        {
            body.angularVelocity = angul;
            //Debug.Log(body.angularVelocity);
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        Physics2D.IgnoreCollision(c.collider, c.otherCollider, false);
        Physics2D.IgnoreCollision(c.otherCollider, c.collider, false);
    }

    //check if we can execute the next attack move based on the position of the weapon
    bool get_new_input()
    {
        //no more moves after the third
        if (swings == 3) return false;
        //don't init new attack when already initing new attack
        if (init_attack) return false;
        //check direction of swing to see if we're in the last half of the swing 
        if (Math.Sign(body.angularVelocity) == 1)
        {
            if (body.rotation > 90f)
            {
                return true;
            }
        }
        else
        {
            if (body.rotation < 90f)
            {
                return true;
            }
        }
        if (body.angularVelocity == 0f) return true;
        return false;
    }
}
