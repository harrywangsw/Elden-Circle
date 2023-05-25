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
    public GameObject marker, item, item_description, item_menu;
    TMPro.TextMeshProUGUI description_text, UI_control;
    inventory_manager inv;
    void Start()
    {
        item_menu = GameObject.Find("item_menu");
        UI_control = GameObject.Find("UI_control").GetComponent<TMPro.TextMeshProUGUI>();
        item_description = GameObject.Find("item_description");
        description_text = item_description.GetComponent<TMPro.TextMeshProUGUI>();
        b = gameObject.GetComponent<Button>();
        player = GameObject.Find("player").GetComponent<player_control>();
        player.unbuffed_player_stat.inv = player.unbuffed_player_stat.inv;
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

        int a = player.unbuffed_player_stat.inv.quickslot_up_indexes.FindIndex(obj => obj == item_index);
        in_uquick_slot = a>=0;

        a = player.unbuffed_player_stat.inv.quickslot_right_indexes.FindIndex(obj => obj == item_index);
        in_rquick_slot = a>=0;
        
        a = player.unbuffed_player_stat.inv.quickslot_left_indexes.FindIndex(obj => obj == item_index);
        in_lquick_slot = a>=0;       
    }

    
    void Update()
    {
        if(item_menu.transform.localScale == Vector3.zero) ignore_input();
        if(transform.childCount>1) transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = player.unbuffed_player_stat.inv.inv[item_index].num_left.ToString();
        if(!wait_for_input) return;
        if(player.unbuffed_player_stat.inv.inv[item_index].item_type=="weapon"){
            UI_control.text = "LeftShift/LeftCtrl to insert item into right/left quickslot.";
        }
        else if(player.unbuffed_player_stat.inv.inv[item_index].item_type=="item"){
            UI_control.text = "e to insert item into upper quickslot.";
        }
        //if(item!=null) item_index = statics.search_for_item(player.unbuffed_player_stat.inv, item.name);
        if(Input.GetAxisRaw("xboxdpadhori")>0.75f&&player.unbuffed_player_stat.inv.inv[item_index].item_type=="weapon"){
            //Debug.Log("addpls");
            if(!in_rquick_slot){
                player.unbuffed_player_stat.inv.quickslot_right_indexes.Add(item_index);
                in_rquick_slot = true;
                inv.r_gameobjects.Add(gameObject);
                inv.current_itemr = inv.r_gameobjects.Count-1;
                inv.switchr();
            }
            else{
                player.unbuffed_player_stat.inv.quickslot_right_indexes.Remove(item_index);
                in_rquick_slot = false;
                inv.r_gameobjects.Remove(gameObject);
            }
        }
        if(Input.GetAxisRaw("xboxdpadhori")<-0.75f&&player.unbuffed_player_stat.inv.inv[item_index].item_type=="weapon"){
            if(!in_lquick_slot){
                player.unbuffed_player_stat.inv.quickslot_left_indexes.Add(item_index);
                in_rquick_slot = false;
                in_lquick_slot = true;
                inv.l_gameobjects.Add(gameObject);
                inv.current_iteml = inv.l_gameobjects.Count-1;
                inv.switchl();
            }
            else{
                player.unbuffed_player_stat.inv.quickslot_left_indexes.Remove(item_index);
                in_lquick_slot = false;
                inv.l_gameobjects.Remove(gameObject);
            }
        }
        if(Input.GetAxisRaw("xboxdpadverti")>0.75f&&player.unbuffed_player_stat.inv.inv[item_index].item_type=="item"&&player.unbuffed_player_stat.inv.inv[item_index].num_left>0){
            if(!in_uquick_slot){
                player.unbuffed_player_stat.inv.quickslot_up_indexes.Add(item_index);
                inv.u_gameobjects.Add(gameObject);
                in_uquick_slot = true;
                inv.current_itemu = inv.u_gameobjects.Count-1;
                inv.switchu();
            }
            else{
                player.unbuffed_player_stat.inv.quickslot_up_indexes.Remove(item_index);
                inv.remove_item_from_quickslot(gameObject);
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
        else{
            transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
    }

    public void get_input(){
        //Debug.Log("Sda");
        string text = "<b>"+gameObject.name.Replace('_', ' ')+"</b>\n\n"+item_descriptions.des[gameObject.name]+"\n";
        if(player.unbuffed_player_stat.inv.inv[item_index].item_type=="weapon"){
            damage_manager d = Resources.Load<GameObject>("weapons/"+gameObject.name).GetComponent<damage_manager>();
            statics.apply_stats(d, d, player.unbuffed_player_stat);
            text+=@"
slash damage: "+d.slash.ToString()+@"
strike damage: "+d.strike.ToString()+@"
pierce damage: "+d.pierce.ToString()+@"
magic damage: "+d.magic.ToString();
        }
        description_text.text = text;
        wait_for_input = true;
        marker.SetActive(true);

    }

    public void ignore_input(){
        wait_for_input = false;
        marker.SetActive(false);
    }
}
