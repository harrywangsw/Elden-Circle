using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroycircles : MonoBehaviour
{
    public int index;
    public circle c;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameInvisible()
    {
        fractal_generator gen = GameObject.Find("apollonian gasket").GetComponent<fractal_generator>();
        gen.object_list.Remove(gameObject);
        gen.circle_list.Remove(c);
        Destroy(gameObject);

    }
}
