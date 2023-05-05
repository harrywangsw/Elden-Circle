using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class glint_stone : MonoBehaviour
{
    public bool new_input, attacking;
    public float range, stamina_cost;
    public bool* p_newinput;
    public Vector3 init_loc;

    public GameObject stone, user, target;
    public float speed, curve;
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
        target = null;
    }

    void Update()
    {
        if(player!=null) target = player.locked_enemy;
        else if(enemy!=null) target = enemy.player;
        new_input = *p_newinput;
        if(new_input&&!attacking){
            StartCoroutine(fire(target));
        }
    }

    IEnumerator fire(GameObject current_target){
        attacking = true;
        GameObject s = GameObject.Instantiate(stone, gameObject.transform, false);
        s.transform.parent = null;
        statics.apply_stats(s.GetComponent<damage_manager>(), player.player_stat);
        Rigidbody2D body;
        body = s.GetComponent<Rigidbody2D>();
        StartCoroutine(check_for_new_attack(body));
        Vector3 heading = user.transform.rotation*Vector3.up*speed;
        body.AddForce((Vector2) heading*speed, ForceMode2D.Impulse);
        if(current_target==null) yield break;
        while((body.position-(Vector2)user.transform.position).sqrMagnitude<((Vector2)user.transform.position-(Vector2)current_target.transform.position).sqrMagnitude){
            if(body==null) break;
            s.transform.eulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, body.velocity));
            float angle_between = Vector2.SignedAngle(body.velocity, target.transform.position-user.transform.position);
            Vector2 force = new Vector2(body.velocity.y, -body.velocity.x).normalized;
            force*=curve;
            Debug.Log(angle_between);
            if(Mathf.Abs(angle_between)>1f){
                Debug.Log(force);
                if(angle_between>0) body.AddForce(-force, ForceMode2D.Impulse);
                else body.AddForce(force, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            if(body==null) break;
        }
    }

    IEnumerator end(Rigidbody2D body){
        float time = 0f;
        while(time<range/speed){
            if(body==null) yield break;
            body.gameObject.transform.eulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, body.velocity));
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        Destroy(body.gameObject);
    }

    IEnumerator check_for_new_attack(Rigidbody2D body){
        StartCoroutine(end(body));
        float time = 0f;
        while(time<0.5f*range/speed){
            if(body==null){
                attacking = false;
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        attacking = false;
    }
}
