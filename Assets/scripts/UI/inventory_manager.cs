using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class inventory_manager : MonoBehaviour
{
    inventory player_items;
    GameObject row, last_slot;
    int i;
    void Start()
    {
        row = Resources.Load<GameObject>("prefab/row");
        player_items = GameObject.Find("player").GetComponent<player_control>().player_items;
        last_slot = transform.GetChild(0).GetChild(0).gameObject;
        for (i = 0; i<player_items.inv.Count; i++)
        {
            if(player_items.inv[i].Item2==0) continue;
            add_item(Resources.Load<GameObject>("prefab/UI_items/"+player_items.inv[i].Item1));
        }
    }

    void Update()
    {
        // if(player_items.inv[^1]!=last_item){
        //     //add the newly picked-up item to the inventory screen
        //     GameObject item = Resources.Load<GameObject>("prefab/UI_items"+player_items.inv[^1].Item1);
        //     if(transform.GetChild(transform.childCount-1).GetChild(4).GetChild(0)!=null){
        //         GameObject.Instantiate(row, transform);
        //     }
        //     GameObject.Instantiate(item, slots[i/5].transform.GetChild(column));
        // }
    }

    public void add_item(GameObject item){
        Transform tran = last_slot.transform;
        GameObject.Instantiate(item, tran);
        if(tran.GetSiblingIndex()+1< tran.parent.childCount) last_slot = tran.parent.GetChild(tran.GetSiblingIndex() + 1).gameObject;
        else{
            GameObject new_row = GameObject.Instantiate(row, transform);
            last_slot = new_row.transform.GetChild(0).gameObject;
        }
    }
}
