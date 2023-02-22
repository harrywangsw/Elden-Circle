using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class stats
{

    public float slash_def, strike_def, pierce_def, health;

    public stats()
    {
        slash_def = 0f;
        strike_def = 0f;
        pierce_def = 0f;
        health = 0f;
    }
}
