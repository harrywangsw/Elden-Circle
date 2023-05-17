using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter_to_areana : MonoBehaviour
{
    public GameObject notes;
    reactive_messages r;
    void Start()
    {
        r = GameObject.Find("temporary_messages").GetComponent<reactive_messages>();
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D c){
        foreach(Transform child in notes.transform){
            if(child.gameObject.GetComponent<note_behaviour>().space!=3){
                StartCoroutine(r.show_message("teleporter to the stage will open once the notes are aligned to this teleporter"));
                return;
            }
        }
        if(c.collider.gameObject.name=="player"){
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
