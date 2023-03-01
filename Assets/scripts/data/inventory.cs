using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Numerics;

[System.Serializable]
public class inventory
{
    public List<Tuple<string, int>> inv;
    public int quickslot_up = -1;
    public int quickslot_down = -1;
    public int quickslot_left = -1;
    public int quickslot_right = -1;

    public inventory()
    {
        inv = new List<Tuple<string, int>>();
        //inv.Add(Tuple.Create("health_potion", 1));
    }
}
