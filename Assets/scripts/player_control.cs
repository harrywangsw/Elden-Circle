using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class player_control : MonoBehaviour
{
    public float speed, walkAcceleration;
    public bool attack_order, attacking, movable;
    public bool* pattacking;
    Vector2 velocity = new Vector2();
    public GameObject rweapon;
    string type;
    void Start()
    {
        update_weapon();
    }
    public void update_weapon()
    {
        string type = rweapon.tag;
        if (type == "straight_sword")
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<straight_sword>().attacking) { pattacking = pattack_fixed; }
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
