using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class player_control : MonoBehaviour
{
    public float speed, walkAcceleration;
    public bool new_input, attacking, movable;
    public float health;
    public stats player_stat;
    public bool* pattacking;
    damage_manager damages;
    Vector2 velocity = new Vector2();
    public GameObject rweapon;
    string type;
    public List<GameObject> hit_by;
    Rigidbody2D body;
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
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

    void OnCollisionEnter2D(Collision2D c)
    {
        if (hit_by.Contains(c.gameObject)) return;
        else hit_by.Add(c.gameObject);
        if (c.gameObject.tag == "harmable") return;
        else
        {
            body.velocity = Vector3.zero;
            damages = c.gameObject.GetComponent<damage_manager>();           
            health -= calc_damage();
            animate_hurt();
            if (health < 0f) Destroy(gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        if (hit_by.Contains(c.gameObject)) hit_by.Remove(c.gameObject);
    }


    float calc_damage()
    {
        return player_stat.slash_def* damages.slash + player_stat.strike_def * damages.strike + player_stat.pierce_def * damages.pierce;
    }

    IEnumerator animate_hurt()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
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
