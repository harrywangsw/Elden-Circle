using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider2D))]
public unsafe class enemy_control : MonoBehaviour
{
    public float current_health, previous_health;
    public int exp;
    public bool new_input, attacking, movable;
    public bool* pattacking;
    public stats enemy_stat;
    damage_manager damages;
    GameObject healthbar, greybar, player;
    Transform end_marker;
    public GameObject rweapon;
    Rigidbody2D body;
    public List<GameObject> hit_by;
    public List<string> spawnable_item;
    public List<float> spawn_chance;
    void Start()
    {
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
    }

    void Update()
    {
        if ((player.transform.position - transform.position).magnitude<=3f) new_input = true;
        else new_input = false;
        face_player();
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
            Debug.Log("damage: "+calc_damage().ToString());
            animate_hurt();
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
                GameObject item = Resources.Load<GameObject>("prefab/"+spawnable_item[i]);
                GameObject.Instantiate(item, gameObject.transform);
                break;
            }
        }
    }

    IEnumerator animate_hurt()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
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
        string type = rweapon.tag;
        //get the pointers of variables in weapon that must be controled by the player at the start, so we don't have to do these if statements every frame
        if (type == "straight_sword")
        {
            fixed (bool* p_attack_order = &new_input) { rweapon.GetComponent<straight_sword>().p_newinput = p_attack_order; }
        }
        else if (type == "spear")
        {
            fixed (bool* p_attack_order = &new_input) { rweapon.GetComponent<spear_attack>().p_newinput = p_attack_order; }
            rweapon.transform.position = new Vector3(end_marker.position.x+0.08f, end_marker.position.y, end_marker.position.z);
        }
    }
}
