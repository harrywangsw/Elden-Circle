using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public unsafe class player_control : MonoBehaviour
{
    public float cursor_speed, dash_force, stun_lock_period, dash_stamina, stamina_cost_l, stamina_cost, stamina_increment, stamina, walkAcceleration, item_speed, range, l_range, death_period, health, lock_dura, lock_angle;
    public float speed;
    public bool use_controller, attack_interrupted, stop, restore_stamina = true, locked_on, new_input, new_input_l, attacking, movable, dashing, start_new_dash = true, using_item, ramming = true;
    public stats player_stat, unbuffed_player_stat;
    public world_details current_world;
    public bool* pattacking, pattacking_l;
    damage_manager damages;
    public Vector2 velocity = new Vector2();
    public Vector3 previous_angle = new Vector3();
    Vector3 init_loc = new Vector3();
    public GameObject npc_marker, locked_npc, n_marker, locked_enemy, marker, rweapon, overlay, death_screen, menu, inventory_content, lweapon, lock_on_marker;
    TMPro.TextMeshProUGUI Exp;
    public SpriteRenderer player_sprite;
    List<enemy_control> enemies = new List<enemy_control>();
    string type;
    Rigidbody2D body;
    public List<GameObject> near_by_npcs = new List<GameObject>();
    List<GameObject> u_gameobjects = new List<GameObject>(), l_gameobjects = new List<GameObject>(), r_gameobjects = new List<GameObject>();
    float orig_cam_size;

    void Start()
    {
        orig_cam_size = Camera.main.orthographicSize; 
        update_weapon(rweapon, lweapon);
        if(rweapon!=null){
            Destroy(gameObject.GetComponent<damage_manager>());
            ramming = false;
        }
        player_sprite = gameObject.GetComponent<SpriteRenderer>();
        body = gameObject.GetComponent<Rigidbody2D>();
        n_marker = Resources.Load<GameObject>("prefab/npc_lock_marker");
        //Physics2D.IgnoreCollision(GameObject.Find("ground").GetComponent<TilemapCollider2D>(), GetComponent<CircleCollider2D>(), true);
        //GameObject.Find("ground").GetComponent<TilemapCollider2D>().enabled = false;
        init();
        DontDestroyOnLoad(gameObject);
    }

    public void init(){
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("enemy")){
            enemies.Add(enemy.GetComponent<enemy_control>());
        }
        inventory_content = GameObject.Find("inventory_content");
        menu = GameObject.Find("item_menu");
        Exp = GameObject.Find("Exp").GetComponent<TMPro.TextMeshProUGUI>();
        overlay = GameObject.Find("overlay");
        death_screen = GameObject.Find("death_screen");

        health = player_stat.health;
        speed = player_stat.spd;
        stamina = player_stat.stamina;

        statics.apply_world_details();
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

            else if (rweapon.GetComponent<machine_gun>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<machine_gun>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<machine_gun>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<machine_gun>().range;
                init_loc = rweapon.GetComponent<machine_gun>().init_loc;
                stamina_cost = rweapon.GetComponent<machine_gun>().stamina_cost;
            }

            else if (rweapon.GetComponent<spiked_wall>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &rweapon.GetComponent<spiked_wall>().attacking) { pattacking = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<spiked_wall>().p_newinput = p_attack_order; }
                range = rweapon.GetComponent<spiked_wall>().range;
                init_loc = rweapon.GetComponent<spiked_wall>().init_loc;
                stamina_cost = rweapon.GetComponent<spiked_wall>().stamina_cost;
            }
            rweapon.transform.localPosition = init_loc;
            statics.apply_stats(rweapon.GetComponent<damage_manager>(), rweapon.GetComponent<damage_manager>(), player_stat);
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
                l_range = lweapon.GetComponent<spear_attack>().range;
                init_loc = lweapon.GetComponent<spear_attack>().init_loc;
                stamina_cost_l = lweapon.GetComponent<spear_attack>().stamina_cost;
            }
            else if (lweapon.GetComponent<fire_crackers>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<fire_crackers>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<fire_crackers>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<fire_crackers>().range;
                init_loc = lweapon.GetComponent<fire_crackers>().init_loc;
                stamina_cost_l = lweapon.GetComponent<fire_crackers>().stamina_cost;
            }
            else if (lweapon.GetComponent<dagger_fan>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<dagger_fan>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<dagger_fan>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<dagger_fan>().range;
                init_loc = lweapon.GetComponent<dagger_fan>().init_loc;
                stamina_cost_l = lweapon.GetComponent<dagger_fan>().stamina_cost;
            }
            else if (lweapon.GetComponent<parry_shield>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<parry_shield>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<parry_shield>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<parry_shield>().range;
                init_loc = lweapon.GetComponent<parry_shield>().init_loc;
                stamina_cost_l = lweapon.GetComponent<parry_shield>().stamina_cost;
            }
            else if (lweapon.GetComponent<lightning_strike>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<lightning_strike>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<lightning_strike>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<lightning_strike>().range;
                init_loc = lweapon.GetComponent<lightning_strike>().init_loc;
                stamina_cost_l = lweapon.GetComponent<lightning_strike>().stamina_cost;
            }

            else if (lweapon.GetComponent<glint_stone>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<glint_stone>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<glint_stone>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<glint_stone>().range;
                init_loc = lweapon.GetComponent<glint_stone>().init_loc;
                stamina_cost_l = lweapon.GetComponent<glint_stone>().stamina_cost;
            }

            else if (lweapon.GetComponent<mine>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<mine>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<mine>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<mine>().range;
                init_loc = lweapon.GetComponent<mine>().init_loc;
                stamina_cost_l = lweapon.GetComponent<mine>().stamina_cost;
            }

            else if (lweapon.GetComponent<machine_gun>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<machine_gun>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<machine_gun>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<machine_gun>().range;
                init_loc = lweapon.GetComponent<machine_gun>().init_loc;
                stamina_cost_l = lweapon.GetComponent<machine_gun>().stamina_cost;
            }

            else if (lweapon.GetComponent<spiked_wall>()!=null)
            {
                //get adress of attacking from right-hand weapon and save the adress in pattacking
                fixed (bool* pattack_fixed = &lweapon.GetComponent<spiked_wall>().attacking) { pattacking_l = pattack_fixed; }
                fixed(bool* p_attack_order = &new_input_l) { lweapon.GetComponent<spiked_wall>().p_newinput = p_attack_order; }
                l_range = lweapon.GetComponent<spiked_wall>().range;
                init_loc = lweapon.GetComponent<spiked_wall>().init_loc;
                stamina_cost_l = lweapon.GetComponent<spiked_wall>().stamina_cost;
            }
            lweapon.transform.localPosition = init_loc;
            statics.apply_stats(lweapon.GetComponent<damage_manager>(), lweapon.GetComponent<damage_manager>(), player_stat);
        }
    }
    

    void Update()
    {
        //current_world = new world_details();
        // Debug.Log(player_stat.inv.inv[0].num_left.ToString()+" "+player_stat.inv.inv[1].num_left.ToString());
        if(stop){
            body.velocity = Vector2.zero;
            return;
        }
        if(Input.GetKeyDown("escape")||Input.GetButtonDown("xboxStart")){
            //Debug.Log("wtf");
            if(menu.GetComponent<RectTransform>().localScale==Vector3.one) menu.GetComponent<RectTransform>().localScale= Vector3.zero;
            else menu.GetComponent<RectTransform>().localScale=Vector3.one;
        }
        if(menu.GetComponent<RectTransform>().localScale == Vector3.one){
            move_cursor_with_controller();
            return;
        }
        if(!ramming) {
            if(pattacking!=null) attacking = *pattacking;
            if(pattacking_l!=null&&!attacking) attacking = *pattacking_l;
        }
        else attacking = dashing;
        if(attacking||using_item) {
            body.velocity = Vector2.zero;
        }
        else speed = player_stat.spd;

        if(Input.GetButtonDown("lock")) lock_on();
        if(locked_on) switch_target();
        Exp.text = unbuffed_player_stat.exp.ToString();

        if(stamina<0f){stamina=0f;}
        if(stamina<player_stat.stamina&&restore_stamina) stamina+=stamina_increment;
        if (Input.GetButtonDown("attack_r")&&stamina>=stamina_cost) {
            stamina-=stamina_cost; 
            new_input = true;
            StartCoroutine(stamina_delay());
        }
        else new_input = false;
        if (Input.GetButtonDown("attack_l")&&stamina>=stamina_cost_l) {
            stamina-=stamina_cost_l; 
            new_input_l = true;
            StartCoroutine(stamina_delay());
        }
        else new_input_l = false;
        if (Input.GetButtonDown("dash")&&start_new_dash&&stamina>dash_stamina) {
            StartCoroutine(stamina_delay());
            StartCoroutine(dash());
            stamina-=dash_stamina;
            start_new_dash = false;
        }
        look_at_npc();
    }

    void move_cursor_with_controller(){
        // Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Debug.Log(worldMousePos);
        if(Input.GetAxis("xboxHorizontal")!=0||Input.GetAxis("xboxVertical")!=0) Mouse.current.WarpCursorPosition((Vector2)Input.mousePosition+new Vector2(Input.GetAxis("xboxHorizontal"), -Input.GetAxis("xboxVertical"))*cursor_speed*Time.deltaTime);
    }
    
    IEnumerator stamina_delay(){
        restore_stamina = false;
        yield return new WaitForSeconds(0.8f);
        restore_stamina = true;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        damage_manager d = c.collider.gameObject.GetComponent<damage_manager>();
        if(!d) return;   
        attack_interrupted = true;
        if(player_sprite.color==Color.grey) return;
        health -= statics.calc_damage(player_stat, d);
        StartCoroutine(statics.animate_hurt(player_sprite));
        StartCoroutine(hurt());
        if (health < 0f) StartCoroutine(death());
    }

    IEnumerator hurt(){
        stop = true;
        yield return new WaitForSeconds(stun_lock_period);
        stop = false;
        attack_interrupted = false;
    }

    public IEnumerator death(){
        GetComponent<Collider2D>().enabled = false;
        unbuffed_player_stat.exp_lost = unbuffed_player_stat.exp;
        unbuffed_player_stat.exp = 0;
        overlay.SetActive(false);
        menu.SetActive(false);
        death_screen.transform.localScale = Vector3.one;
        Image death_background = death_screen.transform.GetChild(0).gameObject.GetComponent<Image>();
        while(death_background.color.a<1f){
            death_background.color += new Color(0f, 0f, 0f, Time.deltaTime/death_period);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GameObject.Instantiate(Resources.Load<GameObject>("prefab/lost_exp"), transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        gameObject.name = "old_player";
        //save so that the player can't revert their death
        save_load.SavePlayer(unbuffed_player_stat);
        save_load.Saveworld(current_world, unbuffed_player_stat.name);
        StartCoroutine(statics.load_new_world(SceneManager.GetActiveScene().name, current_world, unbuffed_player_stat, gameObject));
    }


    void FixedUpdate()
    {
        if(!dashing&&!stop&&!attacking) move();
        Camera.main.gameObject.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    void move()
    {
        walkAcceleration = speed*10f;
        transform.rotation = Quaternion.identity;
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //if(use_controller) moveInput = new Vector2(Input.GetAxis("xboxHorizontal"), Input.GetAxis("xboxVertical"));
        //moveInput = statics.rotate(moveInput, transform.eulerAngles.z);
        velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput.x, walkAcceleration * Time.fixedDeltaTime);
        velocity.y = Mathf.MoveTowards(velocity.y, speed * moveInput.y, walkAcceleration * Time.fixedDeltaTime);
        //velocity = Quaternion.AngleAxis(-transform.eulerAngles.z, Vector3.forward) * velocity;
        body.velocity = velocity;
        //transform.Translate(velocity * Time.fixedDeltaTime);

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction;
        
        if(locked_enemy!=null){
            direction = locked_enemy.transform.position-transform.position;
            previous_angle = new Vector3(0f,0f,Vector2.SignedAngle(Vector2.up, direction));
        }
        else{
            direction = (Vector2)((worldMousePos - transform.position));
            if(Input.GetAxis("xboxHorizontal")!=0||Input.GetAxis("xboxVertical")!=0) direction = new Vector2(Input.GetAxis("xboxHorizontal"), -Input.GetAxis("xboxVertical"));
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
        else if(item_name=="shrink"){
            StartCoroutine(shrink_expand(0.4f));
        }
        else if(item_name=="expand"){
            StartCoroutine(shrink_expand(1f/0.4f));
        }
        else if(item_name=="mine"){
            StartCoroutine(statics.spawn_mine(transform.position, player_stat));
        }
    }

    IEnumerator dash()
    {
        dashing = true;
        player_sprite.color = Color.grey;
        float time = 0f;
        while(time<player_stat.dash_dura){
            body.AddForce(body.velocity*dash_force);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            time+=Time.fixedDeltaTime;
        }
        player_sprite.color = Color.black;
        time = 0f;
        while(time<player_stat.dash_dura){
            body.AddForce(-body.velocity*dash_force);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            time+=Time.fixedDeltaTime;
        }
        dashing = false;
        yield return new WaitForSeconds(player_stat.dash_dura*3f);
        start_new_dash = true;
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
        if(health>player_stat.health) health=player_stat.health;
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

    //the multiplayer always shrink objects with respect to Vector3.one
    IEnumerator shrink_expand(float multiplyer){
        using_item = true;
        float time = 0f;
        StartCoroutine(statics.expand(transform, 1f/player_stat.item_speed, Vector3.one*multiplyer));
        float cam_change_rate = (Camera.main.orthographicSize-orig_cam_size*multiplyer)*player_stat.item_speed;
        while(time<1f/player_stat.item_speed){
            Camera.main.orthographicSize-=cam_change_rate*Time.deltaTime;
            time+=Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Camera.main.orthographicSize = multiplyer*orig_cam_size;
        using_item = false;
        yield return new WaitForSeconds(statics.shrink_period);

        StartCoroutine(statics.expand(transform, 1f/player_stat.item_speed, Vector3.one));
        cam_change_rate = (orig_cam_size*multiplyer-Camera.main.orthographicSize)*player_stat.item_speed;
        while(time<1f/player_stat.item_speed){
            Camera.main.orthographicSize-=cam_change_rate*Time.deltaTime;
            time+=Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Camera.main.orthographicSize = orig_cam_size;
    }

    public void lock_on(){
        if(locked_on){
            Destroy(marker);
            locked_on = false;
            locked_enemy = null;
            return;
        }
        
        foreach(enemy_control enemy_c in enemies){
            if(enemy_c.sight_lost&&enemy_c.gameObject==locked_enemy){
                locked_enemy = null;
                Destroy(marker);
                continue;
            }
            if(enemy_c.sight_lost) continue;
            // Debug.Log(Vector2.Angle(transform.rotation*Vector2.up, (Vector2)(enemy_c.gameObject.transform.position-transform.position)).ToString());
            if(Vector2.Angle(transform.rotation*Vector2.up, (Vector2)(enemy_c.gameObject.transform.position-transform.position))<=lock_angle){
                locked_enemy = enemy_c.gameObject;
                Destroy(marker);
                marker = GameObject.Instantiate(lock_on_marker, enemy_c.gameObject.transform, false);
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
        if(locked_enemy==null) {
            locked_on = false;
            return;
        }
        foreach(enemy_control enemy_c in enemies){
            if(enemy_c.gameObject==null) {
                enemies.Remove(enemy_c);
                continue;
            }
            if(enemy_c.gameObject==locked_enemy&&enemy_c.sight_lost){
                locked_enemy = null;
                Destroy(marker);
                continue;
            }
            if(enemy_c.sight_lost) continue;
            if(enemy_c.gameObject == locked_enemy) continue;
            //use dot product=cos(a) to determine which enemy has the least angulare seperation from the direction
            //the player's facing
            float dot1 = Vector3.Dot(enemy_c.gameObject.transform.position, transform.rotation*Vector3.up);
            float dot2 = Vector3.Dot(transform.rotation*Vector3.up, locked_enemy.transform.position);
            if(dot1>dot2){
                locked_enemy = enemy_c.gameObject;
                Destroy(marker);
                marker = GameObject.Instantiate(lock_on_marker, enemy_c.gameObject.transform, false);
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
