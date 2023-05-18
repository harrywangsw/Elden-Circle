using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mine_explode : MonoBehaviour
{
    public int num_of_effects;
    float effect_size;
    
    void Start()
    {
        effect_size = GetComponent<SpriteRenderer>().bounds.extents.magnitude;
    }

    void Update()
    {
        // Debug.Log(statics.hit_effect_period.ToString());
    }

    void OnCollisionEnter2D(Collision2D c){
        StartCoroutine(explode());
    }

    IEnumerator explode(){
        Debug.Log("wtf");
        GameObject[] effs = new GameObject[num_of_effects];
        for(int i = 0; i<num_of_effects; i++){
            Vector3 eff_loc = new Vector3(transform.position.x+Random.Range(-effect_size, effect_size), transform.position.y+Random.Range(-effect_size, effect_size), transform.position.z);
            GameObject eff = GameObject.Instantiate(new GameObject(), eff_loc, Quaternion.identity);
            effs[i] = eff;
            StartCoroutine(statics.hit_effect(eff_loc, eff));
        }
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(statics.hit_effect_period);
        foreach(GameObject g in effs){
            Destroy(g);
        }
        Destroy(gameObject);
    }
}
