using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Random=UnityEngine.Random;

//[RequireComponent(typeof(BoxCollider2D))]
public unsafe class enemy_control : MonoBehaviour
{
    public float attack_seperation, stamina_cost, current_health, walkAcceleration, dash_modifier, range, poise_broken_period, dodge_angle;
    float speed, triggertime, parriable_window;
    public int exp;
    bool trigger_time_not_set;
    public bool start_new_attack = true, dead, new_input, attacking, movable, dashing, stray, sight_lost = true, chasing, poise_broken, visible;
    public bool* pattacking;
    public stats enemy_stat;
    Vector2 velocity = new Vector2();
    Vector3 init_loc = new Vector3();
    damage_manager damages;
    GameObject greybar, broken_word, generic_item;
    public Vector3 prev_player_pos;
    public GameObject rweapon, target, player;
    player_control playerc;
    Rigidbody2D body;
    SpriteRenderer sprite;
    public List<GameObject> hit_by;
    public List<string> spawnable_item;
    public List<float> spawn_chance;
    public UnityEngine.AI.NavMeshAgent agent;
    void Start()
    {
        prev_player_pos = transform.position;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
		agent.updateUpAxis = false;
        prev_player_pos = new Vector3();
        generic_item = Resources.Load<GameObject>("prefab/spawned_item");
        sprite = gameObject.GetComponent<SpriteRenderer>();
        agent.stoppingDistance = sprite.bounds.extents.magnitude;
        player=GameObject.Find("player");
        playerc = player.GetComponent<player_control>();
        hit_by = new List<GameObject>();
        body = gameObject.GetComponent<Rigidbody2D>();
        broken_word = Resources.Load<GameObject>("prefab/broken_word");
        current_health = enemy_stat.health;
        update_weapon();
        //Physics2D.IgnoreCollision(GameObject.Find("ground").GetComponent<TilemapCollider2D>(), GetComponent<Collider2D>(), false);
    }

    void Update()
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
        if(attacking) agent.speed = enemy_stat.spd/8f;
        if(dashing) agent.speed = enemy_stat.spd*2f;
        else agent.speed = enemy_stat.spd;
        if(poise_broken) {
            new_input = false;
            return;
        }

        RaycastHit2D[] objs = Physics2D.LinecastAll(transform.position, player.transform.position);

        if(Array.FindIndex(objs, obj => obj.collider.name == "Grid")>=0)
        {
            //Debug.Log(prev_player_pos);
            //only set prev_player_pos once when sight has just been lost
            if(!sight_lost&&chasing) {
                prev_player_pos = player.transform.position;
            }
            sight_lost = true;
        }
        else{
            sight_lost = false;
            chasing = true;
        }
        if(chasing) follow_player();
        else patrol();
        if((player.transform.position - transform.position).magnitude>=40f){
            sight_lost = true;
            chasing = false;
            agent.SetDestination(transform.position);
        }
        if((agent.destination-transform.position).magnitude<=0.1f&&sight_lost){
            chasing = false;
        }
        //wait a bit before doing another attack to catch up to player and give the player a bit to react
        if ((player.transform.position - transform.position).magnitude<=range&&!sight_lost&&!dashing&&start_new_attack&&!attacking) {
            StartCoroutine(attack());
        }
        else new_input = false;
    }

    public IEnumerator attack(){
        new_input = true;
        start_new_attack = false;
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(Random.Range(attack_seperation, attack_seperation*2f));
        start_new_attack = true;
    }

    public void patrol(){
        // body.angularVelocity = 0f;
        // body.velocity = Vector2.zero;
        // move(new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));
    }

    public void follow_player(){
        float stray_angle = 0f;
        face_player();
        if(sight_lost) agent.SetDestination(prev_player_pos);
        else agent.SetDestination(player.transform.position);
        if(playerc.attacking&&!dashing&&playerc.locked_enemy==gameObject){
            StartCoroutine(dodge());
        }
    }

    void move(Vector3 moveInput)
    {
        walkAcceleration = agent.speed*10f;
        //Debug.DrawRay(transform.position, -moveInput, Color.green);
        moveInput.Normalize();

        velocity.x = Mathf.MoveTowards(velocity.x, agent.speed * moveInput.x, walkAcceleration * Time.fixedDeltaTime);
        velocity.y = Mathf.MoveTowards(velocity.y, agent.speed * moveInput.y, walkAcceleration * Time.fixedDeltaTime);
        //velocity = Quaternion.AngleAxis(-transform.eulerAngles.z, Vector3.forward) * velocity;
        body.velocity = -velocity;
        //transform.Translate(velocity * Time.deltaTime);
    }

    IEnumerator dodge(){
        //Debug.Log("doge");
        agent.isStopped = true;
        agent.ResetPath();
        chasing = false;
        float time = 0f;
        StartCoroutine(dash());
        float rand_angle = Random.Range(0, 2)==0 ? Random.Range(dodge_angle+10f, dodge_angle-10f):Random.Range(-dodge_angle+10f, -dodge_angle-10f);
        while(time<enemy_stat.dash_dura){
            move((Quaternion.Euler(0f, 0f, rand_angle)*(transform.position-player.transform.position)));
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        chasing = true;
    }

    IEnumerator dash()
    {
        dashing = true;
        //Debug.Log("dash");
        yield return new WaitForSeconds(enemy_stat.dash_dura);
        dashing = false;
    }

    void face_player()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Vector3.SignedAngle(Vector3.up, player.transform.position-transform.position, Vector3.forward));
    }


    void OnCollisionEnter2D(Collision2D c)
    {
        if(c.collider.gameObject==rweapon) return;
        if(dead) return;
        if (hit_by.Contains(c.collider.gameObject)) return;
        else hit_by.Add(c.collider.gameObject);
        StartCoroutine(statics.hit_effect(c.GetContact(0).point, gameObject));
        //Debug.Log(c.gameObject.tag);
        if(c.collider.gameObject.tag=="poise_breaker"&&attacking&&Math.Abs(Time.time-triggertime)>parriable_window){
            StartCoroutine(break_poise());
        }
        damage_manager d = c.collider.gameObject.GetComponent<damage_manager>();
        if(!d) return;   
        body.velocity = Vector3.zero;
        damages = c.gameObject.GetComponent<damage_manager>();           
        current_health -= statics.calc_damage(enemy_stat, damages);
        StartCoroutine(statics.animate_hurt(sprite));
        if (current_health < 0f) {
            death();
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
        if(dead) return;
        dead = true;
        player.GetComponent<player_control>().unbuffed_player_stat.exp+=exp;
        spawn_item();
        Destroy(gameObject);
    }

    void spawn_item(){
        int i;
        //THIS GIVES THE WRONG PROBABILITY!!!!! BUT WHATEVER for now
        for(i=0; i<spawnable_item.Count; i++){
            if(Random.Range(0.0f, 1.0f)<=spawn_chance[i]){
                //Debug.Log("prefab/"+spawnable_item[i]);
                GameObject spawned = GameObject.Instantiate(generic_item, gameObject.transform.position, Quaternion.identity);
                spawned.transform.position = new Vector3(spawned.transform.position.x, spawned.transform.position.y, 0f);
                spawned.name = spawnable_item[i];
                spawned.GetComponent<item_behaviour>().type = statics.item_types[spawnable_item[i]];
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        if (hit_by.Contains(c.collider.gameObject)) hit_by.Remove(c.collider.gameObject);
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
        rweapon.transform.localPosition = init_loc*GetComponent<SpriteRenderer>().bounds.extents.magnitude;
        statics.apply_stats(rweapon.GetComponent<damage_manager>(), rweapon.GetComponent<damage_manager>(), enemy_stat);
    }

    void OnBecameVisible()
    {
        visible = true;
    }

    void OnBecameInvisible()
    {
        visible = false;
    }
}
