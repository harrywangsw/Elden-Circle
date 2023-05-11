using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;

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
        {"spawn_bees", "weapon"},
        {"glintstone", "weapon"},
        {"mine", "weapon"}
    };

    public static Dictionary<string, int> world_index = new Dictionary<string, int>(){
        {"start", 0},
	    {"lodge_of_voyagers", 1},
    };

    public static Dictionary<string, List<string>> npc_lines = new Dictionary<string, List<string>>(){
        {"Thales", npc_dialogues.thales},
        {"Hali", npc_dialogues.hali},
    };


    public static IEnumerator animate_hurt(SpriteRenderer damaged_sprite)
    {
        damaged_sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        damaged_sprite.color = Color.black;
    }
    public static int search_for_item(inventory inv, string item){
        //Debug.Log(item+" "+inv.inv.FindIndex(obj => obj.item_name == item));
        return inv.inv.FindIndex(obj => obj.item_name == item);
    }

    public static void apply_world_details(world_details w){
        int i;
        int world_num = statics.world_index[w.current_world];
        //Debug.Log(w.opened_doors.Count);
        GameObject p = GameObject.Find("player");
        p.GetComponent<Transform>().position = new Vector2(w.player_pos_x, w.player_pos_y);
        GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
        for(i=0; i<doors.Length; i++){
            doors[i].GetComponent<doors>().num = i;
            bool closed = doors[i].GetComponent<Collider2D>().enabled;
            if(i>=w.opened_doors[world_num].Count) w.opened_doors[world_num].Add(!closed);
            //if door is closed but saved world detail say it's open, then open it
            if(closed&&w.opened_doors[world_num][i]){
                doors[i].GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public static IEnumerator load_new_world(string world_name, world_details world, stats player_stat, GameObject loader_object, GameObject cloned_player = null){
        Debug.Log(player_stat.inv.inv.Count.ToString());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(world_name);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameObject player = GameObject.Find("player");
        if(player==null&&cloned_player!=null) GameObject.Instantiate(cloned_player, Vector3.zero, Quaternion.identity);
        player_control p = GameObject.Find("player").GetComponent<player_control>();
        p.player_stat = player_stat;
        p.update_stats();
        GameObject inventory_content =  GameObject.Find("inventory_content");
        //Debug.Log(inventory_content.name);
        inventory_content.GetComponent<inventory_manager>().refresh_inv_menu();
        apply_world_details(world);
        Debug.Log("finished loading scene?");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(world_name));
        asyncLoad.allowSceneActivation = true;
        UnityEngine.Object.Destroy(loader_object);
    }

    public static float calc_damage(stats s, damage_manager damages) {
        //Debug.Log((damages.slash/s.slash_def + damages.strike/s.strike_def +damages.pierce/s.pierce_def +damages.magic/s.mag_def).ToString());
        return damages.slash/s.slash_def + damages.strike/s.strike_def +damages.pierce/s.pierce_def +damages.magic/s.mag_def;
    }

    // used to apply damage from stats and apply damage from a weapon to the things it spawns
    public static void apply_stats(damage_manager base_damage, damage_manager damage_to_be_changed, stats modifers){
        damage_to_be_changed.magic = base_damage.magic*modifers.mag_dmg;
        damage_to_be_changed.slash = base_damage.slash*modifers.slash_dmg;
        damage_to_be_changed.strike = base_damage.strike*modifers.strike_dmg;
        damage_to_be_changed.pierce = base_damage.pierce*modifers.peirce_dmg;
    }

    public static IEnumerator hit_effect(Vector3 pos, GameObject hitted_object){
        foreach (Transform child in hitted_object.transform){
            if(child.gameObject.name=="hit_effect") yield break;
        }
        GameObject hit_image = Resources.Load<GameObject>("prefab/hit_effect");
        float rand_angle = Random.Range(90f+hit_effect_angle_range, 90f-hit_effect_angle_range);
        GameObject h1 = GameObject.Instantiate(hit_image, pos, Quaternion.Euler(0f, 0f, rand_angle));
        GameObject h2 = GameObject.Instantiate(hit_image, pos, Quaternion.Euler(0f, 0f, -rand_angle));
        h1.name = "hit_effect";
        h2.name = "hit_effect";
        h1.transform.parent = hitted_object.transform;
        h2.transform.parent = hitted_object.transform;
        float time = 0f;
        while(time<hit_effect_period){
            //h1.transform.localPosition+=new Vector3(0f, Time.deltaTime*range/hit_effect_period, 0f);
            h1.transform.localScale-=Vector3.one*Time.deltaTime/hit_effect_period;

            //h2.transform.localPosition+=new Vector3(0f, Time.deltaTime*range/hit_effect_period, 0f);
            h2.transform.localScale-=Vector3.one*Time.deltaTime/hit_effect_period;
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        UnityEngine.Object.Destroy(h1);
        UnityEngine.Object.Destroy(h2);
    }
}
