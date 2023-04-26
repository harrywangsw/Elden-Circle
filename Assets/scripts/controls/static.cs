using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class statics
{
    public static Dictionary<string, string> item_types = new Dictionary<string, string>(){
        {"fire_cracker", "weapon"},
	    {"health_potion", "item"},
	    {"spear", "weapon"},
        {"dagger_fan", "weapon"},
        {"parry_shield", "weapon"},
    };

    public static Vector2 rotate(Vector2 original, float angle){
        return new Vector2(original.x*Mathf.Cos(angle)-original.y*Mathf.Sin(angle), original.x*Mathf.Sin(angle)+original.y*Mathf.Cos(angle));
    }
    public static bool out_of_bound(Vector3 pos){
        return(Physics.Linecast(pos, Vector3.down));
    }
    public static IEnumerator animate_hurt(SpriteRenderer damaged_sprite)
    {
        damaged_sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        damaged_sprite.color = Color.black;
    }
    public static int search_for_item(inventory inv, string item){
        Debug.Log(item+" "+inv.inv.FindIndex(obj => obj.Item1 == item));
        return inv.inv.FindIndex(obj => obj.Item1 == item);
    }

    public static void apply_world_details(world_details w){
        int i;
        GameObject p = GameObject.Find("player");
        p.GetComponent<Transform>().position = new Vector2(w.player_pos_x, w.player_pos_y);
        GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
        for(i=0; i<doors.Length; i++){
            doors[i].GetComponent<doors>().num = i;
            bool closed = doors[i].GetComponent<Collider2D>().enabled;
            if(i>=w.opened_doors.Count) w.opened_doors.Add(!closed);
            //if door is closed but saved world detail say it's open, then open it
            if(closed&&w.opened_doors[i]){
                doors[i].GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
