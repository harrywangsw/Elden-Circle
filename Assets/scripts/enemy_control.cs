using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[RequireComponent(typeof(BoxCollider2D))]
public unsafe class enemy_control : MonoBehaviour
{
    public float current_health, previous_health;
    public bool attack_order, attacking, movable;
    public bool* pattacking;
    public stats enemy_stat;
    damage_manager damages;
    GameObject healthbar, greybar, player, rweapon;
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
        greybar = GameObject.Instantiate(healthbar, healthbar.transform);
        greybar.GetComponent<SpriteRenderer>().color = Color.grey;
        greybar.transform.localPosition = Vector3.zero;
        greybar.transform.SetParent(transform);
        greybar.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    void Update()
    {
        health_bar();
    }

    void health_bar()
    {
        if (current_health != previous_health)
        {
            StartCoroutine(health_bar_anim());
            previous_health = current_health;
        }
    }

    IEnumerator health_bar_anim()
    {
        int inc_dec = Math.Sign(previous_health - current_health);
        if (inc_dec == 1)
        {
            healthbar.transform.localScale = new Vector3(current_health / enemy_stat.health, 1f, 1f);
            while (greybar.transform.localScale.x > current_health / enemy_stat.health)
            {
                greybar.transform.localScale -= new Vector3(0.001f, 0f, 0f);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
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
            if (current_health < 0f) Destroy(gameObject);
        }
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
}
