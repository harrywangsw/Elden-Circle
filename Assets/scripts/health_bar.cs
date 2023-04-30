using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health_bar : MonoBehaviour
{
    GameObject p;
    float max_p_health;
    public float p_health;
    player_control control;
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
    }
}
