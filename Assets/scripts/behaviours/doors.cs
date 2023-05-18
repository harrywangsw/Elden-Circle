using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class doors : MonoBehaviour
{
    public int num;
    public bool open_right;
    public bool open_left;
    public bool breakable;
    public bool conditional_opening;
    public bool entered = false;
    public float trigger_dist, period;
    GameObject player;
    GameObject message_screen;
    reactive_messages temporary_messages;
    switchmessages swi;

    void Start()
    {
        player = GameObject.Find("player");
        message_screen = GameObject.Find("message_screen");
        swi = message_screen.GetComponent<switchmessages>();
        temporary_messages = GameObject.Find("temporary_messages").GetComponent<reactive_messages>();
    }


    void Update()
    {
        if(breakable) return;
        int ind = swi.messages.IndexOf("press enter to open the door");
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            Debug.Log(ind.ToString());
            if(ind<0&&!entered){
                Debug.Log("addded");
                entered = true;
                swi.messages.Add("press enter to open the door");
                swi.current = swi.messages.Count-1;
            }
        }
        else{
            if(ind>=0&&entered) {
                swi.messages.RemoveAt(ind);
                swi.current = swi.messages.Count-1;
            }
            entered = false;
        }

        if(Input.GetKeyDown(KeyCode.Return)&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to open the door"&&entered){
            if(conditional_opening){
                StartCoroutine(temporary_messages.show_message("Locked by some mechanism, or someone."));
            }
            if(Vector3.Angle(transform.up, (player.transform.position-transform.position))>0&&open_left){
                StartCoroutine(open());
            }
            else if(Vector3.Angle(transform.up, (player.transform.position-transform.position))<0&&open_right){
                StartCoroutine(open());
            }
            else{
                temporary_messages.show_message("This door does not open from this side!");
            }
        }

    }

    IEnumerator open(){
        float time = 0f;
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        while(time<period){
            time+=Time.deltaTime;
            sp.color=sp.color-new Color(0f, 0f, 0f, Time.deltaTime/period);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log(statics.world_index[SceneManager.GetActiveScene().name].ToString()+" Doors: "+player.GetComponent<player_control>().current_world.opened_doors.Count.ToString());
        Debug.Log(num.ToString()+" D: "+player.GetComponent<player_control>().current_world.opened_doors[statics.world_index[SceneManager.GetActiveScene().name]].Count.ToString());
        player.GetComponent<player_control>().current_world.opened_doors[statics.world_index[SceneManager.GetActiveScene().name]][num] = true;
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D c){
        if(c.collider.gameObject.GetComponent<damage_manager>()!=null&&breakable){
            StartCoroutine(open());
        }
    }
}
