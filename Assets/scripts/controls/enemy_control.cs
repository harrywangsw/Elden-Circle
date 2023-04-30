using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Random=UnityEngine.Random;

//[RequireComponent(typeof(BoxCollider2D))]
public unsafe class enemy_control : MonoBehaviour
{
    public float current_health, previous_health, const_speed, walkAcceleration, dash_modifier, dash_dura, rweapon_range, poise_broken_period;
    float speed, triggertime, parriable_window;
    public int exp;
    bool trigger_time_not_set;
    public bool new_input, attacking, movable, dashing, dash_command, stray, sight_lost, chasing, poise_broken;
    public bool* pattacking;
    public stats enemy_stat;
    Vector2 velocity = new Vector2();
    damage_manager damages;
    GameObject greybar, player, broken_word, generic_item;
    Vector3 prev_player_pos;
    public GameObject rweapon;
    Rigidbody2D body;
    SpriteRenderer sprite;
    public List<GameObject> hit_by;
    public List<string> spawnable_item;
    public List<float> spawn_chance;
    UnityEngine.AI.NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
		agent.updateUpAxis = false;
        prev_player_pos = new Vector3();
        generic_item = Resources.Load<GameObject>("prefab/spawned_item");
        sprite = gameObject.GetComponent<SpriteRenderer>();
        player=GameObject.Find("player");
        hit_by = new List<GameObject>();
        body = gameObject.GetComponent<Rigidbody2D>();
        broken_word = Resources.Load<GameObject>("prefab/broken_word");
        current_health = enemy_stat.health;
        previous_health = current_health;
        update_weapon();
        //Physics2D.IgnoreCollision(GameObject.Find("ground").GetComponent<TilemapCollider2D>(), GetComponent<Collider2D>(), false);
    }

    void FixedUpdate()
    {
        attacking = *pattacking;
        //tracking the time when attack started to determine if enemy can be parried right now
        if(attacking&&trigger_time_not_set){
            triggertime = Time.time;
            trigger_time_not_set = false;
        }
        if(!attacking){
            trigger_time_not_set = true;
        }
        if(attacking) speed = const_speed/8f;
        else speed = const_speed;
        if(poise_broken) {
            new_input = false;
            return;
        }

        if(chasing) follow_player();
        else patrol();
        if ((player.transform.position - transform.position).magnitude<=rweapon_range) new_input = true;
        else new_input = false;
    }

    public void patrol(){
        // body.angularVelocity = 0f;
        // body.velocity = Vector2.zero;
        // move(new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));
    }

    public void follow_player(){
        if(!sight_lost) chasing = true;
        if((player.transform.position - transform.position).magnitude>=40f){
            sight_lost = true;
            chasing = false;
        }
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
        RaycastHit2D[] objs = Physics2D.LinecastAll(transform.position, player.transform.position);

        if(Array.FindIndex(objs, obj => obj.collider.name == "Grid")>=0)
        {
            //Debug.Log(prev_player_pos);
            //only set prev_player_pos once when sight has just been lost
            if(!sight_lost) prev_player_pos = player.transform.position;
            sight_lost = true;
            if((transform.position-prev_player_pos).magnitude<=0.1f){
                chasing = false;
            }
            agent.SetDestination(prev_player_pos+rweapon_range*(transform.position-prev_player_pos).normalized);
            //move(transform.position-prev_player_pos);
        }
        else agent.SetDestination(player.transform.position);
    }

    void move(Vector3 moveInput)
    {
        Debug.DrawRay(transform.position, -moveInput, Color.green);
        moveInput.Normalize();

        velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput.x, walkAcceleration * Time.fixedDeltaTime);
        velocity.y = Mathf.MoveTowards(velocity.y, speed * moveInput.y, walkAcceleration * Time.fixedDeltaTime);
        //velocity = Quaternion.AngleAxis(-transform.eulerAngles.z, Vector3.forward) * velocity;
        if(dash_command&&!dashing){
            dash_command = false;
            StartCoroutine(dash());
        }
        body.velocity = -velocity;
        //transform.Translate(velocity * Time.deltaTime);
    }

    IEnumerator dash()
    {
        dashing = true;
        //Debug.Log("dash");
        speed*=dash_modifier;
        sprite.color =  Color.grey;
        yield return new WaitForSeconds(dash_dura);
        speed/=dash_modifier;
        sprite.color =  Color.black;
        yield return new WaitForSeconds(dash_dura*4f);
        dashing = false;
    }

    void face_player()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Vector3.SignedAngle(Vector3.up, player.transform.position-transform.position, Vector3.forward));
    }


    void OnCollisionEnter2D(Collision2D c)
    {
        //Debug.Log(c.gameObject.tag);
        if(c.collider.gameObject.tag=="poise_breaker"&&attacking&&Math.Abs(Time.time-triggertime)>parriable_window){
            StartCoroutine(break_poise());
        }
        if (hit_by.Contains(c.gameObject)) return;
        else hit_by.Add(c.gameObject);
        if (c.gameObject.GetComponent<damage_manager>()!=null)
        {
            if(dashing) return;
            body.velocity = Vector3.zero;
            damages = c.gameObject.GetComponent<damage_manager>();           
            current_health -= calc_damage();
            StartCoroutine(statics.animate_hurt(sprite));
            if (current_health < 0f) death();
        }
    }

    IEnumerator break_poise(){
        //Debug.Log("wtf");
        enemy_stat.strike_def/=3f;
        enemy_stat.pierce_def/=3f;
        enemy_stat.slash_def/=3f;
        bool turn_dark = true;
        poise_broken = true;
        //GameObject b = GameObject.Instantiate(broken_word, transform, false);
        float time = 0f;
        while(time<poise_broken_period){
            time+=Time.deltaTime;
            if(turn_dark){
                sprite.color = sprite.color+new Color(0f, 0f, 0f, Time.deltaTime/(poise_broken_period/8f));
            }
            else{
                sprite.color = sprite.color-new Color(0f, 0f, 0f, Time.deltaTime/(poise_broken_period/8f));
            }
            if(sprite.color.a>=1f){
                turn_dark = false;
            }
            if(sprite.color.a<=0){
                turn_dark = true;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        sprite.color = Color.black;
        //Destroy(b);
        poise_broken = false;
    }

    void death(){
        player.GetComponent<player_control>().player_stat.exp+=exp;
        spawn_item();
        Destroy(gameObject);
    }

    void spawn_item(){
        int i;
        //THIS GIVES THE WRONG PROBABILITY!!!!! BUT WHATEVER
        for(i=0; i<spawnable_item.Count; i++){
            if(Random.Range(0.0f, 1.0f)<=spawn_chance[i]){
                //Debug.Log("prefab/"+spawnable_item[i]);
                GameObject spawned = GameObject.Instantiate(generic_item, gameObject.transform.position, Quaternion.identity);
                spawned.name = spawnable_item[i];
                spawned.GetComponent<item_behaviour>().type = statics.item_types[spawnable_item[i]];
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
        return enemy_stat.slash_def * damages.slash + enemy_stat.strike_def * damages.strike + enemy_stat.pierce_def * damages.pierce;
    }

    public void update_weapon()
    {
        rweapon = GameObject.Instantiate(rweapon, transform, false);
        //get the pointers of variables in weapon that must be controled by the player at the start, so we don't have to do these if statements every frame
        if (rweapon.GetComponent<spear_attack>()!=null)
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<spear_attack>().attacking) { pattacking = pattack_fixed; }
            fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<spear_attack>().p_newinput = p_attack_order; }
            parriable_window = rweapon.GetComponent<spear_attack>().parriable_window;
            rweapon_range = rweapon.GetComponent<spear_attack>().range;
        }
        else if (rweapon.GetComponent<fire_crackers>()!=null)
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<fire_crackers>().attacking) { pattacking = pattack_fixed; }
            fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<fire_crackers>().p_newinput = p_attack_order; }
            
        }
        else if (rweapon.GetComponent<dagger_fan>()!=null)
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<dagger_fan>().attacking) { pattacking = pattack_fixed; }
            fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<dagger_fan>().p_newinput = p_attack_order; }
        }
        else if (rweapon.GetComponent<parry_shield>()!=null)
        {
            //get adress of attacking from right-hand weapon and save the adress in pattacking
            fixed (bool* pattack_fixed = &rweapon.GetComponent<parry_shield>().attacking) { pattacking = pattack_fixed; }
            fixed(bool* p_attack_order = &new_input) { rweapon.GetComponent<parry_shield>().p_newinput = p_attack_order; }
        }
    }
}
