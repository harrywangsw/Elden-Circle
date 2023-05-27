using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroycircles : MonoBehaviour
{
    public int index;
    public circle c;
    SpriteRenderer s;
    public bool visible = false;
    void Start()
    {
        // s = GetComponent<SpriteRenderer>();
        StartCoroutine(destroy_invisible());
    }

    //gives it a chance for onbecamevisible to trigger, once the time runs out, destroy it
    IEnumerator destroy_invisible(){
        yield return new WaitForSeconds(0.1f);
        if(!visible){
            Debug.Log("destroyed");
            visible = false;
            fractal_generator gen = GameObject.Find("apollonian gasket").GetComponent<fractal_generator>();
            gen.object_list.Remove(gameObject);
            gen.circle_list.Remove(c);
            Destroy(gameObject);
        }
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
        Debug.Log("wtf");
        visible = false;
        fractal_generator gen = GameObject.Find("apollonian gasket").GetComponent<fractal_generator>();
        gen.object_list.Remove(gameObject);
        gen.circle_list.Remove(c);
        Destroy(gameObject);

    }
}
