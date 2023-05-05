using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class glint_stone : MonoBehaviour
{
    public bool new_input, attacking;
    public float range;
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
        body.velocity = user.transform.rotation*Vector2.up*speed;
        if(current_target==null) yield break;
        while((body.position-(Vector2)user.transform.position).sqrMagnitude<(body.position-(Vector2)current_target.transform.position).sqrMagnitude){
            if(body==null) break;
            body.AddForce(((Vector2)target.transform.position-body.position).normalized*curve);
            s.transform.eulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, body.velocity));
            yield return new WaitForSeconds(Time.deltaTime);
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
