using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System;
public static class save_load 
{
    public static void SavePlayer(stats stattobesaved, string player_name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = (string)Application.persistentDataPath+"/"+player_name+".pl";
        FileStream stream = new FileStream(path, FileMode.Create);

        stats stat = stattobesaved;
        formatter.Serialize(stream, stat);
        stream.Close();
        Debug.Log("saved!");
    }

    public static stats LoadPlayer(string loadname)
    {
        string path = Application.persistentDataPath + "/" + loadname +".pl";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            stats data = formatter.Deserialize(stream) as stats;
            stream.Close();
            Debug.Log("loaded");
            return data;
        }
        else
        {
            Debug.Log("path not found");
            return null;
        }
    }
}
