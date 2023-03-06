using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class make_new_player : MonoBehaviour
{
    public stats new_stat;
    List<edit_stat> stat_changers;
    void Start()
    {
        new_stat = new stats();
        stat_changers = new List<edit_stat>();
        transform.GetChild(transform.childCount - 1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{finish_creation();});
    }
    
    void Update()
    {
        new_stat.health = transform.GetChild(0).GetChild(1).gameObject.GetComponent<edit_stat>().stat;
        new_stat.strike_def = transform.GetChild(1).GetChild(1).gameObject.GetComponent<edit_stat>().stat;
        new_stat.slash_def = transform.GetChild(2).GetChild(1).gameObject.GetComponent<edit_stat>().stat;
        new_stat.pierce_def = transform.GetChild(3).GetChild(1).gameObject.GetComponent<edit_stat>().stat;
        new_stat.dash_modifier = transform.GetChild(4).GetChild(1).gameObject.GetComponent<edit_stat>().stat;
        new_stat.strike_dmg = transform.GetChild(5).GetChild(1).gameObject.GetComponent<edit_stat>().stat;
        new_stat.slash_dmg = transform.GetChild(6).GetChild(1).gameObject.GetComponent<edit_stat>().stat;
        new_stat.peirce_dmg = transform.GetChild(7).GetChild(1).gameObject.GetComponent<edit_stat>().stat;
        new_stat.mag_dmg = transform.GetChild(8).GetChild(1).gameObject.GetComponent<edit_stat>().stat;
    }

    public void finish_creation(){
        save_load.SavePlayer(new_stat);
        main_menu m = transform.parent.gameObject.GetComponent<main_menu>();
        m.worlds.Add(new world_details());
        m.stat.Add(new stats());
        m.inventorys.Add(new inventory());
        StartCoroutine(m.load_game(m.worlds.Count-1));
    }
}
