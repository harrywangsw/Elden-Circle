using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class item_behaviour : MonoBehaviour
{
    GameObject player, message_screen;
    public float trigger_dist, flash_period;
    public string type;
    public bool entered = false;
    SpriteRenderer sprite;
    inventory_manager inv_manager;
    player_control plac;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("player");
        message_screen = GameObject.Find("message_screen");
        StartCoroutine(flash());
        inv_manager = GameObject.Find("inventory_content").GetComponent<inventory_manager>();
        plac = player.GetComponent<player_control>();
    }

    void Update()
    {
        int ind = message_screen.GetComponent<switchmessages>().messages.IndexOf("press enter to pick up item");
        if((player.transform.position-transform.position).magnitude<=trigger_dist){
            // Debug.Log("vector3 angle: "+Vector3.Angle(transform.up, (player.transform.position-transform.position)).ToString());
            // Debug.Log(player.transform.rotation.eulerAngles.z);
            // float ang = Vector3.Angle(transform.position-player.transform.position);
            // float player_pointing = player.transform.rotation.eulerAngles.z;

            if(ind<0&&!entered){
                entered = true;
                message_screen.GetComponent<switchmessages>().messages.Add("press enter to pick up item");
                message_screen.GetComponent<switchmessages>().current = message_screen.GetComponent<switchmessages>().messages.Count-1;
            }
        }
        else{
            if(ind>=0&&entered) {
                message_screen.GetComponent<switchmessages>().messages.RemoveAt(ind);
                message_screen.GetComponent<switchmessages>().current = message_screen.GetComponent<switchmessages>().messages.Count-1;
            }
            entered = false;
        }

        if(Input.GetKeyDown(KeyCode.Return)&&message_screen.GetComponent<TMPro.TextMeshProUGUI>().text=="press enter to pick up item"&&entered){
                int i;
                for(i=0; i<plac.player_stat.inv.inv.Count; i++){
                    //Debug.Log(plac.player_stat.inv.inv[i].Item1);
                    if(plac.player_stat.inv.inv[i].Item1==gameObject.name){
                        plac.player_stat.inv.inv[i] = Tuple.Create(gameObject.name, plac.player_stat.inv.inv[i].Item2+1, statics.item_types[gameObject.name]);
                        //Debug.Log(plac.player_stat.inv.inv[i].Item2.ToString());
                        plac.Update_quickslot();
                        if(ind>=0) message_screen.GetComponent<switchmessages>().messages.RemoveAt(ind);
                        Destroy(gameObject);
                    }
                }
                plac.player_stat.inv.inv.Add(Tuple.Create(gameObject.name, 1, statics.item_types[gameObject.name]));
                inv_manager.add_item(Resources.Load<GameObject>("prefab/UI_items/"+gameObject.name));
                if(ind>=0) message_screen.GetComponent<switchmessages>().messages.RemoveAt(ind);
                Destroy(gameObject);
        }
    }
    
    IEnumerator flash(){
        bool decreasing = true;
        while(true){
            if(!decreasing){
                sprite.color+=new Color(0f, 0f, 0f, Time.deltaTime/flash_period);
                if(sprite.color.a>=1f){
                    decreasing = true;
                }
            }
            else{
                sprite.color-=new Color(0f, 0f, 0f, Time.deltaTime/flash_period);
                if(sprite.color.a<=0f) decreasing = false;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
