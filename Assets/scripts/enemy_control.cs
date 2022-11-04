using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[RequireComponent(typeof(BoxCollider2D))]
public unsafe class enemy_control : MonoBehaviour
{
    public float current_health, previous_health;
    public bool new_input, attacking, movable;
    public bool* pattacking;
    public stats enemy_stat;
    damage_manager damages;
    GameObject healthbar, greybar, player;
    public GameObject rweapon;
    Rigidbody2D body;
    public List<GameObject> hit_by;
    void Start()
    {
        player=GameObject.Find("player");
        hit_by = new List<GameObject>();
        body = gameObject.GetComponent<Rigidbody2D>();
        current_health = enemy_stat.health;
        previous_health = current_health;
        for (int i = 0; i < transform.childCount; i++) 
        {
            if (transform.GetChild(i).gameObject.name == "health_bar") healthbar = transform.GetChild(i).gameObject;
        }
        update_weapon();
    }

    void Update()
    {
        if ((player.transform.position - transform.position).magnitude<=3f) new_input = true;
        else new_input = false;
        face_player();
    }

    void face_player()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, -Vector3.Angle(player.transform.position-transform.position, Vector3.up));
    }


    void OnCollisionEnter2D(Collision2D c)
    {
        if (hit_by.Contains(c.gameObject)) return;
        else hit_by.Add(c.gameObject);
        if (c.gameObject.tag == "harmable") return;
        else
        {
            body.velocity = Vector3.zero;
            damages = c.gameObject.GetComponent<damage_manager>();           
            current_health -= calc_damage();
            animate_hurt();
            if (current_health < 0f) Destroy(gameObject);
        }
    }

    IEnumerator animate_hurt()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
    }

    void OnCollisionExit2D(Collision2D c)
    {
        if (hit_by.Contains(c.gameObject)) hit_by.Remove(c.gameObject);
    }

    float calc_damage()
    {
        return enemy_stat.slash_def* damages.slash + enemy_stat.strike_def * damages.strike + enemy_stat.pierce_def * damages.pierce;
    }

    public void update_weapon()
    {
        string type = rweapon.tag;
        //get the pointers of variables in weapon that must be controled by the player at the start, so we don't have to do these if statements every frame
        if (type == "straight_sword")
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<straight_sword>().attacking) { pattacking = pattack_fixed; }
            fixed (bool* p_attack_order = &new_input) { rweapon.GetComponent<straight_sword>().p_newinput = p_attack_order; }
        }
        else if (type == "spear")
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<spear_attack>().attacking) { pattacking = pattack_fixed; }
        }
    }
}
