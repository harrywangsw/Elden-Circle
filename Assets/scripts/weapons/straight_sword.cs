using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(damage_manager))]
public class straight_sword : MonoBehaviour
{
    public float extend_time, swing_angle;
    public float torque;
    public float angul;
    int swing_dir = 1;
    public int swings = 0;
    public bool attack_order, new_input, attacking, left;
    Rigidbody2D body;
    damage_manager manager;
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        body.rotation = swing_angle;
        manager = gameObject.GetComponent<damage_manager>();
    }

    IEnumerator extend()
    {
        attacking = true;
        if (manager.slash > manager.pierce) (manager.slash, manager.pierce) = (manager.pierce, manager.slash);
        manager.pierce *= 1.2f;
        //extend
        while (transform.localScale.x < 1f)
        {
            transform.localScale += new Vector3(Time.deltaTime / extend_time, 0f, 0f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.localScale = Vector3.one;
        yield return new WaitForSeconds(0.2f);
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
        while (transform.localScale.x > 0f)
        {
            //transition to extend if attack ordered, except for when thrusting
            if (attack_order&&swings!=3) 
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
                attack_order = false;
                new_input = false;
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator swing()
    {
        if(manager.slash<manager.pierce) (manager.slash, manager.pierce) = (manager.pierce, manager.slash);
        attacking = true;
        attack_order = false;
        new_input = false;
        swings++;
        if (swings == 3)
        {
            yield return new WaitForSeconds(0.1f);
            transform.localScale = new Vector3(0f, 1f, 1f);
            body.rotation = 90f;
            //Debug.Log("ended with thrust");
            yield return extend();
            yield break;
        }
        if (swing_dir == 1)
        {
            //accelerating
            while (body.rotation <= 90f && swing_angle <= body.rotation)
            {
                body.AddTorque(torque * swing_dir);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            //deccelerating +2 to make sure decce stops
            while (body.rotation <= 180f - swing_angle-18f && 90f <= body.rotation)
            {
                body.AddTorque(-1f * torque * swing_dir);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            //Debug.Log(body.rotation.ToString() + ", " + body.angularVelocity.ToString());
            body.rotation = 180f - swing_angle;
            body.angularVelocity = 0f;
        }
        else
        {
            //accelerating
            while (body.rotation >= 90f && 180f-swing_angle >= body.rotation)
            {
                body.AddTorque(torque * swing_dir);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            //deccelerating, +2 to make sure decce stops
            while (body.rotation >= 18f+swing_angle && 90f >= body.rotation)
            {
                body.AddTorque(-1f * torque * swing_dir);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            body.rotation = swing_angle;
            body.angularVelocity = 0f;
        }
        //at the end of swing, check if we need to begin the next swing or retract
        body.angularVelocity = 0f;
        //Debug.Log("swing ended");
        yield return new WaitForSeconds(0.1f);
        attack_order = new_input;
        if (!attack_order)
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
        //new input will be equated as a new attack order when swing has ended
        if (Input.GetMouseButtonDown(0)) new_input = true;
        //to trigger the first attack_order
        if (transform.localScale.x == 0f) attack_order = new_input;
        if (attack_order&&!attacking) StartCoroutine(extend());
    }
    void FixedUpdate()
    {
        angul = body.angularVelocity;
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
            Debug.Log(body.angularVelocity);
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
