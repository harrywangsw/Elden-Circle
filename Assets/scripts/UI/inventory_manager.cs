using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;

public class inventory_manager : MonoBehaviour
{
    public GameObject row, last_slot, up, right, left, empty_slot;
    player_control p;
    public int i, current_itemr, current_iteml, current_itemu;
    public List<GameObject> u_gameobjects = new List<GameObject>(), l_gameobjects = new List<GameObject>(), r_gameobjects = new List<GameObject>();

    void Start()
    {
        p = GameObject.Find("player").GetComponent<player_control>();
        row = Resources.Load<GameObject>("prefab/row");
        up = GameObject.Find("up");
        right = GameObject.Find("right");
        left = GameObject.Find("left");
        last_slot = transform.GetChild(0).GetChild(0).gameObject;
        empty_slot = Resources.Load<GameObject>("prefab/slot");
        u_gameobjects.Add(empty_slot);
        l_gameobjects.Add(empty_slot);
        r_gameobjects.Add(empty_slot);
    }

    public void refresh_inv_menu(){
        //Debug.Log(p.unbuffed_player_stat.inv.inv.Count.ToString());
        for (i = 0; i<p.unbuffed_player_stat.inv.inv.Count; i++)
        {
            if(p.unbuffed_player_stat.inv.inv[i].num_left==0) continue;
            GameObject it = Resources.Load<GameObject>("prefab/UI_items/"+p.unbuffed_player_stat.inv.inv[i].item_name);
            add_item(it, p.unbuffed_player_stat.inv.inv[i].num_left);

            //quickslot_up/down/left/right_indexes are the indexes of inv that represent the items in each respective quickslot
            //u/d/l/r_gameobjects are the gameobject for each item
            if(p.unbuffed_player_stat.inv.quickslot_up_indexes.FindIndex(x=>x==i)>=0){
                u_gameobjects.Add(it);
            }
            if(p.unbuffed_player_stat.inv.quickslot_right_indexes.FindIndex(x=>x==i)>=0){
                r_gameobjects.Add(it);
            }
            if(p.unbuffed_player_stat.inv.quickslot_left_indexes.FindIndex(x=>x==i)>=0){
                l_gameobjects.Add(it);
            }

            if(r_gameobjects.Count>0){
                switchr();
            }
            if(l_gameobjects.Count>0){
                switchl();
            }
            if(u_gameobjects.Count>0){
                switchu();
            }
        }
    }

    public void switchr(){
        GameObject slot = right;
        Destroy(slot.transform.GetChild(0).gameObject);
        if(current_itemr<0) return;
        GameObject weapon = GameObject.Instantiate(r_gameobjects[current_itemr], slot.transform);
        weapon.GetComponent<RectTransform>().localPosition = Vector3.zero;
        Destroy(weapon.transform.GetChild(0).gameObject);
        Destroy(weapon.transform.GetChild(1).gameObject);
        Destroy(weapon.transform.GetChild(2).gameObject);
        p.update_weapon(Resources.Load<GameObject>("weapons/"+r_gameobjects[current_itemr].name), null);
    }

    public void switchl(){
        GameObject slot = left;
        Destroy(slot.transform.GetChild(0).gameObject);
        if(current_iteml<0) return;
        GameObject weapon = GameObject.Instantiate(l_gameobjects[current_iteml], slot.transform);
        weapon.GetComponent<RectTransform>().localPosition = Vector3.zero;
        Destroy(weapon.transform.GetChild(0).gameObject);
        Destroy(weapon.transform.GetChild(1).gameObject);
        Destroy(weapon.transform.GetChild(2).gameObject);
        p.update_weapon(null, Resources.Load<GameObject>("weapons/"+l_gameobjects[current_iteml].name));
    }

    public void switchu(){
        //Debug.Log(current_itemu.ToString());
        GameObject slot = up;
        Destroy(slot.transform.GetChild(0).gameObject);
        if(current_itemu<0) return;
        GameObject item = GameObject.Instantiate(u_gameobjects[current_itemu], slot.transform);
        item.GetComponent<RectTransform>().localPosition = Vector3.zero;
        item.GetComponent<iventory_button>().enabled = false;
        Destroy(item.transform.GetChild(0).gameObject);
        Destroy(item.transform.GetChild(2).gameObject);
    }

    public void switch_quickslot_item(){
        if(!p.attacking&&Input.GetKey(KeyCode.LeftShift)&&Input.mouseScrollDelta.y!=0f&&r_gameobjects.Count>0){
            if(Input.mouseScrollDelta.y>0f) current_itemr = (current_itemr+1)%r_gameobjects.Count;
            else if(current_itemr==0){
                current_itemr = r_gameobjects.Count;
                current_itemr = (current_itemr-1)%r_gameobjects.Count;
            }
            else{
                current_itemr = (current_itemr-1)%r_gameobjects.Count;
            }
            switchr();
        }

        if(Input.mouseScrollDelta.y!=0f&&u_gameobjects.Count>0){
            if(Input.mouseScrollDelta.y>0f) current_itemu = (current_itemu+1)%u_gameobjects.Count;
            else if(current_itemu==0){
                current_itemu = u_gameobjects.Count;
                current_itemu = (current_itemu-1)%u_gameobjects.Count;
            }
            else{
                current_itemu = (current_itemu-1)%u_gameobjects.Count;
            }
            switchu();
        }

        if(!p.using_item&&!p.attacking&&!p.dashing&&Input.GetKeyDown("e")&&u_gameobjects.Count>0){
            p.use_item(u_gameobjects[current_itemu].name);
            int ind = p.unbuffed_player_stat.inv.inv.FindIndex(obj=>obj.item_name==u_gameobjects[current_itemu].name);
            p.unbuffed_player_stat.inv.inv[ind] = new item(p.unbuffed_player_stat.inv.inv[ind].item_name, p.unbuffed_player_stat.inv.inv[ind].num_left-1, p.unbuffed_player_stat.inv.inv[ind].item_type);
            int num_left = p.unbuffed_player_stat.inv.inv[ind].num_left;
            u_gameobjects[current_itemu].transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = num_left.ToString();
            up.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = num_left.ToString();
            if(p.unbuffed_player_stat.inv.inv[ind].num_left==0) {
                remove_item_from_quickslot(u_gameobjects[current_itemu]);
            }
        }
        
        // GameObject item;
        // //player_item.quickslot_up is the index of u_gameobject and quickslot_up_items
        // if(u_gameobjects.Count>0){
        //     Transform slot = up.transform;
        //     item = GameObject.Instantiate(u_gameobjects[unbuffed_player_stat.inv.quickslot_up], slot);
        //     item.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = unbuffed_player_stat.inv.inv[unbuffed_player_stat.inv.quickslot_up_indexes[unbuffed_player_stat.inv.quickslot_up]].num_left.ToString();
        // }
        // if(l_gameobjects.Count>0){
        //     Transform slot = GameObject.Find("left").transform;
        //     item = GameObject.Instantiate(l_gameobjects[unbuffed_player_stat.inv.quickslot_left], slot);            
        // }
        // if(r_gameobjects.Count>0){
        //     Transform slot = GameObject.Find("right").transform;
        //     item = GameObject.Instantiate(r_gameobjects[unbuffed_player_stat.inv.quickslot_right], slot);
        // }
    }

    void Update()
    {
        if(transform.parent.parent.parent.parent.localScale == Vector3.zero) switch_quickslot_item();
    }

    //add an item into the inventory menu and hope that it has already been added to unbuffed_player_stat.inv
    public void add_item(GameObject item, int num){
        Transform tran = last_slot.transform;
        tran.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = num.ToString();
        GameObject item_inserted = GameObject.Instantiate(item, tran);
        item_inserted.name = item_inserted.name.Replace("(Clone)", "");
        tran.gameObject.name = item_inserted.name;
        tran.gameObject.GetComponent<iventory_button>().enabled = true;
        tran.gameObject.GetComponent<iventory_button>().item = item_inserted;
        tran.gameObject.GetComponent<iventory_button>().item_index = statics.search_for_item(p.unbuffed_player_stat.inv, item_inserted.name);
        if(tran.GetSiblingIndex()+1< tran.parent.childCount) last_slot = tran.parent.GetChild(tran.GetSiblingIndex() + 1).gameObject;
        else{
            GameObject new_row = GameObject.Instantiate(row, transform);
            last_slot = new_row.transform.GetChild(0).gameObject;
        }
    }

    public void remove_item_from_quickslot(GameObject item){
        u_gameobjects.Remove(item);
        if(current_itemu>=u_gameobjects.Count) current_itemu--;
        switchu();
    }
}
