using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health_bar : MonoBehaviour
{
    GameObject p;
    public float p_health, e_health, max_p_health, max_e_health, max_p_stamina, p_stamina;
    player_control control;
    public GameObject enemy_health_bar, stamina_bar;
    void Start()
    {
        p = GameObject.Find("player");
        control = p.GetComponent<player_control>();
        max_p_health = control.player_stat.health;
    }

    void Update()
    {
        p_health = control.health;
        max_p_health = control.player_stat.health;
        transform.localScale = new Vector3(p_health/max_p_health, 1, 1);
        p_stamina = control.stamina;
        max_p_stamina = control.player_stat.stamina;
        stamina_bar.transform.localScale = new Vector3(p_stamina/max_p_stamina, 1, 1);
        if(control.locked_enemy==null) {
            enemy_health_bar.transform.parent.localScale = Vector3.zero;
            return;
        }
        enemy_health_bar.transform.parent.localScale = Vector3.one;
        max_e_health = control.locked_enemy.GetComponent<enemy_control>().enemy_stat.health;
        e_health = control.locked_enemy.GetComponent<enemy_control>().current_health;
        enemy_health_bar.transform.localScale = new Vector3(e_health/max_e_health, 1, 1);
    }
}
