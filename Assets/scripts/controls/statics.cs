using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;

public static class statics
{

    public static float hit_effect_angle_range = 30f, hit_effect_period = 0.3f, range = 3f;
    public static float shrink_period = 30f;
    public static Vector3 mine_pos = new Vector3(0f, 3f, 0f);

    public static Dictionary<string, string> item_types = new Dictionary<string, string>(){
        {"fire_cracker", "weapon"},
	    {"health_potion", "item"},
	    {"spear", "weapon"},
        {"dagger_fan", "weapon"},
        {"parry_shield", "weapon"},
        {"lightning_strike", "weapon"},
        {"spawn_bees", "weapon"},
        {"glintstone", "weapon"},
        {"mine", "item"},
        {"machine_gun", "weapon"},
        {"shrink", "item"},
        {"expand", "item"},
        {"spiked_wall", "weapon"}
    };

    public static Dictionary<string, int> world_index = new Dictionary<string, int>(){
        {"start", 0},
	    {"lodge_of_voyagers", 1},
        {"tight_corridors", 2},
	    {"the_grand_staff", 3}
    };

    public static Dictionary<string, List<string>> npc_lines = new Dictionary<string, List<string>>(){
        {"Thales", npc_dialogues.thales},
        {"hari", npc_dialogues.hari},
        {"Huygens", npc_dialogues.Huygens},
        {"Kirchhoff", npc_dialogues.Kirchhoff},
        {"patches", npc_dialogues.patches},
        {"magic mirror", npc_dialogues.magic_mirror},
        {"Hermite", npc_dialogues.Hermite}
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

    public static void apply_world_details(){
        int i;
        GameObject p = GameObject.Find("player");
        player_control pc = p.GetComponent<player_control>();
        pc.current_world.current_world = SceneManager.GetActiveScene().name;
        p.GetComponent<Transform>().position = new Vector2(pc.current_world.player_pos_x, pc.current_world.player_pos_y);
        GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
        int world_num = statics.world_index[pc.current_world.current_world];
        for(i=0; i<doors.Length; i++){
            doors[i].GetComponent<doors>().num = i;
            bool closed = doors[i].GetComponent<Collider2D>().enabled;
            if(i>=pc.current_world.opened_doors[world_num].Count) {
                Debug.Log("world num: "+world_num.ToString()+" num: "+doors[i].GetComponent<doors>().num.ToString());
                pc.current_world.opened_doors[world_num].Add(!closed);
            }
            //if door is closed but saved world detail say it's open, then open it
            if(closed&&pc.current_world.opened_doors[world_num][i]){
                doors[i].SetActive(false);
            }
        }
    }

    public static IEnumerator load_new_world(string world_name, world_details world, stats player_stat, GameObject loader_object = null){
        world.current_world = world_name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(world_name);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //Debug.Log(player_stat.inv.inv.Count.ToString());
        // while(true){
        //     GameObject player = GameObject.Find("player");
        //     if(player==null&&cloned_player!=null) {
        //         GameObject.Instantiate(cloned_player, Vector3.zero, Quaternion.identity);
        //         break;
        //     }
        //     if(player!=cloned_player) {
        //         UnityEngine.Object.Destroy(player);
        //         break;
        //     }
        // }
        UnityEngine.Object.Destroy(GameObject.Find("old_player"));
        if(loader_object!=null) UnityEngine.Object.Destroy(loader_object);
        player_control p = GameObject.Find("player").GetComponent<player_control>();
        p.player_stat = player_stat;
        p.unbuffed_player_stat = player_stat;
        p.init();
        GameObject inventory_content =  GameObject.Find("inventory_content");
        //Debug.Log(inventory_content.name);
        inventory_content.GetComponent<inventory_manager>().refresh_inv_menu();
        p.current_world = world;
        Debug.Log("finished loading scene?");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(world_name));
        asyncLoad.allowSceneActivation = true;
        apply_world_details();
    }

    public static float calc_damage(stats s, damage_manager damages) {
        if(!damages) return 0f;
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
    
    public static IEnumerator expand(Transform object_to_expand, float expand_period, Vector3 end_size){
        float time = 0f;
        Vector3 init_size = object_to_expand.localScale;
        while(time<expand_period){
            object_to_expand.localScale+=(end_size-init_size)*Time.deltaTime/expand_period;
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        object_to_expand.localScale = end_size;
    }

    public static IEnumerator spawn_mine(Vector3 player_pos, stats player_stat){
        float period = 3f;
        GameObject mines = Resources.Load<GameObject>("prefab/mine");
        GameObject m = GameObject.Instantiate(mines, player_pos+mine_pos, Quaternion.identity);
        statics.apply_stats(m.GetComponent<damage_manager>(), m.GetComponent<damage_manager>(), player_stat);
        Collider2D c = m.GetComponent<Collider2D>();
        c.enabled = false;
        m.transform.localScale = Vector3.zero;
        float time = 0f;
        while(time<period){
            m.transform.localScale+=Vector3.one*Time.deltaTime/period;
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        c.enabled = true;
    }
}
