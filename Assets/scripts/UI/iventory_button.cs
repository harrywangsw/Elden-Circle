using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class iventory_button : MonoBehaviour
{
    Button b;
    player_control player;
    bool wait_for_input, in_uquick_slot, in_lquick_slot, in_rquick_slot;
    public int item_index;
    public GameObject marker, item;
    inventory_manager inv;
    void Start()
    {
        b = gameObject.GetComponent<Button>();
        player = GameObject.Find("player").GetComponent<player_control>();
        player.player_stat.inv = player.player_stat.inv;
        inv = GameObject.Find("inventory_content").GetComponent<inventory_manager>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { get_input(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry Entry = new EventTrigger.Entry();
        Entry.eventID = EventTriggerType.PointerExit;
        Entry.callback.AddListener((data) => { ignore_input(); });
        trigger.triggers.Add(Entry);

        int a = player.player_stat.inv.quickslot_up_indexes.FindIndex(obj => obj == item_index);
        in_uquick_slot = a>=0;

        a = player.player_stat.inv.quickslot_right_indexes.FindIndex(obj => obj == item_index);
        in_rquick_slot = a>=0;
        
        a = player.player_stat.inv.quickslot_left_indexes.FindIndex(obj => obj == item_index);
        in_lquick_slot = a>=0;       
    }

    
    void Update()
    {
        if(!wait_for_input) return;
        if(Input.GetKeyDown(KeyCode.LeftShift)&&player.player_stat.inv.inv[item_index].item_type=="weapon"){
            //Debug.Log("addpls");
            if(!in_rquick_slot){
                player.player_stat.inv.quickslot_right_indexes.Add(item_index);
                in_rquick_slot = true;
                inv.r_gameobjects.Add(gameObject);
                inv.current_itemr = inv.r_gameobjects.Count-1;
                inv.switchr();
            }
            else{
                player.player_stat.inv.quickslot_right_indexes.Remove(item_index);
                in_rquick_slot = false;
                inv.r_gameobjects.Remove(gameObject);
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl)&&player.player_stat.inv.inv[item_index].item_type=="weapon"){
            if(!in_lquick_slot){
                player.player_stat.inv.quickslot_left_indexes.Add(item_index);
                in_rquick_slot = false;
                in_lquick_slot = true;
                inv.l_gameobjects.Add(gameObject);
                inv.current_iteml = inv.l_gameobjects.Count-1;
                inv.switchl();
            }
            else{
                player.player_stat.inv.quickslot_left_indexes.Remove(item_index);
                in_lquick_slot = false;
                inv.l_gameobjects.Remove(gameObject);
            }
        }
        if(Input.GetKeyDown("e")&&player.player_stat.inv.inv[item_index].item_type=="item"){
            if(!in_uquick_slot){
                player.player_stat.inv.quickslot_up_indexes.Add(item_index);
                inv.u_gameobjects.Add(gameObject);
                in_uquick_slot = true;
                inv.current_itemu = inv.u_gameobjects.Count-1;
                inv.switchu();
            }
            else{
                player.player_stat.inv.quickslot_up_indexes.Remove(item_index);
                inv.u_gameobjects.Remove(gameObject);
                in_uquick_slot = false;
            }
        }
        if(in_lquick_slot){
            transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "L";
        }
        else if(in_rquick_slot){
            transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "R";
        }
        else if(in_uquick_slot){
            transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "U";
        }
    }

    public void get_input(){
        //Debug.Log("Sda");
        wait_for_input = true;
        marker.SetActive(true);
    }

    public void ignore_input(){
        wait_for_input = false;
        marker.SetActive(false);
    }
}
