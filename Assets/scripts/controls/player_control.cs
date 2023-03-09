using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Tilemaps;

public unsafe class player_control : MonoBehaviour
{
    public float speed, walkAcceleration, item_speed, health_up_amount, range;
    public string player_name;
    public int exp;
    public bool new_input, attacking, movable, dashing, dash_command, using_item;
    public float health;
    public stats player_stat;
    public inventory player_items;
    public world_details current_world;
    public bool* pattacking;
    damage_manager damages;
    public Vector2 velocity = new Vector2();
    Vector3 previous_pos = new Vector3();
    public GameObject rweapon, overlay, death_screen, menu;
    public SpriteRenderer player_sprite;
    string type;
    Rigidbody2D body;
    void Start()
    {
        player_name = player_stat.name;
        speed = player_stat.spd;
        player_items = new inventory();
        player_sprite = gameObject.GetComponent<SpriteRenderer>();
        health = player_stat.health;
        body = gameObject.GetComponent<Rigidbody2D>();
        update_weapon();
        spawn_quickslot();
        //Physics2D.IgnoreCollision(GameObject.Find("ground").GetComponent<TilemapCollider2D>(), GetComponent<CircleCollider2D>(), true);
        //GameObject.Find("ground").GetComponent<TilemapCollider2D>().enabled = false;
    }
    void update_weapon()
    {
        //get the pointers of variables in weapon that must be controled by the player at the start, so we don't have to do these if statements every frame
        if (rweapon.GetComponent<straight_sword>()!=null)
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<straight_sword>().attacking) { pattacking = pattack_fixed; }
            fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<straight_sword>().p_newinput = p_attack_order; }
            range = rweapon.GetComponent<straight_sword>().range;
        }
        else if (rweapon.GetComponent<straight_sword>()!=null)
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<spear_attack>().attacking) { pattacking = pattack_fixed; }
            fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<spear_attack>().p_newinput = p_attack_order; }
            range = rweapon.GetComponent<spear_attack>().range;
        }
    }
    
    
    public void spawn_quickslot(){
        GameObject item;
        if(player_items.quickslot_up==-1) return;
        Debug.Log("prefab/"+player_items.inv[player_items.quickslot_up].Item1);
        GameObject i = Resources.Load<GameObject>("prefab/"+player_items.inv[player_items.quickslot_up].Item1);
        Transform slot = GameObject.Find("up").transform;
        item = GameObject.Instantiate(i, slot);
        item.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_items.inv[player_items.quickslot_up].Item2.ToString();

        if(player_items.quickslot_down==-1) return;
        i = Resources.Load<GameObject>("prefab/"+player_items.inv[player_items.quickslot_down].Item1);
        slot = GameObject.Find("down").transform;
        item = GameObject.Instantiate(i, slot);
        item.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_items.inv[player_items.quickslot_down].Item2.ToString();

        if(player_items.quickslot_left==-1) return;
        i = Resources.Load<GameObject>("prefab/"+player_items.inv[player_items.quickslot_left].Item1);
        slot = GameObject.Find("left").transform;
        item = GameObject.Instantiate(i, slot);
        item.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_items.inv[player_items.quickslot_left].Item2.ToString();

        if(player_items.quickslot_right==-1) return;
        i = Resources.Load<GameObject>("prefab/"+player_items.inv[player_items.quickslot_right].Item1);
        slot = GameObject.Find("right").transform;
        item = GameObject.Instantiate(i, slot);
        item.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_items.inv[player_items.quickslot_right].Item2.ToString();


        // SpriteRenderer up = GameObject.Find("up").transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        // if (player_items.quickslot_up!="") up.
        // SpriteRenderer up = GameObject.Find("up").transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        // if (player_items.quickslot_up!="") up.
    }

    public void Update_quickslot(){
        GameObject it = GameObject.Find("up").transform.GetChild(0).gameObject;
        it.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_items.inv[player_items.quickslot_up].Item2.ToString();

        it = GameObject.Find("down").transform.GetChild(0).gameObject;
        it.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_items.inv[player_items.quickslot_down].Item2.ToString();

        it = GameObject.Find("left").transform.GetChild(0).gameObject;
        it.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_items.inv[player_items.quickslot_left].Item2.ToString();

        it = GameObject.Find("right").transform.GetChild(0).gameObject;
        it.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_items.inv[player_items.quickslot_right].Item2.ToString();
    }

    void Update()
    {
        // if(control_functions.out_of_bound(transform.position)){
        //     death();
        // }
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)((worldMousePos - transform.position));
        //Debug.Log(direction);
        direction.Normalize();
        transform.eulerAngles = new Vector3(0f,0f,Vector2.SignedAngle(Vector2.up, direction));

        attacking = *pattacking;
        if (Input.GetMouseButtonDown(0)) new_input = true;
        else new_input = false;
        if (Input.GetKeyDown("space")&&!dashing) dash_command = true;
        foreach(Transform child in overlay.transform){
            if(child.gameObject.name == "Exp") child.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = exp.ToString();
        }
        
    }


    void OnCollisionEnter2D(Collision2D c)
    {
        transform.position = previous_pos;
        if (c.gameObject.tag == "harmful")
        {
            damages = c.gameObject.GetComponent<damage_manager>();           
            health -= calc_damage();
            animate_hurt();
            if (health < 0f) death();
        }
    }

    void death(){
        overlay.SetActive(false);
        menu.SetActive(false);
        death_screen.SetActive(true);
        Destroy(gameObject);
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
        previous_pos = transform.position;
        if(!attacking&&!using_item) {
            move();
            if(!dashing){
                check_quickslot();
            }
        }
    }

    void move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //moveInput = control_functions.rotate(moveInput, transform.eulerAngles.z);
        velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput.x, walkAcceleration * Time.fixedDeltaTime);
        velocity.y = Mathf.MoveTowards(velocity.y, speed * moveInput.y, walkAcceleration * Time.fixedDeltaTime);
        //velocity = Quaternion.AngleAxis(-transform.eulerAngles.z, Vector3.forward) * velocity;
        if(dash_command&&!dashing){
            dash_command = false;
            StartCoroutine(dash());
        }
        transform.Translate(velocity * Time.deltaTime);
        Camera.main.gameObject.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    void check_quickslot(){
        if(Input.GetKeyDown("e")){
            if(player_items.inv[player_items.quickslot_up].Item2<=0) return;
            player_items.inv[player_items.quickslot_up] = Tuple.Create(player_items.inv[player_items.quickslot_up].Item1, player_items.inv[player_items.quickslot_up].Item2-1);
            GameObject.Find("up").transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = player_items.inv[player_items.quickslot_up].Item2.ToString();
            use_item(player_items.inv[player_items.quickslot_up].Item1);
        }
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
        speed*=player_stat.dash_modifier;
        player_sprite.color =  Color.grey;
        yield return new WaitForSeconds(player_stat.dash_length);
        speed/=player_stat.dash_modifier;
        player_sprite.color =  Color.black;
        yield return new WaitForSeconds(player_stat.dash_length*4f);
        dashing = false;
    }

    IEnumerator health_potion(){
        //Debug.Log("h");
        using_item = true;
        yield return new WaitForSeconds(player_stat.item_speed/2f);
        health+=health_up_amount;
        yield return new WaitForSeconds(player_stat.item_speed/2f);
        using_item = false;
    }
}
