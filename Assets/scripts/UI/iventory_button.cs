using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class iventory_button : MonoBehaviour
{
    Button b;
    inventory player_items;
    player_control player;
    bool wait_for_input, in_uquick_slot, in_lquick_slot, in_rquick_slot;
    int item_index;
    GameObject item;
    public GameObject marker;
    void Start()
    {
        item = transform.GetChild(0).gameObject;
        b = gameObject.GetComponent<Button>();
        player = GameObject.Find("player").GetComponent<player_control>();
        player_items = player.player_items;

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { get_input(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry Entry = new EventTrigger.Entry();
        Entry.eventID = EventTriggerType.PointerExit;
        Entry.callback.AddListener((data) => { ignore_input(); });
        trigger.triggers.Add(Entry);

        item_index = statics.search_for_item(player_items, item.name);

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
        if(Input.GetKeyDown("e")&&player_items.inv[item_index].Item3=="weapon"&&!in_rquick_slot){
            player_items.quickslot_right_indexes.Add(item_index);
        }
        if(Input.GetKeyDown("q")&&player_items.inv[item_index].Item3=="weapon"&&!in_lquick_slot){
            player_items.quickslot_left_indexes.Add(item_index);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift)&&player_items.inv[item_index].Item3=="item"&&!in_uquick_slot){
            player_items.quickslot_up_indexes.Add(item_index);
        }
    }

    public void get_input(){
        wait_for_input = true;
        marker.SetActive(true);
    }

    public void ignore_input(){
        wait_for_input = false;
        marker.SetActive(false);
    }
}
