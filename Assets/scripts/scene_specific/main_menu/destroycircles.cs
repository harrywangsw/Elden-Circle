using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroycircles : MonoBehaviour
{
    public int index;
    public circle c;
    SpriteRenderer s;
    public bool visible = false;
    fractal_generator gen;
    void Start()
    {
        // s = GetComponent<SpriteRenderer>();
        gen = GameObject.Find("apollonian gasket").GetComponent<fractal_generator>();
        // if(gen.object_list.FindIndex(obj=>obj==gameObject)<0) {
        //     Debug.Log("somehow");
        //     gen.object_list.Add(gameObject);
        // }
        //StartCoroutine(destroy_invisible());
    }

    //gives it a chance for onbecamevisible to trigger, once the time runs out, destroy it
    IEnumerator destroy_invisible(){
        yield return new WaitForSeconds(0.5f);
        if(!visible){
            visible = false;
            destroy_circle();
        }
    }

    void destroy_circle(){
        int index = transform.GetSiblingIndex();
        gen.circle_list.RemoveAt(index);
        Destroy(gen.object_list[index]);
        gen.object_list.RemoveAt(index);
        // for(int i = 0; i<=gen.object_list.Count; i++){
        //     // if(i==gen.object_list.Count){
        //     //     GetComponent<SpriteRenderer>().color = Color.red;
        //     //     transform.SetParent(null);
        //     //     return;
        //     // }
        //     if(gen.object_list[i].GetComponent<destroycircles>().c==c) {
        //         gen.object_list.RemoveAt(i);
        //         // Debug.Log("removed? please?");
        //         break;
        //     }
        // }
        if(gen.object_list.Count!=gen.circle_list.Count&&(gen.object_list.Count-gen.circle_list.Count)<3) {
            Debug.Log("wtf"+gen.circle_list.Count.ToString()+" "+gen.object_list.Count.ToString());
            Debug.Log(transform.localScale.ToString()+" "+c.r.ToString());
        }
        //Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // if (!s.isVisible)
        // {
        //     fractal_generator gen = GameObject.Find("apollonian gasket").GetComponent<fractal_generator>();
        //     gen.object_list.Remove(gameObject);
        //     gen.circle_list.Remove(c);
        //     Destroy(gameObject);
        // }
    }

    void OnBecameVisible(){
        visible = true;
    }

    //this is never triggered if the object is never visible to begin with...hmmmm...
    void OnBecameInvisible()
    {
        visible = false;
        //destroy_circle();

    }
}
