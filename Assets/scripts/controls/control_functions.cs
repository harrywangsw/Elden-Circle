using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class control_functions
{
    public static Vector2 rotate(Vector2 original, float angle){
        return new Vector2(original.x*Mathf.Cos(angle)-original.y*Mathf.Sin(angle), original.x*Mathf.Sin(angle)+original.y*Mathf.Cos(angle));
    }
}
