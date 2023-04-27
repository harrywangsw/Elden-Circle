using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class iventory_button : MonoBehaviour
{
    Button b;
    inventory player_items;
    player_control player;
    bool wait_for_input, in_uquick_slot, in_lquick_slot, in_rquick_slot;
    public int item_index;
    public GameObject marker, item;
    inventory_manager inv;
    void Start()
    {
        b = gameObject.GetComponent<Button>();
        player = GameObject.Find("player").GetComponent<player_control>();
        player_items = player.player_stat.inv;
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

        int a = player_items.quickslot_up_indexes.FindIndex(obj => obj == item_index);
        in_uquick_slot = a>=0;

        a = player_items.quickslot_right_indexes.FindIndex(obj => obj == item_index);
        in_rquick_slot = a>=0;
        
        a = player_items.quickslot_left_indexes.FindIndex(obj => obj == item_index);
        in_lquick_slot = a>=0;       
    }

    
    void Update()
    {
        if(!wait_for_input) return;
        if(Input.GetKeyDown(KeyCode.LeftShift)&&player_items.inv[item_index].Item3=="weapon"&&!in_rquick_slot){
            //Debug.Log("addpls");
            player_items.quickslot_right_indexes.Add(item_index);
            in_rquick_slot = true;
            in_lquick_slot = false;
            inv.r_gameobjects.Add(gameObject);
            inv.current_itemr = inv.r_gameobjects.Count-1;
            inv.switchr();
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt)&&player_items.inv[item_index].Item3=="weapon"&&!in_lquick_slot){
            player_items.quickslot_left_indexes.Add(item_index);
            in_rquick_slot = false;
            in_lquick_slot = true;
            inv.l_gameobjects.Add(gameObject);
            inv.current_iteml = inv.l_gameobjects.Count-1;
            inv.switchl();
        }
        if(Input.GetKeyDown("e")&&player_items.inv[item_index].Item3=="item"&&!in_uquick_slot){
            player_items.quickslot_up_indexes.Add(item_index);
            inv.u_gameobjects.Add(gameObject);
            inv.current_itemu = inv.u_gameobjects.Count-1;
            inv.switchu();
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
