using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class madman_control : MonoBehaviour
{
    public GameObject player, bullet, dialogue_screen, t_left, t_right, b_left, b_right, door;
    player_control player_c;
    Rigidbody2D body;
    reactive_messages temp_message;
    public bool shooting, start_chase, ended, player_in_sight, visible;
    public float shoot_duration, bullet_fire_duration, loop_length, vel, loop_length_y, velocity_modifier;
    public TMPro.TextMeshProUGUI dialogue_text_bar;
    public Sprite mine;
    public Vector2 destination;
    public Vector3 prev_player_pos;
    void Start()
    {
        player = GameObject.Find("player");
        player_c = player.GetComponent<player_control>();
        if(player_c.current_world.madman_dead) Destroy(gameObject);
        temp_message = GameObject.Find("temporary_messages").GetComponent<reactive_messages>();
        dialogue_screen = GameObject.Find("dialogue_screen");
        dialogue_text_bar = dialogue_screen.GetComponent<TMPro.TextMeshProUGUI>();
        body = GetComponent<Rigidbody2D>();
        destination = transform.position;
    }

    void Update()
    {
        if(ended) return;
        player_in_sight = in_sight();
        if(!start_chase) start_chase = player_in_sight;
        if(!start_chase) return;

        if(player_in_sight&&((Vector2)transform.position-destination).magnitude<=2f&&!shooting){
            transform.position = (Vector3) destination;
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot(){
        Debug.Log("shoot");
        body.velocity = Vector2.zero;
        float time = 0f;
        shooting = true;
        while(time<shoot_duration){
            transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, (Vector2)(player.transform.position-transform.position)));
            GameObject b = GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
            b.transform.rotation = transform.rotation;
            Physics2D.IgnoreCollision(b.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            b.GetComponent<Rigidbody2D>().velocity = transform.rotation*Vector2.up*vel;
            statics.apply_stats(GetComponent<damage_manager>(), b.GetComponent<damage_manager>(), new stats());
            yield return new WaitForSeconds(bullet_fire_duration);
            time+=bullet_fire_duration;
        }
        shooting = false;
        get_new_dest();
    }

    void get_new_dest(){
        //if the player is below you, run to the left or the right, depending on which one is open
        if(Mathf.Abs((prev_player_pos-transform.position).y)>=Mathf.Abs((prev_player_pos-transform.position).x)){
            if((t_left.transform.position-transform.position).magnitude<=2f||(b_left.transform.position-transform.position).magnitude<=2f){
                destination = (Vector2)transform.position+Vector2.right*loop_length;
                body.velocity = Vector2.right*velocity_modifier;
            }
            else if((t_right.transform.position-transform.position).magnitude<=2f||(b_right.transform.position-transform.position).magnitude<=2f){
                destination = (Vector2)transform.position-Vector2.right*loop_length;
                body.velocity = -Vector2.right*velocity_modifier;
            }
        }
        else{
            if((b_left.transform.position-transform.position).magnitude<=2f||(b_right.transform.position-transform.position).magnitude<=2f){
                destination = (Vector2)transform.position+Vector2.up*loop_length_y;
                body.velocity = Vector2.up*velocity_modifier;
            }
            else if((t_left.transform.position-transform.position).magnitude<=2f||(t_right.transform.position-transform.position).magnitude<=2f){
                destination = (Vector2)transform.position-Vector2.up*loop_length_y;
                body.velocity = -Vector2.up*velocity_modifier;
            }
            Debug.Log((t_right.transform.position-transform.position).magnitude.ToString());
        }
    }

    bool in_sight(){
        RaycastHit2D[] objs = Physics2D.LinecastAll(transform.position, player.transform.position);

        if(Array.FindIndex(objs, obj => obj.collider.name == "Grid")>=0)
        {
            return false;
        }
        else{
            prev_player_pos = player.transform.position;
            return true;
        }
    }

    void OnCollisionEnter2D(Collision2D c){
        //Debug.Log(c.collider.gameObject.name);
        if(c.collider.gameObject==gameObject) return;
        damage_manager d = c.collider.gameObject.GetComponent<damage_manager>();
        if(!d) return; 
        body.velocity = Vector2.zero;
        ended = true;  
        if(c.collider.gameObject.GetComponent<SpriteRenderer>().sprite == mine) {
            StartCoroutine(sad_ending());
        }
        else {
            StartCoroutine(mad_ending());
        }
    }

    IEnumerator sad_ending(){
        ended = true;
        while((transform.position-player.transform.position).magnitude>18f||!visible){
            yield return new WaitForSeconds(Time.deltaTime);
        }
        player_c.stop = true;
        dialogue_screen.transform.parent.localScale = Vector3.one;
        dialogue_text_bar.text = "He sent you here, didn't he?";
        yield return new WaitForSeconds(3f);
        dialogue_text_bar.text = "Hahaha! He remembers me still! Hahaha!";
        player_c.stop = false;
        body.constraints = RigidbodyConstraints2D.None;
        body.angularVelocity = 88f;
        yield return new WaitForSeconds(3f);
        dialogue_screen.transform.parent.localScale = Vector3.zero;
        player_c.current_world.madman_dead = true;
        save_load.Saveworld(player_c.current_world, player_c.player_stat.name);
        door.GetComponent<doors>().conditional_opening = false;
        StartCoroutine(temp_message.show_message("Somewhere, a door is unlocked."));
        Destroy(gameObject);
    }

    IEnumerator mad_ending(){
        ended = true;
        dialogue_screen.transform.parent.localScale = Vector3.one;
        dialogue_text_bar.text = "O SH*T";
        yield return new WaitForSeconds(3f);
        dialogue_screen.transform.parent.localScale = Vector3.zero;
        player_c.current_world.madman_dead = true;
        save_load.Saveworld(player_c.current_world, player_c.player_stat.name);
        door.GetComponent<doors>().conditional_opening = false;
        StartCoroutine(temp_message.show_message("Somewhere, a door is unlocked."));
        Destroy(gameObject);
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
