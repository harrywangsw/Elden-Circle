using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class npc_dialogues 
{
    //buffer dialogues is in the element next to the main one
    public static List<string> thales = new List<string>(){
        //0
        @"Well met, voyager.
it's nice talking to you",
        //1
        "It was a pleasure",
        //2
        "Created by Hali the all-knowing",
        //3
        @"What do you mean? I did not pass anyone.
        Most of our actions can be rationalized and predicted, except for a small set of axiomatic actions that cannot. 
        Me swerving to the left just then is an example of such action. "
    };

    public static List<string> hali = new List<string>(){
        //0
        @"I told you so.
*Sigh*, No matter. As far as I'm concerned, you've already finished the game
Con-*cough*-*cough*-Congratulations. 
唉
For what, traveller? Why do you dally this dismal and dreary place? 
Is that bravery or foolishness I smell?
Or are you simply unconscious?
After all this time, still unconscious of the nature of your existence in this world?
Hehe, I’m afraid there’s little I can do to remedy that. 
Oh, come. I am not so cruel as to abandon you during this hour of confusion. 
Press escape; I have laid out the basic tools you’ll need to interact with this world.",
//1
"Farewell, traveller. Next time we meet, I hope you won’t fail like last time.",
        //2
        "Welcome, traveller. To the lodge of voyagers. "
    };
}
