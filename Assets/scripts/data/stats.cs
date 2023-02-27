using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class stats
{

    public float slash_def, strike_def, pierce_def, health, dash_length, item_speed, dash_modifier;

    public stats()
    {
        slash_def = 0f;
        strike_def = 0f;
        pierce_def = 0f;
        health = 0f;
        dash_length = 0f;
        item_speed = 0f;
    }
}
