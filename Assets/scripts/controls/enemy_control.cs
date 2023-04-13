using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[RequireComponent(typeof(BoxCollider2D))]
public unsafe class enemy_control : MonoBehaviour
{
    public float current_health, previous_health, speed, walkAcceleration, dash_modifier, dash_length, rweapon_range;
    public int exp;
    public bool new_input, attacking, movable, dashing, dash_command, stray;
    public bool* pattacking;
    public stats enemy_stat;
    Vector2 velocity = new Vector2();
    damage_manager damages;
    GameObject healthbar, greybar, player;
    Transform end_marker;
    public GameObject rweapon;
    Rigidbody2D body;
    SpriteRenderer sprite;
    public List<GameObject> hit_by;
    public List<string> spawnable_item;
    public List<float> spawn_chance;
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        end_marker = transform.GetChild(0);
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
        //Physics2D.IgnoreCollision(GameObject.Find("ground").GetComponent<TilemapCollider2D>(), GetComponent<Collider2D>(), false);
    }

    void Update()
    {
        attacking = *pattacking;
        if(!attacking){
            follow_player();
        }
        if ((player.transform.position - transform.position).magnitude<=rweapon_range) new_input = true;
        else new_input = false;
    }

    public void follow_player(){
        float stray_angle = 0f;
        face_player();
        //code for dodging
        // if((player.transform.position - transform.position).magnitude<=rweapon_range){
        //     if(Random.value<0.5f){
        //         stray_angle = 90f;
        //     }
        //     else{
        //         stray_angle = -90f;
        //     }
        // }
        if(Physics.Linecast(transform.position, player.transform.position))
        {
             
        }
        else{
            move(/*Quaternion.AngleAxis(stray_angle, (player.transform.position-transform.position))*/(transform.position-player.transform.position));
        }
    }

    void move(Vector3 moveInput)
    {
        moveInput.Normalize();
        velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput.x, walkAcceleration * Time.fixedDeltaTime);
        velocity.y = Mathf.MoveTowards(velocity.y, speed * moveInput.y, walkAcceleration * Time.fixedDeltaTime);
        //velocity = Quaternion.AngleAxis(-transform.eulerAngles.z, Vector3.forward) * velocity;
        if(dash_command&&!dashing){
            dash_command = false;
            StartCoroutine(dash());
        }
        transform.Translate(velocity * Time.deltaTime);
    }

    IEnumerator dash()
    {
        dashing = true;
        //Debug.Log("dash");
        speed*=dash_modifier;
        sprite.color =  Color.grey;
        yield return new WaitForSeconds(dash_length);
        speed/=dash_modifier;
        sprite.color =  Color.black;
        yield return new WaitForSeconds(dash_length*4f);
        dashing = false;
    }

    void face_player()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Vector3.SignedAngle(Vector3.up, player.transform.position-transform.position, Vector3.forward));
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
            //Debug.Log("damage: "+calc_damage().ToString());
            control_functions.animate_hurt(sprite);
            if (current_health < 0f) death();
        }
    }

    void death(){
        player.GetComponent<player_control>().exp+=exp;
        spawn_item();
        Destroy(gameObject);
    }

    void spawn_item(){
        int i;
        for(i=0; i<spawnable_item.Count; i++){
            if(Random.Range(0.0f, 1.0f)<=spawn_chance[i]){
                Debug.Log("prefab/"+spawnable_item[i]);
                GameObject item = Resources.Load<GameObject>("item_spawns/"+spawnable_item[i]);
                GameObject spawned = GameObject.Instantiate(item, gameObject.transform.position, Quaternion.identity);
                spawned.name = spawnable_item[i];
                break;
            }
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
        //get the pointers of variables in weapon that must be controled by the player at the start, so we don't have to do these if statements every frame
        if (rweapon.GetComponent<straight_sword>()!=null)
        {
            straight_sword s = rweapon.GetComponent<straight_sword>();
            fixed (bool* pattack_fixed = &s.attacking) { pattacking = pattack_fixed; }
            fixed (bool* p_attack_order = &new_input) { s.p_newinput = p_attack_order; }
            rweapon_range = s.range;
        }
        else if (rweapon.GetComponent<spear_attack>()!=null)
        {
            spear_attack s = rweapon.GetComponent<spear_attack>();
            fixed (bool* pattack_fixed = &s.attacking) { pattacking = pattack_fixed; }
            fixed (bool* p_attack_order = &new_input) { s.p_newinput = p_attack_order; }
            rweapon.transform.position = new Vector3(end_marker.position.x+0.08f, end_marker.position.y, end_marker.position.z);
            rweapon_range = s.range;
        }
    }
}
