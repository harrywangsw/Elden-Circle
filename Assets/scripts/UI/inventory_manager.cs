using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class inventory_manager : MonoBehaviour
{
    public inventory player_items;
    GameObject row, last_slot;
    int i;
    void Start()
    {
        row = Resources.Load<GameObject>("prefab/row");
        player_items = GameObject.Find("player").GetComponent<player_control>().player_stat.inv;
        last_slot = transform.GetChild(0).GetChild(0).gameObject;
        for (i = 0; i<player_items.inv.Count; i++)
        {
            if(player_items.inv[i].Item2==0) continue;
            GameObject it = Resources.Load<GameObject>("prefab/UI_items/"+player_items.inv[i].Item1);
            it.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = player_items.inv[i].Item2.ToString();
            add_item(it);
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

    //add an item into the inventory menu and hope that it has already been added to player_stat.inv
    public void add_item(GameObject item){
        Transform tran = last_slot.transform;
        GameObject item_inserted = GameObject.Instantiate(item, tran);
        item_inserted.name = item_inserted.name.Replace("(Clone)", "");
        tran.gameObject.GetComponent<iventory_button>().item = item_inserted;
        //player_items = GameObject.Find("player").GetComponent<player_control>().player_stat.inv;
        tran.gameObject.GetComponent<iventory_button>().item_index = statics.search_for_item(player_items, item_inserted.name);
        if(tran.GetSiblingIndex()+1< tran.parent.childCount) last_slot = tran.parent.GetChild(tran.GetSiblingIndex() + 1).gameObject;
        else{
            GameObject new_row = GameObject.Instantiate(row, transform);
            last_slot = new_row.transform.GetChild(0).gameObject;
        }
    }
}
