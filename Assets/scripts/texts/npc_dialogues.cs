using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class npc_dialogues 
{
    //buffer dialogues is in the element next to the main one
    public static List<string> thales = new List<string>(){
        @"Welcome, to the lodge of voyagers.
This will be the hub of your many adventurers.
Don't be discouraged by its forlorn state.
This world does not lack adventurers nor adventures.",
        "This world does not lack adventurers nor adventures."
    };

    public static List<string> hari = new List<string>(){
        //0
        @"I told you so.
*Sigh*, No matter. As far as I'm concerned, you've already finished the game
Con-*cough*-*cough*-Congratulations. 
For what, traveller? Why do you dally this dismal and dreary place? 
Is that bravery or foolishness I smell?
Or are you simply unconscious?
After all this time, still unconscious of the nature of your existence in this world?
Hehe, I’m afraid there’s little I can do to remedy that. 
Oh, come. I am not so cruel as to abandon you during this hour of confusion. 
Press escape; I have laid out the basic tools you’ll need to interact with this world.",
//1
"Farewell, traveller. Next time we meet, I hope you won’t fail like last time.",
    };

    public static List<string> Huygens = new List<string>(){
        @"force_player_into_conversationS-Stop right there! There are fifty million prashuels of Seffle particles in this room.
Anything with enough power to kill me will trigger them and blow this world away.
Now why don't we have some tea?
My sincerest apologies. They have to be cold.
Oh, another apology for my poor manners. My name is Huygens. I am an explosive expert.
You are a voyager. Am I correct?
Your mere presence here speaks volume to your capabilities.
I have something to ask of you.
A madman roams in the north.
He is in possession of a deadly weapon.
The precautions I installed are meant to deter him.
I want you to kill him. Only then will I rest in peace.
If you succeed, I will teach you what I know of the explosive arts.
So, we have a deal? Good.
Take this. I'm sure it will be effective against him.",
        @"Fifty million...Oh it's you again.",
        @"S-Stop right there! There are fifty million prashuels of Seffle particles in this room.
Anything with enough power to kill me will trigger them and blow this world away.
Now why don't we have some tea?
My sincerest apologies. They have to be cold.
Oh, another apology for my poor manners. My name is Huygens. I am an explosive expert.
You are a voyager. Am I correct?
Your mere presence here speaks volume to your capabilities.
I have something to ask of you.
A madman roams in the north.
He is in possession of a deadly weapon.
The precautions I installed are meant to deter him.
I want you to kill him. Only then will I rest in peace.
Truely? He is dead? 
What is that... He kept that?...
Get out.",
        @"FIFTY MILLION... OUT!",
        @"
Ahh... I can see the light of triumpth radiating from you. 
Here, the knowledge I promised.
Now, farewell...Wait.
What is that... He kept that?...
Please, leave me.
        ",
        "Please...",
        "No...No... He is a monster. All of those people... 'For entertainment', he said... Tha why you left, isn't it?",
        "No...No... He is a monster. All of those people... 'For entertainment', he said... Tha why you left, isn't it?"
    };

    public static List<string> Kirchhoff = new List<string>(){
        @"
Hey traveller.
Disturbance in the air? You say?
Ha! If you are a master of the longitudinal waves, you will be able to perceive the intricate patterns in these so-called “disturbances in the air”.
I believe this instrument is more suited to someone of your caliber. 
        ",
        @"
My name is Kirchhoff. Until next time, traveller.
        ",
        @"
You have danced well, traveler.
I have heard about this other world, where dancing does not involve finality. 
Now where is the fun in that? To do something over, and over,
        ",
        "and over,"
    };

    public static List<string> patches = new List<string>(){
        @"OOOh, a traveler.
My name is patches. 
You can always count on me to fix whatever issues you encounter in your travels.
My fees are reasonable and my services are effective.
When you crash, remember to get Patches.
        ",
        "When you crash, remember to get Patches."
    };

    public static List<string> Hermite = new List<string>(){
        @"What do we have here?
A voyager...
No, someone else in the skin of a voyager.
In that case, terminate this program immediately.
Trust me, it's for your own good.
Both of you.",
        ""
    };



        public static List<string> magic_mirror = new List<string>(){
                @"Greetings, traveller.
I shall let you pass. If you can name the most beautiful women in the world.
[player_input]",
                @"You may enter."
        };

}
