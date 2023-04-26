using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Tilemaps;

public unsafe class player_control : MonoBehaviour
{
    public float const_speed, walkAcceleration, item_speed, health_up_amount, range, death_period, health;
    float speed;
    public string player_name;
    public int exp;
    public bool new_input, attacking, movable, dashing, dash_command, using_item;
    public stats player_stat;
    public world_details current_world;
    public bool* pattacking;
    damage_manager damages;
    public Vector2 velocity = new Vector2();
    public Vector3 previous_pos = new Vector3();
    Vector3 init_loc = new Vector3();
    public GameObject rweapon, overlay, death_screen, menu, inventory_content;
    public SpriteRenderer player_sprite;
    string type;
    Rigidbody2D body;
    List<GameObject> u_gameobjects = new List<GameObject>(), l_gameobjects = new List<GameObject>(), r_gameobjects = new List<GameObject>();

    void Start()
    {
        inventory_content = GameObject.Find("inventory_content");
        if(rweapon!=null){
            update_weapon();
            gameObject.GetComponent<damage_manager>().enabled = false;
        }
        player_name = player_stat.name;
        const_speed = player_stat.spd;
        player_sprite = gameObject.GetComponent<SpriteRenderer>();
        health = player_stat.health;
        body = gameObject.GetComponent<Rigidbody2D>();
        spawn_quickslot();
        //Physics2D.IgnoreCollision(GameObject.Find("ground").GetComponent<TilemapCollider2D>(), GetComponent<CircleCollider2D>(), true);
        //GameObject.Find("ground").GetComponent<TilemapCollider2D>().enabled = false;
    }

    void update_weapon()
    {
        rweapon = GameObject.Instantiate(rweapon, transform, false);
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
    
    
    public void spawn_quickslot(){
        //quickslot_up/down/left/right_indexes are the indexes of inv that represent the items in each respective quickslot
        //u/d/l/r_gameobjects are the gameobject for each item
        foreach(int x in player_stat.inv.quickslot_up_indexes){
            u_gameobjects.Add(Resources.Load<GameObject>("prefab/UI_items/"+player_stat.inv.inv[x].Item1));
        }
        foreach(int x in player_stat.inv.quickslot_left_indexes){
            l_gameobjects.Add(Resources.Load<GameObject>("prefab/UI_items/"+player_stat.inv.inv[x].Item1));
        }
        foreach(int x in player_stat.inv.quickslot_right_indexes){
            r_gameobjects.Add(Resources.Load<GameObject>("prefab/UI_items/"+player_stat.inv.inv[x].Item1));
        }
        
        GameObject item;
        //player_item.quickslot_up is the index of u_gameobject and quickslot_up_items
        if(u_gameobjects.Count>0){
            Transform slot = GameObject.Find("up").transform;
            item = GameObject.Instantiate(u_gameobjects[player_stat.inv.quickslot_up], slot);
            item.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_stat.inv.inv[player_stat.inv.quickslot_up_indexes[player_stat.inv.quickslot_up]].Item2.ToString();
        }
        if(l_gameobjects.Count>0){
            Transform slot = GameObject.Find("left").transform;
            item = GameObject.Instantiate(l_gameobjects[player_stat.inv.quickslot_left], slot);            
        }
        if(r_gameobjects.Count>0){
            Transform slot = GameObject.Find("right").transform;
            item = GameObject.Instantiate(r_gameobjects[player_stat.inv.quickslot_right], slot);
        }
    }

    public void Update_quickslot(){
        GameObject it = GameObject.Find("up").transform.GetChild(0).gameObject;
        it.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_stat.inv.inv[player_stat.inv.quickslot_up].Item2.ToString();

        it = GameObject.Find("down").transform.GetChild(0).gameObject;
        it.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_stat.inv.inv[player_stat.inv.quickslot_down].Item2.ToString();
    }

    void check_quickslot(){
        if(Input.GetKeyDown("e")){
            if(player_stat.inv.inv[player_stat.inv.quickslot_up].Item2<=0) return;
            player_stat.inv.inv[player_stat.inv.quickslot_up] = Tuple.Create(player_stat.inv.inv[player_stat.inv.quickslot_up].Item1, player_stat.inv.inv[player_stat.inv.quickslot_up].Item2-1, player_stat.inv.inv[player_stat.inv.quickslot_up].Item3);
            GameObject.Find("up").transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = player_stat.inv.inv[player_stat.inv.quickslot_up].Item2.ToString();
            use_item(player_stat.inv.inv[player_stat.inv.quickslot_up].Item1);
        }
        if(!attacking&&Input.GetKeyDown(KeyCode.LeftShift)){
            player_stat.inv.quickslot_right+=1;
            player_stat.inv.quickslot_right = player_stat.inv.quickslot_right%player_stat.inv.quickslot_right_indexes.Count;
            Destroy(GameObject.Find("right").transform.GetChild(0).gameObject);
            GameObject item;
            Transform slot = GameObject.Find("right").transform;
            item = GameObject.Instantiate(r_gameobjects[player_stat.inv.quickslot_right], slot);

            rweapon = Resources.Load<GameObject>("prefab/weapon/"+item.name);

        }
    }

    void Update()
    {
        // if(statics.out_of_bound(transform.position)){
        //     death();
        // }
        attacking = *pattacking;
        if(attacking||using_item) speed = const_speed/8f;
        else speed = const_speed;

        if(Input.GetKeyDown("escape")){
            //Debug.Log("wtf");
            menu.SetActive(!menu.activeSelf);
        }
        if (Input.GetMouseButtonDown(0)) {
            //Debug.Log("wtffffff");
            new_input = true;
        }
        else new_input = false;
        if (Input.GetKeyDown("space")&&!dashing) dash_command = true;
        foreach(Transform child in overlay.transform){
            if(child.gameObject.name == "Exp") child.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = exp.ToString();
        }
        if(!dashing&&!inventory_content.activeSelf){
            check_quickslot();
        }
    }


    void OnCollisionEnter2D(Collision2D c)
    {
        //if(c.gameObject.name=="tilemap") StartCoroutine(death());

        if (c.gameObject.GetComponent<damage_manager>()!=null)
        {
            damages = c.gameObject.GetComponent<damage_manager>();           
            health -= calc_damage();
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


    float calc_damage()
    {
        return player_stat.slash_def* damages.slash + player_stat.strike_def * damages.strike + player_stat.pierce_def * damages.pierce;
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

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)((worldMousePos - transform.position));
        //Debug.Log(direction);
        direction.Normalize();
        transform.eulerAngles = new Vector3(0f,0f,Vector2.SignedAngle(Vector2.up, direction));
    }

    void use_item(string item_name){
        if(item_name=="health_potion"){
            StartCoroutine(health_potion());
        }
    }

    IEnumerator dash()
    {
        dashing = true;
        //Debug.Log("dash");
        const_speed*=player_stat.dash_modifier;
        player_sprite.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(player_stat.dash_length);
        const_speed/=player_stat.dash_modifier;
        player_sprite.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(player_stat.dash_length*4f);
        dashing = false;
    }

    IEnumerator health_potion(){
        //Debug.Log("h");
        using_item = true;
        float time = 0f;
        while(time<player_stat.item_speed*0.5f){
            player_sprite.color = Color.green;
            yield return new WaitForSeconds(Time.deltaTime*8f);
            time+=Time.deltaTime*8f;
            player_sprite.color = Color.black;
            yield return new WaitForSeconds(Time.deltaTime*8f);
            time+=Time.deltaTime*8f;
        }
        health+=health_up_amount;
        while(time<player_stat.item_speed){
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
