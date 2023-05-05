using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;

public static class statics
{

    public static float hit_effect_angle_range = 30f, hit_effect_period = 0.3f, range = 3f;

    public static Dictionary<string, string> item_types = new Dictionary<string, string>(){
        {"fire_cracker", "weapon"},
	    {"health_potion", "item"},
	    {"spear", "weapon"},
        {"dagger_fan", "weapon"},
        {"parry_shield", "weapon"},
        {"lightning_strike", "weapon"},
        {"spawn_bees", "weapon"}
    };


    public static IEnumerator animate_hurt(SpriteRenderer damaged_sprite)
    {
        damaged_sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        damaged_sprite.color = Color.black;
    }
    public static int search_for_item(inventory inv, string item){
        //Debug.Log(item+" "+inv.inv.FindIndex(obj => obj.Item1 == item));
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

    public static float calc_damage(stats s, damage_manager damages) {
        //Debug.Log((damages.slash/s.slash_def + damages.strike/s.strike_def +damages.pierce/s.pierce_def +damages.magic/s.mag_def).ToString());
        return damages.slash/s.slash_def + damages.strike/s.strike_def +damages.pierce/s.pierce_def +damages.magic/s.mag_def;
    }

    public static void apply_stats(damage_manager base_damage, stats modifers){
        base_damage.magic*=modifers.mag_dmg;
        base_damage.slash*=modifers.slash_dmg;
        base_damage.strike*=modifers.strike_dmg;
        base_damage.pierce*=modifers.peirce_dmg;
    }

    static public IEnumerator hit_effect(Vector3 pos, GameObject hitted_object){
        GameObject hit_image = Resources.Load<GameObject>("prefab/hit_effect");
        float rand_angle = Random.Range(90f+hit_effect_angle_range, 90f-hit_effect_angle_range);
        GameObject h1 = GameObject.Instantiate(hit_image, pos, Quaternion.Euler(0f, 0f, rand_angle));
        GameObject h2 = GameObject.Instantiate(hit_image, pos, Quaternion.Euler(0f, 0f, -rand_angle));
        h1.transform.parent = hitted_object.transform;
        h2.transform.parent = hitted_object.transform;
        float time = 0f;
        while(time<hit_effect_period){
            h1.transform.position+=new Vector3(0f, Time.deltaTime*range/hit_effect_period, 0f);
            h1.transform.localScale-=Vector3.one*Time.deltaTime/hit_effect_period;

            h2.transform.position+=new Vector3(0f, Time.deltaTime*range/hit_effect_period, 0f);
            h2.transform.localScale-=Vector3.one*Time.deltaTime/hit_effect_period;
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        UnityEngine.Object.Destroy(h1);
        UnityEngine.Object.Destroy(h2);
    }
}
