using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Numerics;

[System.Serializable]
public class inventory
{
    //tracks the name of the object and how many the player has, the third string is its type
    public List<Tuple<string, int, string>> inv;
    public List<int> quickslot_up_indexes, quickslot_left_indexes, quickslot_right_indexes;

    public inventory()
    {
        inv = new List<Tuple<string, int, string>>();
        quickslot_left_indexes = new List<int>();
        quickslot_right_indexes = new List<int>();
        quickslot_up_indexes = new List<int>();
    }
}
