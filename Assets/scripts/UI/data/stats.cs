using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class stats
{
    //note that all of these def and dmg stats are multiplyers
    public float slash_def, strike_def, pierce_def, mag_def, health, dash_dura, item_speed, health_up_amount;
    public float slash_dmg, strike_dmg, peirce_dmg, mag_dmg, stamina;
    public float spd, exp_pos_x, exp_pos_y;
    public int exp, level, exp_lost;
    public string name;
    public inventory inv;
    public stats()
    {
        exp_pos_x = float.PositiveInfinity;
        exp_pos_y = float.PositiveInfinity;
        slash_def = 1f;
        strike_def = 1f;
        pierce_def = 1f;
        mag_def = 1f;
        health = 100f;
        dash_dura = 0.2f;
        item_speed = 1f;
        health_up_amount = 30f;
        slash_dmg = 1f;
        strike_dmg = 1f;
        peirce_dmg = 1f;
        mag_dmg = 1f;
        stamina = 100f;
        spd = 10f;
        exp = 0;
        level = 1;
        name = "";
        inv = new inventory();
    }
}
