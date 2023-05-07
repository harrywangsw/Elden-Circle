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
    public List<item> inv;
    public List<int> quickslot_up_indexes, quickslot_left_indexes, quickslot_right_indexes;

    public inventory()
    {
        inv = new List<item>();
        quickslot_left_indexes = new List<int>();
        quickslot_right_indexes = new List<int>();
        quickslot_up_indexes = new List<int>();
    }
}

[System.Serializable]
public class item
{
    public string item_name, item_type;
    public int num_left;

    public item(string n, int num, string t){
        item_name = n;
        item_type = t;
        num_left = num;
    }
}
