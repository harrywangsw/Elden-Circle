using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter_behaviour : MonoBehaviour
{
    public float angle, period, eccen, major_axis;
    public string destination;
    public GameObject c1, c2;
    void Start()
    {
        major_axis = GetComponent<SpriteRenderer>().bounds.extents.magnitude;
        transform.localScale = new Vector3(Mathf.Sqrt(1-eccen*eccen)/major_axis, 1f, 1f);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        angle+=Time.deltaTime*(2f*Mathf.PI)/period;
        float r = r_theta(angle);
        c1.transform.localPosition = Quaternion.Euler(0f, 0f, angle*Mathf.Rad2Deg)*Vector3.up;
        c1.transform.localPosition*=r;
        c1.transform.rotation = Quaternion.Euler(0f, 0f, (angle+Mathf.PI/2f)*Mathf.Rad2Deg);

        c2.transform.localPosition = Quaternion.Euler(0f, 0f, angle*Mathf.Rad2Deg)*(-Vector3.up);
        c2.transform.localPosition*=r;
        c2.transform.rotation = Quaternion.Euler(0f, 0f, (angle+Mathf.PI/2f)*Mathf.Rad2Deg);
    }

    float r_theta(float theta){
        return major_axis*(1-eccen*eccen)/(1-Mathf.Cos(theta)*eccen);
    }

    void OnCollisionEnter2D(Collision2D c){
        if(c.collider.gameObject.name=="player"){
            GameObject player = GameObject.Find("player");
            player.name = "old_player";
            StartCoroutine(statics.load_new_world(destination, c.gameObject.GetComponent<player_control>().current_world, c.gameObject.GetComponent<player_control>().unbuffed_player_stat, gameObject));
        }
    }
}
