using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class item_descriptions 
{
    public static Dictionary<string, string> des = new Dictionary<string, string>(){
        {"fire_cracker", @"spawns a series of exploding stars that propagates 
        horizontally in front of the player."},
	    {"health_potion", "it restores health...duh"},
	    {"spear", @"The simplest of the weapons"},
        {"dagger_fan", "spawns 6 daggers around you that shoots outwards"},
        {"parry_shield", "trigger this right befor an attack hits to (somehow) stun the enemy and execute a viseral attack/critical attack/riposte...ect"},
        {"lightning_strike", @"Spawns a lightning strike that may fork and deviate from its intended target"},
        {"spawn_bees", "weapon"},
        {"glintstone", "shoots...something"},
        {"mine", "deploys a mine that explodes when an enemy steps on it"},
        {"machine_gun", "Shoots shrapnel at a rapid rate. Takes a long time to deploy."},
        {"shrink", "shrinks the player character; makes it more nimble"},
        {"expand", "enlarge the player character"},
        {"spiked_wall", "spawns a spiked wall behind the enemy and force it toward you"}
    };
}
