using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Tilemaps;

public unsafe class player_control : MonoBehaviour
{
    public float walkAcceleration, item_speed, range, death_period, health;
    float speed;
    public string player_name;
    public bool new_input, new_input_l, attacking, movable, dashing, dash_command, using_item, ramming = true;
    public stats player_stat;
    public world_details current_world;
    public bool* pattacking, pattacking_l;
    damage_manager damages;
    public Vector2 velocity = new Vector2();
    public Vector3 previous_pos = new Vector3(), previous_angle = new Vector3();
    Vector3 init_loc = new Vector3();
    public GameObject rweapon, overlay, death_screen, menu, inventory_content, lweapon, Exp;
    public SpriteRenderer player_sprite;
    string type;
    Rigidbody2D body;
    List<GameObject> u_gameobjects = new List<GameObject>(), l_gameobjects = new List<GameObject>(), r_gameobjects = new List<GameObject>();

    void Start()
    {
        inventory_content = GameObject.Find("inventory_content");
        if(rweapon!=null){
            update_weapon(rweapon, null);
            gameObject.GetComponent<damage_manager>().enabled = false;
            ramming = false;
        }
        player_sprite = gameObject.GetComponent<SpriteRenderer>();
        body = gameObject.GetComponent<Rigidbody2D>();
        //Physics2D.IgnoreCollision(GameObject.Find("ground").GetComponent<TilemapCollider2D>(), GetComponent<CircleCollider2D>(), true);
        //GameObject.Find("ground").GetComponent<TilemapCollider2D>().enabled = false;
    }

    public void update_stats(){
        health = player_stat.health;
        speed = player_stat.spd;
    }

    public void update_weapon(GameObject new_rweapon, GameObject new_lweapon)
    {
        //if a weapon is currently equippted and a new waepon is being added, destroy the old one
        if(rweapon!=null&&new_rweapon!=null){
            if(rweapon.transform.parent==transform){
                Destroy(rweapon);
            }
        }

        if(lweapon!=null&&new_lweapon!=null){
            if(lweapon.transform.parent==transform){
                Destroy(lweapon);
            }
        }

        if(new_rweapon!=null){
            rweapon = GameObject.Instantiate(new_rweapon, transform, false);
            ramming = false;
            //get the pointers of variables in weapon that must be controled by the player at the start, so we don't have to do these if statements every frame
            if (rweapon.GetComponent<spear_attack>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<spear_attack>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<spear_attack>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<spear_attack>().range;
                init_loc = rweapon.GetComponent<spear_attack>().init_loc;
            }
            else if (rweapon.GetComponent<fire_crackers>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<fire_crackers>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<fire_crackers>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<fire_crackers>().range;
                init_loc = rweapon.GetComponent<fire_crackers>().init_loc;
            }
            else if (rweapon.GetComponent<dagger_fan>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<dagger_fan>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<dagger_fan>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<dagger_fan>().range;
                init_loc = rweapon.GetComponent<dagger_fan>().init_loc;
            }
            else if (rweapon.GetComponent<parry_shield>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<parry_shield>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<parry_shield>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<parry_shield>().range;
                init_loc = rweapon.GetComponent<parry_shield>().init_loc;
            }
        }

        if(new_lweapon!=null){
            lweapon = GameObject.Instantiate(new_lweapon, transform, false);
            ramming = false;
            //get the pointers of variables in weapon that must be controled by the player at the start, so we don't have to do these if statements every frame
            if (lweapon.GetComponent<spear_attack>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<spear_attack>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<spear_attack>().p_newinput = p_attack_order; }
                range = lweapon.GetComponent<spear_attack>().range;
                init_loc = lweapon.GetComponent<spear_attack>().init_loc;
            }
            else if (lweapon.GetComponent<fire_crackers>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<fire_crackers>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<fire_crackers>().p_newinput = p_attack_order; }
                range = lweapon.GetComponent<fire_crackers>().range;
                init_loc = lweapon.GetComponent<fire_crackers>().init_loc;
            }
            else if (lweapon.GetComponent<dagger_fan>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<dagger_fan>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<dagger_fan>().p_newinput = p_attack_order; }
                range = lweapon.GetComponent<dagger_fan>().range;
                init_loc = lweapon.GetComponent<dagger_fan>().init_loc;
            }
            else if (lweapon.GetComponent<parry_shield>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<parry_shield>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<parry_shield>().p_newinput = p_attack_order; }
                range = lweapon.GetComponent<parry_shield>().range;
                init_loc = lweapon.GetComponent<parry_shield>().init_loc;
            }
        }
    }
    

    void Update()
    {
        if(!ramming) {
            if(pattacking!=null) attacking = *pattacking;
            if(pattacking_l!=null&&!attacking) attacking = *pattacking_l;
        }
        else attacking = dashing;
        if(attacking||using_item) speed = player_stat.spd/8f;
        else speed = player_stat.spd;

        if(Input.GetKeyDown("escape")){
            //Debug.Log("wtf");
            if(menu.GetComponent<RectTransform>().localScale==Vector3.one) menu.GetComponent<RectTransform>().localScale= Vector3.zero;
            else menu.GetComponent<RectTransform>().localScale=Vector3.one;
        }
        if(menu.GetComponent<RectTransform>().localScale == Vector3.one){
            return;
        }
        if (Input.GetMouseButtonDown(1)) {new_input = true;}
        else new_input = false;
        if (Input.GetMouseButtonDown(0)) new_input_l = true;
        else new_input_l = false;
        if (Input.GetKeyDown("space")&&!dashing) dash_command = true;
        Exp.GetComponent<TMPro.TextMeshProUGUI>().text = player_stat.exp.ToString();
    }


    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.gameObject.GetComponent<damage_manager>()!=null)
        {      
            if(dashing) return;
            health -= calc_damage(c.collider.gameObject.GetComponent<damage_manager>());
            StartCoroutine(statics.animate_hurt(player_sprite));
            if (health < 0f) StartCoroutine(death());
        }
    }

    IEnumerator death(){
        overlay.SetActive(false);
        menu.SetActive(false);
        death_screen.SetActive(true);
        while(player_sprite.color.a>0f){
            player_sprite.color = new Color(0f, 0f, 0f, player_sprite.color.a-Time.deltaTime/death_period);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        player_sprite.sprite = null;
    }


    float calc_damage(damage_manager damages) {
        return damages.slash/player_stat.slash_def + damages.strike/player_stat.strike_def +damages.pierce/player_stat.pierce_def +damages.magic/player_stat.mag_def;
    }

    void FixedUpdate()
    {
        previous_pos = transform.position;
        move();
    }

    void move()
    {
        transform.rotation = Quaternion.identity;
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //moveInput = statics.rotate(moveInput, transform.eulerAngles.z);
        velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput.x, walkAcceleration * Time.fixedDeltaTime);
        velocity.y = Mathf.MoveTowards(velocity.y, speed * moveInput.y, walkAcceleration * Time.fixedDeltaTime);
        //velocity = Quaternion.AngleAxis(-transform.eulerAngles.z, Vector3.forward) * velocity;
        if(dash_command&&!dashing){
            dash_command = false;
            StartCoroutine(dash());
        }
        body.velocity = velocity;
        //transform.Translate(velocity * Time.fixedDeltaTime);
        Camera.main.gameObject.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y, -10f);

        if(attacking) {
            transform.eulerAngles = previous_angle;
            return;
        }
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)((worldMousePos - transform.position));
        direction.Normalize();
        transform.eulerAngles = new Vector3(0f,0f,Vector2.SignedAngle(Vector2.up, direction));
        previous_angle = transform.eulerAngles;
    }

    public void use_item(string item_name)
    {
        if(item_name=="health_potion"){
            StartCoroutine(health_potion());
        }
    }

    IEnumerator dash()
    {
        dashing = true;
        //Debug.Log("dash");
        speed*=888f;
        player_sprite.color = new Color(0f, 0f, 0f, 0.5f);
        yield return new WaitForSeconds(player_stat.dash_dura);
        speed/=888f;
        player_sprite.color = Color.black;
        yield return new WaitForSeconds(player_stat.dash_dura*3f);
        dashing = false;
    }

    IEnumerator health_potion(){
        //Debug.Log("h");
        using_item = true;
        float time = 0f;
        while(time<0.5f/player_stat.item_speed){
            player_sprite.color = Color.green;
            yield return new WaitForSeconds(Time.deltaTime*8f);
            time+=Time.deltaTime*8f;
            player_sprite.color = Color.black;
            yield return new WaitForSeconds(Time.deltaTime*8f);
            time+=Time.deltaTime*8f;
        }
        player_stat.health+=player_stat.health_up_amount;
        while(time<1f/player_stat.item_speed){
            player_sprite.color = Color.green;
            yield return new WaitForSeconds(Time.deltaTime*8f);
            time+=Time.deltaTime*8f;
            player_sprite.color = Color.black;
            yield return new WaitForSeconds(Time.deltaTime*8f);
            time+=Time.deltaTime*8f;
        }
        using_item = false;
    }
}
