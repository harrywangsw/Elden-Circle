using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class make_new_player : MonoBehaviour
{
    public GameObject level_indicator;
    player_control p;
    public bool listener_added, level_down_listener_added;
    //this is the only place that can alter unaltered_unbuffed_player_stat
    void Start()
    {
        transform.GetChild(transform.childCount - 1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{finish_creation();});
        p = GameObject.Find("player").GetComponent<player_control>();
        transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: "+p.unbuffed_player_stat.level.ToString();
        transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Exp: "+p.unbuffed_player_stat.exp.ToString();
        //it's ok to addlisteners for level down here because they'll get canceled later
        //add the stats to the textmeshproguis
        level_down();
    }

    public void on_edit(int up_down){
        remove_status();
        p.unbuffed_player_stat.exp+=100*up_down;
        p.unbuffed_player_stat.level-=up_down;
        transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: "+p.unbuffed_player_stat.level.ToString();
        transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Exp: "+p.unbuffed_player_stat.exp.ToString();
        p.player_stat = p.unbuffed_player_stat;
    }

    public void remove_status(){
        p.player_stat = p.unbuffed_player_stat;
        //remove status icons??
    }

    public void level_up(){
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: "+p.unbuffed_player_stat.health.ToString();
        transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.health+=10f;
            transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: "+p.unbuffed_player_stat.health.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        });

        transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Defence: "+p.unbuffed_player_stat.strike_def.ToString();
        transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.strike_def+=0.1f;
            transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Defence: "+p.unbuffed_player_stat.strike_def.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        });   

        transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Defence: "+p.unbuffed_player_stat.slash_def.ToString();
        transform.GetChild(0).GetChild(2).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.slash_def+=0.1f;
            transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Defence: "+p.unbuffed_player_stat.slash_def.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        }); 

        transform.GetChild(0).GetChild(3).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Defence: "+p.unbuffed_player_stat.pierce_def.ToString();
        transform.GetChild(0).GetChild(3).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.pierce_def+=0.1f;
            transform.GetChild(0).GetChild(3).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Defence: "+p.unbuffed_player_stat.pierce_def.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        }); 

        transform.GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Defence: "+p.unbuffed_player_stat.mag_def.ToString();
        transform.GetChild(0).GetChild(4).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.mag_def+=0.1f;
            transform.GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Defence: "+p.unbuffed_player_stat.mag_def.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        });

        transform.GetChild(0).GetChild(5).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Damage: "+p.unbuffed_player_stat.mag_dmg.ToString();
        transform.GetChild(0).GetChild(5).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.mag_dmg+=0.1f;
            transform.GetChild(0).GetChild(5).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Damage: "+p.unbuffed_player_stat.mag_dmg.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        });

        transform.GetChild(0).GetChild(6).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Damage: "+p.unbuffed_player_stat.strike_dmg.ToString();
        transform.GetChild(0).GetChild(6).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.strike_dmg+=0.1f;
            transform.GetChild(0).GetChild(6).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Damage: "+p.unbuffed_player_stat.strike_dmg.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        });


        transform.GetChild(0).GetChild(7).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Damage: "+p.unbuffed_player_stat.slash_dmg.ToString();
        transform.GetChild(0).GetChild(7).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.slash_dmg+=0.1f;
            transform.GetChild(0).GetChild(7).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Damage: "+p.unbuffed_player_stat.slash_dmg.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        });

        transform.GetChild(0).GetChild(8).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Damage: "+p.unbuffed_player_stat.peirce_dmg.ToString();
        transform.GetChild(0).GetChild(8).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.peirce_dmg+=0.1f;
            transform.GetChild(0).GetChild(8).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Damage: "+p.unbuffed_player_stat.peirce_dmg.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        });


        transform.GetChild(0).GetChild(9).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Speed: "+p.unbuffed_player_stat.spd.ToString();
        transform.GetChild(0).GetChild(9).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.spd+=0.5f;
            transform.GetChild(0).GetChild(9).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Speed: "+p.unbuffed_player_stat.spd.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        });

        transform.GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Item Speed: "+p.unbuffed_player_stat.dash_dura.ToString();
        transform.GetChild(0).GetChild(10).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.item_speed+=0.1f;
            transform.GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Item Speed: "+p.unbuffed_player_stat.dash_dura.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        });


        transform.GetChild(0).GetChild(11).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Stamina: "+p.unbuffed_player_stat.stamina.ToString();
        transform.GetChild(0).GetChild(11).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.stamina+=10f;
            transform.GetChild(0).GetChild(11).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Stamina: "+p.unbuffed_player_stat.stamina.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            on_edit(-1);
        });
    }

    public void level_down(){
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: "+p.unbuffed_player_stat.health.ToString();
        transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.health-=10f;
            transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: "+p.unbuffed_player_stat.health.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.health<=0f) transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });

        transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Defence: "+p.unbuffed_player_stat.strike_def.ToString();
        transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.strike_def-=0.1f;
            transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Defence: "+p.unbuffed_player_stat.strike_def.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.strike_def<=0f) transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });   

        transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Defence: "+p.unbuffed_player_stat.slash_def.ToString();
        transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.slash_def-=0.1f;
            transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Defence: "+p.unbuffed_player_stat.slash_def.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.slash_def<=0f) transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        }); 

        transform.GetChild(0).GetChild(3).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Defence: "+p.unbuffed_player_stat.pierce_def.ToString();
        transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.pierce_def-=0.1f;
            transform.GetChild(0).GetChild(3).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Defence: "+p.unbuffed_player_stat.pierce_def.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.pierce_def<=0f) transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        }); 

        transform.GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Defence: "+p.unbuffed_player_stat.mag_def.ToString();
        transform.GetChild(0).GetChild(4).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.mag_def-=0.1f;
            transform.GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Defence: "+p.unbuffed_player_stat.mag_def.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.mag_def<=0f) transform.GetChild(0).GetChild(4).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });

        transform.GetChild(0).GetChild(5).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Damage: "+p.unbuffed_player_stat.mag_dmg.ToString();
        transform.GetChild(0).GetChild(5).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.mag_dmg-=0.1f;
            transform.GetChild(0).GetChild(5).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Damage: "+p.unbuffed_player_stat.mag_dmg.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.mag_dmg<=0f) transform.GetChild(0).GetChild(5).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });

        transform.GetChild(0).GetChild(6).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Damage: "+p.unbuffed_player_stat.strike_dmg.ToString();
        transform.GetChild(0).GetChild(6).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.strike_dmg-=0.1f;
            transform.GetChild(0).GetChild(6).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Damage: "+p.unbuffed_player_stat.strike_dmg.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.strike_dmg<=0f) transform.GetChild(0).GetChild(6).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });


        transform.GetChild(0).GetChild(7).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Damage: "+p.unbuffed_player_stat.slash_dmg.ToString();
        transform.GetChild(0).GetChild(7).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.slash_dmg-=0.1f;
            transform.GetChild(0).GetChild(7).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Damage: "+p.unbuffed_player_stat.slash_dmg.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.slash_dmg<=0f) transform.GetChild(0).GetChild(7).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });

        transform.GetChild(0).GetChild(8).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Damage: "+p.unbuffed_player_stat.peirce_dmg.ToString();
        transform.GetChild(0).GetChild(8).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.peirce_dmg-=0.1f;
            transform.GetChild(0).GetChild(8).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Damage: "+p.unbuffed_player_stat.peirce_dmg.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.peirce_dmg<=0f) transform.GetChild(0).GetChild(8).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });


        transform.GetChild(0).GetChild(9).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Speed: "+p.unbuffed_player_stat.spd.ToString();
        transform.GetChild(0).GetChild(9).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.spd-=0.5f;
            transform.GetChild(0).GetChild(9).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Speed: "+p.unbuffed_player_stat.spd.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.spd<=0f) transform.GetChild(0).GetChild(9).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });

        transform.GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Item Speed: "+p.unbuffed_player_stat.dash_dura.ToString();
        transform.GetChild(0).GetChild(10).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.item_speed-=0.1f;
            transform.GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Item Speed: "+p.unbuffed_player_stat.dash_dura.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.item_speed<=0f) transform.GetChild(0).GetChild(10).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });


        transform.GetChild(0).GetChild(11).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Stamina: "+p.unbuffed_player_stat.stamina.ToString();
        transform.GetChild(0).GetChild(11).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            p.unbuffed_player_stat.stamina-=10f;
            transform.GetChild(0).GetChild(11).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Stamina: "+p.unbuffed_player_stat.stamina.ToString();
            on_edit(1);
            level_down_listener_added = false;
            if(p.unbuffed_player_stat.stamina<=0f) transform.GetChild(0).GetChild(11).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });
    }

    void Update(){
        foreach(Transform adjust_stat in transform.GetChild(0)){
            if(adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable == false) listener_added = false;
        }
        //if we can level up and we havn't added any listners, 
        if(p.unbuffed_player_stat.exp>=100&&!listener_added){
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = true;
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            level_up();
            listener_added = true;
        }
        if(p.unbuffed_player_stat.exp<100){
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
        }
        if(!level_down_listener_added&&p.unbuffed_player_stat.level>1) {
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = true;
                adjust_stat.GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            level_down();
            level_down_listener_added = true;
        }
        if(p.unbuffed_player_stat.level<=1){
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void finish_creation(){
        // save_load.SavePlayer(p.unbuffed_player_stat);
        // save_load.Saveworld(new world_details(), p.unbuffed_player_stat.name);
        // main_menu m = transform.parent.gameObject.GetComponent<main_menu>();
        // m.worlds.Add(new world_details());
        // m.stat.Add(p.unbuffed_player_stat);
        // StartCoroutine(m.LoadYourAsyncScene(m.worlds.Count-1));
    }
}
