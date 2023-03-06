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
    public int quickslot_up;
    public int quickslot_down;
    public int quickslot_left;
    public int quickslot_right;

    public inventory()
    {
        inv = new List<Tuple<string, int>>();
        //inv.Add(Tuple.Create("health_potion", 1));
        quickslot_up = -1;
        quickslot_down = -1;
        quickslot_left = -1;
        quickslot_right = -1;
    }
}
