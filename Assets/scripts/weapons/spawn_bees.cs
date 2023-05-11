using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class spawn_bees : MonoBehaviour
{
    public bool init_attack, new_input, attacking;
    public float bee_speed, range, parriable_window, init_angle_range, stamina_cost;
    public int max_num;
    public bool* p_newinput;
    public Vector3 init_loc;
    public GameObject bee, user;
    stats player_stat, enemy_stat;
    Collider2D userc;

    void Start()
    {
        user = transform.parent.gameObject;
        if(transform.parent.GetComponent<enemy_control>()!=null){
            enemy_stat = transform.parent.GetComponent<enemy_control>().enemy_stat;
            
        }
        else if(transform.parent.GetComponent<player_control>()!=null){
            player_stat = transform.parent.GetComponent<player_control>().player_stat;
        }
        userc = user.GetComponent<Collider2D>();
    }


    void Update()
    {
        new_input = *p_newinput;
        if(new_input&&!attacking){
            attacking = true;
            StartCoroutine(spawn());
        }
    }

    IEnumerator spawn(){
        while(new_input){
            StartCoroutine(new_wave());
            yield return new WaitForSeconds(1.8f);
        }
        attacking = false;
    }

    IEnumerator new_wave(){
        int num = Random.Range(8, max_num);
        UnityEngine.AI.NavMeshAgent agent, user_agent;
        user_agent = user.GetComponent<UnityEngine.AI.NavMeshAgent>();
        user_agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
        for(int i=0; i<num; i++){
            GameObject b = GameObject.Instantiate(bee, user.transform, false);
            Physics2D.IgnoreCollision(b.GetComponent<Collider2D>(), userc, true);
            statics.apply_stats(GetComponent<damage_manager>(), b.GetComponent<damage_manager>(), new stats());
            Vector3 heading = transform.rotation*Vector3.up;
            b.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0f, 0f, Random.Range(180f-init_angle_range, 180f+init_angle_range))*heading*bee_speed;
            agent = b.GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.updateRotation = false;
		    agent.updateUpAxis = false;
            agent.SetDestination(transform.position+heading*range);
            //
            //
            //
            //FIGURE OUT HOW TO MAKE THE BEE_HIVE NAVMESH AGENT IGNORE ITS BEE'S NAVMESH AGENTS!!!
            //
            //
            //
            agent.speed = 2f*bee_speed;
            StartCoroutine(destroy_bee(b));
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator destroy_bee(GameObject bee){
        yield return new WaitForSeconds(20f*range/bee_speed);
        Destroy(bee);
    }
}
