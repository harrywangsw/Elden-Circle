﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class stats
{

    public float slash_def, strike_def, pierce_def, health, dash_length, item_speed, dash_modifier;
    public float slash_dmg, strike_dmg, peirce_dmg, mag_dmg;
    public string name;
    public stats()
    {
        name = "";
    }
}