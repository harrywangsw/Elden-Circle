using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//actual chain 122cm, 10 links are 5.5cm, 1 link is 0.55cm by 0.45cm, 222 links, 132.7 g, 0.598 g per link
//approx. period for rod: 1.8091711368281

public class rope : MonoBehaviour
{
    public int fragmentCount, sign, num_arms;
    public GameObject fragmentPrefab, Marker;
    GameObject marker, first_link, player;
    Vector3 COM, distance, velocity;
    public float torq, period, ang_vel, dash_period, force, health;
    Rigidbody2D body;
    public stats stat;
    public bool dead;
    void Start()
    {
        
    }

    public void init(){
        health = stat.health;
        body = GetComponent<Rigidbody2D>();
        float bound = GetComponent<SpriteRenderer>().bounds.extents.magnitude;
        Collider2D[,] colliders = new Collider2D[num_arms,fragmentCount];
        for(int i=0; i<num_arms; i++){
            init_arm(Quaternion.Euler(0f, 0f, i*360f/num_arms)*Vector3.up*bound, Quaternion.Euler(0f, 0f, i*360f/num_arms), colliders, i);
        }
        for(int i=0; i<num_arms; i++){
            for(int j = 0; j<fragmentCount; j++){
                for(int k=0; k<num_arms&&k!=i; k++){
                    for(int l = 0; l<fragmentCount; l++){
                        Physics2D.IgnoreCollision(colliders[i,j], colliders[k,l], true);
                    }
                }
            }
        }
        player = GameObject.Find("player");
        StartCoroutine(move());
    }


    void init_arm(Vector3 rela_position, Quaternion rot, Collider2D[,] col, int ind){
        GameObject[] fragments = new GameObject[fragmentCount];
        Rigidbody2D[] bodies = new Rigidbody2D[fragmentCount];
        Collider2D[] cs = new Collider2D[fragmentCount];

        for (var i = 0; i < fragmentCount; i++)
        {
            fragments[i] = Instantiate(fragmentPrefab, rela_position+transform.position, Quaternion.identity);
            //Physics2D.IgnoreCollision(fragments[i].GetComponent<Collider2D>(), GameObject.Find("Grid").GetComponent<Collider2D>(), true);
            bodies[i] = fragments[i].GetComponent<Rigidbody2D>();
            // fragments[i].transform.SetParent(transform);
            var joint = fragments[i].GetComponent<FixedJoint2D>();
            joint.anchor = rot*new Vector2(0f, 0.8f);
            if (i > 0) {
                joint.connectedBody = fragments[i - 1].GetComponent<Rigidbody2D>();
                fragments[i].transform.position = fragments[i-1].transform.position+1.3f*fragments[i].GetComponent<SpriteRenderer>().bounds.size.y*rela_position.normalized;
            }
            joint.connectedAnchor = rot*new Vector2(0f, -0.8f);
            col[ind,i] = fragments[i].GetComponent<Collider2D>();
        }
        fragments[0].GetComponent<FixedJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
        StartCoroutine(swing_back_forth(fragments, bodies));
    }


    Vector3 getCOM(GameObject parent)
    {
        Vector3 COM = Vector3.zero;
        for(int i=0; i<parent.transform.childCount; i++)
        {
            COM.x += parent.transform.GetChild(i).localPosition.x;
            COM.y += parent.transform.GetChild(i).localPosition.y;
        }
        COM /= parent.transform.childCount;
        return COM;
    }


    public void WriteString(string text)
    {
        string path = "Assets/test.txt";

        if (text == null) File.Create(path).Close();

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);

        writer.WriteLine(text);

        writer.Close();
    }

    IEnumerator swing_back_forth(GameObject[] fragments, Rigidbody2D[] bodies){
        while(!dead){
            body.angularVelocity = ang_vel;

            // bodies[0].AddForce(fragments[0].transform.rotation*Vector2.right*torq);
            // yield return new WaitForSeconds(period);
            // bodies[0].AddForce(fragments[0].transform.rotation*Vector2.right*torq*-2f);
            // yield return new WaitForSeconds(period);

            foreach(Rigidbody2D b in bodies){
                //if(b==null) StartCoroutine(respawn_frag(ind));
                b.AddTorque(torq);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            yield return new WaitForSeconds(period);
            body.angularVelocity = -ang_vel;
            foreach(Rigidbody2D b in bodies){
                //if(b==null) StartCoroutine(respawn_frag());
                b.AddTorque(-2f*torq);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            yield return new WaitForSeconds(period);
        }
        foreach(Rigidbody2D b in bodies){
                //if(b==null) StartCoroutine(respawn_frag());
            b.angularVelocity = 0f;
        }
        StartCoroutine(death(fragments));
    }

    // IEnumerator respawn_frag(int index){
    //     bodies[index] = new Rigidbody2D();
    //     yield return new WaitForSeconds(4f);
    //     fragments[index] = 
    // }

    IEnumerator move(){
        Vector3 prev_pos = player.transform.position;
        float time = 0f;
        while(!dead){
            if((transform.position-prev_pos).magnitude>=8f&&time<dash_period){
                prev_pos = player.transform.position;
                body.AddForce((Vector2)(prev_pos-transform.position)*force);
                time = 0f;
            }
            if((transform.position-prev_pos).magnitude<=1f) body.AddForce(-(Vector2)(prev_pos-transform.position)*force);
            yield return new WaitForSeconds(Time.deltaTime);
            time+=Time.deltaTime;
        }
        body.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D c){
        damage_manager d = c.collider.gameObject.GetComponent<damage_manager>();
        if(!d) return;  
        statics.animate_hurt(GetComponent<SpriteRenderer>());
        health-=statics.calc_damage(stat, d);
        if(health<=0f){
            dead = true;
        }
    }

    IEnumerator death(GameObject[] fragments){
        for(int i=0; i<num_arms; i++){
            for(int j = 0; j<fragmentCount; j++){
                Destroy(fragments[j]);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
