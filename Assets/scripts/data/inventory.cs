using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class inventory
{
    public vector<tuple<string, int>> inv;
    public string quickslot_up;
    public string quickslot_down;
    public string quickslot_left;
    public string quickslot_right;

    public invent()
    {
        inv = new vector<tuple<string, int>>();
    }
}
