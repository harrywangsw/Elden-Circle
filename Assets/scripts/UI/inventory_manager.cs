using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;

public class inventory_manager : MonoBehaviour
{
    public GameObject row, last_slot, up, right, left;
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
        refresh_inv_menu();
    }

    public void refresh_inv_menu(){
        last_slot = transform.GetChild(0).GetChild(0).gameObject;
        for (i = 0; i<p.player_stat.inv.inv.Count; i++)
        {
            if(p.player_stat.inv.inv[i].Item2==0) continue;
            GameObject it = Resources.Load<GameObject>("prefab/UI_items/"+p.player_stat.inv.inv[i].Item1);
            add_item(it, p.player_stat.inv.inv[i].Item2);

            //quickslot_up/down/left/right_indexes are the indexes of inv that represent the items in each respective quickslot
            //u/d/l/r_gameobjects are the gameobject for each item
            if(p.player_stat.inv.quickslot_up_indexes.FindIndex(x=>x==i)>=0){
                u_gameobjects.Add(it);
            }
            if(p.player_stat.inv.quickslot_right_indexes.FindIndex(x=>x==i)>=0){
                r_gameobjects.Add(it);
            }
            if(p.player_stat.inv.quickslot_left_indexes.FindIndex(x=>x==i)>=0){
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
        GameObject weapon = GameObject.Instantiate(l_gameobjects[current_iteml], slot.transform);
        weapon.GetComponent<RectTransform>().localPosition = Vector3.zero;
        Destroy(weapon.transform.GetChild(0).gameObject);
        Destroy(weapon.transform.GetChild(1).gameObject);
        Destroy(weapon.transform.GetChild(2).gameObject);
        p.update_weapon(null, Resources.Load<GameObject>("weapons/"+l_gameobjects[current_iteml].name));
    }

    public void switchu(){
        GameObject slot = up;
        Destroy(slot.transform.GetChild(0).gameObject);
        GameObject weapon = GameObject.Instantiate(u_gameobjects[current_itemu], slot.transform);
        weapon.GetComponent<RectTransform>().localPosition = Vector3.zero;
        Destroy(weapon.transform.GetChild(0).gameObject);
        Destroy(weapon.transform.GetChild(2).gameObject);
    }

    public void switch_quickslot_item(){
        if(!p.attacking&&Input.GetKeyDown(KeyCode.LeftShift)&&r_gameobjects.Count>0){
            current_itemr+=1;
            current_itemr = current_itemr%r_gameobjects.Count;
            switchr();
        }

        if(!p.attacking&&!p.dashing&&Input.GetKeyDown("e")&&u_gameobjects.Count>0){
            p.use_item(u_gameobjects[current_itemu].name);
            int ind = p.player_stat.inv.inv.FindIndex(obj=>obj.Item1==u_gameobjects[current_itemu].name);
            p.player_stat.inv.inv[ind] = Tuple.Create(p.player_stat.inv.inv[ind].Item1, p.player_stat.inv.inv[ind].Item2-1, p.player_stat.inv.inv[ind].Item3);
            int num_left = p.player_stat.inv.inv[ind].Item2;
            u_gameobjects[current_itemu].transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = num_left.ToString();
            up.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = num_left.ToString();
            if(p.player_stat.inv.inv[ind].Item2==0) {
                p.player_stat.inv.inv.RemoveAt(ind);
                Destroy(u_gameobjects[current_itemu]);
                u_gameobjects.RemoveAt(current_itemu);
            }
        }
        
        // GameObject item;
        // //player_item.quickslot_up is the index of u_gameobject and quickslot_up_items
        // if(u_gameobjects.Count>0){
        //     Transform slot = up.transform;
        //     item = GameObject.Instantiate(u_gameobjects[player_stat.inv.quickslot_up], slot);
        //     item.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = player_stat.inv.inv[player_stat.inv.quickslot_up_indexes[player_stat.inv.quickslot_up]].Item2.ToString();
        // }
        // if(l_gameobjects.Count>0){
        //     Transform slot = GameObject.Find("left").transform;
        //     item = GameObject.Instantiate(l_gameobjects[player_stat.inv.quickslot_left], slot);            
        // }
        // if(r_gameobjects.Count>0){
        //     Transform slot = GameObject.Find("right").transform;
        //     item = GameObject.Instantiate(r_gameobjects[player_stat.inv.quickslot_right], slot);
        // }
    }

    void Update()
    {
        p.player_stat.inv = p.player_stat.inv;
        if(transform.parent.parent.parent.parent.localScale == Vector3.zero) switch_quickslot_item();
    }

    //add an item into the inventory menu and hope that it has already been added to player_stat.inv
    public void add_item(GameObject item, int num){
        p.player_stat.inv.inv.Add(Tuple.Create(item.name, 1, statics.item_types[item.name]));
        Transform tran = last_slot.transform;
        tran.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = num.ToString();
        GameObject item_inserted = GameObject.Instantiate(item, tran);
        item_inserted.name = item_inserted.name.Replace("(Clone)", "");
        tran.gameObject.name = item_inserted.name;
        tran.gameObject.GetComponent<iventory_button>().item = item_inserted;
        tran.gameObject.GetComponent<iventory_button>().item_index = statics.search_for_item(p.player_stat.inv, item_inserted.name);
        if(tran.GetSiblingIndex()+1< tran.parent.childCount) last_slot = tran.parent.GetChild(tran.GetSiblingIndex() + 1).gameObject;
        else{
            GameObject new_row = GameObject.Instantiate(row, transform);
            last_slot = new_row.transform.GetChild(0).gameObject;
        }
    }
}
