using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class lightning_strike : MonoBehaviour
{
    
    public float period, range, new_branch_prob, stamina_cost;
    public int max_count;
    public bool* p_newinput;
    public bool attacking, new_input, init_new_attack=true;
    public Vector3 offset, new_loc, init_loc;
    public GameObject lightning_piece;
    damage_manager selfd;
    Collider2D userc;
    stats enemy_stat, player_stat;

    void Start()
    {
        userc = transform.parent.gameObject.GetComponent<Collider2D>();
        selfd = GetComponent<damage_manager>();
        if(transform.parent.GetComponent<enemy_control>()!=null){
            enemy_stat = transform.parent.GetComponent<enemy_control>().enemy_stat;
        }
        else if(transform.parent.GetComponent<player_control>()!=null){
            player_stat = transform.parent.GetComponent<player_control>().player_stat;
        }
    }

    
    void Update()
    {
        new_input = *p_newinput;
        if(!attacking&&new_input) {
            attacking = true;
            StartCoroutine(strike(Random.Range(8, max_count), transform.position, new_branch_prob, transform.rotation.eulerAngles.z, true));
        }
    }

    IEnumerator strike(int num, Vector3 new_loc, float prob, float branch_angle, bool first_branch){
        List<GameObject> pieces = new List<GameObject>();
        int i;
        for(i=0; i<num; i++){
            float angle = branch_angle+Random.Range(-18f, 18f);
            pieces.Add(spawn_new_piece(angle, new_loc));
            new_loc = new_loc+Quaternion.AngleAxis(angle, Vector3.forward)*offset;
            if(Random.Range(0f, 1f)<=prob){
                prob/=2f;
                StartCoroutine(strike(Random.Range(4, max_count/2), new_loc, prob, angle+Random.Range(40f, 60f), false));
            }
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.1f);
        foreach(GameObject p in pieces){
            Destroy(p);
        }
        if(first_branch) attacking = false;
    }

    GameObject spawn_new_piece(float angle, Vector3 new_location){
        GameObject p = GameObject.Instantiate(lightning_piece, new_location, Quaternion.AngleAxis(angle, Vector3.forward));    
        statics.apply_stats(GetComponent<damage_manager>(), p.GetComponent<damage_manager>(), new stats());
        Physics2D.IgnoreCollision(p.GetComponent<Collider2D>(), userc, true);
        return p;
    }
}
