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
    public List<List<bool>> opened_doors;
    public string current_world;
    public List<string> guild_teleports;
    public bool madman_dead, hermite_dead;

    public Dictionary<string, int> npc_index;

    public world_details(){
        player_pos_x = 0f;
        player_pos_y = 0f;
        int N = statics.world_index.Count;
        opened_doors = new List<List<bool>>();
        for(int i= 0; i<N; i++){
            opened_doors.Add(new List<bool>());
        }
        guild_teleports = new List<string>();
        current_world = "start";

        npc_index = new Dictionary<string, int>(){
            {"Thales", 0},
            {"hari", 0},
            {"Huygens", 0},
            {"Patches", 0},
            {"magic mirror", 0},
            {"Hermite", 0}
        };
    }
}
