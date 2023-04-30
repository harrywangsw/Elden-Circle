using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class make_new_player : MonoBehaviour
{
    public stats new_stat;
    public GameObject level_indicator;
    player_control p;
    public bool listener_added, level_down_listener_added;
    void Start()
    {
        transform.GetChild(transform.childCount - 1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{finish_creation();});
        p = GameObject.Find("player").GetComponent<player_control>();
        new_stat = p.player_stat;
        transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = p.player_stat.level.ToString();
        transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = p.player_stat.exp.ToString();
    }

    public void level_up(){
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: "+new_stat.health.ToString();
        transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            Debug.Log("wtf");
            new_stat.health+=10f;
            transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: "+new_stat.health.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        });

        transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Defence: "+new_stat.strike_def.ToString();
        transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.strike_def+=0.1f;
            transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Defence: "+new_stat.strike_def.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        });   

        transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Defence: "+new_stat.slash_def.ToString();
        transform.GetChild(0).GetChild(2).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.slash_def+=0.1f;
            transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Defence: "+new_stat.slash_def.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        }); 

        transform.GetChild(0).GetChild(3).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Defence: "+new_stat.pierce_def.ToString();
        transform.GetChild(0).GetChild(3).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.pierce_def+=0.1f;
            transform.GetChild(0).GetChild(3).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Defence: "+new_stat.pierce_def.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        }); 

        transform.GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Defence: "+new_stat.mag_def.ToString();
        transform.GetChild(0).GetChild(4).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.mag_def+=0.1f;
            transform.GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Defence: "+new_stat.mag_def.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        });

        transform.GetChild(0).GetChild(5).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Damage: "+new_stat.mag_dmg.ToString();
        transform.GetChild(0).GetChild(5).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.mag_dmg+=0.1f;
            transform.GetChild(0).GetChild(5).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Damage: "+new_stat.mag_dmg.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        });

        transform.GetChild(0).GetChild(6).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Damage: "+new_stat.strike_dmg.ToString();
        transform.GetChild(0).GetChild(6).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.strike_dmg+=0.1f;
            transform.GetChild(0).GetChild(6).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Damage: "+new_stat.strike_dmg.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        });


        transform.GetChild(0).GetChild(7).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Damage: "+new_stat.slash_dmg.ToString();
        transform.GetChild(0).GetChild(7).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.slash_dmg+=0.1f;
            transform.GetChild(0).GetChild(7).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Damage: "+new_stat.slash_dmg.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        });

        transform.GetChild(0).GetChild(8).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Damage: "+new_stat.peirce_dmg.ToString();
        transform.GetChild(0).GetChild(8).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.peirce_dmg+=0.1f;
            transform.GetChild(0).GetChild(8).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Damage: "+new_stat.peirce_dmg.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        });


        transform.GetChild(0).GetChild(9).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Speed: "+new_stat.spd.ToString();
        transform.GetChild(0).GetChild(9).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.spd+=0.5f;
            transform.GetChild(0).GetChild(9).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Speed: "+new_stat.spd.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        });

        transform.GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Item Speed: "+new_stat.dash_dura.ToString();
        transform.GetChild(0).GetChild(10).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.item_speed+=0.1f;
            transform.GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Item Speed: "+new_stat.dash_dura.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        });


        transform.GetChild(0).GetChild(11).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Stamina: "+new_stat.stamina.ToString();
        transform.GetChild(0).GetChild(11).GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.stamina+=10f;
            transform.GetChild(0).GetChild(11).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Stamina: "+new_stat.stamina.ToString();
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }
            new_stat.exp-=100;
        });
    }

    public void level_down(){
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: "+new_stat.health.ToString();
        transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.health-=10f;
            transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: "+new_stat.health.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.health<=0f) transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });

        transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Defence: "+new_stat.strike_def.ToString();
        transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.strike_def-=0.1f;
            transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Defence: "+new_stat.strike_def.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.strike_def<=0f) transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });   

        transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Defence: "+new_stat.slash_def.ToString();
        transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.slash_def-=0.1f;
            transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Defence: "+new_stat.slash_def.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.slash_def<=0f) transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        }); 

        transform.GetChild(0).GetChild(3).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Defence: "+new_stat.pierce_def.ToString();
        transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.pierce_def-=0.1f;
            transform.GetChild(0).GetChild(3).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Defence: "+new_stat.pierce_def.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.pierce_def<=0f) transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        }); 

        transform.GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Defence: "+new_stat.mag_def.ToString();
        transform.GetChild(0).GetChild(4).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.mag_def-=0.1f;
            transform.GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Defence: "+new_stat.mag_def.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.mag_def<=0f) transform.GetChild(0).GetChild(4).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });

        transform.GetChild(0).GetChild(5).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Damage: "+new_stat.mag_dmg.ToString();
        transform.GetChild(0).GetChild(5).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.mag_dmg-=0.1f;
            transform.GetChild(0).GetChild(5).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Magick Damage: "+new_stat.mag_dmg.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.mag_dmg<=0f) transform.GetChild(0).GetChild(5).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });

        transform.GetChild(0).GetChild(6).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Damage: "+new_stat.strike_dmg.ToString();
        transform.GetChild(0).GetChild(6).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.strike_dmg-=0.1f;
            transform.GetChild(0).GetChild(6).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Strike Damage: "+new_stat.strike_dmg.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.strike_dmg<=0f) transform.GetChild(0).GetChild(6).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });


        transform.GetChild(0).GetChild(7).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Damage: "+new_stat.slash_dmg.ToString();
        transform.GetChild(0).GetChild(7).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.slash_dmg-=0.1f;
            transform.GetChild(0).GetChild(7).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Slash Damage: "+new_stat.slash_dmg.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.slash_dmg<=0f) transform.GetChild(0).GetChild(7).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });

        transform.GetChild(0).GetChild(8).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Damage: "+new_stat.peirce_dmg.ToString();
        transform.GetChild(0).GetChild(8).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.peirce_dmg-=0.1f;
            transform.GetChild(0).GetChild(8).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Pierce Damage: "+new_stat.peirce_dmg.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.peirce_dmg<=0f) transform.GetChild(0).GetChild(8).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });


        transform.GetChild(0).GetChild(9).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Speed: "+new_stat.spd.ToString();
        transform.GetChild(0).GetChild(9).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.spd-=0.5f;
            transform.GetChild(0).GetChild(9).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Speed: "+new_stat.spd.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.spd<=0f) transform.GetChild(0).GetChild(9).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });

        transform.GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Item Speed: "+new_stat.dash_dura.ToString();
        transform.GetChild(0).GetChild(10).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.item_speed-=0.1f;
            transform.GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Item Speed: "+new_stat.dash_dura.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.item_speed<=0f) transform.GetChild(0).GetChild(10).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });


        transform.GetChild(0).GetChild(11).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Stamina: "+new_stat.stamina.ToString();
        transform.GetChild(0).GetChild(11).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{
            new_stat.stamina-=10f;
            transform.GetChild(0).GetChild(11).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Stamina: "+new_stat.stamina.ToString();
            new_stat.exp+=100;
            level_down_listener_added = false;
            if(new_stat.stamina<=0f) transform.GetChild(0).GetChild(11).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        });
    }

    void Update(){
        transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: "+p.player_stat.level.ToString();
        transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Exp: "+p.player_stat.exp.ToString();
        foreach(Transform adjust_stat in transform.GetChild(0)){
            if(adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable == false) listener_added = false;
        }
        if(new_stat.exp>=100&&!listener_added){
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = true;
                adjust_stat.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            level_up();
            listener_added = true;
        }
        p.player_stat = new_stat;
        if(!level_down_listener_added) {
            foreach(Transform adjust_stat in transform.GetChild(0)){
                adjust_stat.GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            level_down();
            level_down_listener_added = true;
        }
    }

    public void finish_creation(){
        // save_load.SavePlayer(new_stat);
        // save_load.Saveworld(new world_details(), new_stat.name);
        // main_menu m = transform.parent.gameObject.GetComponent<main_menu>();
        // m.worlds.Add(new world_details());
        // m.stat.Add(new_stat);
        // StartCoroutine(m.LoadYourAsyncScene(m.worlds.Count-1));
    }
}
