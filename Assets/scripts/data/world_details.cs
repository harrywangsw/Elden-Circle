using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Numerics;

[System.Serializable]
public class world_details
{
    public float player_pos_x;
    public float player_pos_y;

    public world_details(){
        player_pos_x = 0f;
        player_pos_y = 0f;
    }
}
