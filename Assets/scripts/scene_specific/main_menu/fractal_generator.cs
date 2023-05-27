using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;

public class fractal_generator : MonoBehaviour
{

    //circle object has radius of 1
    public GameObject circle_object, zooming_object;
    public float radius_range, min_radius, expand_period, min_change;
    public List<circle> circle_list;
    public List<GameObject> object_list; 
    public circle smallest;
    public Vector2 amount_shifted;
    public List<Tuple<circle, circle, circle>> break_point = new List<Tuple<circle, circle, circle>>();
    void Start()
    {
        circle_list = new List<circle>();
        object_list = new List<GameObject>();
        min_change = float.PositiveInfinity;
        generate_first_three();
        generate_apollo(circle_list[0], circle_list[1], circle_list[2]);
        StartCoroutine(expand());
    }

    IEnumerator expand(){
        while(true){
            for(int i=0; i<object_list.Count; i++){
                if(!object_list[i]){
                    object_list.RemoveAt(i);
                    circle_list.RemoveAt(i);
                    continue;
                }
                //calculate the circle's position in world sapce first, then stretch the position vector
                object_list[i].transform.position=(Vector3)(circle_list[i].pos-smallest.pos);
                //because the circles's positions are calculated as if zooming_object has to stretch, 
                //we must multiply all distance by the scale
                object_list[i].transform.position*=zooming_object.transform.localScale.x;
                //object_list[i].GetComponent<SpriteRenderer>().enabled = true;
            }

            Vector3 final_size = zooming_object.transform.localScale*expand_period;
            yield return StartCoroutine(statics.expand(zooming_object.transform, expand_period, final_size));
            min_radius/=expand_period;
            List<Tuple<circle, circle, circle>> break_point_cpy = break_point;
            break_point = new List<Tuple<circle, circle, circle>>();
            min_change = float.PositiveInfinity;
            foreach(Tuple<circle, circle, circle> triplet in break_point_cpy){
                if(circle_list.IndexOf(triplet.Item1)<0||circle_list.IndexOf(triplet.Item2)<0||circle_list.IndexOf(triplet.Item3)<0){
                    Debug.Log("sdas");
                    continue;
                }
                generate_apollo(triplet.Item1, triplet.Item2, triplet.Item3);
            }
        }
    }

    void generate_apollo(circle tangent_1, circle tangent_2, circle tangent_3){
        circle new_circle = generate_next_circle(tangent_1, tangent_2, tangent_3);
        if(new_circle.r<=min_radius) {
            break_point.Add(Tuple.Create(tangent_1, tangent_2, tangent_3));
            //get the circle that requires the least shifting for the entire fractal
            //if(min_change>(new_circle.pos-amount_shifted).magnitude) smallest = new_circle;
            smallest = new_circle;
            return;
        }
        spawn_circle_object(new_circle);
        //Debug.Log(new_circle.r.ToString());
        generate_apollo(new_circle, tangent_1, tangent_2);
        generate_apollo(tangent_1, tangent_3, new_circle);
        generate_apollo(tangent_2, tangent_3, new_circle);
        
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
        else if(tangent4(fst, snd, trd, new circle(rad, pos2))){
            return new circle(rad, pos2);
        }

        // if(tangent4(fst, snd, trd, new circle(rad, pos1))){
        //     return new circle(rad, pos1);
        // }
        // else if(tangent4(fst, snd, trd, new circle(rad, pos2))){
        //     return new circle(rad, pos2);
        // }
        spawn_circle_object(fst, true);
        spawn_circle_object(snd, true);
        spawn_circle_object(trd, true);
        spawn_circle_object(new circle(rad, pos2), true);
        spawn_circle_object(new circle(rad, pos1), true);
        //Debug.Log("none??");
        return null;
    }

    bool tangent4(circle a, circle b, circle c, circle d){
        return (isexternalTangent(a, b, d)&&isexternalTangent(c, b, d)&&isexternalTangent(a, c, d));
    }

    bool isexternalTangent(circle a, circle b, circle c){
        if(Mathf.Abs((a.pos-b.pos).magnitude-(a.r+b.r))>0.1f) return false;
        if(Mathf.Abs((a.pos-c.pos).magnitude-(a.r+c.r))>0.1f) return false;
        if(Mathf.Abs((b.pos-c.pos).magnitude-(b.r+c.r))>0.1f) return false;
        return true;
    }

    void generate_first_three(){
        float rad = Random.Range(radius_range, radius_range*2f);
        circle fir = new circle(rad, new Vector2(0f, 0f));
        float snd_rad = Random.Range(radius_range, radius_range*2f);
        float angle = Random.Range(0f, 2*Mathf.PI);
        Vector2 snd_pos = new Vector2((snd_rad+rad)*Mathf.Cos(angle), (snd_rad+rad)*Mathf.Sin(angle));
        circle snd = new circle(snd_rad, snd_pos);
        float trd_rad = Random.Range(radius_range, 2f*radius_range);
        Vector2 trd_pos = intersection_of_two_circles(new circle(fir.r+trd_rad, fir.pos), new circle(snd.r+trd_rad, snd.pos));
        circle trd = new circle(trd_rad, trd_pos);
        spawn_circle_object(fir);
        spawn_circle_object(snd);
        spawn_circle_object(trd);
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

    public void spawn_circle_object(circle c, bool painted = false){
        //spawn the circle in world space first and then move it to the zoom_object's position
        GameObject spawned_c = GameObject.Instantiate(circle_object, (c.pos), Quaternion.identity);
        spawned_c.transform.localScale = c.r*Vector3.one*zooming_object.transform.localScale.x;
        spawned_c.transform.SetParent(zooming_object.transform);
        circle_list.Add(c);
        object_list.Add(spawned_c);
        spawned_c.GetComponent<destroycircles>().c = c;
        if(painted) spawned_c.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
