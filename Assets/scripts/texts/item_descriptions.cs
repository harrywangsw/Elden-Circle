using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class item_descriptions 
{
    public static Dictionary<string, string> des = new Dictionary<string, string>(){
        {"fire_cracker", @"spawns a series of exploding stars that propagates 
        horizontally in front of the player."},
	    {"health_potion", "item"},
	    {"spear", @"The simplist of the weapons"},
        {"dagger_fan", "weapon"},
        {"parry_shield", "weapon"},
        {"lightning_strike", @"Forks in random direction"},
        {"spawn_bees", "weapon"},
        {"glintstone", "weapon"}
    };
}
