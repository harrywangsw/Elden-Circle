using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class machine_gun : MonoBehaviour
{
    public bool new_input, attacking, attack_interrupted;
    public float range, stamina_cost;
    public bool* p_newinput;
    public Vector3 init_loc;
    public GameObject user;

    public float expand_period, bullet_fire_duration, vel;
    public GameObject gun_barrel, gun_tripod, bullet;
    GameObject barrel;
    public bool used_by_enemy;
    enemy_control enemy;
    player_control player;
    void Start()
    {
        user = transform.parent.gameObject;
        if(user.GetComponent<player_control>()!=null){
            player = user.GetComponent<player_control>();
        }
        else{
            enemy = user.GetComponent<enemy_control>();
        }
    }

    void Update()
    {
        if(player!=null) attack_interrupted = player.attack_interrupted;
        new_input = *p_newinput;
        if(new_input&&!attacking){
            if(player!=null) player.stop = true;
            StartCoroutine(spawn_gun());
        }
        if(!barrel) return;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Vector2.SignedAngle(Vector2.up, (Vector2)((worldMousePos - transform.position)).normalized);
        if(Mathf.Abs(angle)<90f) barrel.transform.eulerAngles = new Vector3(0f,0f,angle);
    }

    IEnumerator spawn_gun(){
        attacking = true;
        float time = 0f;
        barrel = GameObject.Instantiate(gun_barrel, transform.position, player.gameObject.transform.rotation);
        barrel.transform.localScale = new Vector3(1f, 0f, 1f);
        yield return statics.expand(barrel.transform, expand_period/2f, Vector3.one);
        GameObject tripod = GameObject.Instantiate(gun_tripod, transform.position, Quaternion.identity);
        tripod.transform.localScale = Vector3.zero;
        yield return statics.expand(tripod.transform, expand_period/2f, Vector3.one);

        time = 0f;
        if(player!=null){
            while((Input.GetMouseButton(0)||Input.GetMouseButton(1))&&!player.attack_interrupted){
                GameObject b = GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
                b.transform.rotation = barrel.transform.rotation;
                b.GetComponent<Rigidbody2D>().velocity = barrel.transform.rotation*Vector2.up*vel;
                statics.apply_stats(GetComponent<damage_manager>(), b.GetComponent<damage_manager>(), new stats());
                player.stamina-=stamina_cost;
                if(player.stamina<=0) break;
                yield return new WaitForSeconds(bullet_fire_duration);
            }
        }
        else{

        }
        Destroy(barrel);
        Destroy(tripod);
        attacking = false;
        player.stop = false;
    }
}
