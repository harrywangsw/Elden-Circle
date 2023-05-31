using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;

public class fractal_generator : MonoBehaviour
{

    //circle object has radius of 1
    public GameObject circle_object, zooming_object;
    public float radius_range, min_radius, expand_period, min_change, cam_size_y, cam_size_x;
    //circle_list's positions are not shifted/scaled by zooming_object
    public List<circle> circle_list;
    //object_list keeps track of the objects' shifted positions
    public List<GameObject> object_list; 
    public circle smallest;
    public Vector2 amount_shifted;
    public bool regenerate, expanding;
    public List<Tuple<GameObject, GameObject, GameObject>> break_point = new List<Tuple<GameObject, GameObject, GameObject>>();
    void Start()
    {
        circle_list = new List<circle>();
        object_list = new List<GameObject>();
        min_change = float.PositiveInfinity;
        Tuple<GameObject, GameObject, GameObject> triplet = generate_first_three();
        generate_apollo(triplet.Item1, triplet.Item2, triplet.Item3);
    }

    void Update(){
        if(!expanding) StartCoroutine(expand());
    }

    //once in a while, we need to realign the three largest circles and regenerate the enter fractal based on that
    //to reduce the calculational errors that have accumulated
    void re_generate_apollo(){
        float first = float.NegativeInfinity, second = float.NegativeInfinity, third = float.NegativeInfinity;
        circle one = new circle(), two = new circle(), three = new circle();
        foreach(Transform c in zooming_object.transform){
            float radius = c.localScale.x;
            if(out_of_view(c)) continue;
            if(radius>first){
                third = second;
                second = first;
                first = radius;
                three = two;
                two = one;
                one = new circle(radius, (Vector2)c.transform.position);
            }
            else if(radius>second){
                third = second;
                second = radius;
                three = two;
                two = new circle(radius, (Vector2)c.transform.position);
            }
            else if(radius>third){
                third = radius;
                three = new circle(radius, (Vector2)c.transform.position);
            }
        }
        // spawn_circle_object(one, true);
        // spawn_circle_object(two, true);
        // spawn_circle_object(three, true);
        foreach(Transform cir in zooming_object.transform){
            Destroy(cir.gameObject);
        }
        object_list = new List<GameObject>();
        circle_list = new List<circle>();
        //keep circle one unchanged
        //keep the pos of circle two unchanged and change its rad so that it's more perfectly tangent to one
        two.r +=(one.pos-two.pos).magnitude-(one.r+two.r);
        //re-find the pos of three
        Vector2 trd_pos = intersection_of_two_circles(new circle(one.r+three.r, one.pos), new circle(two.r+three.r, two.pos));
        Vector2 trd_pos_neg = intersection_of_two_circles(new circle(one.r+three.r, one.pos), new circle(two.r+three.r, two.pos), -1);
        if((trd_pos-three.pos).magnitude<(trd_pos_neg-three.pos).magnitude){
            three.pos = trd_pos;
        }
        else three.pos = trd_pos_neg;
        GameObject o = spawn_circle_object(one);
        GameObject t = spawn_circle_object(two);
        GameObject tr = spawn_circle_object(three);
        generate_apollo(o, t, tr);
        expanding = false;
    }

    bool out_of_view(Transform c){
        float radius = c.localScale.x;
        Vector2 closes_point = (Vector2)c.position.normalized*(c.position.magnitude-radius);
        if(Mathf.Abs(closes_point.y)>cam_size_y||Mathf.Abs(closes_point.x)>cam_size_x) return true;
        return false;
    }

    IEnumerator expand(){
        expanding = true;
        float zooming_object_size;
        int i, j = 0;
        while(j<8){
            foreach(Transform c in zooming_object.transform){
                
                //move each circle so that the smallest circle is aligned with the center of the camera
                c.position=(Vector3)((Vector2)c.position-smallest.pos);
                //because the circles's positions are calculated as if zooming_object has to stretch, 
                //we must multiply all distance by the scale
                c.transform.position*=zooming_object.transform.localScale.x;
                //object_list[i].GetComponent<SpriteRenderer>().enabled = true;
            }

            Vector3 final_size = zooming_object.transform.localScale*expand_period;
            yield return StartCoroutine(statics.expand(zooming_object.transform, expand_period/2.8f, final_size));
            //min_radius/=expand_period;

            //to prevent the radius of the circles from getting too small:
            //First, shrink zooming_object and all the circles to the original size
            //then, de-parent all the circles, enlarge them, and move them to the correct spot to look like as if nothing's changed
            //add the circles back to the zooming_object to start another zooming cycle with the min_rad unchanged
            zooming_object_size = zooming_object.transform.localScale.x;
            zooming_object.transform.localScale = Vector3.one;
            foreach(Transform c in zooming_object.transform){
                //c.SetParent(null);
                c.position*=zooming_object_size;
                c.localScale *= zooming_object_size;
                //now that the radius and position of the circle objects have been "reset" as if they haven't been stretched, 
                //change the circle objects in circle_list to match the objects
                // circle_list[i].pos=(Vector2)object_list[i].transform.position;
                // circle_list[i].r = object_list[i].transform.localScale.x;
                //c.SetParent(zooming_object.transform);
            }

            List<Tuple<GameObject, GameObject, GameObject>> break_point_cpy = break_point;
            break_point = new List<Tuple<GameObject, GameObject, GameObject>>();
            min_change = float.PositiveInfinity;
            foreach(Tuple<GameObject, GameObject, GameObject> triplet in break_point_cpy){
                if(!triplet.Item1||!triplet.Item2||!triplet.Item3){
                    continue;
                }
                generate_apollo(triplet.Item1, triplet.Item2, triplet.Item3);
            }
            j++;
        }
        re_generate_apollo();
        yield break;
    }

    void generate_apollo(GameObject object_1, GameObject object_2, GameObject object_3){
        circle tangent_1 = new circle(object_1.transform.localScale.x, (Vector2)object_1.transform.position);
        circle tangent_2 = new circle(object_2.transform.localScale.x, (Vector2)object_2.transform.position);
        circle tangent_3 = new circle(object_3.transform.localScale.x, (Vector2)object_3.transform.position);
        circle new_circle = generate_next_circle(tangent_1, tangent_2, tangent_3);
        if(new_circle.r<=min_radius) {
            // GameObject c1 = Instantiate(circle_object, tangent_1.pos, Quaternion.identity);
            // c1.transform.localScale = tangent_1.r*Vector3.one;
            // GameObject c2 = Instantiate(circle_object, tangent_2.pos, Quaternion.identity);
            // c2.transform.localScale = tangent_2.r*Vector3.one;
            // GameObject c3 = Instantiate(circle_object, tangent_3.pos, Quaternion.identity);
            // c3.transform.localScale = tangent_3.r*Vector3.one;
            break_point.Add(Tuple.Create(object_1, object_2, object_3));
            //get the circle that requires the least shifting for the entire fractal
            //if(min_change>(new_circle.pos-amount_shifted).magnitude) smallest = new_circle;
            smallest = new_circle;
            return;
        }
        GameObject new_object = spawn_circle_object(new_circle);
        //Debug.Log(new_circle.r.ToString());
        generate_apollo(new_object, object_1, object_2);
        generate_apollo(object_1, object_3, new_object);
        generate_apollo(object_2, object_3, new_object);
        
        return;
    }

    public circle generate_next_circle(circle fst, circle snd, circle trd){
        float k_1 = 1f/fst.r;
        float k_2 = 1f/snd.r;
        float k_3 = 1f/trd.r;
        float rad = 1f/(k_1+k_2+k_3+2f*Mathf.Sqrt(k_1*k_2+k_2*k_3+k_1*k_3));
        if(Mathf.Abs(1f/(k_1+k_2+k_3-2f*Mathf.Sqrt(k_1*k_2+k_2*k_3+k_1*k_3)))<rad&&1f/(k_1+k_2+k_3-2f*Mathf.Sqrt(k_1*k_2+k_2*k_3+k_1*k_3))>0f){
            rad = 1f/(k_1+k_2+k_3-2f*Mathf.Sqrt(k_1*k_2+k_2*k_3+k_1*k_3));
        }

        Vector2 pos1 = intersection_of_two_circles(new circle(fst.r+rad, fst.pos), new circle(snd.r+rad, snd.pos));
        Vector2 pos2 = intersection_of_two_circles(new circle(fst.r+rad, fst.pos), new circle(snd.r+rad, snd.pos), -1);
        if(float.IsNaN(pos1.x)||float.IsNaN(pos2.y)){
            //Debug.Log(fst.pos.ToString()+fst.r.ToString()+" "+snd.pos.ToString()+snd.r.ToString()+" "+trd.pos.ToString()+trd.r.ToString());
        }

        if((pos1-trd.pos).magnitude<(pos2-trd.pos).magnitude){
            return new circle(rad, pos1);
        }
        else{
            return new circle(rad, pos2);
        }
        spawn_circle_object(fst, true);
        spawn_circle_object(snd, true);
        spawn_circle_object(trd, true);
        spawn_circle_object(new circle(rad, pos2), true);
        spawn_circle_object(new circle(rad, pos1), true);
        //Debug.Log("none??");
        return null;
    }

    Tuple<GameObject, GameObject, GameObject> generate_first_three(){
        float rad = Random.Range(radius_range, radius_range*2f);
        circle fir = new circle(rad, new Vector2(0f, 0f));
        float snd_rad = Random.Range(radius_range, radius_range*2f);
        float angle = Random.Range(0f, 2*Mathf.PI);
        Vector2 snd_pos = new Vector2((snd_rad+rad)*Mathf.Cos(angle), (snd_rad+rad)*Mathf.Sin(angle));
        circle snd = new circle(snd_rad, snd_pos);
        float trd_rad = Random.Range(radius_range, 2f*radius_range);
        Vector2 trd_pos = intersection_of_two_circles(new circle(fir.r+trd_rad, fir.pos), new circle(snd.r+trd_rad, snd.pos));
        circle trd = new circle(trd_rad, trd_pos);
        return Tuple.Create(spawn_circle_object(fir), spawn_circle_object(snd), spawn_circle_object(trd));
    }

    public Vector2 intersection_of_two_circles(circle one, circle two, int pos_neg = 1){
        float dist_between = (one.pos-two.pos).magnitude;
        float angle_from_one = Mathf.Acos((two.r*two.r-dist_between*dist_between-one.r*one.r)/(-2f*one.r*dist_between));
        angle_from_one*=pos_neg;
        angle_from_one += Mathf.PI*Vector2.SignedAngle(Vector2.right, (two.pos-one.pos))/180f;
        Vector2 wtf = new Vector2(one.pos.x+one.r*Mathf.Cos(angle_from_one), one.pos.y+one.r*Mathf.Sin(angle_from_one));
        //Debug.Log(angle_from_one);
        return wtf;
    }

    public GameObject spawn_circle_object(circle c, bool painted = false){
        //spawn the circle in world space first and then move it to the zoom_object's position
        GameObject spawned_c = GameObject.Instantiate(circle_object, (c.pos), Quaternion.identity);
        spawned_c.transform.localScale = (float)c.r*Vector3.one;
        spawned_c.transform.SetParent(zooming_object.transform);
        // circle_list.Add(c);
        // object_list.Add(spawned_c);
        spawned_c.GetComponent<destroycircles>().c = c;
        if(painted) spawned_c.GetComponent<SpriteRenderer>().color = Color.blue;
        return spawned_c;
    }
}
