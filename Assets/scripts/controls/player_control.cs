﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Tilemaps;

public unsafe class player_control : MonoBehaviour
{
    public float stun_lock_period, dash_stamina, stamina_cost, stamina_increment, stamina, walkAcceleration, item_speed, range, l_range, death_period, health, lock_dura, lock_angle;
    public float speed;
    public bool attack_interrupted, stop, restore_stamina = true, locked_on, new_input, new_input_l, attacking, movable, dashing, dash_command, using_item, ramming = true;
    public stats player_stat;
    public world_details current_world;
    public bool* pattacking, pattacking_l;
    damage_manager damages;
    public Vector2 velocity = new Vector2();
    public Vector3 previous_angle = new Vector3();
    Vector3 init_loc = new Vector3();
    public GameObject npc_marker, locked_npc, n_marker, locked_enemy, marker, rweapon, overlay, death_screen, menu, inventory_content, lweapon, Exp, lock_on_marker;
    public SpriteRenderer player_sprite;
    List<SpriteRenderer> enemies = new List<SpriteRenderer>();
    string type;
    Rigidbody2D body;
    public List<GameObject> near_by_npcs = new List<GameObject>();
    List<GameObject> u_gameobjects = new List<GameObject>(), l_gameobjects = new List<GameObject>(), r_gameobjects = new List<GameObject>();

    void Start()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("enemy")){
            enemies.Add(enemy.GetComponent<SpriteRenderer>());
        }
        if(menu==null){
            inventory_content = GameObject.Find("inventory_content");
            menu = GameObject.Find("item_menu");
            Exp = GameObject.Find("Exp");
        }
        update_weapon(rweapon, lweapon);
        if(rweapon!=null){
            gameObject.GetComponent<damage_manager>().enabled = false;
            ramming = false;
        }
        player_sprite = gameObject.GetComponent<SpriteRenderer>();
        body = gameObject.GetComponent<Rigidbody2D>();
        n_marker = Resources.Load<GameObject>("prefab/npc_lock_marker");
        //Physics2D.IgnoreCollision(GameObject.Find("ground").GetComponent<TilemapCollider2D>(), GetComponent<CircleCollider2D>(), true);
        //GameObject.Find("ground").GetComponent<TilemapCollider2D>().enabled = false;
        update_stats();
    }

    public void update_stats(){
        health = player_stat.health;
        speed = player_stat.spd;
        stamina = player_stat.stamina;
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
                stamina_cost = rweapon.GetComponent<spear_attack>().stamina_cost;
            }
            else if (rweapon.GetComponent<fire_crackers>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<fire_crackers>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<fire_crackers>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<fire_crackers>().range;
                init_loc = rweapon.GetComponent<fire_crackers>().init_loc;
                stamina_cost = rweapon.GetComponent<fire_crackers>().stamina_cost;
            }
            else if (rweapon.GetComponent<dagger_fan>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<dagger_fan>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<dagger_fan>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<dagger_fan>().range;
                init_loc = rweapon.GetComponent<dagger_fan>().init_loc;
                stamina_cost = rweapon.GetComponent<dagger_fan>().stamina_cost;
            }
            else if (rweapon.GetComponent<parry_shield>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<parry_shield>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<parry_shield>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<parry_shield>().range;
                init_loc = rweapon.GetComponent<parry_shield>().init_loc;
                stamina_cost = rweapon.GetComponent<parry_shield>().stamina_cost;
            }
            else if (rweapon.GetComponent<lightning_strike>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<lightning_strike>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<lightning_strike>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<lightning_strike>().range;
                init_loc = rweapon.GetComponent<lightning_strike>().init_loc;
                stamina_cost = rweapon.GetComponent<lightning_strike>().stamina_cost;
            }
            else if (rweapon.GetComponent<glint_stone>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<glint_stone>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<glint_stone>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<glint_stone>().range;
                init_loc = rweapon.GetComponent<glint_stone>().init_loc;
                stamina_cost = rweapon.GetComponent<glint_stone>().stamina_cost;
            }

            else if (rweapon.GetComponent<mine>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<mine>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<mine>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<mine>().range;
                init_loc = rweapon.GetComponent<mine>().init_loc;
                stamina_cost = rweapon.GetComponent<mine>().stamina_cost;
            }
        }
        rweapon.transform.localPosition = init_loc;

        if(new_lweapon!=null){
            lweapon = GameObject.Instantiate(new_lweapon, transform, false);
            ramming = false;
            //get the pointers of variables in weapon that must be controled by the player at the start, so we don't have to do these if statements every frame
            if (lweapon.GetComponent<spear_attack>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<spear_attack>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<spear_attack>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<spear_attack>().range;
                init_loc = lweapon.GetComponent<spear_attack>().init_loc;
            }
            else if (lweapon.GetComponent<fire_crackers>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<fire_crackers>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<fire_crackers>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<fire_crackers>().range;
                init_loc = lweapon.GetComponent<fire_crackers>().init_loc;
            }
            else if (lweapon.GetComponent<dagger_fan>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<dagger_fan>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<dagger_fan>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<dagger_fan>().range;
                init_loc = lweapon.GetComponent<dagger_fan>().init_loc;
            }
            else if (lweapon.GetComponent<parry_shield>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<parry_shield>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<parry_shield>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<parry_shield>().range;
                init_loc = lweapon.GetComponent<parry_shield>().init_loc;
            }
            else if (lweapon.GetComponent<lightning_strike>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<lightning_strike>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<lightning_strike>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<lightning_strike>().range;
                init_loc = lweapon.GetComponent<lightning_strike>().init_loc;
            }

            else if (lweapon.GetComponent<mine>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<mine>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<mine>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<mine>().range;
                init_loc = lweapon.GetComponent<mine>().init_loc;
            }
            lweapon.transform.localPosition = init_loc;
        }
    }
    

    void Update()
    {
        if(stop){
            speed = 0f;
            return;
        }
        if(!ramming) {
            if(pattacking!=null) attacking = *pattacking;
            if(pattacking_l!=null&&!attacking) attacking = *pattacking_l;
        }
        else attacking = dashing;
        if(attacking||using_item) speed = player_stat.spd/8f;
        else if(dashing) speed = player_stat.spd*2f;
        else speed = player_stat.spd;

        if(Input.GetKeyDown("escape")){
            //Debug.Log("wtf");
            if(menu.GetComponent<RectTransform>().localScale==Vector3.one) menu.GetComponent<RectTransform>().localScale= Vector3.zero;
            else menu.GetComponent<RectTransform>().localScale=Vector3.one;
        }
        if(menu.GetComponent<RectTransform>().localScale == Vector3.one){
            return;
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt)) lock_on();
        if(locked_on) switch_target();
        Exp.GetComponent<TMPro.TextMeshProUGUI>().text = player_stat.exp.ToString();

        if(stamina<0f){stamina=0f;}
        if(stamina<player_stat.stamina&&restore_stamina) stamina+=stamina_increment;
        if (Input.GetMouseButtonDown(1)&&stamina>=stamina_cost) {
            stamina-=stamina_cost; 
            new_input = true;
            StartCoroutine(stamina_delay());
        }
        else new_input = false;
        if (Input.GetMouseButtonDown(0)) new_input_l = true;
        else new_input_l = false;
        if (Input.GetKeyDown("space")&&!dashing&&stamina>dash_stamina) {
            StartCoroutine(stamina_delay());
            stamina-=dash_stamina;
            dash_command = true;
        }
        look_at_npc();
    }
    
    IEnumerator stamina_delay(){
        restore_stamina = false;
        yield return new WaitForSeconds(0.8f);
        restore_stamina = true;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.gameObject.GetComponent<damage_manager>()!=null)
        {      
            attack_interrupted = true;
            if(player_sprite.color==Color.grey) return;
            health -= statics.calc_damage(player_stat, c.collider.gameObject.GetComponent<damage_manager>());
            StartCoroutine(statics.animate_hurt(player_sprite));
            StartCoroutine(hurt());
            if (health < 0f) StartCoroutine(death());
        }
    }

    IEnumerator hurt(){
        stop = true;
        yield return new WaitForSeconds(stun_lock_period);
        stop = false;
        attack_interrupted = false;
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


    void FixedUpdate()
    {
        move();
    }

    void move()
    {
        walkAcceleration = speed*10f;
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

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction;
        
        if(locked_enemy!=null){
            direction = locked_enemy.transform.position-transform.position;
            previous_angle = new Vector3(0f,0f,Vector2.SignedAngle(Vector2.up, direction));
        }
        else{
            direction = (Vector2)((worldMousePos - transform.position));
        }
        if(attacking) {
            transform.eulerAngles = previous_angle;
            return;
        }
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
        player_sprite.color = Color.grey;
        yield return new WaitForSeconds(player_stat.dash_dura);
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
        health+=player_stat.health_up_amount;
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

    public void lock_on(){
        if(locked_on){
            Destroy(marker);
            locked_on = false;
            locked_enemy = null;
            return;
        }
        
        foreach(SpriteRenderer enemy_sprite in enemies){
            if(!enemy_sprite.isVisible) continue;
            // Debug.Log(Vector2.Angle(transform.rotation*Vector2.up, (Vector2)(enemy_sprite.gameObject.transform.position-transform.position)).ToString());
            if(Vector2.Angle(transform.rotation*Vector2.up, (Vector2)(enemy_sprite.gameObject.transform.position-transform.position))<=lock_angle){
                locked_enemy = enemy_sprite.gameObject;
                Destroy(marker);
                marker = GameObject.Instantiate(lock_on_marker, enemy_sprite.gameObject.transform, false);
                StartCoroutine(lock_anim(marker, locked_enemy));
                locked_on = true;
                return;
            }     
        }
    }

    void look_at_npc(){
        foreach(GameObject npc in near_by_npcs){
            if(npc==null) {
                near_by_npcs.Remove(npc);
                continue;
            }
            if(npc == locked_npc) continue;
            //use dot product=cos(a) to determine which enemy has the least angulare seperation from the direction
            //the player's facing
            float dot1 = Vector3.Dot(npc.transform.position, transform.rotation*Vector3.up);
            float dot2 = Vector3.Dot(transform.rotation*Vector3.up, locked_npc.transform.position);
            if(dot1>dot2){
                locked_npc = npc;
                Destroy(npc_marker);
                npc_marker = GameObject.Instantiate(n_marker, npc.transform, false);
                StartCoroutine(lock_anim(npc_marker, locked_npc));
            }
        }
    }

    void switch_target(){
        foreach(SpriteRenderer enemy_sprite in enemies){
            if(enemy_sprite==null) {
                enemies.Remove(enemy_sprite);
                continue;
            }
            if(!enemy_sprite.isVisible) continue;
            if(enemy_sprite.gameObject == locked_enemy) continue;
            //use dot product=cos(a) to determine which enemy has the least angulare seperation from the direction
            //the player's facing
            float dot1 = Vector3.Dot(enemy_sprite.gameObject.transform.position, transform.rotation*Vector3.up);
            float dot2 = Vector3.Dot(transform.rotation*Vector3.up, locked_enemy.transform.position);
            if(dot1>dot2){
                locked_enemy = enemy_sprite.gameObject;
                Destroy(marker);
                marker = GameObject.Instantiate(lock_on_marker, enemy_sprite.gameObject.transform, false);
                StartCoroutine(lock_anim(marker, locked_enemy));
            }
        }
    }

    IEnumerator lock_anim(GameObject m, GameObject locked){
        float size = locked.GetComponent<SpriteRenderer>().bounds.extents.magnitude;
        //Debug.Log(size.ToString());
        m.transform.localScale = 2f*size*Vector3.one;
        float time = 0f;
        while(time<lock_dura){
            //Quaternion.RotateTowards(m.transform.rotation, Quaternion.Euler(0f, 0f, 90f), Time.deltaTime*90f/lock_dura);
            if(m==null) break;
            m.transform.localScale-=Vector3.one*(Time.deltaTime*size/lock_dura);
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        m.transform.localScale = size*Vector3.one;
    }
}
