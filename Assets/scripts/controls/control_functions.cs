using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class control_functions
{
    public static Vector2 rotate(Vector2 original, float angle){
        return new Vector2(original.x*Mathf.Cos(angle)-original.y*Mathf.Sin(angle), original.x*Mathf.Sin(angle)+original.y*Mathf.Cos(angle));
    }
    public static bool out_of_bound(Vector3 pos){
        return(Physics.Linecast(pos, Vector3.down));
    }
    public static IEnumerator animate_hurt(SpriteRenderer damaged_sprite)
    {
        damaged_sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        damaged_sprite.color = Color.black;
    }
}
