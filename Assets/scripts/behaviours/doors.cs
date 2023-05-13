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
    public float triggerdist, period;
    GameObject player;
    GameObject message_screen;

    void Start()
    {
        player = GameObject.Find("player");
        message_screen = GameObject.Find("message_screen");
    }


    void Update()
    {
        if((player.transform.position-transform.position).magnitude<=triggerdist){
            int ind = message_screen.GetComponent<switchmessages>().messages.IndexOf("press enter to open door");
            if(ind<0){
                message_screen.GetComponent<switchmessages>().messages.Add("press enter to open door");
                message_screen.GetComponent<switchmessages>().current = message_screen.GetComponent<switchmessages>().messages.Count-1;
            }
            if(Input.GetKeyDown(KeyCode.Return)&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to open door"){
                if(Vector3.Angle(transform.up, (player.transform.position-transform.position))>0&&open_left){
                    StartCoroutine(open());
                }
                else if(Vector3.Angle(transform.up, (player.transform.position-transform.position))<0&&open_right){
                    StartCoroutine(open());
                }
                else{
                    ind = message_screen.GetComponent<switchmessages>().messages.IndexOf("this door does not open from this side");
                    if(ind<0){
                        message_screen.GetComponent<switchmessages>().messages.Add("this door does not open from this side");
                        message_screen.GetComponent<switchmessages>().current = message_screen.GetComponent<switchmessages>().messages.Count-1;
                    }
                }
            }
        }
        else{
            int ind = message_screen.GetComponent<switchmessages>().messages.IndexOf("press enter to open door");
            if(ind>=0){
                message_screen.GetComponent<switchmessages>().messages.RemoveAt(ind);
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
