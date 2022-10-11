using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class player_control : MonoBehaviour
{
    public float speed, walkAcceleration;
    public bool new_input, attacking, movable;
    public bool* pattacking;
    Vector2 velocity = new Vector2();
    public GameObject rweapon;
    string type;
    void Start()
    {
        update_weapon();
    }
    void update_weapon()
    {
        string type = rweapon.tag;
        //get the pointers of variables in weapon that must be controled by the player at the start, so we don't have to do these if statements every frame
        if (type == "straight_sword")
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<straight_sword>().attacking) { pattacking = pattack_fixed; }
            fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<straight_sword>().p_newinput = p_attack_order; }
        }
        else if (type == "spear")
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<spear_attack>().attacking) { pattacking = pattack_fixed; }
        }
    }
    void Update()
    {
        attacking = *pattacking;
        if (Input.GetMouseButtonDown(0)) new_input = true;
        else new_input = false;
    }

    void FixedUpdate()
    {
        if(!attacking) move();
    }

    void move()
    {
        float moveInputx = Input.GetAxisRaw("Horizontal");
        float moveInputy = Input.GetAxisRaw("Vertical");
        velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInputx, walkAcceleration * Time.fixedDeltaTime);
        velocity.y = Mathf.MoveTowards(velocity.y, speed * moveInputy, walkAcceleration * Time.fixedDeltaTime);
        transform.Translate(velocity * Time.deltaTime);
        Camera.main.gameObject.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y, -10f);
    }
}
